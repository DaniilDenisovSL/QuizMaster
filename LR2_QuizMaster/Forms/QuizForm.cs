using System.ComponentModel;
using LR2_QuizMaster.Backend.Domain;
using LR2_QuizMaster.Backend.Services;
using LR2_QuizMaster.UI;

namespace LR2_QuizMaster.Forms;

public partial class QuizForm : Form
{
    private static readonly Color AnswerSurface = Color.FromArgb(27, 38, 59);
    private static readonly Color CorrectSurface = Color.FromArgb(22, 109, 84);
    private static readonly Color IncorrectSurface = Color.FromArgb(125, 48, 67);
    private readonly System.Windows.Forms.Timer refreshTimer;
    private readonly QuizButton[] answerButtons;
    private QuizSession? session;
    private QuizRequest? request;
    private QuizOptions options = new();
    private LeaderboardStore leaderboardStore = new();
    private QuizState? displayedState;
    private Guid displayedQuestion;
    private bool finishing;
    private Task<SaveOutcome>? saveTask;

    public QuizForm()
    {
        InitializeComponent();
        WindowPresentation.Attach(this);
        components ??= new Container();
        refreshTimer = new System.Windows.Forms.Timer(components) { Interval = 100 };
        refreshTimer.Tick += (_, _) => RefreshState();
        answerButtons = [btnAnswerA, btnAnswerB, btnAnswerC, btnAnswerD];
        foreach (var button in answerButtons) { button.Enabled = false; button.Click += AnswerClicked; }
        btnNextQuestion.Click += NextQuestion;
        btnExitQuiz.Click += (_, _) => { DialogResult = DialogResult.Cancel; Close(); };
        CancelButton = btnExitQuiz;
        Shown += (_, _) => StartSession();
        FormClosing += ClosingSession;
        prgTime.Maximum = 1000;
        prgTime.Value = 0;
    }

    public QuizForm(QuizRequest request, QuestionBank bank, LeaderboardStore leaderboardStore, TimeProvider? clock = null) : this()
    {
        this.request = request ?? throw new ArgumentNullException(nameof(request));
        this.leaderboardStore = leaderboardStore ?? throw new ArgumentNullException(nameof(leaderboardStore));
        options = request.Options ?? new QuizOptions();
        session = new QuizSession(bank, clock);
        lblCategory.Text = $"{QuizCatalog.CategoryName(request.Category)}  /  {QuizCatalog.DifficultyName(request.Difficulty)}";
    }

