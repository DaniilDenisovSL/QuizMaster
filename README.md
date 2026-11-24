# Quiz Master

Локальна вікторина на C# / .NET 10 / Windows Forms.

- Backend без залежності від форм — у LR2_QuizMaster/Backend.
- Банк — LR2_QuizMaster/Backend/Data/questions.json.
- Правила, API, інтеграція форм і тести — docs/BACKEND_AND_UI_INTEGRATION.md.
- Окремі пропозиції підключення code-behind — docs/UI_INTEGRATION.patch.
- Перевірки backend — LR2_QuizMaster.Tests.

Збірка та запуск перевірок із папки рішення:

    dotnet build .\LR2_QuizMaster\LR2_QuizMaster.csproj
    dotnet run --project .\LR2_QuizMaster.Tests\LR2_QuizMaster.Tests.csproj

Рекорди зберігаються в LocalAppData\QuizMaster\leaderboard.json.

