using System;
using System.Collections.Generic;
using BussinessLogic;
using BussinessLogic.Dto;
using BussinessLogic.Services.Import;
using BussinessLogic.Services.Import.Models;
using Model;
using Presenter.Events;

namespace Presenter
{
    /// <summary>
    /// Фасад над бизнес-логикой для MVP паттерна.
    /// Предоставляет единую точку доступа к сервисам и публикует события.
    /// </summary>
    public class CarSharingModel : IModelEventPublisher
    {
        private readonly ICarService _carService;
        private readonly IPromoService _promoService;
        private readonly ICarImportService _importService;


        /// <summary>
        /// Событие выполнения операции с автомобилем.
        /// </summary>
        public event EventHandler<CarOperationEventArgs>? CarOperationPerformed;

        /// <summary>
        /// Событие валидации данных.
        /// </summary>
        public event EventHandler<ValidationEventArgs>? ValidationPerformed;

        /// <summary>
        /// Событие импорта/экспорта данных.
        /// </summary>
        public event EventHandler<ImportExportEventArgs>? ImportExportPerformed;

        /// <summary>
        /// Событие ошибки в модели.
        /// </summary>
        public event EventHandler<ModelEventArgs>? ErrorOccurred;

        /// <summary>
        /// Событие информационного сообщения от модели.
        /// </summary>
        public event EventHandler<ModelEventArgs>? InfoMessage;

        /// <summary>
        /// Инициализирует модель с сервисами бизнес-логики.
        /// </summary>
        public CarSharingModel(ICarService carService, IPromoService promoService, ICarImportService importService)
        {
            _carService = carService ?? throw new ArgumentNullException(nameof(carService));
            _promoService = promoService ?? throw new ArgumentNullException(nameof(promoService));
            _importService = importService ?? throw new ArgumentNullException(nameof(importService));
        }


        /// <summary>
        /// Создает новый автомобиль.
        /// </summary>
        public Car CreateCar(string brand, string model, string licensePlate, int year, int mileage, decimal rentalPricePerHour)
        {
            try
            {
                var car = _carService.CreateCar(brand, model, licensePlate, year, mileage, rentalPricePerHour);

                CarOperationPerformed?.Invoke(this, new CarOperationEventArgs(
                    car.Id, "Create", true, $"Created: {car.Brand} {car.Model} ({car.LicensePlate})"));

                return car;
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(this, new ModelEventArgs($"Ошибка создания автомобиля: {ex.Message}"));
                throw;
            }
        }

        /// <summary>
        /// Получает автомобиль по ID.
        /// </summary>
        public Car? GetCar(int id)
        {
            try
            {
                return _carService.GetCar(id);
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(this, new ModelEventArgs($"Ошибка получения автомобиля: {ex.Message}"));
                return null;
            }
        }

        /// <summary>
        /// Получает все автомобили.
        /// </summary>
        public List<Car> GetAllCars()
        {
            try
            {
                return _carService.GetAllCars();
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(this, new ModelEventArgs($"Ошибка получения списка автомобилей: {ex.Message}"));
                return new List<Car>();
            }
        }

        /// <summary>
        /// Получает список автомобилей для отображения (DTO).
        /// </summary>
        public List<CarListItemDto> GetCarsForDisplay()
        {
            try
            {
                return _carService.GetCarsForDisplay();
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(this, new ModelEventArgs($"Ошибка получения данных для отображения: {ex.Message}"));
                return new List<CarListItemDto>();
            }
        }

        /// <summary>
        /// Получает детальную информацию об автомобиле (DTO).
        /// </summary>
        public CarDetailsDto? GetCarForDisplay(int carId)
        {
            try
            {
                return _carService.GetCarForDisplay(carId);
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(this, new ModelEventArgs($"Ошибка получения деталей автомобиля: {ex.Message}"));
                return null;
            }
        }

