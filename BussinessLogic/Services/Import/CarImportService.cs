using System.Text.Json;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using BussinessLogic.Validation;
using BussinessLogic.Services.Import.Models;
using BussinessLogic.Logging;
using Model;
using DataAccessLayer;

namespace BussinessLogic.Services.Import
{
    /// <summary>
    /// Сервис для импорта автомобилей из внешних источников (CSV, JSON).
    /// Реализует валидацию данных и обработку дубликатов по государственному номеру.
    /// </summary>
    public class CarImportService : ICarImportService
    {
        private readonly IRepository<Car> _repository;
        private readonly ICarValidator _validator;
        private readonly ILogger _logger;

        /// <summary>
        /// Инициализирует новый экземпляр сервиса импорта.
        /// </summary>
        /// <param name="repository">Репозиторий для доступа к данным автомобилей.</param>
        /// <param name="validator">Валидатор для проверки корректности данных автомобилей.</param>
        /// <param name="logger">Логгер для записи операций импорта.</param>
        public CarImportService(
            IRepository<Car> repository,
            ICarValidator validator,
            ILogger logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Импортирует автомобили из CSV файла.
        /// </summary>
        /// <param name="filePath">Полный путь к CSV файлу.</param>
        /// <returns>Результат импорта с детальной информацией.</returns>
        public ImportResult ImportFromCsv(string filePath)
        {
            _logger.Log($"Начало импорта из CSV: {filePath}");

            var result = new ImportResult();

            try
            {
                if (!File.Exists(filePath))
                    throw new FileNotFoundException($"Файл не найден: {filePath}");

                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = true,
                    Delimiter = ";", // Точка с запятой для совместимости с русской Excel
                    MissingFieldFound = null,
                    BadDataFound = null
                };

                using var reader = new StreamReader(filePath);
                using var csv = new CsvReader(reader, config);

                csv.Context.RegisterClassMap<CarCsvMap>();

                var records = csv.GetRecords<CarImportDto>().ToList();
                result.TotalRecords = records.Count;

                _logger.Log($"Прочитано записей из CSV: {result.TotalRecords}");

                // Получаем существующие госномера для проверки дубликатов
                var existingPlates = _repository.ReadAll()
                    .Select(c => c.LicensePlate.ToUpperInvariant())
                    .ToHashSet();

                int lineNumber = 2; // Начинаем с 2 (строка 1 - заголовок)

                foreach (var record in records)
                {
                    try
                    {
                        // Проверка на дубликат по госномеру
                        if (existingPlates.Contains(record.LicensePlate.ToUpperInvariant()))
                        {
                            result.SkippedRecords++;
                            result.Errors.Add(new ImportError
                            {
                                LineNumber = lineNumber,
                                ErrorMessage = $"Автомобиль с госномером '{record.LicensePlate}' уже существует в системе",
                                RawData = $"{record.Brand} {record.Model}"
                            });
                            lineNumber++;
                            continue;
                        }

                        // Валидация данных через существующий валидатор
                        _validator.ValidateForUpdate(
                            record.Brand,
                            record.Model,
                            record.LicensePlate,
                            record.Year,
                            record.Mileage,
                            record.RentalPricePerHour,
                            (int)record.Status
                        );

                        // Создание и сохранение автомобиля
                        var car = new Car
                        {
                            Brand = record.Brand.Trim(),
                            Model = record.Model.Trim(),
                            LicensePlate = record.LicensePlate.Trim(),
                            Year = record.Year,
                            Mileage = record.Mileage,
                            Status = record.Status,
                            RentalPricePerHour = record.RentalPricePerHour
                        };

                        _repository.Add(car);

                        result.SuccessfulImports++;
                        existingPlates.Add(record.LicensePlate.ToUpperInvariant());

                        _logger.Log($"IMPORT: Импортирован {car.Brand} {car.Model} ({car.LicensePlate})");
                    }
                    catch (Exception ex)
                    {
                        result.FailedRecords++;
                        result.Errors.Add(new ImportError
                        {
                            LineNumber = lineNumber,
                            ErrorMessage = ex.Message,
                            RawData = $"{record.Brand} {record.Model} {record.LicensePlate}"
                        });

                        _logger.Log($"IMPORT ERROR: Строка {lineNumber}: {ex.Message}");
                    }

                    lineNumber++;
                }

                _logger.Log($"Импорт завершен. Успешно: {result.SuccessfulImports}, Пропущено: {result.SkippedRecords}, Ошибок: {result.FailedRecords}");
            }
            catch (Exception ex)
            {
                _logger.Log($"IMPORT CRITICAL ERROR: {ex.Message}");
                result.Errors.Add(new ImportError
                {
                    LineNumber = 0,
                    ErrorMessage = $"Критическая ошибка чтения файла: {ex.Message}",
                    RawData = ""
                });
            }

            return result;
        }

