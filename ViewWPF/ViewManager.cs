using System.Windows;
using Presenter;
using Presenter.Events;
using Presenter.ViewModels;
using BussinessLogic;
using BussinessLogic.Services.Import;
using BussinessLogic.Services.QRCode;

namespace ViewWPF
{
    /// <summary>
    /// Менеджер View, управляющий созданием окон и связыванием их с ViewModel.
    /// </summary>
    public class ViewManager
    {
        private readonly VMManager _vmManager;
        private readonly Dictionary<Type, Type> _viewMapping;
        private readonly Dictionary<BaseViewModel, Window> _openWindows;
        private readonly ICarImportService _importService;
        private readonly IQRCodeService _qrService;
        private Window? _mainWindow;

        /// <summary>
        /// Инициализирует ViewManager с бизнес-логикой.
        /// </summary>
        /// <param name="carService">Сервис управления автомобилями.</param>
        /// <param name="importService">Сервис импорта/экспорта автомобилей.</param>
        /// <param name="qrService">Сервис генерации QR-кодов.</param>
        public ViewManager(ICarService carService, ICarImportService importService, IQRCodeService qrService)
        {
            if (carService == null)
                throw new ArgumentNullException(nameof(carService));
            if (importService == null)
                throw new ArgumentNullException(nameof(importService));
            if (qrService == null)
                throw new ArgumentNullException(nameof(qrService));

            _vmManager = new VMManager(carService, importService, qrService);
            _vmManager.ViewModelReady += OnViewModelReady;
            _importService = importService;
            _qrService = qrService;

            _viewMapping = new Dictionary<Type, Type>();
            _openWindows = new Dictionary<BaseViewModel, Window>();
        }

        /// <summary>
        /// Регистрирует маппинг между ViewModel и View.
        /// </summary>
        /// <typeparam name="TViewModel">Тип ViewModel.</typeparam>
        /// <typeparam name="TView">Тип View.</typeparam>
        public void RegisterViewMapping<TViewModel, TView>()
            where TViewModel : BaseViewModel
            where TView : Window
        {
            _viewMapping[typeof(TViewModel)] = typeof(TView);
        }

        /// <summary>
        /// Запускает приложение, создавая главную ViewModel.
        /// </summary>
        /// <typeparam name="TViewModel">Тип главной ViewModel.</typeparam>
        public void Start<TViewModel>() where TViewModel : BaseViewModel
        {
            _vmManager.CreateViewModel<TViewModel>();
        }

        /// <summary>
        /// Обработчик события готовности ViewModel.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void OnViewModelReady(object? sender, ViewModelReadyEventArgs e)
        {
            var viewModelType = e.ViewModel.GetType();

            if (!_viewMapping.ContainsKey(viewModelType))
            {
                throw new InvalidOperationException(
                    $"Не найден маппинг для ViewModel типа {viewModelType.Name}. " +
                    $"Зарегистрируйте маппинг через RegisterViewMapping."
                );
            }

            var viewType = _viewMapping[viewModelType];
            ShowView(e.ViewModel, viewType);
        }

        /// <summary>
        /// Создает и показывает окно для ViewModel.
        /// </summary>
        /// <param name="viewModel">ViewModel для окна.</param>
        /// <param name="viewType">Тип окна.</param>
        private void ShowView(BaseViewModel viewModel, Type viewType)
        {
            var window = (Window)Activator.CreateInstance(viewType)!;
            window.DataContext = viewModel;

            if (window is MainWindow mainWindow)
            {
                mainWindow.SetImportService(_importService);
            }

            if (window is CarQRWindow qrWindow && viewModel is CarQRViewModel qrViewModel)
            {
                qrWindow.SetDependencies(_qrService, qrViewModel.GetCarDto());
            }

            _openWindows[viewModel] = window;

            window.Closed += (s, e) =>
            {
                _openWindows.Remove(viewModel);
                _vmManager.CloseViewModel(viewModel);

                if (viewModel is MainViewModel)
                {
                    Application.Current.Shutdown();
                }
            };

            if (viewModel is MainViewModel)
            {
                _mainWindow = window;
                window.Show();
            }
            else
            {
                if (_mainWindow != null)
                {
                    window.Owner = _mainWindow;
                }
                window.ShowDialog();
            }
        }
    }
}