        /// <summary>
        /// Получает данные автомобиля для расчета (DTO).
        /// </summary>
        public CarForCalculationDto? GetCarForCalculation(int carId)
        {
            try
            {
                return _carService.GetCarForCalculation(carId);
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(this, new ModelEventArgs($"Ошибка получения данных для расчета: {ex.Message}"));
                return null;
            }
        }

        /// <summary>
        /// Получает значения полей автомобиля для редактирования.
        /// </summary>
        public object[]? GetCarValuesForEdit(int id)
        {
            try
            {
                return _carService.GetCarValuesForEdit(id);
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(this, new ModelEventArgs($"Ошибка получения данных для редактирования: {ex.Message}"));
                return null;
            }
        }

        /// <summary>
        /// Обновляет данные автомобиля.
        /// </summary>
        public bool UpdateCarDetails(int id, string brand, string model, string plate, int year, int mileage, decimal price, int status)
        {
            try
            {
                var result = _carService.UpdateCarDetails(id, brand, model, plate, year, mileage, price, status);

                if (result)
                {
                    CarOperationPerformed?.Invoke(this, new CarOperationEventArgs(
                        id, "Update", true, $"Updated: {brand} {model} ({plate})"));
                }
                else
                {
                    CarOperationPerformed?.Invoke(this, new CarOperationEventArgs(
                        id, "Update", false, "Update failed"));
                }

                return result;
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(this, new ModelEventArgs($"Ошибка обновления автомобиля: {ex.Message}"));
                throw;
            }
        }

        /// <summary>
        /// Удаляет автомобиль.
        /// </summary>
        public bool DeleteCar(int id)
        {
            try
            {
                var car = GetCar(id);
                var result = _carService.DeleteCar(id);

                if (result && car != null)
                {
                    CarOperationPerformed?.Invoke(this, new CarOperationEventArgs(
                        id, "Delete", true, $"Deleted: {car.Brand} {car.Model}"));
                }
                else
                {
                    CarOperationPerformed?.Invoke(this, new CarOperationEventArgs(
                        id, "Delete", false, "Delete failed"));
                }

                return result;
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(this, new ModelEventArgs($"Ошибка удаления автомобиля: {ex.Message}"));
                return false;
            }
        }

        /// <summary>
        /// Арендует автомобиль.
        /// </summary>
        public bool RentCar(int carId)
        {
            try
            {
                var result = _carService.RentCar(carId);

                if (result)
                {
                    CarOperationPerformed?.Invoke(this, new CarOperationEventArgs(
                        carId, "Rent", true, "Car rented successfully"));
                }
                else
                {
                    CarOperationPerformed?.Invoke(this, new CarOperationEventArgs(
                        carId, "Rent", false, "Rent failed - car not available"));
                }

                return result;
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(this, new ModelEventArgs($"Ошибка аренды автомобиля: {ex.Message}"));
                return false;
            }
        }

        /// <summary>
        /// Рассчитывает стоимость аренды.
        /// </summary>
        public decimal CalculateRentalCost(int carId, int hours, string? promoCode = null)
        {
            try
            {
                var cost = _carService.CalculateRentalCost(carId, hours, promoCode);

                CarOperationPerformed?.Invoke(this, new CarOperationEventArgs(
                    carId, "Calculate", true, $"Cost calculated: {cost:C} for {hours} hours{(promoCode != null ? $" with promo '{promoCode}'" : "")}"));

                return cost;
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(this, new ModelEventArgs($"Ошибка расчета стоимости: {ex.Message}"));
                throw;
            }
        }

        /// <summary>
        /// Получает детальное описание примененных коэффициентов динамического ценообразования.
        /// </summary>
        /// <param name="hours">Количество часов аренды.</param>
        /// <returns>Текстовое описание всех примененных коэффициентов.</returns>
        public string GetPricingBreakdown(int hours)
        {
            try
            {
                return _carService.GetPricingBreakdown(hours);
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(this, new ModelEventArgs($"Ошибка получения детализации: {ex.Message}"));
                return $"Ошибка: {ex.Message}";
            }
        }

