# UrbanGo - Система управления каршерингом

Современная система управления автомобилями для каршеринга, разработанная на C# с использованием Windows Forms и поддержкой множественных провайдеров данных.

## 📋 Описание проекта

UrbanGo - это desktop-приложение для управления автопарком каршеринга с поддержкой Entity Framework и Dapper. Система позволяет:
- Управлять базой данных автомобилей через SQL Server LocalDB
- Выбирать провайдер данных (Entity Framework или Dapper)
- Отслеживать статус каждого автомобиля (доступен, арендован, на обслуживании)
- Рассчитывать стоимость аренды
- Логировать все операции в файл
- Работать как через WinForms, так и через консольный интерфейс

## 🏗️ Архитектура проекта

Проект построен по принципу многослойной архитектуры (N-Layer Architecture) с использованием паттерна Repository:

### Структура решения

```
AIS/
├── Model/              # Слой данных (модели и интерфейсы)
├── BussinessLogic/     # Слой бизнес-логики
├── DataAccessLayer/    # Слой доступа к данным
├── AIS/               # Слой представления (WinForms)
└── Console/           # Консольное приложение
```

### Компоненты

- **Model** - Содержит модели данных (`Car`, `CarStatus`, `IDomainObject`)
- **BussinessLogic** - Бизнес-логика и сервисы (`ICarService`, `CarService`)
- **DataAccessLayer** - Слой доступа к данным с поддержкой EF и Dapper
- **AIS** - WinForms интерфейс пользователя
- **Console** - Консольная версия приложения

## 🚗 Функциональность

### Основные возможности

- ✅ **Управление автомобилями**
  - Добавление новых автомобилей
  - Редактирование информации об автомобилях
  - Удаление автомобилей из системы
  - Просмотр списка всех автомобилей

- ✅ **Управление статусами**
  - Доступен для аренды
  - Арендован
  - На техническом обслуживании

- ✅ **Расчет стоимости**
  - Расчет стоимости аренды по часам
  - Отображение цен за час для каждого автомобиля

- ✅ **Логирование**
  - Автоматическое логирование всех операций
  - Файл логов сохраняется на рабочем столе пользователя

### Интерфейс

- **Главная форма** - отображение списка автомобилей с возможностью поиска и фильтрации
- **Форма редактирования** - добавление и редактирование данных автомобилей
- **Форма расчета стоимости** - расчет стоимости аренды

## 🛠️ Технологии

- **Язык**: C# (.NET 8.0)
- **UI Framework**: Windows Forms
- **База данных**: SQL Server LocalDB
- **ORM**: Entity Framework Core + Dapper
- **Целевая платформа**: .NET 8.0 (кроссплатформенная)
- **Архитектурный паттерн**: N-Layer Architecture
- **Паттерн проектирования**: Repository Pattern + Dependency Injection
- **Строка подключения**: `Server=(localdb)\\mssqllocaldb;Database=UrbanGoDB;Trusted_Connection=true;TrustServerCertificate=true;`

## 📦 Установка и запуск

### Требования

- **Windows**: Windows 10/11 или Windows Server 2012 R2+
- **.NET Runtime**: .NET 8.0
- **SQL Server**: SQL Server LocalDB (входит в состав Visual Studio)
- **Visual Studio**: 2022 или новее (рекомендуется)

### Запуск

1. Клонируйте репозиторий:
```bash
git clone <repository-url>
```

2. Откройте решение в Visual Studio:
```
AIS.sln
```

3. Соберите решение (Build → Build Solution)

4. Запустите приложение:
   - Для WinForms: запустите проект `AIS` (выберите провайдер данных при запуске)
   - Для консольной версии: запустите проект `Console` (выберите провайдер данных в меню)

### Альтернативный запуск через CLI

```bash
# Переход в директорию проекта
cd AIS

# Восстановление зависимостей
dotnet restore

# Сборка проекта
dotnet build

# Запуск WinForms приложения
dotnet run --project AIS

# Запуск консольного приложения
dotnet run --project Console
```

