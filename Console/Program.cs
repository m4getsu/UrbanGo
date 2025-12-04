using System;
using BussinessLogic;
using Ninject;
using ConsoleApp;

/// <summary>
/// Точка входа консольного приложения для управления системой каршеринга.
/// </summary>
class Program
{
    /// <summary>
    /// Точка входа консольного приложения. Инициализирует контейнер зависимостей и запускает меню.
    /// </summary>
    /// <param name="args">Аргументы командной строки.</param>
    static void Main(string[] args)
    {
        var useEF = ChooseProvider();
        if (useEF == null)
        {
            Console.WriteLine("Выход из программы...");
            return;
        }

        var useDynamicPricing = ChoosePricingStrategy();

        IConfiguration config = new AppConfiguration();
        var di = new DependencyContainer(config, useEF.Value, useDynamicPricing);
        var menu = new MenuController(di.CarService, di.PromoService);
        menu.Run();
    }

    /// <summary>
    /// Показывает меню выбора провайдера данных (Entity Framework или Dapper).
    /// </summary>
    /// <returns>True для EF, False для Dapper, null при выходе.</returns>
    private static bool? ChooseProvider()
    {
        Console.Clear();
        Console.WriteLine("=== Выбор провайдера данных ===");
        Console.WriteLine("1 - Entity Framework");
        Console.WriteLine("2 - Dapper");
        Console.WriteLine("0 - Выход");
        Console.Write("Ваш выбор: ");
        string userInput = Console.ReadLine();
        if (userInput == "0") return null;
        if (userInput == "1") return true;
        if (userInput == "2") return false;
        Console.WriteLine("Неверный выбор. Нажмите любую клавишу и попробуйте снова...");
        Console.ReadKey();
        return ChooseProvider();
    }

    /// <summary>
    /// Показывает меню выбора стратегии ценообразования.
    /// </summary>
    /// <returns>True для динамического ценообразования, False для стандартного.</returns>
    private static bool ChoosePricingStrategy()
    {
        Console.Clear();
        Console.WriteLine("=== Выбор стратегии ценообразования ===");
        Console.WriteLine("1 - Стандартная (цена × часы)");
        Console.WriteLine("2 - Динамическая (время суток, день недели, сезон, праздники)");
        Console.Write("Ваш выбор: ");
        string userInput = Console.ReadLine();
        if (userInput == "2")
        {
            Console.WriteLine("Выбрана стратегия: Динамическая 🔥");
            return true;
        }
        else
        {
            Console.WriteLine("Выбрана стратегия: Стандартная 💵");
            return false;
        }
    }
}