        // Методы импорта/экспорта

        /// <summary>
        /// Импортирует автомобили из CSV файла.
        /// </summary>
        public ImportResult ImportFromCsv(string filePath)
        {
            try
            {
                var result = _importService.ImportFromCsv(filePath);

                ImportExportPerformed?.Invoke(this, new ImportExportEventArgs(
                    filePath, "Import CSV",
                    result.TotalRecords, result.SuccessfulImports, result.FailedRecords));

                return result;
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(this, new ModelEventArgs($"Ошибка импорта CSV: {ex.Message}"));
                throw;
            }
        }

        /// <summary>
        /// Импортирует автомобили из JSON файла.
        /// </summary>
        public ImportResult ImportFromJson(string filePath)
        {
            try
            {
                var result = _importService.ImportFromJson(filePath);

                ImportExportPerformed?.Invoke(this, new ImportExportEventArgs(
                    filePath, "Import JSON",
                    result.TotalRecords, result.SuccessfulImports, result.FailedRecords));

                return result;
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(this, new ModelEventArgs($"Ошибка импорта JSON: {ex.Message}"));
                throw;
            }
        }

        /// <summary>
        /// Валидирует файл перед импортом.
        /// </summary>
        public ImportResult ValidateImportFile(string filePath, ImportFormat format)
        {
            try
            {
                var result = _importService.ValidateImportFile(filePath, format);

                ValidationPerformed?.Invoke(this, new ValidationEventArgs(
                    result.FailedRecords == 0,
                    result.Errors.ConvertAll(e => e.ErrorMessage)));

                return result;
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(this, new ModelEventArgs($"Ошибка валидации файла: {ex.Message}"));
                throw;
            }
        }

        /// <summary>
        /// Экспортирует все автомобили в CSV файл.
        /// </summary>
        public int ExportToCsv(string filePath)
        {
            try
            {
                var count = _importService.ExportToCsv(filePath);

                ImportExportPerformed?.Invoke(this, new ImportExportEventArgs(
                    filePath, "Export CSV", count, count, 0));

                return count;
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(this, new ModelEventArgs($"Ошибка экспорта CSV: {ex.Message}"));
                throw;
            }
        }

        /// <summary>
        /// Экспортирует все автомобили в JSON файл.
        /// </summary>
        public int ExportToJson(string filePath)
        {
            try
            {
                var count = _importService.ExportToJson(filePath);

                ImportExportPerformed?.Invoke(this, new ImportExportEventArgs(
                    filePath, "Export JSON", count, count, 0));

                return count;
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(this, new ModelEventArgs($"Ошибка экспорта JSON: {ex.Message}"));
                throw;
            }
        }

        /// <summary>
        /// Экспортирует выбранные автомобили в CSV файл.
        /// </summary>
        public int ExportToCsv(IEnumerable<int> carIds, string filePath)
        {
            try
            {
                var count = _importService.ExportToCsv(carIds, filePath);

                ImportExportPerformed?.Invoke(this, new ImportExportEventArgs(
                    filePath, "Export CSV (selected)", count, count, 0));

                return count;
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(this, new ModelEventArgs($"Ошибка экспорта CSV: {ex.Message}"));
                throw;
            }
        }

        /// <summary>
        /// Экспортирует выбранные автомобили в JSON файл.
        /// </summary>
        public int ExportToJson(IEnumerable<int> carIds, string filePath)
        {
            try
            {
                var count = _importService.ExportToJson(carIds, filePath);

                ImportExportPerformed?.Invoke(this, new ImportExportEventArgs(
                    filePath, "Export JSON (selected)", count, count, 0));

                return count;
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(this, new ModelEventArgs($"Ошибка экспорта JSON: {ex.Message}"));
                throw;
            }
        }
    }
}