### Исполняемые файлы

После сборки исполняемые файлы будут находиться в:
- `AIS/bin/Debug/net8.0/AIS.dll` - Windows Forms приложение
- `Console/bin/Debug/net8.0/Console.dll` - Консольное приложение

## 📁 Структура данных

### Модель Car

```csharp
public class Car : IDomainObject
{
    public int Id { get; set; }                    // Уникальный ID
    public string Brand { get; set; }              // Марка (Toyota, Kia)
    public string Model { get; set; }              // Модель (Camry, Rio)
    public string LicensePlate { get; set; }       // Гос. номер
    public int Year { get; set; }                  // Год выпуска
    public int Mileage { get; set; }               // Пробег (км)
    public CarStatus Status { get; set; }          // Статус
    public decimal RentalPricePerHour { get; set; } // Цена за час
}
```

### Интерфейс IDomainObject

```csharp
public interface IDomainObject
{
    int Id { get; set; }
}
```

### Статусы автомобилей

```csharp
public enum CarStatus
{
    Available,          // Доступен
    Rented,            // Арендован
    UnderMaintenance   // На обслуживании
}
```

## 🔧 API сервиса

### Основные методы ICarService

- `CreateCar()` - создание нового автомобиля
- `GetCar(int id)` - получение автомобиля по ID
- `GetAllCars()` - получение всех автомобилей
- `UpdateCar(Car car)` - обновление данных автомобиля
- `DeleteCar(int id)` - удаление автомобиля
- `GetAvailableCars()` - получение доступных автомобилей
- `RentCar(int id)` - аренда автомобиля
- `CalculateRentalCost(int id, int hours)` - расчет стоимости

### Слой доступа к данным

#### Интерфейс IRepository<T>

```csharp
public interface IRepository<T> where T : IDomainObject
{
    void Add(T entity);
    void Delete(int id);
    IEnumerable<T> ReadAll();
    T ReadById(int id);
    void Update(T entity);
}
```

#### Реализации репозитория

- **EntityRepository<T>** - реализация через Entity Framework Core
- **DapperRepository<T>** - реализация через Dapper ORM
- **CarSharingContext** - DbContext для Entity Framework

## 📝 Логирование

Все операции автоматически логируются в файл `actions.log` на рабочем столе пользователя. Формат записи:

```
[2024-01-15 14:30:25] Создан автомобиль: Toyota Camry (А123БВ77)
[2024-01-15 14:35:10] Автомобиль ID=1 арендован
```

## 🗄️ База данных

### Настройка SQL Server LocalDB

1. Убедитесь, что SQL Server LocalDB установлен (входит в состав Visual Studio)
2. Создайте базу данных `UrbanGoDB` в LocalDB
3. Создайте таблицу `Cars`:

```sql
CREATE TABLE Cars (
    Id int IDENTITY(1,1) PRIMARY KEY,
    Brand nvarchar(50) NOT NULL,
    Model nvarchar(50) NOT NULL,
    LicensePlate nvarchar(20) NOT NULL UNIQUE,
    Year int NOT NULL,
    Mileage int NOT NULL,
    Status int NOT NULL,
    RentalPricePerHour decimal(10,2) NOT NULL
);
```

### Выбор провайдера данных

При запуске приложения пользователь может выбрать:
- **Entity Framework** - для работы через ORM с автоматическим отслеживанием изменений
- **Dapper** - для работы через прямые SQL-запросы с высокой производительностью

## 🔄 Архитектурные улучшения

Проект был модернизирован с добавлением современных паттернов:

- ✅ **Repository Pattern** - абстракция доступа к данным
- ✅ **Dependency Injection** - внедрение зависимостей
- ✅ **Interface Segregation** - разделение интерфейсов
- ✅ **Entity Framework Core** - современный ORM
- ✅ **Dapper** - микро-ORM для производительности
- ✅ **Множественные провайдеры данных** - гибкость выбора

## 👨‍💻 Автор

Разработано в рамках учебного проекта по дисциплине "Архитектура информационных систем".

---


