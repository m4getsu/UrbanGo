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

            Console.WriteLine("Выберите провайдер данных:");
            Console.WriteLine("1. Entity Framework Core");
            Console.WriteLine("2. Dapper");
            Console.Write("Ваш выбор: ");

            string? ormChoice = Console.ReadLine();
            bool useEF = ormChoice == "1";

            Console.WriteLine($"Выбран провайдер: {(useEF ? "Entity Framework" : "Dapper")}");
            Console.WriteLine();

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
                string connectionString = "Server=(localdb)\\mssqllocaldb;Database=UrbanGoDB;Trusted_Connection=True;";
                var kernel = new StandardKernel(new BussinessLogic.SimpleConfigModule(useEF, connectionString, useDynamicPricing));

                var carService = kernel.Get<ICarService>();
                var exportService = kernel.Get<BussinessLogic.Services.Import.ICarImportService>();
                var importService = kernel.Get<BussinessLogic.Services.Import.ICarImportService>();

                Console.WriteLine("Инициализация завершена. Открытие главной формы...");
                Console.WriteLine();

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                var mainForm = new MainForm();

                Func<ICarEditView> carEditFactory = () => new CarEditForm();
                Func<int, ICalculateCostView> calcFactory = (carId) =>
                {
                    var calcView = new CalculateCostForm(carId);
                    var calcPresenter = new CalculateCostPresenter(calcView, carService);
                    return calcView;
                };
                Func<ICarImportView> importFactory = () =>
                {
                    var importView = new CarImportForm();
                    var importPresenter = new CarImportPresenter(importView, importService);
                    return importView;
                };

                var mainPresenter = new MainPresenter(mainForm, carService, exportService, carEditFactory, calcFactory, importFactory);

                Console.WriteLine("✓ MVP архитектура инициализирована!");
                Console.WriteLine("✓ MainPresenter создан и подписан на события View");
                Console.WriteLine();
                Console.WriteLine("Запуск WinForms приложения...");

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

            Console.WriteLine("Выберите провайдер данных:");
            Console.WriteLine("1. Entity Framework Core");
            Console.WriteLine("2. Dapper");
            Console.Write("Ваш выбор: ");

            string? ormChoice = Console.ReadLine();
            bool useEF = ormChoice == "1";

            Console.WriteLine($"Выбран провайдер: {(useEF ? "Entity Framework" : "Dapper")}");
            Console.WriteLine();

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
                var config = new ConsoleApp.AppConfiguration();
                var di = new ConsoleApp.DependencyContainer(config, useEF, useDynamicPricing);

                Console.WriteLine("Инициализация завершена!");
                Console.WriteLine();

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
