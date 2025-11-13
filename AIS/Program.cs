using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using BussinessLogic;
using Ninject;
using AIS.Controllers;


namespace AIS
{
    /// <summary>
    /// Точка входа WinForms-приложения и обработчики глобальных исключений.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// Точка входа приложения. Инициализирует UI, обработчики исключений и контейнер зависимостей.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += Application_ThreadException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            var useEF = ChooseProvider();
            if (useEF == null)
            {
                return; 
            }

            var config = new AppConfiguration();
            var di = new DependencyContainer(useEF.Value, config);
            var controller = new MainFormController(di.CarService);
            var mainForm = new MainForm(controller, di);
            Application.Run(mainForm);
        }

        /// <summary>
        /// Показывает диалог выбора провайдера данных (Entity Framework или Dapper).
        /// </summary>
        /// <returns>True для EF, False для Dapper, null при отмене.</returns>
        private static bool? ChooseProvider()
        {
            var result = MessageBox.Show(
                "Выберите провайдер данных:\n\n" +
                "Да - Entity Framework\n" +
                "Нет - Dapper\n" +
                "Отмена - Выход из приложения",
                "Выбор провайдера данных",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question);

            if (result == DialogResult.Cancel) return null;
            if (result == DialogResult.Yes) return true;
            return false;
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
