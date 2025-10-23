using Model;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;

namespace BussinessLogic
{
    /// <summary>
    /// Реализует бизнес-логику по управлению автомобилями в системе каршеринга.
    /// Обеспечивает выполнение операций CRUD и бизнес-функций.
    /// </summary>
    public class CarService : ICarService
    {
        private readonly IRepository<Car> _repository;
        private readonly PromoService _promoService;
        private readonly object _logSync = new object();
        private readonly string _logFilePath = Path.Combine(@"C:\Users\kosty\OneDrive\Desktop", "actions.log");

        /// <summary>
        /// Инициализирует новый экземпляр сервиса с репозиторием для работы с данными.
        /// </summary>
        /// <param name="repository">Репозиторий для доступа к данным автомобилей.</param>
        /// <param name="promoService">Сервис для работы с промокодами.</param>
        public CarService(IRepository<Car> repository, PromoService promoService)
        {
            _repository = repository;
            _promoService = promoService;
        }

        /// <summary>
        /// Записывает строку в файл журнала действий на рабочем столе пользователя.
        /// Потокобезопасно (используется блокировка на время записи) и не прерывает
        /// основную логику приложения при ошибках записи (исключения подавляются).
        /// </summary>
        /// <param name="message">Текст сообщения для записи в лог.</param>
        private void WriteLog(string message)
        {
            try
            {
                var line = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}" + Environment.NewLine;
                lock (_logSync)
                {
                    var directory = Path.GetDirectoryName(_logFilePath);
                    if (!string.IsNullOrEmpty(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }
                    File.AppendAllText(_logFilePath, line, Encoding.UTF8);
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Создает новый автомобиль и добавляет его в систему.
        /// </summary>
        /// <param name="brand">Марка автомобиля.</param>
        /// <param name="model">Модель автомобиля.</param>
        /// <param name="licensePlate">Государственный номер.</param>
        /// <param name="year">Год выпуска.</param>
        /// <param name="mileage">Текущий пробег.</param>
        /// <param name="rentalPricePerHour">Стоимость аренды в час.</param>
        /// <returns>Созданный объект автомобиля.</returns>
        /// <exception cref="ArgumentException">Выбрасывается, если предоставлены недопустимые данные.</exception>
        public Car CreateCar(string brand, string model, string licensePlate, int year, int mileage, decimal rentalPricePerHour)
        {
            if (string.IsNullOrWhiteSpace(brand))
                throw new ArgumentException("Марка автомобиля не может быть пустой.", nameof(brand));

            if (string.IsNullOrWhiteSpace(model))
                throw new ArgumentException("Модель автомобиля не может быть пустой.", nameof(model));

            if (string.IsNullOrWhiteSpace(licensePlate))
                throw new ArgumentException("Государственный номер не может быть пустым.", nameof(licensePlate));

            if (year < 1900 || year > DateTime.Now.Year + 1)
                throw new ArgumentException($"Год выпуска должен быть между 1900 и {DateTime.Now.Year + 1}.", nameof(year));

            if (mileage < 0)
                throw new ArgumentException("Пробег не может быть отрицательным.", nameof(mileage));

            if (rentalPricePerHour <= 0)
                throw new ArgumentException("Стоимость аренды должна быть положительной.", nameof(rentalPricePerHour));

            var existingCars = _repository.ReadAll();
            if (existingCars.Any(c => c.LicensePlate.Equals(licensePlate, StringComparison.OrdinalIgnoreCase)))
                throw new ArgumentException("Автомобиль с таким государственным номером уже существует.", nameof(licensePlate));

            var car = new Car
            {
                Brand = brand.Trim(),
                Model = model.Trim(),
                LicensePlate = licensePlate.Trim(),
                Year = year,
                Mileage = mileage,
                RentalPricePerHour = rentalPricePerHour,
                Status = CarStatus.Available
            };

            _repository.Add(car);
            WriteLog($"CREATE: Id={car.Id}, {car.Brand} {car.Model}, Plate={car.LicensePlate}, Year={car.Year}, Mileage={car.Mileage}, PricePerHour={car.RentalPricePerHour}");
            return car;
        }

        /// <summary>
        /// Возвращает автомобиль по его уникальному идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор автомобиля.</param>
        /// <returns>Найденный автомобиль или null, если автомобиль не найден.</returns>
        public Car GetCar(int id)
        {
            return _repository.ReadById(id);
        }

        /// <summary>
        /// Возвращает список всех автомобилей в системе.
        /// </summary>
        /// <returns>Список автомобилей.</returns>
        public List<Car> GetAllCars()
        {
            return _repository.ReadAll().ToList();
        }

        /// <summary>
        /// Обновляет данные существующего автомобиля.
        /// </summary>
        /// <param name="carToUpdate">Объект автомобиля с обновленными данными.</param>
        /// <returns>True, если обновление прошло успешно, иначе False.</returns>
        /// <exception cref="ArgumentNullException">Выбрасывается, если переданный объект равен null.</exception>
        public bool UpdateCar(Car carToUpdate)
        {
            if (carToUpdate == null)
                throw new ArgumentNullException(nameof(carToUpdate));

            var existingCar = GetCar(carToUpdate.Id);
            if (existingCar == null)
                return false;

            var allCars = _repository.ReadAll();
            if (allCars.Any(c => c.Id != carToUpdate.Id && c.LicensePlate.Equals(carToUpdate.LicensePlate, StringComparison.OrdinalIgnoreCase)))
                return false;

            _repository.Update(carToUpdate);
            WriteLog($"UPDATE: Id={carToUpdate.Id}, {carToUpdate.Brand} {carToUpdate.Model}, Plate={carToUpdate.LicensePlate}, Year={carToUpdate.Year}, Mileage={carToUpdate.Mileage}, Status={carToUpdate.Status}, PricePerHour={carToUpdate.RentalPricePerHour}");
            return true;
        }

        /// <summary>
        /// Удаляет автомобиль из системы по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор автомобиля для удаления.</param>
        /// <returns>True, если удаление прошло успешно, иначе False.</returns>
        public bool DeleteCar(int id)
        {
            var carToRemove = GetCar(id);
            if (carToRemove == null)
                return false;

            if (carToRemove.Status == CarStatus.Rented)
                return false;

            _repository.Delete(id);
            WriteLog($"DELETE: Id={carToRemove.Id}, {carToRemove.Brand} {carToRemove.Model}, Plate={carToRemove.LicensePlate}");
            return true;
        }

        /// <summary>
        /// Возвращает список автомобилей, доступных для аренды.
        /// </summary>
        /// <returns>Список доступных автомобилей.</returns>
        public List<Car> GetAvailableCars()
        {
            return _repository.ReadAll().Where(c => c.Status == CarStatus.Available).ToList();
        }

        /// <summary>
        /// Выполняет операцию аренды автомобиля.
        /// </summary>
        /// <param name="carId">Идентификатор автомобиля для аренды.</param>
        /// <returns>True, если аренда прошла успешно, иначе False.</returns>
        public bool RentCar(int carId)
        {
            var carToRent = GetCar(carId);
            if (carToRent == null || carToRent.Status != CarStatus.Available)
                return false;

            carToRent.Status = CarStatus.Rented;
            _repository.Update(carToRent);
            WriteLog($"RENT: Id={carToRent.Id}, {carToRent.Brand} {carToRent.Model}, Plate={carToRent.LicensePlate}");
            return true;
        }

        /// <summary>
        /// Рассчитывает стоимость аренды автомобиля на указанное количество часов.
        /// </summary>
        /// <param name="carId">Идентификатор автомобиля.</param>
        /// <param name="hours">Количество часов аренды.</param>
        /// <param name="promoCode">Необязательный промокод для применения скидки.</param>
        /// <returns>Рассчитанная стоимость аренды.</returns>
        /// <exception cref="ArgumentException">Выбрасывается, если количество часов неположительное или автомобиль не найден.</exception>
        public decimal CalculateRentalCost(int carId, int hours, string promoCode = null)
        {
            if (hours <= 0)
                throw new ArgumentException("Количество часов аренды должно быть положительным числом.", nameof(hours));

            var car = GetCar(carId);
            if (car == null)
                throw new ArgumentException("Автомобиль с указанным ID не найден.", nameof(carId));

            decimal originalPrice = car.RentalPricePerHour * hours;

            if (string.IsNullOrWhiteSpace(promoCode))
                return originalPrice;

            try
            {
                return _promoService.ApplyPromoCode(promoCode, originalPrice);
            }
            catch (ArgumentException)
            {
                throw;
            }
        }

        /// <summary>
        /// Получает строковое представление автомобиля по его идентификатору.
        /// </summary>
        /// <param name="carId">Идентификатор автомобиля.</param>
        /// <returns>Строковое описание автомобиля или сообщение об ошибке, если автомобиль не найден.</returns>
        public string GetCarDescription(int carId)
        {
            var car = GetCar(carId);
            if (car == null)
                return $"Автомобиль с ID {carId} не найден.";

            string statusText;
            switch (car.Status)
            {
                case CarStatus.Available:
                    statusText = "Свободен";
                    break;
                case CarStatus.Rented:
                    statusText = "В аренде";
                    break;
                case CarStatus.UnderMaintenance:
                    statusText = "На тех. обслуживании";
                    break;
                default:
                    statusText = "Неизвестен";
                    break;
            }

            return $"ID: {car.Id}, {car.Brand} {car.Model}, Гос.номер: {car.LicensePlate}, " +
                   $"Год: {car.Year}, Пробег: {car.Mileage} км, " +
                   $"Статус: {statusText}, Цена/час: {car.RentalPricePerHour:C}";
        }

        /// <summary>
        /// Получает список строковых представлений всех автомобилей в системе.
        /// </summary>
        /// <returns>Список строковых описаний всех автомобилей.</returns>
        public List<string> GetAllCarsDescriptions()
        {
            var descriptions = new List<string>();
            foreach (var car in _repository.ReadAll())
            {
                descriptions.Add(GetCarDescription(car.Id));
            }
            return descriptions;
        }

        /// <summary>
        /// Получает список строковых представлений доступных для аренды автомобилей.
        /// </summary>
        /// <returns>Список строковых описаний доступных автомобилей.</returns>
        public List<string> GetAvailableCarsDescriptions()
        {
            var descriptions = new List<string>();
            foreach (var car in _repository.ReadAll())
            {
                if (car.Status == CarStatus.Available)
                {
                    descriptions.Add(GetCarDescription(car.Id));
                }
            }
            return descriptions;
        }

        /// <summary>
        /// Обновляет данные автомобиля с использованием отдельных параметров.
        /// </summary>
        /// <param name="id">Идентификатор автомобиля.</param>
        /// <param name="brand">Марка автомобиля.</param>
        /// <param name="model">Модель автомобиля.</param>
        /// <param name="licensePlate">Государственный номер.</param>
        /// <param name="year">Год выпуска.</param>
        /// <param name="mileage">Текущий пробег.</param>
        /// <param name="rentalPricePerHour">Стоимость аренды в час.</param>
        /// <param name="status">Статус автомобиля в числовом формате (0 - Свободен, 1 - В аренде, 2 - На тех. обслуживании).</param>
        /// <returns>True, если обновление прошло успешно, иначе False.</returns>
        public bool UpdateCarDetails(int id, string brand, string model, string licensePlate, int year, int mileage, decimal rentalPricePerHour, int status)
        {
            CarStatus carStatus;
            switch (status)
            {
                case 0:
                    carStatus = CarStatus.Available;
                    break;
                case 1:
                    carStatus = CarStatus.Rented;
                    break;
                case 2:
                    carStatus = CarStatus.UnderMaintenance;
                    break;
                default:
                    carStatus = CarStatus.Available;
                    break;
            }

            var carToUpdate = new Car
            {
                Id = id,
                Brand = brand,
                Model = model,
                LicensePlate = licensePlate,
                Year = year,
                Mileage = mileage,
                RentalPricePerHour = rentalPricePerHour,
                Status = carStatus
            };

            return UpdateCar(carToUpdate);
        }

        /// <summary>
        /// Получает текущие значения автомобиля для редактирования.
        /// </summary>
        /// <param name="id">Идентификатор автомобиля.</param>
        /// <returns>Массив значений [brand, model, licensePlate, year, mileage, price, status] или null, если автомобиль не найден.</returns>
        public object[] GetCarValuesForEdit(int id)
        {
            var car = GetCar(id);
            if (car == null) return null;

            return new object[]
            {
                car.Brand,
                car.Model,
                car.LicensePlate,
                car.Year,
                car.Mileage,
                car.RentalPricePerHour,
                (int)car.Status
            };
        }

        /// <summary>
        /// Получает информацию об автомобиле для отображения в пользовательском интерфейсе.
        /// </summary>
        /// <param name="carId">Идентификатор автомобиля.</param>
        /// <returns>Объект с данными для отображения или null, если автомобиль не найден.</returns>
        public object  GetCarForDisplay(int carId)
        {
            var car = GetCar(carId);
            if (car == null) return null;

            return new
            {
                Id = car.Id,
                Brand = car.Brand,
                Model = car.Model,
                LicensePlate = car.LicensePlate,
                Year = car.Year,
                Mileage = car.Mileage,
                Status = car.Status,
                RentalPricePerHour = car.RentalPricePerHour,
                StatusText = car.Status.ToString(),
                DisplayText = $"{car.Brand} {car.Model} ({car.LicensePlate})"
            };
        }

        /// <summary>
        /// Получает список всех автомобилей для отображения в пользовательском интерфейсе.
        /// </summary>
        /// <returns>Список объектов с данными для отображения.</returns>
        public List<object> GetCarsForDisplay()
        {
            var result = new List<object>();
            foreach (var car in _repository.ReadAll())
            {
                result.Add(new
                {
                    Id = car.Id,
                    Brand = car.Brand,
                    Model = car.Model,
                    LicensePlate = car.LicensePlate,
                    Year = car.Year,
                    Mileage = car.Mileage,
                    Status = car.Status,
                    RentalPricePerHour = car.RentalPricePerHour,
                    StatusText = car.Status.ToString(),
                    DisplayText = $"{car.Brand} {car.Model} ({car.LicensePlate})"
                });
            }
            return result;
        }

        /// <summary>
        /// Получает информацию об автомобиле для расчета стоимости аренды.
        /// </summary>
        /// <param name="carId">Идентификатор автомобиля.</param>
        /// <returns>Объект с данными для расчета или null, если автомобиль не найден.</returns>
        public object GetCarForCalculation(int carId)
        {
            var car = GetCar(carId);
            if (car == null) return null;

            return new
            {
                Id = car.Id,
                Brand = car.Brand,
                Model = car.Model,
                LicensePlate = car.LicensePlate,
                RentalPricePerHour = car.RentalPricePerHour,
                DisplayText = $"{car.Brand} {car.Model} ({car.LicensePlate})"
            };
        }
    }
}