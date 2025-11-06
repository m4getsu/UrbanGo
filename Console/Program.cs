using System;
using BussinessLogic;
using Ninject;
using ConsoleApp;

class Program
{
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