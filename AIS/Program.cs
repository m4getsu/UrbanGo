using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using BussinessLogic;

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

            Application.Run(new MainForm());
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
