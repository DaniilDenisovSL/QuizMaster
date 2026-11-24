# Backend Quiz Master

Backend винесений у **Backend/Domain** і **Backend/Services** та не посилається на WinForms. Файли **Forms/*.Designer.cs**, **.resx** і **UI/** не змінювалися. Пропозиція окремого підключення до форм — у **docs/UI_INTEGRATION.patch**.

## Класи

- **Question**, **Answer** — текст питання та чотири відповіді зі стабільними ID; правильність перевіряється через CorrectAnswerId.
- **QuizCategory**, **Difficulty**, **QuizCatalog** — внутрішні ключі та зіставлення українських назв категорій і складності з формою.
- **QuizOptions**, **QuizRequest** — налаштування правил і параметри запуску.
- **QuestionBank** — завантажує JSON, перевіряє ID/відповіді та наявність достатньої кількості питань.
- **QuizSession** — стан гри, перемішування, дедлайни, відповіді, рахунок, життя, серії та результат.
- **QuestionTicket**, **QuizState**, **AnswerOutcome**, **QuizResult** — UI-контракт і незмінні знімки стану.
- **LeaderboardStore** — локальне JSON-збереження, сортування, фільтр за категорією та захист від повторного SessionId.
- **InsufficientQuestionsException** — зрозуміле повідомлення, якщо банк не покриває запитану сесію.

Банк: **LR2_QuizMaster/Backend/Data/questions.json**. Він містить 150 питань — по 10 для кожної з 5 категорій і 3 рівнів складності. Запуск читання: **QuestionBank.Load(QuestionBank.DefaultFilePath)**. Некоректний JSON та записи зупиняють запуск із конкретним повідомленням; банк не доповнюється повторами.

## Запуск та відповідь

~~~csharp
var bank = QuestionBank.Load(QuestionBank.DefaultFilePath);
var session = new QuizSession(bank);
var rules = new QuizOptions
{
    ShuffleQuestions = setup.ShuffleQuestions,
    ShuffleAnswers = setup.ShuffleAnswers
};
var sessionId = session.Start(new QuizRequest(
    playerName,
    QuizCatalog.ParseCategory(setup.SelectedCategory),
    QuizCatalog.ParseDifficulty(setup.SelectedDifficulty),
    rules));

var state = session.GetState();
var ticket = state.CurrentQuestion!;
var answerId = ticket.Answers[0].Id; // У формі ID має бути збережений у Tag кнопки.
var outcome = session.SubmitAnswer(sessionId, ticket.QuestionToken, answerId);
~~~

Перехід після показу результату відповіді:

~~~csharp
var state = session.GetState();
if (state.Status == SessionStatus.AwaitingNext && state.CurrentQuestion is { } ticket)
    session.NextQuestion(sessionId, ticket.QuestionToken);
~~~

QuestionTicket містить одноразовий QuestionToken. Зберігайте його з ID відповіді. SubmitAnswer приймає SessionId, QuestionToken та AnswerId. Повторний клік повертає AlreadyAnswered; старий токен чи сесія — StaleQuestion/StaleSession. Невідомий AnswerId не витрачає спробу. Виклики потокобезпечні.

## Час і стани

Єдине джерело часу — TimeProvider у QuizSession. GetState повертає RemainingTime і сам фіксує таймаут, якщо дедлайн минув. Backend не створює WinForms-таймерів і не викликає подій із фонового потоку. UI може опитувати GetState на UI-потоці коротким WinForms Timer для перемальовування. Число в Label не повинно зменшуватися незалежно від backend. Зупиняйте UI Timer після відповіді, завершення чи виходу.

QuestionActive означає, що можна відповісти; AwaitingNext — що відповідь/таймаут уже зафіксовані; Finished — питання завершились або закінчились життя; Interrupted — гравець перервав гру. Після останньої відповіді стан стає Finished одразу. Якщо відповідь або опитування приходить після дедлайну, зараховується лише таймаут. Повторний Start очищає стан і створює новий SessionId та токени.

## Початкові правила

Усі числові правила зосереджені в QuizOptions:

- 10 питань;
- 20 секунд на питання;
- 3 життя;
- 100 балів за правильну відповідь;
- окремі прапорці перемішування питань і відповідей;
- помилка або таймаут забирає життя й скидає поточну серію;
- бонусу за швидкість і штрафу балів немає.

Кожна сесія відбирає питання без повторень. Перемішуються копії; вихідний банк не змінюється.

## Результати й точність

QuizResult містить SessionId, гравця, категорію, складність, бали, правильні відповіді, кількість зафіксованих спроб, повну кількість питань, точність, найкращу серію та причину завершення.

Точність = CorrectAnswers / QuestionsPlayed у діапазоні 0..1; якщо зафіксованих відповідей ще немає, точність дорівнює 0. Таймаути входять у знаменник. Питання без відповіді на момент переривання не входять. Для відсотків у UI використовуйте Accuracy * 100.

Interrupted не зберігається як завершене проходження. Вичерпання життів — звичайне завершення раунду й може потрапити до рекордів.

~~~csharp
var store = new LeaderboardStore(); // LocalAppData/QuizMaster/leaderboard.json
var saved = await store.SaveAsync(result);
var sorted = await store.GetSortedAsync(QuizCategory.Programming);
~~~

Повторний SessionId або interrupted-результат повертають false. Відсутній чи порожній файл читається як порожній список. Пошкоджений JSON повертає порожній список і LeaderboardLoadResult.Warning; перед наступним записом оригінал копіюється у файл *.corrupt-<timestamp>.bak.

## Що підключати у формах

- **MainMenuForm:** один раз завантажити QuestionBank і створити LeaderboardStore; перевіряти ім’я; передавати сервіси в QuizForm і LeaderboardForm.
- **GameSetupForm:** змін не потрібно. Вона вже повертає українські назви категорій/складності й прапорці shuffle. У MainMenuForm мапити назви через QuizCatalog.ParseCategory та ParseDifficulty.
- **QuizForm:** замінити одне демонстраційне питання на state.CurrentQuestion; зберігати Answer.Id у Tag кнопки, а не виводити ID з A–D; показувати номер, час, бали, життя й серії зі QuizState; підсвічувати LastAnswer.CorrectAnswerId; викликати NextQuestion зі збереженим токеном. Вихід викликає Interrupt і не відкриває успішний результат.
- **ResultsForm:** приймати QuizResult; замінити 820, 8/10, 80% і серію 5 його полями; показати причину завершення. Зіграти ще може лишитися DialogResult.Retry, оскільки MainMenuForm вже перезапускає налаштування.
- **LeaderboardForm:** видалити demoRows; отримувати записи зі Store, сортувати за балами/точністю/датою, фільтрувати вибрану категорію та заповнювати ті самі колонки. Приховати підпис демо; порожній стан показувати через наявний lblEmptyState.

Зміни code-behind для форм запропоновані окремо в **UI_INTEGRATION.patch**. Не потрібно редагувати .Designer.cs, .resx або файли UI/.

## Перевірка

LR2_QuizMaster.Tests — тестовий harness без зовнішніх NuGet-пакетів. Він напряму компілює backend-файли й перевіряє завантаження банку/локалізацію, правильну й неправильну відповідь, таймаут, останнє життя, останнє питання, серії, подвійне натискання, застарілі токени, перемішування, повторну гру та рекорди.

З кореня рішення:

~~~powershell
dotnet build .\LR2_QuizMaster\LR2_QuizMaster.csproj
dotnet run --project .\LR2_QuizMaster.Tests\LR2_QuizMaster.Tests.csproj
~~~

