using System;
using System.Windows;
using Ninject;
using BussinessLogic;
using BussinessLogic.Services.Import;
using BussinessLogic.Services.QRCode;
using Presenter.ViewModels;

namespace ViewWPF
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Вызывается при запуске приложения. Инициализирует DI контейнер и запускает главное окно.
        /// </summary>
        /// <param name="e">Аргументы запуска приложения.</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            try
            {
                // Показываем окно выбора конфигурации
                var configWindow = new ConfigurationWindow();
                if (configWindow.ShowDialog() != true)
                {
                    // Пользователь отменил - выходим из приложения
                    Shutdown();
                    return;
                }

                // Получаем выбранные настройки
                bool useEF = configWindow.UseEntityFramework;
                bool useDynamicPricing = configWindow.UseDynamicPricing;

                // Настройка Ninject для Dependency Injection
                string connectionString = "Server=(localdb)\\mssqllocaldb;Database=UrbanGoDB;Trusted_Connection=True;";

                var kernel = new StandardKernel(new BussinessLogic.SimpleConfigModule(useEF, connectionString, useDynamicPricing));
                var carService = kernel.Get<ICarService>();
                var importService = kernel.Get<ICarImportService>();
                var qrService = kernel.Get<IQRCodeService>();

                // Создаем ViewManager
                var viewManager = new ViewManager(carService, importService, qrService);

                // Регистрируем маппинги ViewModel -> View
                viewManager.RegisterViewMapping<MainViewModel, MainWindow>();
                viewManager.RegisterViewMapping<CarEditViewModel, CarEditWindow>();
                viewManager.RegisterViewMapping<CalculateCostViewModel, CalculateCostWindow>();
                viewManager.RegisterViewMapping<CarImportViewModel, CarImportWindow>();
                viewManager.RegisterViewMapping<CarQRViewModel, CarQRWindow>();

                // Запускаем приложение через ViewManager
                viewManager.Start<MainViewModel>();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Ошибка при запуске приложения:\n\n{ex.Message}\n\nСтек вызовов:\n{ex.StackTrace}",
                    "Ошибка запуска",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
                Shutdown();
            }
        }
    }
}
