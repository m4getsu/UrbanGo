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
        IConfiguration config = new AppConfiguration();
        var di = new DependencyContainer(config, useEF.Value);
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
}