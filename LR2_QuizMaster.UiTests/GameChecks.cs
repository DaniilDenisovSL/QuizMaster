using System.Reflection;
using System.Drawing.Imaging;
using LR2_QuizMaster.Forms;
using LR2_QuizMaster.UI;
using LR2_QuizMaster.Backend.Domain;
using LR2_QuizMaster.Backend.Services;

internal static class GameChecks
{
    private static int checks;
    private static string output = "";
    private static QuestionBank bank = null!;
    private static bool requireFocus;
    [STAThread]
    internal static int Run(string[] args)
    {
        Application.SetHighDpiMode(args.Contains("--dpi-aware") ? HighDpiMode.PerMonitorV2 : HighDpiMode.DpiUnaware);
        Application.SetUnhandledExceptionMode(UnhandledExceptionMode.ThrowException);
        Application.EnableVisualStyles();
        output = args[0]; Directory.CreateDirectory(output);
        requireFocus = !args.Contains("--no-focus");
        bank = QuestionBank.Load(QuestionBank.DefaultFilePath);
        Exception? failure = null;
        using var host = new Form { Opacity = 0, ShowInTaskbar = false };
        host.Shown += async (_, _) =>
        {
            try { await Run(); }
            catch (Exception ex) { failure = ex; Console.WriteLine(ex); }
            finally { host.Close(); }
        };
        Application.Run(host);
        if (failure != null) return 1;
        Console.WriteLine($"PASS {checks} UI regression scenarios.");
        return 0;
    }

    private static async Task Run()
    {
        await SetupAndFullScreen();
        await ConfirmAndPause();
        foreach (var category in Enum.GetValues<QuizCategory>())
            foreach (var difficulty in Enum.GetValues<Difficulty>()) await CategoryFlow(category,difficulty);
        await TimeoutAndContinue();
        await ResultFlow(false);
        await ResultFlow(true);
        await ResultFlow(false, failSave: true);
        await MainMenuFlow();
    }

    private static async Task SetupAndFullScreen()
    {
        using var setup = new GameSetupForm { ShowInTaskbar = false };
        setup.Show(); await Task.Delay(260);
        Assert(!All(setup).Any(c => c is CheckBox || c is RadioButton), "shuffle controls really removed");
        foreach (string level in new[] { "Easy", "Hard", "Medium" })
        {
            var button = Find<QuizButton>(setup, "btn" + level); button.PerformClick();
            Assert(setup.SelectedDifficulty == button.Text && button.Selected, "difficulty selection");
            Assert(!string.IsNullOrEmpty(button.Description) && !string.IsNullOrEmpty(button.AccessibleDescription), "description and tooltip content");
            Assert(All(setup).OfType<QuizButton>().Count(b => b.Selected) == 1, "one selected difficulty");
        }
        Capture(setup, "01-setup.png");
        if (!requireFocus)
        {
            for (int i = 0; i < 5; i++)
            {
                WindowPresentation.ToggleFullScreen(setup);
                Assert(WindowPresentation.IsFullScreen(setup), "fullscreen API entered");
                WindowPresentation.ToggleFullScreen(setup);
                Assert(!WindowPresentation.IsFullScreen(setup) && setup.Visible, "fullscreen API restored");
            }
            setup.Close();
            Console.WriteLine("SKIP native F11/Esc and active-window maximize routing: --no-focus");
            Pass("difficulty cards and repeated fullscreen API transitions");
            return;
        }
        int forms = Application.OpenForms.Count;
        for (int i = 0; i < 5; i++)
        {
            setup.Activate(); await Task.Delay(20);
            Assert(Key(setup, Keys.F11), "F11 consumed");
            Assert(WindowPresentation.IsFullScreen(setup) && setup.FormBorderStyle == FormBorderStyle.None, "fullscreen active");
            Assert(Key(setup, Keys.Escape), "fullscreen Esc consumed");
            Assert(!WindowPresentation.IsFullScreen(setup) && setup.Visible && setup.FormBorderStyle == FormBorderStyle.Sizable, "same form restored");
            Assert(Application.OpenForms.Count == forms, "no additional form on Esc");
        }
        setup.WindowState = FormWindowState.Maximized; await Task.Delay(40);
        Key(setup, Keys.F11); Key(setup, Keys.Escape);
        Assert(setup.WindowState == FormWindowState.Maximized, "maximized state restored");
        setup.WindowState = FormWindowState.Normal; await Task.Delay(30);
        setup.Close(); Pass("difficulty cards, no shuffle controls, repeated F11/Esc and maximize restore");
    }

