using BussinessLogic;

namespace ConsoleApp
{
    /// <summary>
    /// Контроллер консольного меню для управления системой каршеринга.
    /// </summary>
    public class MenuController
    {
        private readonly ICarService _carService;
        private readonly IPromoService _promoService;

        /// <summary>
        /// Инициализирует новый экземпляр контроллера меню с сервисами.
        /// </summary>
        /// <param name="carService">Сервис для работы с автомобилями.</param>
        /// <param name="promoService">Сервис для работы с промокодами.</param>
        public MenuController(ICarService carService, IPromoService promoService)
        {
            _carService = carService;
            _promoService = promoService;
        }

        /// <summary>
        /// Запускает главный цикл консольного меню приложения.
        /// </summary>
        public void Run()
        {
            bool exitRequested = false;
            while (!exitRequested)
            {
                DisplayMenu();
                string userInput = Console.ReadLine();

                switch (userInput)
                {
                    case "1": ShowAllCars(); break;
                    case "2": CreateNewCar(); break;
                    case "3": ShowCarDetails(); break;
                    case "4": UpdateCar(); break;
                    case "5": DeleteCar(); break;
                    case "6": ShowAvailableCars(); break;
                    case "7": RentCar(); break;
                    case "8": CalculateCost(); break;
                    case "0": exitRequested = true; Console.WriteLine("Выход из программы..."); break;
                    default: Console.WriteLine("Неизвестная команда. Нажмите любую клавишу для продолжения..."); Console.ReadKey(); break;
                }
                Console.WriteLine();
            }
        }

        private void DisplayMenu()
        {
            Console.Clear();
            Console.WriteLine("=== Система управления каршерингом ===");
            Console.WriteLine("1. Показать все автомобили");
            Console.WriteLine("2. Добавить новый автомобиль");
            Console.WriteLine("3. Найти автомобиль по ID");
            Console.WriteLine("4. Обновить данные автомобиля");
            Console.WriteLine("5. Удалить автомобиль");
            Console.WriteLine("6. Показать доступные автомобили");
            Console.WriteLine("7. Арендовать автомобиль");
            Console.WriteLine("8. Рассчитать стоимость аренды");
            Console.WriteLine("0. Выход");
            Console.Write("Ваш выбор: ");
        }

