using System;
using System.Windows.Forms;
using Ninject;
using BussinessLogic;
using BussinessLogic.Services.Import;
using Model;
using Shared;
using AIS;
using AIS.Forms;

namespace Presenter
{
    /// <summary>
    /// Главная точка входа приложения (Presenter).
    /// Консольное приложение с выбором режима запуска: WinForms или Console.
    /// </summary>
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("==============================================");
            Console.WriteLine("  UrbanGo - Система управления каршерингом");
            Console.WriteLine("  Архитектура: MVP (Model-View-Presenter)");
            Console.WriteLine("==============================================");
            Console.WriteLine();

            // Выбор режима запуска
            Console.WriteLine("Выберите режим запуска:");
            Console.WriteLine("1. WinForms (графический интерфейс)");
            Console.WriteLine("2. Console (консольный интерфейс)");
            Console.WriteLine("0. Выход");
            Console.WriteLine();
            Console.Write("Ваш выбор: ");

            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    RunWinFormsMode();
                    break;
                case "2":
                    RunConsoleMode();
                    break;
                case "0":
                    Console.WriteLine("Выход из приложения...");
                    break;
                default:
                    Console.WriteLine("Неверный выбор!");
                    break;
            }
        }

        /// <summary>
        /// Запускает приложение в режиме WinForms.
        /// </summary>
        static void RunWinFormsMode()
        {
            Console.WriteLine();
            Console.WriteLine("Запуск WinForms приложения...");
            Console.WriteLine();

            // Выбор ORM провайдера
            Console.WriteLine("Выберите провайдер данных:");
            Console.WriteLine("1. Entity Framework Core");
            Console.WriteLine("2. Dapper");
            Console.Write("Ваш выбор: ");

            string? ormChoice = Console.ReadLine();
            bool useEF = ormChoice == "1";

            Console.WriteLine($"Выбран провайдер: {(useEF ? "Entity Framework" : "Dapper")}");
            Console.WriteLine();

            // Выбор стратегии ценообразования
            Console.WriteLine("Выберите стратегию ценообразования:");
            Console.WriteLine("1. Стандартная (цена × часы)");
            Console.WriteLine("2. Динамическая (время суток, день недели, сезон, праздники)");
            Console.Write("Ваш выбор: ");

            string? pricingChoice = Console.ReadLine();
            bool useDynamicPricing = pricingChoice == "2";

            Console.WriteLine($"Выбрана стратегия: {(useDynamicPricing ? "Динамическая 🔥" : "Стандартная 💵")}");
            Console.WriteLine();

            try
            {
                // Настройка DI контейнера с выбранной стратегией ценообразования
                string connectionString = "Server=(localdb)\\mssqllocaldb;Database=UrbanGoDB;Trusted_Connection=True;";
                var kernel = new StandardKernel(new BussinessLogic.SimpleConfigModule(useEF, connectionString, useDynamicPricing));

                // Получаем сервисы из контейнера
                var carService = kernel.Get<ICarService>();
                var promoService = kernel.Get<IPromoService>();
                var importService = kernel.Get<BussinessLogic.Services.Import.ICarImportService>();

                // Создаем модель
                var model = new CarSharingModel(carService, promoService, importService);

                Console.WriteLine("Инициализация завершена. Открытие главной формы...");
                Console.WriteLine();

                // Настройка WinForms (ВАЖНО: до создания форм!)
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                // Создаем главное представление
                var mainForm = new MainForm();

                // Фабрики для дочерних представлений
                Func<ICarEditView> carEditFactory = () => new CarEditForm();
                Func<int, ICalculateCostView> calcFactory = (carId) =>
                {
                    var calcController = new AIS.Controllers.CalculateCostFormController(carService);
                    return new CalculateCostForm(carId, calcController);
                };
                Func<ICarImportView> importFactory = () => new CarImportForm(importService);

                // Создаем главный presenter
                var mainPresenter = new MainPresenter(mainForm, model, carEditFactory, calcFactory, importFactory);

                Console.WriteLine("✓ MVP архитектура инициализирована!");
                Console.WriteLine("✓ MainPresenter создан и подписан на события View и Model");
                Console.WriteLine();
                Console.WriteLine("Запуск WinForms приложения...");

                // Запуск приложения
                Application.Run(mainForm);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ОШИБКА при запуске: {ex.Message}");
                Console.WriteLine();
                Console.WriteLine("Нажмите любую клавишу для выхода...");
                Console.ReadKey();
            }
        }

        /// <summary>
        /// Запускает приложение в режиме Console.
        /// Делегирует работу консольного интерфейса существующему Console проекту.
        /// </summary>
        static void RunConsoleMode()
        {
            Console.WriteLine();
            Console.WriteLine("Запуск консольного приложения...");
            Console.WriteLine();

            // Выбор ORM провайдера
            Console.WriteLine("Выберите провайдер данных:");
            Console.WriteLine("1. Entity Framework Core");
            Console.WriteLine("2. Dapper");
            Console.Write("Ваш выбор: ");

            string? ormChoice = Console.ReadLine();
            bool useEF = ormChoice == "1";

            Console.WriteLine($"Выбран провайдер: {(useEF ? "Entity Framework" : "Dapper")}");
            Console.WriteLine();

            // Выбор стратегии ценообразования
            Console.WriteLine("Выберите стратегию ценообразования:");
            Console.WriteLine("1. Стандартная (цена × часы)");
            Console.WriteLine("2. Динамическая (время суток, день недели, сезон, праздники)");
            Console.Write("Ваш выбор: ");

            string? pricingChoice = Console.ReadLine();
            bool useDynamicPricing = pricingChoice == "2";

            Console.WriteLine($"Выбрана стратегия: {(useDynamicPricing ? "Динамическая 🔥" : "Стандартная 💵")}");
            Console.WriteLine();

            try
            {
                // Настройка DI контейнера через Console проект
                var config = new ConsoleApp.AppConfiguration();
                var di = new ConsoleApp.DependencyContainer(config, useEF, useDynamicPricing);

                Console.WriteLine("Инициализация завершена!");
                Console.WriteLine();

                // Запуск консольного меню из Console проекта
                var menu = new ConsoleApp.MenuController(di.CarService, di.PromoService);
                menu.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ОШИБКА при запуске: {ex.Message}");
                Console.WriteLine();
                Console.WriteLine("Нажмите любую клавишу для выхода...");
                Console.ReadKey();
            }
        }
    }
}