    private static async Task ConfirmAndPause()
    {
        var clock = new ManualClock(); var store = Store("abort");
        using var quiz = new QuizForm(Request(), bank, store, clock) { ShowInTaskbar = false };
        quiz.Show(); await Task.Delay(240);
        var before = Session(quiz).GetState(); int confirmations = 0;
        Confirm(quiz, _ => { confirmations++; clock.Advance(60); return DialogResult.No; });
        Find<Button>(quiz, "btnExitQuiz").PerformClick();
        Assert(quiz.Visible && !quiz.IsDisposed && quiz.DialogResult == DialogResult.None, "cancel exit button leaves quiz open");
        var after = Session(quiz).GetState();
        Assert(before.CurrentQuestion!.QuestionToken == after.CurrentQuestion!.QuestionToken && before.RemainingTime == after.RemainingTime && after.Lives == 3, "confirmation time excluded without losing state");
        clock.Advance(1); await Task.Delay(150);
        Assert(Session(quiz).GetState().RemainingTime == before.RemainingTime - TimeSpan.FromSeconds(1), "timer resumes after cancellation");
        quiz.Close(); Assert(quiz.Visible && confirmations == 2, "native close also prompts and cancellation keeps form");
        var ticket = Session(quiz).GetState().CurrentQuestion!;
        var q = bank.Questions.Single(q => q.Id == ticket.QuestionId);
        FindAnswer(quiz,q.CorrectAnswerId).PerformClick();
        before = Session(quiz).GetState(); quiz.Close();
        after = Session(quiz).GetState();
        Assert(after.Status == SessionStatus.AwaitingNext && after.Score == before.Score, "awaiting-next exit cancellation keeps answer and score");
        Confirm(quiz, _ => { confirmations++; return DialogResult.Yes; });
        Find<Button>(quiz,"btnExitQuiz").PerformClick(); await Task.Delay(60);
        Assert(quiz.IsDisposed || !quiz.Visible, "confirmed exit closes");
        Assert((await store.LoadAsync()).Records.Count == 0, "interrupted result not saved");
        Pass("exit button / X confirmation, pause, resume, state protection and abort persistence");
    }

    private static async Task ResultFlow(bool lose, bool failSave = false)
    {
        var store = Store(lose ? "lost" : "won");
        if (failSave)
        {
            var blocked=Path.Combine(output,"save-target-is-directory"); Directory.CreateDirectory(blocked);
            store=new LeaderboardStore(blocked);
        }
        using var quiz = new QuizForm(Request(), bank, store, new ManualClock()) { ShowInTaskbar = false };
        quiz.Show(); await Task.Delay(240);
        bool seen = false; Exception? failure = null;
        using var closer = new System.Windows.Forms.Timer { Interval = 30 };
        closer.Tick += (_, _) =>
        {
            var result = Application.OpenForms.OfType<ResultsForm>().FirstOrDefault(f => f.Visible && f.Opacity >= .99);
            if(result == null) return;
            closer.Stop();
            try
            {
                Assert(Find<Label>(result,"lblHeading").Text.Contains(lose?"програно":"завершено"), "result heading matches reason");
                Assert(Find<QuizIconView>(result,"icoCompleted").IconKind == (lose ? QuizIcon.Defeat : QuizIcon.Completed), "correct result icon");
                Assert(Find<Label>(result,"lblTotalScore").Text == (lose ? "0 балів" : "1000 балів"), "real result score");
                Assert(Find<Label>(result,"lblSummaryNote").Text.Contains(failSave ? "не вдалося" : "збережено"), "save success/failure displayed");
                Capture(result, lose?"03-defeat.png":"02-completed.png"); seen = true;
            }
            catch(Exception ex) { failure = ex; }
            Find<Button>(result,"btnMainMenu").PerformClick();
        };
        closer.Start();
        for(int i=0;i<(lose?3:10);i++)
        {
            var ticket = Session(quiz).GetState().CurrentQuestion!;
            var q = bank.Questions.Single(q=>q.Id==ticket.QuestionId);
            var id = lose ? ticket.Answers.First(a=>a.Id!=q.CorrectAnswerId).Id : q.CorrectAnswerId;
            FindAnswer(quiz,id).PerformClick();
            if(i<(lose?2:9)) Find<Button>(quiz,"btnNextQuestion").PerformClick();
        }
        await Until(()=>seen || failure!=null, "result not shown");
        if(failure!=null) throw failure;
        await Until(()=>quiz.IsDisposed || !quiz.Visible, "quiz not closed after results");
        if (!failSave)
        {
            var records = (await store.LoadAsync()).Records;
            Assert(records.Count==1 && records[0].EndReason==(lose?GameEndReason.LivesExhausted:GameEndReason.Completed), "result saved once");
        }
        Pass(failSave ? "save failure preserves result and reports error" : lose?"real lives-exhausted defeat and saving":"real completion and saving");
    }

