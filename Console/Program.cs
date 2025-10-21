using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BussinessLogic;

namespace ConsoleApp
{
    /// <summary>
    /// Главный класс консольного приложения для управления системой каршеринга.
    /// Предоставляет пользовательский интерфейс для работы с автомобилями.
    /// </summary>
    internal class Program
    {
        private static readonly ICarService _carService = new CarService();

        /// <summary>
        /// Главная точка входа в приложение.
        /// </summary>
        /// <param name="args">Аргументы командной строки.</param>
        static void Main(string[] args)
        {
            bool exitRequested = false;

            while (!exitRequested)
            {
                DisplayMenu();
                string userInput = Console.ReadLine();

                switch (userInput)
                {
                    case "1":
                        ShowAllCars();
                        break;
                    case "2":
                        CreateNewCar();
                        break;
                    case "3":
                        ShowCarDetails();
                        break;
                    case "4":
                        UpdateCar();
                        break;
                    case "5":
                        DeleteCar();
                        break;
                    case "6":
                        ShowAvailableCars();
                        break;
                    case "7":
                        RentCar();
                        break;
                    case "8":
                        CalculateCost();
                        break;
                    case "0":
                        exitRequested = true;
                        Console.WriteLine("Выход из программы...");
                        break;
                    default:
                        Console.WriteLine("Неизвестная команда. Нажмите любую клавишу для продолжения...");
                        Console.ReadKey();
                        break;
                }

                Console.WriteLine();
            }
        }

        /// <summary>
        /// Отображает главное меню приложения.
        /// </summary>
        private static void DisplayMenu()
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

        /// <summary>
        /// Отображает список всех автомобилей в системе.
        /// </summary>
        private static void ShowAllCars()
        {
            Console.Clear();
            Console.WriteLine("=== Список всех автомобилей ===");

            var carDescriptions = _carService.GetAllCarsDescriptions();

            if (carDescriptions.Count == 0)
            {
                Console.WriteLine("В системе нет автомобилей.");
            }
            else
            {
                foreach (var description in carDescriptions)
                {
                    Console.WriteLine(description);
                }
            }
            WaitForUser();
        }

        /// <summary>
        /// Создает новый автомобиль на основе введенных пользователем данных.
        /// </summary>
        private static void CreateNewCar()
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

                var newCar = _carService.CreateCar(brand, model, licensePlate, year, mileage, price);

                Console.WriteLine($"\nАвтомобиль успешно добавлен с ID {newCar.Id}!");
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

        /// <summary>
        /// Отображает детальную информацию об автомобиле по указанному ID.
        /// </summary>
        private static void ShowCarDetails()
        {
            Console.Clear();
            Console.WriteLine("=== Поиск автомобиля по ID ===");

            Console.Write("Введите ID автомобиля: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var description = _carService.GetCarDescription(id);
                Console.WriteLine(description);
            }
            else
            {
                Console.WriteLine("Некорректный формат ID.");
            }
            WaitForUser();
        }

        /// <summary>
        /// Обновляет данные существующего автомобиля.
        /// </summary>

        private static void UpdateCar()
        {
            Console.Clear();
            Console.WriteLine("=== Обновление данных автомобиля ===");

            Console.Write("Введите ID автомобиля для обновления: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Некорректный формат ID.");
                WaitForUser();
                return;
            }
            var currentValues = _carService.GetCarValuesForEdit(id);
            if (currentValues == null)
            {
                Console.WriteLine($"Автомобиль с ID {id} не найден.");
                WaitForUser();
                return;
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
                {
                    Console.WriteLine($"\nДанные автомобиля с ID {id} успешно обновлены!");
                }
                else
                {
                    Console.WriteLine($"\nНе удалось обновить автомобиль с ID {id}. Проверьте уникальность гос. номера.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nОшибка при обновлении: {ex.Message}");
            }
            WaitForUser();
        }

        /// <summary>
        /// Удаляет автомобиль из системы по указанному ID.
        /// </summary>
        private static void DeleteCar()
        {
            Console.Clear();
            Console.WriteLine("=== Удаление автомобиля ===");

            Console.Write("Введите ID автомобиля для удаления: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                if (_carService.DeleteCar(id))
                {
                    Console.WriteLine($"Автомобиль с ID {id} успешно удален.");
                }
                else
                {
                    Console.WriteLine($"Не удалось удалить автомобиль с ID {id}. Возможно, он не существует или арендован.");
                }
            }
            else
            {
                Console.WriteLine("Некорректный формат ID.");
            }
            WaitForUser();
        }

        /// <summary>
        /// Отображает список автомобилей, доступных для аренды.
        /// </summary>
        private static void ShowAvailableCars()
        {
            Console.Clear();
            Console.WriteLine("=== Доступные для аренды автомобили ===");

            var availableDescriptions = _carService.GetAvailableCarsDescriptions();

            if (availableDescriptions.Count == 0)
            {
                Console.WriteLine("Нет доступных автомобилей.");
            }
            else
            {
                foreach (var description in availableDescriptions)
                {
                    Console.WriteLine(description);
                }
            }
            WaitForUser();
        }

        /// <summary>
        /// Выполняет операцию аренды автомобиля по указанному ID.
        /// </summary>
        private static void RentCar()
        {
            Console.Clear();
            Console.WriteLine("=== Аренда автомобиля ===");

            Console.Write("Введите ID автомобиля для аренды: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                if (_carService.RentCar(id))
                {
                    Console.WriteLine($"Автомобиль с ID {id} успешно арендован!");
                }
                else
                {
                    Console.WriteLine($"Не удалось арендовать автомобиль с ID {id}. Возможно, он уже арендован или не существует.");
                }
            }
            else
            {
                Console.WriteLine("Некорректный формат ID.");
            }
            WaitForUser();
        }

