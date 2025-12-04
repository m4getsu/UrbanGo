# 🚗 UrbanGo - Система управления каршерингом

Современная система управления автопарком каршеринга, построенная по принципам **SOLID** с использованием **MVP-архитектуры**, **многослойной архитектуры**, **Dependency Injection (Ninject)** и поддержкой множественных ORM-провайдеров.

![.NET](https://img.shields.io/badge/.NET-8.0-purple)
![C#](https://img.shields.io/badge/C%23-12.0-blue)
![Architecture](https://img.shields.io/badge/Architecture-MVP%20%2B%20N--Layer-green)
![SOLID](https://img.shields.io/badge/Principles-SOLID-orange)

---

## 📋 Описание проекта

**UrbanGo** - это desktop-приложение для управления каршеринг-сервисом с полноценной бизнес-логикой, поддержкой двух ORM (Entity Framework Core и Dapper), валидацией, логированием и гибкой системой расчета стоимости аренды с промокодами.

### 🎯 Ключевые возможности

- ✅ **MVP-архитектура** - полное разделение Model-View-Presenter для WinForms приложения
- ✅ **Управление автопарком** - добавление, редактирование, удаление и просмотр автомобилей
- ✅ **Система статусов** - отслеживание состояния каждого автомобиля (доступен, арендован, на обслуживании)
- ✅ **Динамическое ценообразование** - стратегии расчета стоимости (стандартная и динамическая с учетом времени суток, дня недели, сезона и праздников)
- ✅ **Промокоды** - применение скидок к аренде
- ✅ **Импорт/Экспорт** - импорт автомобилей из CSV/JSON и экспорт в CSV/JSON (библиотека CsvHelper 33.1.0)
- ✅ **Валидация данных** - проверка корректности вводимой информации и валидация перед импортом
- ✅ **Логирование** - автоматическая запись всех операций в файл
- ✅ **Два интерфейса** - WinForms (MVP) и консольное приложение
- ✅ **Выбор ORM** - Entity Framework Core или Dapper (на выбор пользователя при запуске)

---

## 🏗️ Архитектура проекта

Проект построен по принципу **MVP (Model-View-Presenter) + N-Layer Architecture** с использованием **SOLID принципов** и современных паттернов проектирования.

### 🎭 MVP-архитектура

**MVP (Model-View-Presenter)** - архитектурный паттерн, обеспечивающий полное разделение UI-логики от бизнес-логики.

```
┌──────────────┐      События      ┌──────────────┐
│              │ ──────────────→    │              │
│     View     │                    │  Presenter   │
│  (MainForm)  │ ←──────────────    │ (MainPresenter)│
└──────────────┘  Обновление UI    └──────────────┘
                                           │
                                           │ События
                                           ↓
                                    ┌──────────────┐
                                    │    Model     │
                                    │(CarSharingModel)│
                                    └──────────────┘
                                           │
                                           ↓
                                    ┌──────────────┐
                                    │Business Logic│
                                    │  (Services)  │
                                    └──────────────┘
```

**Компоненты MVP:**

1. **View (IMainView, MainForm)** - отображение данных и генерация событий пользовательских действий
2. **Presenter (MainPresenter, CarEditPresenter, CalculateCostPresenter)** - обработка событий View, вызов бизнес-логики через Model, обновление View
3. **Model (CarSharingModel)** - обертка над бизнес-логикой, генерация событий об изменениях данных
4. **Shared (интерфейсы IView)** - решение проблемы циклических зависимостей между View и Presenter

### 📁 Структура решения

```
AIS/
├── Presenter/                 # 🎯 MVP СЛОЙ - ГЛАВНАЯ ТОЧКА ВХОДА
│   ├── Program.cs             # 🚀 Основная точка входа (запускает WinForms или Console)
│   ├── CarSharingModel.cs     # Model в MVP (обертка над сервисами)
│   ├── MainPresenter.cs       # Presenter главной формы
│   ├── CarEditPresenter.cs    # Presenter формы редактирования
│   └── CalculateCostPresenter.cs # Presenter формы расчета стоимости
│
├── Shared/                    # Интерфейсы View (решение циклических зависимостей)
│   ├── IMainView.cs           # Интерфейс главной формы
│   ├── ICarEditView.cs        # Интерфейс формы редактирования
│   ├── ICalculateCostView.cs  # Интерфейс формы расчета
│   ├── IConsoleView.cs        # Интерфейс консольного меню
│   └── IConfiguration.cs      # Интерфейс конфигурации приложения
│
├── AIS/                       # WinForms VIEW (реализация интерфейсов)
│   ├── Forms/                 # UI формы - реализуют IView интерфейсы
│   │   ├── MainForm.cs        # Implements IMainView
│   │   ├── CarEditForm.cs     # Implements ICarEditView
│   │   ├── CalculateCostForm.cs # Implements ICalculateCostView
│   │   └── CarImportForm.cs   # Форма импорта CSV/JSON
│   ├── Controllers/           # ⚠️ УСТАРЕВШИЕ (до MVP), не используются
│   │   ├── MainFormController.cs
│   │   └── CalculateCostFormController.cs
│   ├── Program.cs             # ⚠️ УСТАРЕВШИЙ (используйте Presenter/Program.cs)
│   ├── AppConfiguration.cs    # Implements IConfiguration
│   └── DependencyContainer.cs # Ninject контейнер для WinForms
│
├── Console/                   # Консольное приложение (не MVP)
│   ├── Program.cs             # Точка входа Console
│   ├── AppConfiguration.cs    # Implements IConfiguration
│   ├── DependencyContainer.cs # Ninject контейнер
│   └── MenuController.cs      # Консольное меню (не следует MVP)
│
├── Model/                     # Доменные модели
│   ├── Car.cs                 # Модель автомобиля
│   ├── CarStatus.cs           # Enum статусов
│   ├── PromoCode.cs           # Модель промокода
│   └── IDomainObject.cs       # Базовый интерфейс сущностей
│
├── DataAccessLayer/           # Слой доступа к данным
│   ├── IRepository.cs         # Интерфейсы репозиториев (CRUD фасад)
│   ├── EntityRepository.cs    # Entity Framework реализация
│   ├── DapperRepository.cs    # Dapper реализация (generic)
│   ├── CarDapperRepository.cs # Dapper для Car (OCP)
│   ├── IPromoCodeRepository.cs# Интерфейс промокодов (ISP - только чтение)
│   ├── EFPromoCodeRepository.cs
│   ├── DapperPromoCodeRepository.cs
│   └── CarSharingContext.cs   # EF DbContext
│
└── BussinessLogic/            # Бизнес-логика
    ├── ICarService.cs         # Интерфейс сервиса автомобилей
    ├── CarService.cs          # Реализация бизнес-логики
    ├── Services/              # Разделенные интерфейсы (ISP)
    │   ├── ICarManagementService.cs  # CRUD + бизнес-операции
    │   ├── ICarQueryService.cs       # Запросы данных
    │   ├── ICarDisplayService.cs     # Форматирование для UI
    │   └── Import/                   # Импорт/Экспорт (CsvHelper)
    │       ├── ICarImportService.cs  # Интерфейс импорт/экспорт
    │       ├── CarImportService.cs   # Реализация (CSV/JSON)
    │       └── Models/
    │           └── ImportResult.cs   # Модель результата импорта
    ├── Dto/                   # Data Transfer Objects
    │   ├── CarDetailsDto.cs
    │   ├── CarListItemDto.cs
    │   └── CarForCalculationDto.cs
    ├── IPromoService.cs       # Интерфейс сервиса промокодов
    ├── PromoService.cs        # Реализация
    ├── PromoServiceAdapter.cs # Адаптер (Adapter Pattern)
    ├── Pricing/               # Стратегии ценообразования (Strategy Pattern)
    │   ├── IPricingStrategy.cs
    │   ├── DefaultPricingStrategy.cs      # Стандартная стратегия (цена × часы)
    │   ├── DynamicPricingStrategy.cs      # Динамическая (время, сезон, праздники)
    │   ├── PricingConfiguration.cs        # Конфигурация множителей
    │   ├── IDiscountPolicy.cs
    │   └── PromoServiceDiscountPolicy.cs
    ├── Validation/            # Валидация данных
    │   ├── ICarValidator.cs
    │   └── CarValidator.cs
    ├── Logging/               # Логирование
    │   ├── ILogger.cs
    │   └── FileLogger.cs
    ├── SimpleConfigModule.cs  # Ninject DI конфигурация
    └── ServiceFactory.cs      # Фабрика сервисов (устаревший, заменен на Ninject)
```

**⚠️ ВАЖНО**: Основная точка входа - [Presenter/Program.cs](Presenter/Program.cs). Файл [AIS/Program.cs](AIS/Program.cs) и папка [AIS/Controllers/](AIS/Controllers/) - устаревшие (до внедрения MVP).

### 💡 CarSharingModel - "M" в MVP

**CarSharingModel** - это "Model" в паттерне MVP, который служит оберткой над бизнес-логикой (сервисами) и предоставляет событийно-ориентированный интерфейс для Presenter-ов.

#### Зачем нужен CarSharingModel?

1. **Централизованное управление данными** - все операции с данными проходят через один класс
2. **Уведомления через события** - Model автоматически оповещает все Presenter-ы об изменениях
3. **Упрощение Presenter-ов** - Presenter не работают напрямую с сервисами, только через Model
4. **Соответствие паттерну MVP** - четкое разделение ответственностей
5. **Легкость тестирования** - Model можно легко замокать

#### Структура CarSharingModel:

```csharp
public class CarSharingModel
{
    private readonly ICarService _carService;
    private readonly IPromoService _promoService;
    private readonly ICarImportService _importService;

    // СОБЫТИЯ для уведомления Presenter-ов
    public event EventHandler<IEnumerable<object>> CarsUpdated;
    public event EventHandler<string> ErrorOccurred;

    // МЕТОДЫ для работы с данными
    public void LoadCars()
    {
        try
        {
            var cars = _carService.GetCarsForDisplay();
            CarsUpdated?.Invoke(this, cars);  // Уведомляем всех подписчиков!
        }
        catch (Exception ex)
        {
            ErrorOccurred?.Invoke(this, ex.Message);
        }
    }

    public void AddCar(string brand, string model, ...)
    {
        try
        {
            _carService.CreateCar(brand, model, ...);
            LoadCars();  // Автоматически обновляем все View
        }
        catch (Exception ex)
        {
            ErrorOccurred?.Invoke(this, ex.Message);
        }
    }
}
```

#### Почему не работать напрямую с сервисами?

**Без CarSharingModel** (плохо):
- Каждый Presenter создает свой экземпляр сервиса
- При изменении данных нужно вручную обновлять все View
- Дублирование кода обработки ошибок
- Сложное тестирование

**С CarSharingModel** (хорошо):
- Один источник данных для всех Presenter-ов
- Автоматическое обновление всех View через события
- Централизованная обработка ошибок
- Легко тестируется через моки

### 🔗 Shared - Решение циклических зависимостей

**Shared** - специальный проект, содержащий интерфейсы View (IMainView, ICarEditView и т.д.), который решает проблему циклических зависимостей между View и Presenter.

#### Проблема без Shared:

```
Presenter (проект Presenter)
   ↓ зависит от
MainForm (проект AIS/WinForms)
   ↓ зависит от
MainPresenter (проект Presenter)
   ⚠️ ЦИКЛИЧЕСКАЯ ЗАВИСИМОСТЬ!
```

#### Решение с Shared:

```
Presenter           AIS/WinForms
   ↓                    ↓
   ↓                    ↓
   ↓──→  Shared  ←─────↓
      (интерфейсы)
```

**Как это работает:**
1. Проект **Shared** содержит только интерфейсы View (IMainView, ICarEditView, etc.)
2. Проект **Presenter** зависит от Shared и работает с интерфейсами
3. Проект **AIS/WinForms** зависит от Shared и реализует интерфейсы
4. **Нет циклических зависимостей!**

```csharp
// Shared/IMainView.cs
public interface IMainView
{
    event EventHandler ViewLoaded;           // View → Presenter
    void DisplayCars(IEnumerable<object> cars); // Presenter → View
}

// AIS/Forms/MainForm.cs (реализация)
public partial class MainForm : Form, IMainView
{
    public event EventHandler ViewLoaded;

    public void DisplayCars(IEnumerable<object> cars)
    {
        dataGridViewCars.DataSource = cars;
    }
}

// Presenter/MainPresenter.cs (использование)
public class MainPresenter
{
    private readonly IMainView _view;  // Работает через интерфейс!

    public MainPresenter(IMainView view, ...)
    {
        _view = view;
        _view.ViewLoaded += OnViewLoaded;
    }
}
```

### 🎨 Архитектурные паттерны

#### 1️⃣ **MVP (Model-View-Presenter)**
Полное разделение UI от бизнес-логики:

**View → Presenter:**
```csharp
// MainForm.cs (View)
public event EventHandler ViewLoaded;
public event EventHandler AddCarRequested;

private void MainForm_Load(object sender, EventArgs e)
{
    ViewLoaded?.Invoke(this, EventArgs.Empty);  // Уведомляем Presenter
}
```

**Presenter обрабатывает:**
```csharp
// MainPresenter.cs
public MainPresenter(IMainView view, CarSharingModel model)
{
    _view = view;
    _model = model;

    // Подписываемся на события View
    _view.ViewLoaded += OnViewLoaded;
    _view.AddCarRequested += OnAddCarRequested;

    // Подписываемся на события Model
    _model.CarsUpdated += OnCarsUpdated;
}

private void OnCarsUpdated(object sender, IEnumerable<object> cars)
{
    _view.DisplayCars(cars);  // Обновляем View
}
```

**Model генерирует события:**
```csharp
// CarSharingModel.cs
public event EventHandler<IEnumerable<object>> CarsUpdated;

public void LoadCars()
{
    var cars = _carService.GetCarsForDisplay();
    CarsUpdated?.Invoke(this, cars);  // Уведомляем всех Presenter-ов
}
```

#### 2️⃣ **N-Layer Architecture**
Четкое разделение на слои:
- **Presenter** - MVP презентеры и модель
- **Shared** - интерфейсы View (решение циклических зависимостей)
- **View (AIS/Forms)** - реализация UI форм
- **Model** - доменные модели
- **DataAccessLayer** - доступ к данным
- **BussinessLogic** - бизнес-правила
- **Console** - консольный интерфейс

#### 3️⃣ **Repository Pattern**
Абстракция доступа к данным через `IRepository<T>`:
```csharp
public interface IRepository<T> : IReadRepository<T>, IWriteRepository<T>
    where T : IDomainObject
{
}
```

#### 4️⃣ **Dependency Injection (Ninject)**
Все зависимости внедряются через конструкторы с использованием IoC-контейнера Ninject:
```csharp
var kernel = new StandardKernel(new SimpleConfigModule(useEF, connectionString, useDynamicPricing));
var carService = kernel.Get<ICarService>();
var model = new CarSharingModel(carService, promoService);
```

#### 5️⃣ **DTO Pattern**
Передача данных между слоями через специализированные объекты:
- `CarDetailsDto` - для детального просмотра
- `CarListItemDto` - для списков
- `CarForCalculationDto` - для расчета стоимости

#### 6️⃣ **Strategy Pattern**
Гибкая система расчета стоимости с двумя стратегиями:
```csharp
// Стандартная стратегия
IPricingStrategy -> DefaultPricingStrategy
// Цена = pricePerHour × hours × (1 - discount)

// Динамическая стратегия
IPricingStrategy -> DynamicPricingStrategy
// Учитывает: время суток, день недели, сезон, праздники, длительность
// multiplier = TimeOfDay × DayOfWeek × Season × Holiday × Duration
// Праздники: x2.0, Ночь: x0.7, Выходные: x1.3, Лето: x1.4
```

#### 7️⃣ **Adapter Pattern**
Адаптация `PromoService` к интерфейсу `IPromoService` без изменения исходного класса.

#### 8️⃣ **Observer Pattern**
События для уведомления об изменениях:
- `CarSharingModel.CarsUpdated` - изменение списка автомобилей
- `CarSharingModel.ErrorOccurred` - возникновение ошибок
- View генерирует события пользовательских действий

---

## 🔧 SOLID принципы

Проект строго следует принципам SOLID:

### ✅ **S - Single Responsibility Principle**
Каждый класс имеет одну ответственность:
- `CarService` - бизнес-логика автомобилей
- `CarValidator` - валидация данных
- `FileLogger` - логирование
- `DefaultPricingStrategy` - расчет цен

### ✅ **O - Open/Closed Principle**
Расширение без модификации:
- `CarDapperRepository` создан вместо изменения `DapperRepository<T>`
- Новые стратегии цен можно добавлять без изменения существующих классов (DefaultPricingStrategy, DynamicPricingStrategy)
- Новые политики скидок через интерфейс `IDiscountPolicy`
- Добавление нового Presenter не требует изменения существующих классов

### ✅ **L - Liskov Substitution Principle**
Все реализации взаимозаменяемы:
- `EntityRepository<Car>` ⇄ `CarDapperRepository`
- `EFPromoCodeRepository` ⇄ `DapperPromoCodeRepository`
- Документация универсальна и применима ко всем реализациям

### ✅ **I - Interface Segregation Principle**
Клиенты зависят только от нужных методов:
- `IRepository<T>` - полный CRUD фасад
- `IReadRepository<T>` - только чтение
- `IWriteRepository<T>` - только запись
- `IPromoCodeRepository : IReadRepository<PromoCode>` - промокоды только для чтения
- `ICarManagementService`, `ICarQueryService`, `ICarDisplayService` - разделенные интерфейсы вместо одного большого `ICarService`

### ✅ **D - Dependency Inversion Principle**
Зависимость от абстракций:
- Presenter зависит от `IMainView`, а не от `MainForm`
- Model зависит от `ICarService`, а не от `CarService`
- `CarService` зависит от `IRepository<Car>`, а не от конкретной реализации
- Все зависимости внедряются через Ninject

---

## 🛠️ Технологии и библиотеки

### Основной стек
- **Язык**: C# 12.0
- **Платформа**: .NET 8.0
- **UI Framework**: Windows Forms (net8.0-windows)
- **База данных**: SQL Server LocalDB
- **ORM**: Entity Framework Core 9.0.10 + Dapper 2.1.66
- **DI Container**: Ninject 3.3.6

### Архитектурные библиотеки
- **Microsoft.EntityFrameworkCore.SqlServer** 9.0.10
- **Microsoft.Data.SqlClient** 5.2.2
- **Dapper** 2.1.66
- **Ninject** 3.3.6
- **CsvHelper** 30.0.1 (импорт/экспорт CSV)
- **System.Text.Json** (встроенная, импорт/экспорт JSON)

### Целевая платформа
- WinForms: `net8.0-windows`
- Console: `net8.0`
- Библиотеки: `net8.0`

---

## 📦 Установка и запуск

### Требования
- **ОС**: Windows 10/11 (для WinForms) или кроссплатформенная ОС для Console
- **.NET SDK**: 8.0 или выше
- **SQL Server LocalDB**: Входит в Visual Studio 2022
- **IDE**: Visual Studio 2022 (рекомендуется)

### Шаг 1: Клонирование репозитория
```bash
git clone <repository-url>
cd AIS
```

### Шаг 2: Восстановление зависимостей
```bash
dotnet restore
```

### Шаг 3: Настройка базы данных

#### Вариант А: Использовать готовую БД
База данных будет создана автоматически при первом запуске (Entity Framework).

#### Вариант Б: Создать вручную
```sql
-- Создание базы данных
CREATE DATABASE UrbanGoDB;
GO

USE UrbanGoDB;
GO

-- Таблица автомобилей
CREATE TABLE Cars (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Brand NVARCHAR(50) NOT NULL,
    Model NVARCHAR(50) NOT NULL,
    LicensePlate NVARCHAR(20) NOT NULL UNIQUE,
    Year INT NOT NULL,
    Mileage INT NOT NULL,
    Status INT NOT NULL DEFAULT 0,
    RentalPricePerHour DECIMAL(10,2) NOT NULL
);

-- Таблица промокодов
CREATE TABLE PromoCodes (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Code NVARCHAR(20) NOT NULL UNIQUE,
    DiscountPercent DECIMAL(5,2) NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1
);

-- Пример данных
INSERT INTO Cars (Brand, Model, LicensePlate, Year, Mileage, Status, RentalPricePerHour)
VALUES
    ('Toyota', 'Camry', 'А123БВ77', 2022, 15000, 0, 500.00),
    ('Kia', 'Rio', 'В456ГД199', 2021, 25000, 0, 350.00),
    ('BMW', 'X5', 'С789ЕЖ50', 2023, 5000, 0, 1200.00);

INSERT INTO PromoCodes (Code, DiscountPercent, IsActive)
VALUES
    ('WELCOME10', 10.00, 1),
    ('SUMMER20', 20.00, 1),
    ('VIP30', 30.00, 1);
```

### Шаг 4: Сборка проекта
```bash
dotnet build --configuration Release
```

### Шаг 5: Запуск приложения

#### 🚀 Запуск через Presenter (РЕКОМЕНДУЕТСЯ)
```bash
dotnet run --project Presenter
```
При запуске выберите режим:
- **1 (W)** - WinForms MVP приложение
- **2 (C)** - Консольное приложение
- **0 (Q)** - Выход

Затем выберите ORM-провайдер:
- **Да/1** - Entity Framework Core
- **Нет/2** - Dapper

Затем выберите стратегию ценообразования:
- **1** - Стандартная (цена × часы)
- **2** - Динамическая (время суток, день недели, сезон, праздники)

#### Запуск WinForms напрямую (устаревший)
```bash
dotnet run --project AIS
```
⚠️ Использует старый код до MVP, рекомендуется использовать [Presenter/Program.cs](Presenter/Program.cs)

#### Запуск консольного приложения напрямую
```bash
dotnet run --project Console
```
В консольном меню выберите провайдер и стратегию ценообразования.

---

## 📊 Структура данных

### Модель Car
```csharp
public class Car : IDomainObject
{
    public int Id { get; set; }
    public string Brand { get; set; }
    public string Model { get; set; }
    public string LicensePlate { get; set; }
    public int Year { get; set; }
    public int Mileage { get; set; }
    public CarStatus Status { get; set; }
    public decimal RentalPricePerHour { get; set; }
}
```

### Enum CarStatus
```csharp
public enum CarStatus
{
    Available = 0,          // Доступен для аренды
    Rented = 1,            // Арендован
    UnderMaintenance = 2   // На техническом обслуживании
}
```

### Модель PromoCode
```csharp
public class PromoCode : IDomainObject
{
    public int Id { get; set; }
    public string Code { get; set; }
    public decimal DiscountPercent { get; set; }
    public bool IsActive { get; set; }
}
```

---

## 🔌 API сервисов

### ICarService - Основной интерфейс
```csharp
// Управление автомобилями (CRUD)
Car CreateCar(string brand, string model, string licensePlate, int year, int mileage, decimal rentalPricePerHour);
Car GetCar(int id);
List<Car> GetAllCars();
bool UpdateCar(Car carToUpdate);
bool UpdateCarDetails(int id, string brand, string model, string plate, int year, int mileage, decimal price, int status);
bool DeleteCar(int id);

// Бизнес-операции
List<Car> GetAvailableCars();
bool RentCar(int carId);
decimal CalculateRentalCost(int carId, int hours, string promoCode = null);

// Получение данных для UI
string GetCarDescription(int carId);
List<string> GetAllCarsDescriptions();
List<string> GetAvailableCarsDescriptions();
object[] GetCarValuesForEdit(int id);

// Получение DTO для UI
CarDetailsDto GetCarForDisplay(int carId);
List<CarListItemDto> GetCarsForDisplay();
CarForCalculationDto GetCarForCalculation(int carId);
```

### IPromoService - Промокоды
```csharp
decimal ApplyPromoCode(string promoCode, decimal originalPrice);
```

### ICarImportService - Импорт/Экспорт
```csharp
// Импорт автомобилей
ImportResult ImportFromCsv(string filePath);
ImportResult ImportFromJson(string filePath);
ImportResult ValidateImportFile(string filePath, ImportFormat format);

// Экспорт автомобилей
int ExportToCsv(string filePath);                              // Все автомобили
int ExportToJson(string filePath);                             // Все автомобили
int ExportToCsv(IEnumerable<int> carIds, string filePath);    // Выбранные
int ExportToJson(IEnumerable<int> carIds, string filePath);   // Выбранные
```

---

## 📝 Логирование

Все операции автоматически логируются в файл `UrbanGo.log` на рабочем столе пользователя:

```
[2025-01-15 14:30:25] CREATE: Id=1, Toyota Camry, Plate=А123БВ77, Year=2022, Mileage=15000, PricePerHour=500.00
[2025-01-15 14:35:10] RENT: Id=1, Toyota Camry, Plate=А123БВ77
[2025-01-15 14:40:05] UPDATE: Id=1, Toyota Camry, Plate=А123БВ77, Year=2022, Mileage=16000, Status=Rented, PricePerHour=500.00
[2025-01-15 14:45:00] DELETE: Id=2, Kia Rio, Plate=В456ГД199
[2025-01-15 14:50:30] IMPORT: Импортирован BMW X5 (С789ЕЖ50)
[2025-01-15 14:55:15] EXPORT CSV: Экспортировано 5 автомобилей в C:\Users\...\cars_export.csv
```

Логирование реализовано через:
- `ILogger` - интерфейс
- `FileLogger` - реализация с потокобезопасной записью

---

## 🎯 Примеры использования

### Пример 1: Создание автомобиля через консоль
```csharp
var car = carService.CreateCar(
    brand: "Toyota",
    model: "Camry",
    licensePlate: "А123БВ77",
    year: 2022,
    mileage: 15000,
    rentalPricePerHour: 500.00m
);
Console.WriteLine($"Создан автомобиль: {car.Brand} {car.Model}");
```

### Пример 2: Расчет стоимости с промокодом
```csharp
// Без промокода
decimal cost1 = carService.CalculateRentalCost(carId: 1, hours: 5);
// cost1 = 2500.00

// С промокодом WELCOME10 (скидка 10%)
decimal cost2 = carService.CalculateRentalCost(carId: 1, hours: 5, promoCode: "WELCOME10");
// cost2 = 2250.00
```

### Пример 3: Получение DTO для UI
```csharp
// Для списка автомобилей
var carList = carService.GetCarsForDisplay();
dataGridView.DataSource = carList;

// Для формы расчета
var carForCalc = carService.GetCarForCalculation(carId);
labelCarInfo.Text = carForCalc.DisplayText;
```

### Пример 4: Импорт автомобилей из CSV
```csharp
var importService = dependencyContainer.ImportService;

// Валидация перед импортом
var validationResult = importService.ValidateImportFile("cars.csv", ImportFormat.Csv);
if (validationResult.FailedRecords > 0)
{
    Console.WriteLine($"Найдено ошибок: {validationResult.FailedRecords}");
    foreach (var error in validationResult.Errors)
    {
        Console.WriteLine($"Строка {error.LineNumber}: {error.ErrorMessage}");
    }
}

// Реальный импорт
var importResult = importService.ImportFromCsv("cars.csv");
Console.WriteLine($"Импортировано: {importResult.SuccessfulImports}");
Console.WriteLine($"Пропущено (дубликаты): {importResult.SkippedRecords}");
```

### Пример 5: Экспорт выбранных автомобилей
```csharp
var importService = dependencyContainer.ImportService;

// Экспорт выбранных автомобилей в CSV
var selectedIds = new List<int> { 1, 3, 5, 7 };
int count = importService.ExportToCsv(selectedIds, "selected_cars.csv");
Console.WriteLine($"Экспортировано {count} автомобилей");

// Экспорт всех автомобилей в JSON
int totalCount = importService.ExportToJson("all_cars.json");
Console.WriteLine($"Экспортировано {totalCount} автомобилей");
```

---

## 🔒 Валидация данных

Все входные данные проходят валидацию через `ICarValidator`:

```csharp
// Валидация при создании
ValidateForCreate(brand, model, licensePlate, year, mileage, rentalPricePerHour);

// Валидация при обновлении
ValidateForUpdate(brand, model, licensePlate, year, mileage, rentalPricePerHour, status);
```

**Правила валидации:**
- Марка, модель, гос. номер - не пустые
- Год - от 1900 до текущего года + 1
- Пробег - неотрицательное число
- Цена - положительное число
- Статус - от 0 до 2

---

## 🔄 Dependency Injection (Ninject)

Проект использует **Ninject** для управления зависимостями.

### Конфигурация SimpleConfigModule.cs
```csharp
public class SimpleConfigModule : NinjectModule
{
    private readonly bool _useEF;
    private readonly string _connectionString;
    private readonly bool _useDynamicPricing;

    public SimpleConfigModule(bool useEF, string connectionString, bool useDynamicPricing = false)
    {
        _useEF = useEF;
        _connectionString = connectionString;
        _useDynamicPricing = useDynamicPricing;
    }

    public override void Load()
    {
        // Логгер
        Bind<ILogger>().To<FileLogger>().InSingletonScope();

        // Стратегии ценообразования (выбор стратегии)
        if (_useDynamicPricing)
        {
            Bind<IPricingStrategy>().To<DynamicPricingStrategy>().InSingletonScope();
        }
        else
        {
            Bind<IPricingStrategy>().To<DefaultPricingStrategy>().InSingletonScope();
        }

        Bind<IDiscountPolicy>().To<PromoServiceDiscountPolicy>().InSingletonScope();

        // Валидаторы
        Bind<ICarValidator>().To<CarValidator>().InSingletonScope();

        // Репозитории (в зависимости от выбора ORM)
        if (_useEF) {
            Bind<IRepository<Car>>().To<EntityRepository<Car>>().InSingletonScope();
            Bind<IPromoCodeRepository>().To<EFPromoCodeRepository>().InSingletonScope();
        } else {
            Bind<IRepository<Car>>().To<CarDapperRepository>().InSingletonScope();
            Bind<IPromoCodeRepository>().To<DapperPromoCodeRepository>().InSingletonScope();
        }

        // Сервисы
        Bind<ICarService>().To<CarService>().InSingletonScope();
        Bind<IPromoService>().To<PromoServiceAdapter>().InSingletonScope();
        Bind<ICarImportService>().To<CarImportService>().InSingletonScope();
    }
}
```

### Использование в MVP
```csharp
// Presenter/Program.cs
var kernel = new StandardKernel(
    new SimpleConfigModule(useEF, connectionString, useDynamicPricing)
);

var carService = kernel.Get<ICarService>();
var promoService = kernel.Get<IPromoService>();
var importService = kernel.Get<ICarImportService>();

// Создаем Model в MVP
var model = new CarSharingModel(carService, promoService, importService);

// Создаем View
var mainForm = new MainForm();

// Создаем Presenter (связывает View и Model)
var presenter = new MainPresenter(mainForm, model, carService, promoService, importService);

// Запускаем приложение
Application.Run(mainForm);
```

---

## 🧪 Тестирование

### Ручное тестирование через UI
1. Запустите WinForms приложение через [Presenter/Program.cs](Presenter/Program.cs)
2. Выберите режим WinForms (1 или W)
3. Выберите ORM (EF или Dapper)
4. Выберите стратегию ценообразования (стандартная или динамическая)
5. Добавьте несколько автомобилей
6. Проверьте редактирование через форму CarEditForm
7. Попробуйте арендовать автомобиль
8. Рассчитайте стоимость с промокодом (проверьте разницу между стратегиями)
9. Импортируйте/экспортируйте автомобили из CSV/JSON
10. Проверьте файл логов на рабочем столе

### Проверка смены ORM
1. Запустите приложение через Presenter с EF
2. Добавьте автомобиль
3. Закройте приложение
4. Запустите с Dapper
5. Убедитесь, что данные сохранились

### Проверка MVP архитектуры
1. Убедитесь, что MainForm не содержит бизнес-логики (только генерация событий)
2. Проверьте, что MainPresenter обрабатывает все события View
3. Убедитесь, что при добавлении автомобиля через одну форму, список обновляется автоматически
4. Проверьте, что ошибки отображаются через события Model → Presenter → View

---

## 📚 Документация кода

Весь код полностью задокументирован с использованием XML-комментариев:

```csharp
/// <summary>
/// Создает новый автомобиль и добавляет его в систему.
/// </summary>
/// <param name="brand">Марка автомобиля.</param>
/// <param name="model">Модель автомобиля.</param>
/// <param name="licensePlate">Государственный номер.</param>
/// <param name="year">Год выпуска.</param>
/// <param name="mileage">Текущий пробег.</param>
/// <param name="rentalPricePerHour">Стоимость аренды за час.</param>
/// <returns>Созданный объект автомобиля.</returns>
/// <exception cref="ArgumentException">Если данные невалидны.</exception>
public Car CreateCar(string brand, string model, string licensePlate,
    int year, int mileage, decimal rentalPricePerHour);
```

**Документация соответствует критериям:**
- ✅ Все публичные элементы имеют `<summary>`
- ✅ Все параметры описаны через `<param>`
- ✅ Все возвращаемые значения описаны через `<returns>`
- ✅ Документация универсальна (LSP) - применима ко всем реализациям

---

## 🚀 Расширение проекта

### Добавление новой стратегии ценообразования
Уже реализованы две стратегии:
1. **DefaultPricingStrategy** - стандартная (цена × часы)
2. **DynamicPricingStrategy** - динамическая (время, сезон, праздники)

Пример добавления третьей стратегии:
```csharp
// BussinessLogic/Pricing/WeekendPricingStrategy.cs
public class WeekendPricingStrategy : IPricingStrategy
{
    public decimal CalculateBasePrice(decimal pricePerHour, int hours)
    {
        // Повышенная цена в выходные
        var isWeekend = DateTime.Now.DayOfWeek == DayOfWeek.Saturday
                     || DateTime.Now.DayOfWeek == DayOfWeek.Sunday;
        var multiplier = isWeekend ? 1.5m : 1.0m;
        return pricePerHour * hours * multiplier;
    }
}

// В SimpleConfigModule.cs добавить условие
if (useWeekendPricing)
{
    Bind<IPricingStrategy>().To<WeekendPricingStrategy>().InSingletonScope();
}
```

### Добавление нового провайдера данных
```csharp
public class MongoCarRepository : IRepository<Car>
{
    // Реализация для MongoDB
}

// В SimpleConfigModule.cs
if (useMongo) {
    Bind<IRepository<Car>>().To<MongoCarRepository>().InSingletonScope();
}
```

### Добавление нового формата импорта (XML, Excel)
```csharp
// В ICarImportService добавить метод
ImportResult ImportFromXml(string filePath);
ImportResult ImportFromExcel(string filePath);

// В CarImportService реализовать
public ImportResult ImportFromXml(string filePath)
{
    var result = new ImportResult();
    var xmlDoc = XDocument.Load(filePath);
    var carElements = xmlDoc.Descendants("Car");

    foreach (var element in carElements)
    {
        // Парсинг XML и валидация
        // Добавление в БД
    }

    return result;
}
```

---

## 🐛 Известные ограничения

1. **Только Windows Forms** - WinForms работает только на Windows
2. **LocalDB** - требуется SQL Server LocalDB
3. **Однопользовательский режим** - нет поддержки многопользовательского доступа
4. **Транзакции** - не используются сложные транзакции БД
5. **Консольное приложение не следует MVP** - Console/MenuController не использует MVP паттерн (только WinForms использует MVP)
6. **Устаревший код в AIS/Controllers** - папка Controllers и AIS/Program.cs сохранены для истории, но не используются в текущей версии

---

## 👨‍💻 Автор и лицензия

**Автор**: Разработано в рамках учебного проекта по дисциплине "Архитектура информационных систем"

**Цель проекта**: Демонстрация применения принципов SOLID, паттернов проектирования (MVP, Repository, Strategy, Adapter, Observer) и современных архитектурных подходов в .NET

**Основные достижения**:
- ✅ Полная реализация MVP-архитектуры для WinForms
- ✅ Решение проблемы циклических зависимостей через проект Shared
- ✅ Динамическое ценообразование с использованием Strategy Pattern
- ✅ Поддержка двух ORM (Entity Framework Core и Dapper) с возможностью переключения
- ✅ Полное соблюдение принципов SOLID
- ✅ Импорт/Экспорт данных в CSV/JSON
- ✅ Комплексная валидация и логирование

**Год**: 2025

---

## 📞 Контакты и поддержка

Для вопросов и предложений создайте Issue в репозитории проекта.

---

**Версия проекта**: 4.0 (MVP-архитектура)
**Дата обновления**: Декабрь 2025 (реализован MVP паттерн, добавлено динамическое ценообразование)