    private static async Task MainMenuFlow()
    {
        using var menu = new MainMenuForm(bank, Store("main")) { ShowInTaskbar = false };
        menu.Show(); await Task.Delay(230);
        Find<TextBox>(menu,"txtPlayerName").Text = "Перевірка";
        int phase = 0; Exception? failure = null;
        using var timer = new System.Windows.Forms.Timer { Interval = 280 };
        timer.Tick += (_, _) =>
        {
            try
            {
                if(phase==0 && Application.OpenForms.OfType<GameSetupForm>().FirstOrDefault(f=>f.Visible) is {} setup)
                { phase++; Find<Button>(setup,"btnMovies").PerformClick(); Find<Button>(setup,"btnEasy").PerformClick(); Find<Button>(setup,"btnStartQuiz").PerformClick(); }
                else if(phase==1 && Application.OpenForms.OfType<QuizForm>().FirstOrDefault(f=>f.Visible) is {} quiz && Session(quiz).GetState().CurrentQuestion is {} ticket)
                {
                    var q=bank.Questions.Single(q=>q.Id==ticket.QuestionId);
                    Assert(q.Category==QuizCategory.Movies && q.Difficulty==Difficulty.Easy, "chosen options reach backend");
                    Assert(ticket.QuestionId=="movies-easy-001" && ticket.Answers.SequenceEqual(q.Answers), "fixed question and answer order");
                    Confirm(quiz,_=>DialogResult.Yes); phase++; Find<Button>(quiz,"btnExitQuiz").PerformClick();
                }
            }
            catch(Exception ex) { failure=ex; timer.Stop(); foreach(Form f in Application.OpenForms.Cast<Form>().ToArray()) if(f!=menu && f.GetType()!=typeof(Form)) f.Close(); }
        };
        timer.Start(); Find<Button>(menu,"btnStart").PerformClick(); timer.Stop();
        if(failure!=null) throw failure;
        Assert(phase==2 && menu.Visible && menu.Opacity==1 && menu.Enabled, "menu restored after real flow");
        menu.Close(); Pass("menu -> setup -> quiz -> confirmed exit -> menu with deterministic order");
    }

    private static async Task CategoryFlow(QuizCategory category,Difficulty difficulty)
    {
        var store=Store("category");
        using var quiz=new QuizForm(new QuizRequest("Перевірка",category,difficulty,new QuizOptions{ShuffleQuestions=false,ShuffleAnswers=false}),bank,store,new ManualClock()) { ShowInTaskbar=false };
        quiz.Show(); await Task.Delay(40);
        bool resultSeen=false; Exception? failure=null;
        using var closer=new System.Windows.Forms.Timer{Interval=20};
        closer.Tick+=(_,_)=>
        {
            var result=Application.OpenForms.OfType<ResultsForm>().FirstOrDefault(f=>f.Visible);
            if(result==null) return;
            closer.Stop();
            try { Assert(Find<Label>(result,"lblTotalScore").Text=="1000 балів","full category score"); resultSeen=true; }
            catch(Exception ex) { failure=ex; }
            Find<Button>(result,"btnMainMenu").PerformClick();
        };
        closer.Start();
        for(int i=0;i<10;i++)
        {
            var state=Session(quiz).GetState(); var ticket=state.CurrentQuestion!;
            var source=bank.Find(category,difficulty)[i];
            Assert(ticket.QuestionId==source.Id && Find<Label>(quiz,"lblQuestion").Text==source.Text,"real question and category bound in fixed order");
            foreach(var answer in source.Answers) Assert(FindAnswer(quiz,answer.Id).Text.Contains(answer.Text),"answer ID/text stay attached");
            var label=Find<Label>(quiz,"lblQuestion");
            var needed=TextRenderer.MeasureText(label.Text,label.Font,new Size(label.Width,int.MaxValue),TextFormatFlags.WordBreak|TextFormatFlags.NoPadding);
            if(needed.Height>label.Height) Console.WriteLine("LAYOUT_FINDING "+source.Id+" needs "+needed.Height+" vs "+label.Height);
            var button=FindAnswer(quiz,source.CorrectAnswerId); button.PerformClick();
            int score=Session(quiz).GetState().Score; button.PerformClick(); Assert(Session(quiz).GetState().Score==score,"double click doesn't duplicate score");
            Assert(All(quiz).OfType<QuizButton>().Where(b=>b.Tag is string).All(b=>!b.Enabled),"answers locked after selection");
            if(i<9) Find<Button>(quiz,"btnNextQuestion").PerformClick();
        }
        await Until(()=>resultSeen || failure!=null,"category result not shown");
        if(failure!=null) throw failure;
        await Until(()=>quiz.IsDisposed || !quiz.Visible,"quiz didn't close");
        var records=(await store.LoadAsync()).Records; Assert(records.Count==1 && records[0].Score==1000,"each complete session saved once");
        Pass("all ten questions: "+category+"/"+difficulty);
    }