        /// <summary>
        /// Импортирует автомобили из JSON файла.
        /// </summary>
        /// <param name="filePath">Полный путь к JSON файлу.</param>
        /// <returns>Результат импорта с детальной информацией.</returns>
        public ImportResult ImportFromJson(string filePath)
        {
            _logger.Log($"Начало импорта из JSON: {filePath}");

            var result = new ImportResult();

            try
            {
                if (!File.Exists(filePath))
                    throw new FileNotFoundException($"Файл не найден: {filePath}");

                var jsonString = File.ReadAllText(filePath);
                var records = JsonSerializer.Deserialize<List<CarImportDto>>(jsonString, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (records == null || records.Count == 0)
                {
                    result.Errors.Add(new ImportError
                    {
                        LineNumber = 0,
                        ErrorMessage = "Файл не содержит данных или имеет неверный формат JSON",
                        RawData = ""
                    });
                    return result;
                }

                result.TotalRecords = records.Count;
                _logger.Log($"Прочитано записей из JSON: {result.TotalRecords}");

                // Получаем существующие госномера
                var existingPlates = _repository.ReadAll()
                    .Select(c => c.LicensePlate.ToUpperInvariant())
                    .ToHashSet();

                int lineNumber = 1;

                foreach (var record in records)
                {
                    try
                    {
                        // Проверка на дубликат
                        if (existingPlates.Contains(record.LicensePlate.ToUpperInvariant()))
                        {
                            result.SkippedRecords++;
                            result.Errors.Add(new ImportError
                            {
                                LineNumber = lineNumber,
                                ErrorMessage = $"Автомобиль с госномером '{record.LicensePlate}' уже существует в системе",
                                RawData = JsonSerializer.Serialize(record)
                            });
                            lineNumber++;
                            continue;
                        }

                        // Валидация через существующий валидатор
                        _validator.ValidateForUpdate(
                            record.Brand,
                            record.Model,
                            record.LicensePlate,
                            record.Year,
                            record.Mileage,
                            record.RentalPricePerHour,
                            (int)record.Status
                        );

                        // Создание и сохранение
                        var car = new Car
                        {
                            Brand = record.Brand.Trim(),
                            Model = record.Model.Trim(),
                            LicensePlate = record.LicensePlate.Trim(),
                            Year = record.Year,
                            Mileage = record.Mileage,
                            Status = record.Status,
                            RentalPricePerHour = record.RentalPricePerHour
                        };

                        _repository.Add(car);

                        result.SuccessfulImports++;
                        existingPlates.Add(record.LicensePlate.ToUpperInvariant());

                        _logger.Log($"IMPORT: Импортирован {car.Brand} {car.Model} ({car.LicensePlate})");
                    }
                    catch (Exception ex)
                    {
                        result.FailedRecords++;
                        result.Errors.Add(new ImportError
                        {
                            LineNumber = lineNumber,
                            ErrorMessage = ex.Message,
                            RawData = JsonSerializer.Serialize(record)
                        });

                        _logger.Log($"IMPORT ERROR: Запись {lineNumber}: {ex.Message}");
                    }

                    lineNumber++;
                }

                _logger.Log($"Импорт завершен. Успешно: {result.SuccessfulImports}, Пропущено: {result.SkippedRecords}, Ошибок: {result.FailedRecords}");
            }
            catch (Exception ex)
            {
                _logger.Log($"IMPORT CRITICAL ERROR: {ex.Message}");
                result.Errors.Add(new ImportError
                {
                    LineNumber = 0,
                    ErrorMessage = $"Критическая ошибка чтения файла: {ex.Message}",
                    RawData = ""
                });
            }

            return result;
        }

        /// <summary>
        /// Проверяет валидность файла без фактического импорта в базу данных.
        /// </summary>
        /// <param name="filePath">Полный путь к файлу.</param>
        /// <param name="format">Формат файла (CSV или JSON).</param>
        /// <returns>Результат валидации с информацией о найденных ошибках.</returns>
        public ImportResult ValidateImportFile(string filePath, ImportFormat format)
        {
            _logger.Log($"Валидация файла: {filePath}, формат: {format}");

            var result = new ImportResult();

            try
            {
                if (format == ImportFormat.Csv)
                {
                    // Читаем и валидируем CSV без сохранения
                    var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                    {
                        HasHeaderRecord = true,
                        Delimiter = ";",
                        MissingFieldFound = null,
                        BadDataFound = null
                    };

                    using var reader = new StreamReader(filePath);
                    using var csv = new CsvReader(reader, config);
                    csv.Context.RegisterClassMap<CarCsvMap>();

                    var records = csv.GetRecords<CarImportDto>().ToList();
                    result.TotalRecords = records.Count;

                    // Получаем существующие госномера для проверки дубликатов
                    var existingPlates = _repository.ReadAll()
                        .Select(c => c.LicensePlate.ToUpperInvariant())
                        .ToHashSet();

                    int lineNumber = 2;
                    foreach (var record in records)
                    {
                        try
                        {
                            // Проверка на дубликат
                            if (existingPlates.Contains(record.LicensePlate.ToUpperInvariant()))
                            {
                                result.SkippedRecords++;
                                result.Errors.Add(new ImportError
                                {
                                    LineNumber = lineNumber,
                                    ErrorMessage = $"Автомобиль с госномером '{record.LicensePlate}' уже существует",
                                    RawData = $"{record.Brand} {record.Model}"
                                });
                            }
                            else
                            {
                                // Только валидация
                                _validator.ValidateForUpdate(
                                    record.Brand,
                                    record.Model,
                                    record.LicensePlate,
                                    record.Year,
                                    record.Mileage,
                                    record.RentalPricePerHour,
                                    (int)record.Status
                                );
                                result.SuccessfulImports++;
                            }
                        }
                        catch (Exception ex)
                        {
                            result.FailedRecords++;
                            result.Errors.Add(new ImportError
                            {
                                LineNumber = lineNumber,
                                ErrorMessage = ex.Message,
                                RawData = $"{record.Brand} {record.Model}"
                            });
                        }
                        lineNumber++;
                    }
                }
                else // JSON
                {
                    var jsonString = File.ReadAllText(filePath);
                    var records = JsonSerializer.Deserialize<List<CarImportDto>>(jsonString, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (records != null)
                    {
                        result.TotalRecords = records.Count;

                        var existingPlates = _repository.ReadAll()
                            .Select(c => c.LicensePlate.ToUpperInvariant())
                            .ToHashSet();

                        int lineNumber = 1;

                        foreach (var record in records)
                        {
                            try
                            {
                                if (existingPlates.Contains(record.LicensePlate.ToUpperInvariant()))
                                {
                                    result.SkippedRecords++;
                                    result.Errors.Add(new ImportError
                                    {
                                        LineNumber = lineNumber,
                                        ErrorMessage = $"Автомобиль с госномером '{record.LicensePlate}' уже существует",
                                        RawData = JsonSerializer.Serialize(record)
                                    });
                                }
                                else
                                {
                                    _validator.ValidateForUpdate(
                                        record.Brand,
                                        record.Model,
                                        record.LicensePlate,
                                        record.Year,
                                        record.Mileage,
                                        record.RentalPricePerHour,
                                        (int)record.Status
                                    );
                                    result.SuccessfulImports++;
                                }
                            }
                            catch (Exception ex)
                            {
                                result.FailedRecords++;
                                result.Errors.Add(new ImportError
                                {
                                    LineNumber = lineNumber,
                                    ErrorMessage = ex.Message,
                                    RawData = JsonSerializer.Serialize(record)
                                });
                            }
                            lineNumber++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Errors.Add(new ImportError
                {
                    LineNumber = 0,
                    ErrorMessage = $"Ошибка чтения файла: {ex.Message}",
                    RawData = ""
                });
            }

            return result;
        }

        /// <summary>
        /// Экспортирует все автомобили из базы данных в CSV файл.
        /// </summary>
        /// <param name="filePath">Полный путь к CSV файлу для сохранения.</param>
        /// <returns>Количество экспортированных записей.</returns>
        public int ExportToCsv(string filePath)
        {
            _logger.Log($"Начало экспорта в CSV: {filePath}");

            try
            {
                var cars = _repository.ReadAll().ToList();

                if (cars.Count == 0)
                {
                    _logger.Log("Нет данных для экспорта");
                    throw new InvalidOperationException("В базе данных нет автомобилей для экспорта");
                }

                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = ";",
                    HasHeaderRecord = true
                };

                using var writer = new StreamWriter(filePath, false, System.Text.Encoding.UTF8);
                using var csv = new CsvWriter(writer, config);

                // Конвертируем Car в CarImportDto для экспорта
                var exportData = cars.Select(c => new CarImportDto
                {
                    Brand = c.Brand,
                    Model = c.Model,
                    LicensePlate = c.LicensePlate,
                    Year = c.Year,
                    Mileage = c.Mileage,
                    Status = c.Status,
                    RentalPricePerHour = c.RentalPricePerHour
                });

                csv.Context.RegisterClassMap<CarCsvMap>();
                csv.WriteRecords(exportData);

                _logger.Log($"Экспорт завершен. Экспортировано записей: {cars.Count}");
                return cars.Count;
            }
            catch (Exception ex)
            {
                _logger.Log($"EXPORT ERROR: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Экспортирует все автомобили из базы данных в JSON файл.
        /// </summary>
        /// <param name="filePath">Полный путь к JSON файлу для сохранения.</param>
        /// <returns>Количество экспортированных записей.</returns>
        public int ExportToJson(string filePath)
        {
            _logger.Log($"Начало экспорта в JSON: {filePath}");

            try
            {
                var cars = _repository.ReadAll().ToList();

                if (cars.Count == 0)
                {
                    _logger.Log("Нет данных для экспорта");
                    throw new InvalidOperationException("В базе данных нет автомобилей для экспорта");
                }

                // Конвертируем Car в CarImportDto для экспорта
                var exportData = cars.Select(c => new CarImportDto
                {
                    Brand = c.Brand,
                    Model = c.Model,
                    LicensePlate = c.LicensePlate,
                    Year = c.Year,
                    Mileage = c.Mileage,
                    Status = c.Status,
                    RentalPricePerHour = c.RentalPricePerHour
                });

                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                };

                var json = JsonSerializer.Serialize(exportData, options);
                File.WriteAllText(filePath, json, System.Text.Encoding.UTF8);

                _logger.Log($"Экспорт завершен. Экспортировано записей: {cars.Count}");
                return cars.Count;
            }
            catch (Exception ex)
            {
                _logger.Log($"EXPORT ERROR: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Экспортирует выбранные автомобили в CSV файл.
        /// </summary>
        /// <param name="carIds">Идентификаторы автомобилей для экспорта.</param>
        /// <param name="filePath">Полный путь к CSV файлу для сохранения.</param>
        /// <returns>Количество экспортированных записей.</returns>
        public int ExportToCsv(IEnumerable<int> carIds, string filePath)
        {
            _logger.Log($"Начало экспорта выбранных автомобилей в CSV: {filePath}");

            try
            {
                var carIdsList = carIds.ToList();
                var allCars = _repository.ReadAll();
                var selectedCars = allCars.Where(c => carIdsList.Contains(c.Id)).ToList();

                if (selectedCars.Count == 0)
                {
                    _logger.Log("Нет выбранных автомобилей для экспорта");
                    throw new InvalidOperationException("Выбранные автомобили не найдены в базе данных");
                }

                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = ";",
                    HasHeaderRecord = true
                };

                using var writer = new StreamWriter(filePath, false, System.Text.Encoding.UTF8);
                using var csv = new CsvWriter(writer, config);

                var exportData = selectedCars.Select(c => new CarImportDto
                {
                    Brand = c.Brand,
                    Model = c.Model,
                    LicensePlate = c.LicensePlate,
                    Year = c.Year,
                    Mileage = c.Mileage,
                    Status = c.Status,
                    RentalPricePerHour = c.RentalPricePerHour
                });

                csv.Context.RegisterClassMap<CarCsvMap>();
                csv.WriteRecords(exportData);

                _logger.Log($"Экспорт завершен. Экспортировано записей: {selectedCars.Count}");
                return selectedCars.Count;
            }
            catch (Exception ex)
            {
                _logger.Log($"EXPORT ERROR: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Экспортирует выбранные автомобили в JSON файл.
        /// </summary>
        /// <param name="carIds">Идентификаторы автомобилей для экспорта.</param>
        /// <param name="filePath">Полный путь к JSON файлу для сохранения.</param>
        /// <returns>Количество экспортированных записей.</returns>
        public int ExportToJson(IEnumerable<int> carIds, string filePath)
        {
            _logger.Log($"Начало экспорта выбранных автомобилей в JSON: {filePath}");

            try
            {
                var carIdsList = carIds.ToList();
                var allCars = _repository.ReadAll();
                var selectedCars = allCars.Where(c => carIdsList.Contains(c.Id)).ToList();

                if (selectedCars.Count == 0)
                {
                    _logger.Log("Нет выбранных автомобилей для экспорта");
                    throw new InvalidOperationException("Выбранные автомобили не найдены в базе данных");
                }

                var exportData = selectedCars.Select(c => new CarImportDto
                {
                    Brand = c.Brand,
                    Model = c.Model,
                    LicensePlate = c.LicensePlate,
                    Year = c.Year,
                    Mileage = c.Mileage,
                    Status = c.Status,
                    RentalPricePerHour = c.RentalPricePerHour
                });

                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                };

                var json = JsonSerializer.Serialize(exportData, options);
                File.WriteAllText(filePath, json, System.Text.Encoding.UTF8);

                _logger.Log($"Экспорт завершен. Экспортировано записей: {selectedCars.Count}");
                return selectedCars.Count;
            }
            catch (Exception ex)
            {
                _logger.Log($"EXPORT ERROR: {ex.Message}");
                throw;
            }
        }
    }

    /// <summary>
    /// DTO для импорта данных автомобиля из внешнего источника.
    /// </summary>
    internal class CarImportDto
    {
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string LicensePlate { get; set; } = string.Empty;
        public int Year { get; set; }
        public int Mileage { get; set; }
        public CarStatus Status { get; set; }
        public decimal RentalPricePerHour { get; set; }
    }

    /// <summary>
    /// Маппинг CSV столбцов на свойства CarImportDto для библиотеки CsvHelper.
    /// </summary>
    internal class CarCsvMap : ClassMap<CarImportDto>
    {
        public CarCsvMap()
        {
            Map(m => m.Brand).Name("Марка", "Brand");
            Map(m => m.Model).Name("Модель", "Model");
            Map(m => m.LicensePlate).Name("Госномер", "LicensePlate", "Гос.номер");
            Map(m => m.Year).Name("Год", "Year");
            Map(m => m.Mileage).Name("Пробег", "Mileage");
            Map(m => m.Status).Name("Статус", "Status").TypeConverter<CarStatusConverter>();
            Map(m => m.RentalPricePerHour).Name("Цена за час", "RentalPricePerHour", "Цена/час");
        }
    }

    /// <summary>
    /// Конвертер для преобразования текстового представления статуса в enum CarStatus.
    /// </summary>
    internal class CarStatusConverter : CsvHelper.TypeConversion.DefaultTypeConverter
    {
        public override object ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
        {
            if (string.IsNullOrWhiteSpace(text))
                return CarStatus.Available;

            text = text.Trim().ToLowerInvariant();

            return text switch
            {
                "доступна" or "available" or "свободен" or "0" => CarStatus.Available,
                "арендована" or "rented" or "в аренде" or "1" => CarStatus.Rented,
                "на обслуживании" or "maintenance" or "undermaintenance" or "2" => CarStatus.UnderMaintenance,
                _ => throw new ArgumentException($"Неизвестный статус: '{text}'. Допустимые значения: Доступна/Available/0, Арендована/Rented/1, На обслуживании/Maintenance/2")
            };
        }
    }
}
