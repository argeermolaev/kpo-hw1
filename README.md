# VyshKat — система учета парка

Консольное приложение для учета парка техники и вещей с поддержкой 
техосмотра, подсчета энергопотребления и фильтрации техники для новичков.

## Стек технологий

- C# / .NET 8.0
- Microsoft.Extensions.DependencyInjection (DI-контейнер)
- xUnit + Moq (тестирование)

## Архитектура решения

Решение разбито на 4 проекта по принципам чистой архитектуры:

| Проект | Назначение | Зависит от |
|---|---|---|
| `Domain` | Сущности, интерфейсы, инварианты | — |
| `Application` | Бизнес-логика (`ParkService`, `ServiceCenter`) | Domain |
| `Infrastructure` | Реализации внешних сервисов, настройка DI | Application |
| `Console` | UI, ввод-вывод, меню | Application, Infrastructure |
| `Tests` | Unit-тесты | Application, Domain |

**Структура папок Domain:**
- `Entities/Transport/` — самокаты, велосипеды
- `Entities/Things/` — шлемы, зарядные станции
- `Services/` — интерфейсы (абстракции)

## Запуск

### Требования
- .NET SDK 10.0+
- Visual Studio 2022 (или VS Code + C# Dev Kit)

### Через Visual Studio
1. Открыть `VyshKatSolution.sln`
2. Убедиться, что `VyshKat.Console` — стартовый проект (выделен жирным)
3. `Ctrl+F5` — запуск без отладки

### Через CLI
```bash
dotnet run --project VyshKat.Console/VyshKat.Console.csproj
```

# Обязательные вопросы
1. Наиболее явно применен Dependency Inversion Principle (DIP), а именно в классе ParkService: public ParkService(IServiceCenter serviceCenter) позволяет свободно заменять serviceCenter на мок или другую реализацию.
2. Сознательно нарушен Single Responsibility Principle для класса ConsoleMenu, так как писать отдельные классы для ввода, вывода и пользовательского интерфейса в этой задаче - это overengineering.
3. Новый вид транспорта - ничего не сломается, так как соблюден Open/Closed Principle (потребуется изменение в ConsoleMenu) Сотрудник - так как они не индексируются, нужно будет добавить для них отдельный интерфейс IEmployee (со всеми вытекающими из него методами) Склад запчастей будет требовать отдельной реализации.
4. AddTransport_WhenInspectionPasses_ShouldReturnTrueAndAddTransport и AddTransport_WhenInspectionFails_ShouldReturnFalse фиксируют решения техосмотра (подменяя зависимость с использованием моков), GetTransportsForNovices_ShouldReturnOnlyTransportsWithFriendlinessGreaterOrEqualSix проверяет правило "для новичков"
5. Deepseek помог с настройкой Visual Studio, с подготвкой UNIT-тестов и с интерфейсом моков, а так же пересмотрел структуру domain (предложил убрать отдельные интерфейсы для самокатов и велосипедов).