    private static async Task TimeoutAndContinue()
    {
        var store=Store("timeout"); var clock=new ManualClock();
        using var quiz=new QuizForm(Request(),bank,store,clock){ShowInTaskbar=false};
        quiz.Show(); await Task.Delay(120);
        clock.Advance(20); await Task.Delay(140);
        var state=Session(quiz).GetState();
        Assert(state.Status==SessionStatus.AwaitingNext && state.Lives==2 && state.Score==0,"timeout loses exactly one life");
        Assert(Find<Label>(quiz,"lblFeedback").Text.Contains("Час вичерпано"),"timeout feedback visible");
        await Task.Delay(140); Assert(Session(quiz).GetState().Lives==2,"polling cannot remove a second life");
        Find<Button>(quiz,"btnNextQuestion").PerformClick(); state=Session(quiz).GetState();
        Assert(state.RemainingTime==TimeSpan.FromSeconds(20),"next question resets timer");
        var q=bank.Questions.Single(q=>q.Id==state.CurrentQuestion!.QuestionId);
        FindAnswer(quiz,q.CorrectAnswerId).PerformClick();
        Assert(Session(quiz).GetState().CurrentStreak==1,"correct reply starts streak");
        Find<Button>(quiz,"btnNextQuestion").PerformClick(); clock.Advance(20); await Task.Delay(140);
        state=Session(quiz).GetState();
        Assert(state.Lives==1 && state.CurrentStreak==0 && state.BestStreak==1,"timeout resets current streak and keeps best");
        Confirm(quiz,_=>DialogResult.Yes); quiz.Close();
        Assert((await store.LoadAsync()).Records.Count==0,"timed-out but interrupted test is not saved");
        Pass("timeout, polling, continue, streak reset and interrupted save");
    }

    private static IEnumerable<Control> All(Control root) => root.Controls.Cast<Control>().SelectMany(c=>new[]{c}.Concat(All(c)));
    private static T Find<T>(Control form,string name) where T:Control => (T)form.Controls.Find(name,true).Single();
    private static QuizButton FindAnswer(QuizForm form,string id) => All(form).OfType<QuizButton>().Single(b=>b.Tag is string tag && tag==id);
    private static QuizSession Session(QuizForm form) => (QuizSession)typeof(QuizForm).GetField("session",BindingFlags.NonPublic|BindingFlags.Instance)!.GetValue(form)!;
    private static void Confirm(QuizForm form,Func<IWin32Window,DialogResult> fn) => typeof(QuizForm).GetProperty("ConfirmExit",BindingFlags.NonPublic|BindingFlags.Instance)!.SetValue(form,fn);
    private static bool Key(Form form,Keys key)
    {
        form.Activate();
        var m=Message.Create(form.Handle,0x100,(IntPtr)(int)key,IntPtr.Zero);
        return Application.FilterMessage(ref m);
    }
    private static LeaderboardStore Store(string name) => new(Path.Combine(output,name+Guid.NewGuid().ToString("N"),"records.json"));
    private static QuizRequest Request() => new("Тест",QuizCategory.Programming,Difficulty.Medium,new QuizOptions{ShuffleQuestions=false,ShuffleAnswers=false});
    private static void Capture(Form form,string name) { using var b=new Bitmap(form.Width,form.Height); form.DrawToBitmap(b,new Rectangle(Point.Empty,b.Size)); b.Save(Path.Combine(output,name),ImageFormat.Png); }
    private static void Assert(bool value,string message) { if(!value) throw new Exception(message); }
    private static void Pass(string name) { checks++; Console.WriteLine("PASS "+name); }
    private static async Task Until(Func<bool> fn,string message) { for(int i=0;i<200;i++){if(fn()) return; await Task.Delay(20);} throw new Exception(message); }
    private sealed class ManualClock : TimeProvider { private DateTimeOffset now=DateTimeOffset.UtcNow; public override DateTimeOffset GetUtcNow()=>now; public void Advance(int seconds)=>now+=TimeSpan.FromSeconds(seconds); }
}
