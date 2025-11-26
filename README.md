# 🚗 UrbanGo - Система управления каршерингом

Современная система управления автопарком каршеринга, построенная по принципам **SOLID** с использованием **многослойной архитектуры**, **Dependency Injection (Ninject)** и поддержкой множественных ORM-провайдеров.

![.NET](https://img.shields.io/badge/.NET-8.0-purple)
![C#](https://img.shields.io/badge/C%23-12.0-blue)
![Architecture](https://img.shields.io/badge/Architecture-N--Layer-green)
![SOLID](https://img.shields.io/badge/Principles-SOLID-orange)

---

## 📋 Описание проекта

**UrbanGo** - это desktop-приложение для управления каршеринг-сервисом с полноценной бизнес-логикой, поддержкой двух ORM (Entity Framework Core и Dapper), валидацией, логированием и гибкой системой расчета стоимости аренды с промокодами.

### 🎯 Ключевые возможности

- ✅ **Управление автопарком** - добавление, редактирование, удаление и просмотр автомобилей
- ✅ **Система статусов** - отслеживание состояния каждого автомобиля (доступен, арендован, на обслуживании)
- ✅ **Расчет стоимости** - гибкая система ценообразования с поддержкой промокодов
- ✅ **Промокоды** - применение скидок к аренде
- ✅ **Импорт/Экспорт** - импорт автомобилей из CSV/JSON и экспорт в CSV/JSON (библиотека CsvHelper 30.0.1)
- ✅ **Валидация данных** - проверка корректности вводимой информации и валидация перед импортом
- ✅ **Логирование** - автоматическая запись всех операций в файл
- ✅ **Два интерфейса** - WinForms и консольное приложение
- ✅ **Выбор ORM** - Entity Framework Core или Dapper (на выбор пользователя при запуске)

---

## 🏗️ Архитектура проекта

Проект построен по принципу **N-Layer Architecture** с использованием **SOLID принципов** и современных паттернов проектирования.

### 📁 Структура решения

```
AIS/
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
├── BussinessLogic/            # Бизнес-логика
│   ├── ICarService.cs         # Интерфейс сервиса автомобилей
│   ├── CarService.cs          # Реализация бизнес-логики
│   ├── Services/              # Разделенные интерфейсы (ISP)
│   │   ├── ICarManagementService.cs  # CRUD + бизнес-операции
│   │   ├── ICarQueryService.cs       # Запросы данных
│   │   ├── ICarDisplayService.cs     # Форматирование для UI
│   │   └── Import/                   # Импорт/Экспорт (CsvHelper)
│   │       ├── ICarImportService.cs  # Интерфейс импорт/экспорт
│   │       ├── CarImportService.cs   # Реализация (CSV/JSON)
│   │       └── Models/
│   │           └── ImportResult.cs   # Модель результата импорта
│   ├── Dto/                   # Data Transfer Objects
│   │   ├── CarDetailsDto.cs
│   │   ├── CarListItemDto.cs
│   │   └── CarForCalculationDto.cs
│   ├── IPromoService.cs       # Интерфейс сервиса промокодов
│   ├── PromoService.cs        # Реализация
│   ├── PromoServiceAdapter.cs # Адаптер (Adapter Pattern)
│   ├── Pricing/               # Стратегии ценообразования (Strategy Pattern)
│   │   ├── IPricingStrategy.cs
│   │   ├── DefaultPricingStrategy.cs
│   │   ├── IDiscountPolicy.cs
│   │   └── PromoServiceDiscountPolicy.cs
│   ├── Validation/            # Валидация данных
│   │   ├── ICarValidator.cs
│   │   └── CarValidator.cs
│   ├── Logging/               # Логирование
│   │   ├── ILogger.cs
│   │   └── FileLogger.cs
│   ├── SimpleConfigModule.cs  # Ninject DI конфигурация
│   └── ServiceFactory.cs      # Фабрика сервисов (устаревший, заменен на Ninject)
│
├── AIS/                       # WinForms приложение
│   ├── Program.cs             # Точка входа WinForms
│   ├── AppConfiguration.cs    # Конфигурация приложения
│   ├── DependencyContainer.cs # Ninject контейнер
│   ├── Controllers/           # Контроллеры для форм
│   │   ├── MainFormController.cs
│   │   └── CalculateCostFormController.cs
│   └── Forms/                 # UI формы
│       ├── MainForm.cs        # Главная форма (+ кнопки Импорт/Экспорт)
│       ├── CarEditForm.cs
│       ├── CalculateCostForm.cs
│       └── CarImportForm.cs   # Форма импорта CSV/JSON
│
└── Console/                   # Консольное приложение
    ├── Program.cs             # Точка входа Console
    ├── IConfiguration.cs      # Интерфейс конфигурации
    ├── AppConfiguration.cs    # Конфигурация
    ├── DependencyContainer.cs # Ninject контейнер
    └── MenuController.cs      # Консольное меню
```

### 🎨 Архитектурные паттерны

#### 1️⃣ **N-Layer Architecture**
Четкое разделение на слои:
- **Model** - доменные модели
- **DataAccessLayer** - доступ к данным
- **BussinessLogic** - бизнес-правила
- **Presentation** - UI (WinForms + Console)

#### 2️⃣ **Repository Pattern**
Абстракция доступа к данным через `IRepository<T>`:
```csharp
public interface IRepository<T> : IReadRepository<T>, IWriteRepository<T>
    where T : IDomainObject
{
}
```

#### 3️⃣ **Dependency Injection (Ninject)**
Все зависимости внедряются через конструкторы с использованием IoC-контейнера Ninject:
```csharp
var kernel = new StandardKernel(new SimpleConfigModule(useEF, connectionString));
var carService = kernel.Get<ICarService>();
```

#### 4️⃣ **DTO Pattern**
Передача данных между слоями через специализированные объекты:
- `CarDetailsDto` - для детального просмотра
- `CarListItemDto` - для списков
- `CarForCalculationDto` - для расчета стоимости

#### 5️⃣ **Strategy Pattern**
Гибкая система расчета стоимости:
```csharp
IPricingStrategy -> DefaultPricingStrategy
IDiscountPolicy -> PromoServiceDiscountPolicy
```

#### 6️⃣ **Adapter Pattern**
Адаптация `PromoService` к интерфейсу `IPromoService` без изменения исходного класса.

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
- Новые стратегии цен можно добавлять без изменения существующих классов
- Новые политики скидок через интерфейс `IDiscountPolicy`

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
- UI зависит от `ICarService`, а не от `CarService`
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

#### Запуск WinForms приложения
```bash
dotnet run --project AIS
```
При запуске выберите провайдер данных:
- **Да** - Entity Framework Core
- **Нет** - Dapper
- **Отмена** - Выход

#### Запуск консольного приложения
```bash
dotnet run --project Console
```
В консольном меню выберите провайдер:
- **1** - Entity Framework
- **2** - Dapper
- **0** - Выход

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
    public override void Load()
    {
        // Логгер
        Bind<ILogger>().To<FileLogger>().InSingletonScope();

        // Стратегии
        Bind<IPricingStrategy>().To<DefaultPricingStrategy>().InSingletonScope();
        Bind<IDiscountPolicy>().To<PromoServiceDiscountPolicy>().InSingletonScope();

        // Валидаторы
        Bind<ICarValidator>().To<CarValidator>().InSingletonScope();

        // Репозитории (в зависимости от выбора ORM)
        if (useEF) {
            Bind<IRepository<Car>>().To<EntityRepository<Car>>().InSingletonScope();
        } else {
            Bind<IRepository<Car>>().To<CarDapperRepository>().InSingletonScope();
        }

        // Сервисы
        Bind<ICarService>().To<CarService>().InSingletonScope();
        Bind<IPromoService>().To<PromoServiceAdapter>().InSingletonScope();
    }
}
```

### Использование в UI
```csharp
// Program.cs
var kernel = new StandardKernel(new SimpleConfigModule(useEF, connectionString));
var carService = kernel.Get<ICarService>();
var promoService = kernel.Get<IPromoService>();
```

---

## 🧪 Тестирование

### Ручное тестирование через UI
1. Запустите WinForms приложение
2. Добавьте несколько автомобилей
3. Проверьте редактирование
4. Попробуйте арендовать автомобиль
5. Рассчитайте стоимость с промокодом
6. Проверьте файл логов на рабочем столе

### Проверка смены ORM
1. Запустите приложение с EF
2. Добавьте автомобиль
3. Закройте приложение
4. Запустите с Dapper
5. Убедитесь, что данные сохранились

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
```csharp
public class WeekendPricingStrategy : IPricingStrategy
{
    public decimal CalculateBasePrice(decimal pricePerHour, int hours)
    {
        // Повышенная цена в выходные
        return pricePerHour * hours * 1.5m;
    }
}

// В SimpleConfigModule.cs
Bind<IPricingStrategy>().To<WeekendPricingStrategy>().InSingletonScope();
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

---

## 👨‍💻 Автор и лицензия

**Автор**: Разработано в рамках учебного проекта по дисциплине "Архитектура информационных систем"

**Цель проекта**: Демонстрация применения принципов SOLID, паттернов проектирования и современных архитектурных подходов в .NET

**Год**: 2025

---

## 📞 Контакты и поддержка

Для вопросов и предложений создайте Issue в репозитории проекта.

---


**Версия документации**: 3.0
**Дата обновления**: Январь 2025 (добавлен функционал импорт/экспорт)
