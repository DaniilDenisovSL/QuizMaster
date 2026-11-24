# Quiz Master

Командний навчальний проєкт: локальна вікторина на C# / .NET 10 / Windows Forms. Поточний код імпортовано з архіву **LR2_QuizMaster, версія 6**.

## Можливості

- 150 запитань: 5 категорій × 3 рівні складності × 10 запитань.
- Вибір категорії та складності, таймер, бали, життя й серії відповідей.
- Перемішування запитань і відповідей зі збереженням їхніх ідентифікаторів.
- Екран результатів і локальна таблиця рекордів.
- Перевірка банку запитань і записів рекордів, backend та UI-перевірки.

## Запуск на Windows

Потрібен **.NET SDK 10**. Для роботи через Visual Studio потрібна версія з підтримкою .NET 10 і компонент розробки класичних програм .NET.

Відкрити `LR2_QuizMaster.slnx` або виконати з кореня репозиторію:

```bash
dotnet build LR2_QuizMaster.slnx
dotnet run --project LR2_QuizMaster/LR2_QuizMaster.csproj
```

Рекорди зберігаються поза репозиторієм: `%LOCALAPPDATA%/QuizMaster/leaderboard.json`.

## Перевірки

```bash
dotnet run --project LR2_QuizMaster.Tests/LR2_QuizMaster.Tests.csproj
dotnet run --project LR2_QuizMaster.UiTests/LR2_QuizMaster.UiTests.csproj -- artifacts/data --data-audit
dotnet run --project LR2_QuizMaster.UiTests/LR2_QuizMaster.UiTests.csproj -- artifacts/game --no-focus
dotnet run --project LR2_QuizMaster.UiTests/LR2_QuizMaster.UiTests.csproj -- artifacts/menu --menu
dotnet run --project LR2_QuizMaster.UiTests/LR2_QuizMaster.UiTests.csproj -- artifacts/layout --layout --no-focus
```

UI-перевірки відкривають тестові вікна. Режим `--no-focus` явно пропускає сценарії, яким потрібна активна клавіатура.

## Структура

| Шлях | Призначення |
| --- | --- |
| `LR2_QuizMaster/Forms` | Форми Windows Forms |
| `LR2_QuizMaster/UI` | Елементи інтерфейсу та навігація |
| `LR2_QuizMaster/Backend` | Моделі, банк запитань, сесія гри та рекорди |
| `LR2_QuizMaster/Backend/Data/questions.json` | Банк запитань |
| `LR2_QuizMaster.Tests` | Backend та регресійні перевірки |
| `LR2_QuizMaster.UiTests` | Перевірки інтерфейсу та даних |
| `Design` | Опис стилю й знімки інтерфейсу |
| `docs` | Правила, інтеграція, звіти та походження архівів |

## Команда

| Учасник | Роль |
| --- | --- |
| Данііл Денисов | Team Lead |
| [Артем Скиба](https://github.com/ArtemSkybaASV) | Backend |
| [Дьяконов-Гапін Віталій / cratosCode](https://github.com/ffearofgodss-source) | Full-stack |
| Ярослав Лупій | Tester |

Ролі описують розподіл команди; імпорт архівів не встановлює авторства окремих змін. Правила гілок і комітів: [CONTRIBUTING.md](CONTRIBUTING.md). [Дошка Trello](https://trello.com/b/e4HG0X4K).

## Документація

- [Повторна перевірка під час імпорту 24.11.2026](docs/IMPORT_VERIFICATION_2026-11-24.md).
- [Усі шість архівних версій та їх походження](docs/ARCHIVE_IMPORT.md).
- [Правила й інтеграція backend](docs/BACKEND_AND_UI_INTEGRATION.md).
- [Стан інтеграції форм](docs/UI_INTEGRATION_STATUS.md).
- [Звіт із версії 6](docs/QA_REPORT_2026-11-23.md). Це успадкований звіт, а не новий запуск перевірок під час імпорту; його журнали з архіву збережено в `qa-logs`.

Старі `.patch` збережено як довідкові матеріали. Застосовувати їх до поточної версії не потрібно.