        /// <summary>
        /// Рассчитывает стоимость аренды автомобиля на указанное количество часов.
        /// </summary>
        private static void CalculateCost()
        {
            Console.Clear();
            Console.WriteLine("=== Расчет стоимости аренды ===");

            Console.Write("Введите ID автомобиля: ");
            if (!int.TryParse(Console.ReadLine(), out int carId))
            {
                Console.WriteLine("Некорректный формат ID.");
                WaitForUser();
                return;
            }

            Console.Write("Введите количество часов аренды: ");
            if (!int.TryParse(Console.ReadLine(), out int hours) || hours <= 0)
            {
                Console.WriteLine("Количество часов должно быть положительным числом.");
                WaitForUser();
                return;
            }

            try
            {
                decimal cost = _carService.CalculateRentalCost(carId, hours);
                Console.WriteLine($"\nСтоимость аренды на {hours} часов: {cost:C}");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
            WaitForUser();
        }

        /// <summary>
        /// Считывает непустую строку из консоли, запрашивая повторный ввод при пустом значении.
        /// </summary>
        /// <returns>Очищенная от пробелов непустая строка.</returns>
        private static string ReadNonEmptyString()
        {
            string input;
            do
            {
                input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Поле не может быть пустым. Повторите ввод:");
                }
            } while (string.IsNullOrWhiteSpace(input));

            return input.Trim();
        }

        /// <summary>
        /// Считывает положительное целое число из консоли.
        /// При ошибке запрашивает повторный ввод.
        /// </summary>
        /// <returns>Положительное целое число.</returns>
        private static int ReadPositiveInteger()
        {
            int result;
            while (!int.TryParse(Console.ReadLine(), out result) || result <= 0)
            {
                Console.WriteLine("Некорректный ввод. Введите положительное целое число:");
            }
            return result;
        }

        /// <summary>
        /// Считывает неотрицательное целое число из консоли.
        /// При ошибке запрашивает повторный ввод.
        /// </summary>
        /// <returns>Неотрицательное целое число.</returns>
        private static int ReadNonNegativeInteger()
        {
            int result;
            while (!int.TryParse(Console.ReadLine(), out result) || result < 0)
            {
                Console.WriteLine("Некорректный ввод. Введите неотрицательное целое число:");
            }
            return result;
        }

        /// <summary>
        /// Считывает положительное десятичное число из консоли.
        /// При ошибке запрашивает повторный ввод.
        /// </summary>
        /// <returns>Положительное десятичное число.</returns>
        private static decimal ReadPositiveDecimal()
        {
            decimal result;
            while (!decimal.TryParse(Console.ReadLine(), out result) || result <= 0)
            {
                Console.WriteLine("Некорректный ввод. Введите положительное число:");
            }
            return result;
        }

        /// <summary>
        /// Парсит положительное целое число из строки; при ошибке запрашивает повторный ввод в консоли.
        /// </summary>
        /// <param name="input">Исходная строка для парсинга.</param>
        /// <returns>Положительное целое число.</returns>
        private static int ReadPositiveInteger(string input)
        {
            int result;
            while (!int.TryParse(input, out result) || result <= 0)
            {
                Console.WriteLine("Некорректный ввод. Введите положительное целое число:");
                input = Console.ReadLine();
            }
            return result;
        }

        /// <summary>
        /// Парсит неотрицательное целое число из строки; при ошибке запрашивает повторный ввод в консоли.
        /// </summary>
        /// <param name="input">Исходная строка для парсинга.</param>
        /// <returns>Неотрицательное целое число.</returns>
        private static int ReadNonNegativeInteger(string input)
        {
            int result;
            while (!int.TryParse(input, out result) || result < 0)
            {
                Console.WriteLine("Некорректный ввод. Введите неотрицательное целое число:");
                input = Console.ReadLine();
            }
            return result;
        }

        /// <summary>
        /// Парсит положительное десятичное число из строки; при ошибке запрашивает повторный ввод в консоли.
        /// </summary>
        /// <param name="input">Исходная строка для парсинга.</param>
        /// <returns>Положительное десятичное число.</returns>
        private static decimal ReadPositiveDecimal(string input)
        {
            decimal result;
            while (!decimal.TryParse(input, out result) || result <= 0)
            {
                Console.WriteLine("Некорректный ввод. Введите положительное число:");
                input = Console.ReadLine();
            }
            return result;
        }

        /// <summary>
        /// Считывает статус автомобиля из консоли.
        /// </summary>
        /// <returns>Числовое значение статуса.</returns>
        private static int ReadStatus()
        {
            int result;
            while (!int.TryParse(Console.ReadLine(), out result) || result < 0 || result > 2)
            {
                Console.WriteLine("Некорректный ввод. Введите 0, 1 или 2:");
            }
            return result;
        }

        /// <summary>
        /// Считывает статус автомобиля из строки или запрашивает ввод с консоли.
        /// </summary>
        /// <param name="input">Входная строка для парсинга.</param>
        /// <returns>Числовое значение статуса.</returns>
        private static int ReadStatus(string input)
        {
            int result;
            while (!int.TryParse(input, out result) || result < 0 || result > 2)
            {
                Console.WriteLine("Некорректный ввод. Введите 0, 1 или 2:");
                input = Console.ReadLine();
            }
            return result;
        }

        /// <summary>
        /// Ожидает нажатия любой клавиши пользователем для продолжения работы.
        /// </summary>
        private static void WaitForUser()
        {
            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }
    }
}