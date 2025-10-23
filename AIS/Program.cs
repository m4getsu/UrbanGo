using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using BussinessLogic;
using DataAccessLayer;
using Microsoft.EntityFrameworkCore;

namespace AIS
{
    /// <summary>
    /// Точка входа WinForms-приложения и обработчики глобальных исключений.
    /// </summary>
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += Application_ThreadException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            var dataProvider = ChooseDataProvider();
            if (dataProvider == null)
            {
                return; 
            }

            var promoCodeRepository = CreatePromoCodeRepository(dataProvider);
            var promoService = new PromoService(promoCodeRepository);
            var carService = new CarService(dataProvider, promoService);

            var mainForm = new MainForm(dataProvider, promoService);
            Application.Run(mainForm);
        }

        /// <summary>
        /// Создает репозиторий промокодов в зависимости от выбранного провайдера данных.
        /// </summary>
        /// <param name="carRepository">Репозиторий автомобилей для определения типа провайдера.</param>
        /// <returns>Репозиторий промокодов.</returns>
        private static IPromoCodeRepository CreatePromoCodeRepository(IRepository<Model.Car> carRepository)
        {
            const string connectionString = "Server=(localdb)\\mssqllocaldb;Database=UrbanGoDB;Trusted_Connection=true;TrustServerCertificate=true;";

            if (carRepository is EntityRepository<Model.Car>)
            {
                var options = new DbContextOptionsBuilder<CarSharingContext>()
                    .UseSqlServer(connectionString)
                    .Options;
                
                var context = new CarSharingContext(options);
                return new EFPromoCodeRepository(context);
            }
            else
            {
                return new DapperPromoCodeRepository(connectionString);
            }
        }

        /// <summary>
        /// Показывает диалог выбора провайдера данных.
        /// </summary>
        /// <returns>Выбранный провайдер данных или null, если пользователь отменил.</returns>
        private static IRepository<Model.Car> ChooseDataProvider()
        {
            var result = MessageBox.Show(
                "Выберите провайдер данных:\n\n" +
                "Да - Entity Framework\n" +
                "Нет - Dapper\n" +
                "Отмена - Выход из приложения",
                "Выбор провайдера данных",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question);

            if (result == DialogResult.Cancel)
            {
                return null;
            }

            const string connectionString = "Server=(localdb)\\mssqllocaldb;Database=UrbanGoDB;Trusted_Connection=true;TrustServerCertificate=true;";

            if (result == DialogResult.Yes)
            {
                var options = new DbContextOptionsBuilder<CarSharingContext>()
                    .UseSqlServer(connectionString)
                    .Options;
                
                var context = new CarSharingContext(options);
                return new EntityRepository<Model.Car>(context);
            }
            else
            {
                return new DapperRepository<Model.Car>(connectionString);
            }
        }

        /// <summary>
        /// Обработчик необработанных исключений в UI-потоке.
        /// Показывает сообщение об ошибке пользователю.
        /// </summary>
        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            MessageBox.Show($"Произошла непредвиденная ошибка:\n{e.Exception.Message}",
                "Критическая ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Обработчик необработанных исключений домена приложения (не UI-поток).
        /// Показывает сообщение об ошибке при критических сбоях.
        /// </summary>
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception ex)
            {
                MessageBox.Show($"Произошла критическая ошибка:\n{ex.Message}",
                    "Критическая ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