        // Ниже идут методы взаимодействия с ICarService — копируются из предыдущей Program.cs
        // Метод для показа всех автомобилей
        private void ShowAllCars()
        {
            Console.Clear();
            Console.WriteLine("=== Список всех автомобилей ===");
            var carDescriptions = _carService.GetAllCarsDescriptions();
            if (carDescriptions.Count == 0)
                Console.WriteLine("В системе нет автомобилей.");
            else
                foreach (var description in carDescriptions)
                    Console.WriteLine(description);
            WaitForUser();
        }
        // Метод для добавления нового автомобиля (с интерактивом)
        private void CreateNewCar()
        {
            Console.Clear();
            Console.WriteLine("=== Добавление нового автомобиля ===");
            try
            {
                Console.Write("Марка: ");
                string brand = ReadNonEmptyString();
                Console.Write("Модель: ");
                string model = ReadNonEmptyString();
                Console.Write("Гос. номер: ");
                string licensePlate = ReadNonEmptyString();
                Console.Write("Год выпуска: ");
                int year = ReadPositiveInteger();
                Console.Write("Пробег (км): ");
                int mileage = ReadNonNegativeInteger();
                Console.Write("Стоимость аренды в час: ");
                decimal price = ReadPositiveDecimal();
                _carService.CreateCar(brand, model, licensePlate, year, mileage, price);
                Console.WriteLine($"\nАвтомобиль успешно добавлен!");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"\nОшибка: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nПроизошла непредвиденная ошибка: {ex.Message}");
            }
            WaitForUser();
        }
        private void ShowCarDetails()
        {
            Console.Clear();
            Console.WriteLine("=== Поиск автомобиля по ID ===");
            Console.Write("Введите ID автомобиля: ");
            if (int.TryParse(Console.ReadLine(), out int id))
                Console.WriteLine(_carService.GetCarDescription(id));
            else
                Console.WriteLine("Некорректный формат ID.");
            WaitForUser();
        }
        private void UpdateCar()
        {
            Console.Clear();
            Console.WriteLine("=== Обновление данных автомобиля ===");
            Console.Write("Введите ID автомобиля для обновления: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Некорректный формат ID."); WaitForUser(); return;
            }
            var currentValues = _carService.GetCarValuesForEdit(id);
            if (currentValues == null)
            {
                Console.WriteLine($"Автомобиль с ID {id} не найден."); WaitForUser(); return;
            }
            string currentBrand = (string)currentValues[0];
            string currentModel = (string)currentValues[1];
            string currentLicensePlate = (string)currentValues[2];
            int currentYear = (int)currentValues[3];
            int currentMileage = (int)currentValues[4];
            decimal currentPrice = (decimal)currentValues[5];
            int currentStatus = (int)currentValues[6];
            Console.WriteLine("\nТекущие данные:");
            Console.WriteLine(_carService.GetCarDescription(id));
            Console.WriteLine("\nВведите новые данные (оставьте поле пустым, чтобы оставить текущее значение):");
            try
            {
                Console.Write($"Марка [{currentBrand}]: ");
                string brandInput = Console.ReadLine();
                string brand = string.IsNullOrWhiteSpace(brandInput) ? currentBrand : brandInput.Trim();

                Console.Write($"Модель [{currentModel}]: ");
                string modelInput = Console.ReadLine();
                string model = string.IsNullOrWhiteSpace(modelInput) ? currentModel : modelInput.Trim();

                Console.Write($"Гос. номер [{currentLicensePlate}]: ");
                string licensePlateInput = Console.ReadLine();
                string licensePlate = string.IsNullOrWhiteSpace(licensePlateInput) ? currentLicensePlate : licensePlateInput.Trim();

                Console.Write($"Год выпуска [{currentYear}]: ");
                string yearInput = Console.ReadLine();
                int year = string.IsNullOrWhiteSpace(yearInput) ? currentYear : ReadPositiveInteger(yearInput);

                Console.Write($"Пробег (км) [{currentMileage}]: ");
                string mileageInput = Console.ReadLine();
                int mileage = string.IsNullOrWhiteSpace(mileageInput) ? currentMileage : ReadNonNegativeInteger(mileageInput);

                Console.Write($"Стоимость аренды в час [{currentPrice}]: ");
                string priceInput = Console.ReadLine();
                decimal price = string.IsNullOrWhiteSpace(priceInput) ? currentPrice : ReadPositiveDecimal(priceInput);

                Console.Write($"Статус (0 - Свободен, 1 - В аренде, 2 - На тех. обслуживании) [{currentStatus}]: ");
                string statusInput = Console.ReadLine();
                int status = string.IsNullOrWhiteSpace(statusInput) ? currentStatus : ReadStatus(statusInput);

                if (_carService.UpdateCarDetails(id, brand, model, licensePlate, year, mileage, price, status))
                    Console.WriteLine($"\nДанные автомобиля с ID {id} успешно обновлены!");
                else
                    Console.WriteLine($"\nНе удалось обновить автомобиль с ID {id}. Проверьте уникальность гос. номера.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nОшибка при обновлении: {ex.Message}");
            }
            WaitForUser();
        }
        private void DeleteCar()
        {
            Console.Clear();
            Console.WriteLine("=== Удаление автомобиля ===");
            Console.Write("Введите ID автомобиля для удаления: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                if (_carService.DeleteCar(id))
                    Console.WriteLine($"Автомобиль с ID {id} успешно удален.");
                else
                    Console.WriteLine($"Не удалось удалить автомобиль с ID {id}. Возможно, он не существует или арендован.");
            }
            else Console.WriteLine("Некорректный формат ID.");
            WaitForUser();
        }
        private void ShowAvailableCars()
        {
            Console.Clear();
            Console.WriteLine("=== Доступные для аренды автомобили ===");
            var availableDescriptions = _carService.GetAvailableCarsDescriptions();
            if (availableDescriptions.Count == 0)
                Console.WriteLine("Нет доступных автомобилей.");
            else
                foreach (var description in availableDescriptions)
                    Console.WriteLine(description);
            WaitForUser();
        }
        private void RentCar()
        {
            Console.Clear();
            Console.WriteLine("=== Аренда автомобиля ===");
            Console.Write("Введите ID автомобиля для аренды: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                if (_carService.RentCar(id))
                    Console.WriteLine($"Автомобиль с ID {id} успешно арендован!");
                else
                    Console.WriteLine($"Не удалось арендовать автомобиль с ID {id}. Возможно, он уже арендован или не существует.");
            }
            else Console.WriteLine("Некорректный формат ID.");
            WaitForUser();
        }
        private void CalculateCost()
        {
            Console.Clear();
            Console.WriteLine("=== Расчет стоимости аренды ===");
            Console.Write("Введите ID автомобиля: ");
            if (!int.TryParse(Console.ReadLine(), out int carId))
            {
                Console.WriteLine("Некорректный формат ID."); WaitForUser(); return;
            }
            Console.Write("Введите количество часов аренды: ");
            if (!int.TryParse(Console.ReadLine(), out int hours) || hours <= 0)
            {
                Console.WriteLine("Количество часов должно быть положительным числом."); WaitForUser(); return;
            }
            Console.Write("Введите промокод (или оставьте пустым): ");
            string promoCode = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(promoCode))
                promoCode = null;
            try
            {
                decimal cost = _carService.CalculateRentalCost(carId, hours, promoCode);
                Console.WriteLine($"\nСтоимость аренды на {hours} часов: {cost:C}");
                if (!string.IsNullOrEmpty(promoCode))
                    Console.WriteLine($"Промокод '{promoCode}' применен успешно!");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
            WaitForUser();
        }
        // ----- Утилиты меню -----
        private string ReadNonEmptyString()
        {
            string input;
            do
            {
                input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                    Console.WriteLine("Поле не может быть пустым. Повторите ввод:");
            } while (string.IsNullOrWhiteSpace(input));
            return input.Trim();
        }
        private int ReadPositiveInteger()
        {
            int result;
            while (!int.TryParse(Console.ReadLine(), out result) || result <= 0)
                Console.WriteLine("Некорректный ввод. Введите положительное целое число:");
            return result;
        }
        private int ReadNonNegativeInteger()
        {
            int result;
            while (!int.TryParse(Console.ReadLine(), out result) || result < 0)
                Console.WriteLine("Некорректный ввод. Введите неотрицательное целое число:");
            return result;
        }
        private decimal ReadPositiveDecimal()
        {
            decimal result;
            while (!decimal.TryParse(Console.ReadLine(), out result) || result <= 0)
                Console.WriteLine("Некорректный ввод. Введите положительное число:");
            return result;
        }
        private int ReadPositiveInteger(string input)
        {
            int result;
            while (!int.TryParse(input, out result) || result <= 0)
            {
                Console.WriteLine("Некорректный ввод. Введите положительное целое число:");
                input = Console.ReadLine();
            }
            return result;
        }
        private int ReadNonNegativeInteger(string input)
        {
            int result;
            while (!int.TryParse(input, out result) || result < 0)
            {
                Console.WriteLine("Некорректный ввод. Введите неотрицательное целое число:");
                input = Console.ReadLine();
            }
            return result;
        }
        private decimal ReadPositiveDecimal(string input)
        {
            decimal result;
            while (!decimal.TryParse(input, out result) || result <= 0)
            {
                Console.WriteLine("Некорректный ввод. Введите положительное число:");
                input = Console.ReadLine();
            }
            return result;
        }
        private int ReadStatus(string input)
        {
            int result;
            while (!int.TryParse(input, out result) || result < 0 || result > 2)
            {
                Console.WriteLine("Некорректный ввод. Введите 0, 1 или 2:");
                input = Console.ReadLine();
            }
            return result;
        }
        private void WaitForUser()
        {
            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }
    }
}