    private void StartSession()
    {
        if (session == null || request == null) return;
        try { session.Start(request); RefreshState(); }
        catch (Exception ex) when (ex is ArgumentException or InvalidOperationException)
        {
            MessageBox.Show(this, ex.Message, "Не вдалося почати гру", MessageBoxButtons.OK, MessageBoxIcon.Error);
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }

    private void AnswerClicked(object? sender, EventArgs e)
    {
        if (finishing || session == null || sender is not QuizButton { Tag: string answerId }) return;
        if (displayedState is not { Status: SessionStatus.QuestionActive, CurrentQuestion: { } ticket }) return;
        session.SubmitAnswer(ticket.SessionId, ticket.QuestionToken, answerId);
        RefreshState();
    }

    private void NextQuestion(object? sender, EventArgs e)
    {
        if (finishing || session == null) return;
        var state = session.GetState();
        if (state is { Status: SessionStatus.AwaitingNext, CurrentQuestion: { } ticket })
            session.NextQuestion(ticket.SessionId, ticket.QuestionToken);
        RefreshState();
    }

    private void RefreshState()
    {
        if (finishing || session == null || IsDisposed || Disposing) return;
        var state = session.GetState();
        displayedState = state;
        if (state.CurrentQuestion is not { } ticket) return;
        if (displayedQuestion != ticket.QuestionToken)
        {
            displayedQuestion = ticket.QuestionToken;
            lblQuestion.Text = ticket.Text;
            for (int i = 0; i < answerButtons.Length; i++)
            {
                var button = answerButtons[i];
                button.Text = $"{(char)('A' + i)})  {ticket.Answers[i].Text}";
                button.Tag = ticket.Answers[i].Id;
                button.PreserveColorsWhenDisabled = false;
                button.BackColor = AnswerSurface;
            }
            lblFeedback.Text = "Обери один варіант відповіді";
            lblFeedback.ForeColor = Color.FromArgb(170, 182, 204);
        }
        lblProgress.Text = $"Питання {state.QuestionNumber}/{state.TotalQuestions}";
        lblScore.Text = $"Бали: {state.Score}";
        lblLives.Text = $"Життя: {state.Lives}/{options.StartingLives}";
        lblStreak.Text = $"Серія: {state.CurrentStreak}";
        lblTimer.Text = $"Час: {Math.Max(0, (int)Math.Ceiling(state.RemainingTime.TotalSeconds))} с";
        prgTime.Value = (int)Math.Clamp(state.RemainingTime.TotalMilliseconds / options.TimePerQuestion.TotalMilliseconds * prgTime.Maximum, 0, prgTime.Maximum);
        bool active = state.Status == SessionStatus.QuestionActive;
        foreach (var button in answerButtons) button.Enabled = active;
        btnNextQuestion.Enabled = state.Status == SessionStatus.AwaitingNext;
        btnNextQuestion.BackColor = btnNextQuestion.Enabled ? Color.FromArgb(109, 94, 245) : Color.FromArgb(36, 49, 73);
        lblTimerNote.Text = active ? "Обери одну відповідь" : "Питання завершено";
        if (state.LastAnswer is { } answer)
        {
            foreach (var button in answerButtons)
            {
                if (button.Tag is not string id) continue;
                bool correct = id == answer.CorrectAnswerId;
                bool wrong = id == answer.SubmittedAnswerId && !answer.IsCorrect;
                button.PreserveColorsWhenDisabled = correct || wrong;
                button.BackColor = correct ? CorrectSurface : wrong ? IncorrectSurface : AnswerSurface;
            }
            lblFeedback.Text = answer.Disposition switch
            {
                AnswerDisposition.Correct => "Правильно!",
                AnswerDisposition.Incorrect => "Неправильно. Правильну відповідь позначено зеленим.",
                AnswerDisposition.TimedOut => "Час вичерпано. Правильну відповідь позначено зеленим.",
                _ => "Відповідь уже опрацьована."
            };
            lblFeedback.ForeColor = answer.IsCorrect ? Color.FromArgb(39, 198, 152) : Color.FromArgb(239, 153, 166);
        }
        if (active) refreshTimer.Start(); else refreshTimer.Stop();
        if (state is { Status: SessionStatus.Finished, Result: { } result }) _ = ShowResultAsync(result);
    }

    private async Task ShowResultAsync(QuizResult result)
    {
        if (finishing) return;
        finishing = true;
        refreshTimer.Stop();
        btnNextQuestion.Enabled = false;
        btnNextQuestion.Text = "Результат…";
        btnExitQuiz.Enabled = false;
        var saving = EnsureSavedAsync(result);
        await Task.Delay(450);
        var saved = await saving;
        if (IsDisposed || Disposing || !Visible) return;
        Hide();
        using var form = new ResultsForm(result, leaderboardStore, saved.Warning);
        DialogResult = form.ShowDialog() == DialogResult.Retry ? DialogResult.Retry : DialogResult.OK;
        Close();
    }

    private Task<SaveOutcome> EnsureSavedAsync(QuizResult result) => saveTask ??= SaveResultAsync(result);

    private async Task<SaveOutcome> SaveResultAsync(QuizResult result)
    {
        try { await leaderboardStore.SaveAsync(result); return new SaveOutcome(null); }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or System.Security.SecurityException)
        { return new SaveOutcome(ex.Message); }
    }

    private void ClosingSession(object? sender, FormClosingEventArgs e)
    {
        refreshTimer.Stop();
        if (session == null) return;
        var state = session.GetState();
        if (state.Status == SessionStatus.NotStarted) return;
        if (state.Result is { EndReason: not GameEndReason.Interrupted } result) _ = EnsureSavedAsync(result);
        else if (state.Status != SessionStatus.Interrupted) session.Interrupt();
    }

    private sealed record SaveOutcome(string? Warning);
}
