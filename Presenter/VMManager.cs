using System;
using Presenter.Events;
using BussinessLogic;
using BussinessLogic.Services.Import;
using BussinessLogic.Services.QRCode;

namespace Presenter
{
    /// <summary>
    /// Менеджер ViewModel, управляющий созданием и жизненным циклом ViewModel.
    /// </summary>
    public class VMManager
    {
        private readonly ICarService _carService;
        private readonly ICarImportService? _importService;
        private readonly IQRCodeService? _qrService;

        /// <summary>
        /// Событие готовности ViewModel.
        /// </summary>
        public event EventHandler<ViewModelReadyEventArgs>? ViewModelReady;

        /// <summary>
        /// Инициализирует VMManager с сервисом бизнес-логики.
        /// </summary>
        /// <param name="carService">Сервис управления автомобилями.</param>
        /// <param name="importService">Сервис импорта/экспорта автомобилей (опционально).</param>
        /// <param name="qrService">Сервис генерации QR-кодов (опционально).</param>
        public VMManager(ICarService carService, ICarImportService? importService = null, IQRCodeService? qrService = null)
        {
            _carService = carService ?? throw new ArgumentNullException(nameof(carService));
            _importService = importService;
            _qrService = qrService;
        }

        /// <summary>
        /// Создает и инициализирует ViewModel указанного типа.
        /// </summary>
        /// <typeparam name="T">Тип ViewModel для создания.</typeparam>
        /// <returns>Созданная и инициализированная ViewModel.</returns>
        public T CreateViewModel<T>() where T : BaseViewModel
        {
            BaseViewModel viewModel;

            if (typeof(T) == typeof(ViewModels.CarImportViewModel) && _importService != null)
            {
                viewModel = (T)Activator.CreateInstance(typeof(T), this, _carService, _importService)!;
            }
            else
            {
                viewModel = (T)Activator.CreateInstance(typeof(T), this, _carService)!;
            }

            viewModel.SetVMManager(this);

            viewModel.Initialize();

            return (T)viewModel;
        }

        /// <summary>
        /// Создает и инициализирует ViewModel указанного типа с параметром.
        /// </summary>
        /// <typeparam name="T">Тип ViewModel для создания.</typeparam>
        /// <param name="parameter">Параметр для конструктора ViewModel.</param>
        /// <returns>Созданная и инициализированная ViewModel.</returns>
        public T CreateViewModelWithParameter<T>(int parameter) where T : BaseViewModel
        {
            var viewModel = (T)Activator.CreateInstance(typeof(T), this, _carService, parameter)!;

            viewModel.SetVMManager(this);

            viewModel.Initialize();

            return viewModel;
        }

        /// <summary>
        /// Создает и инициализирует ViewModel указанного типа с nullable параметром.
        /// </summary>
        /// <typeparam name="T">Тип ViewModel для создания.</typeparam>
        /// <param name="parameter">Nullable параметр для конструктора ViewModel.</param>
        /// <returns>Созданная и инициализированная ViewModel.</returns>
        public T CreateViewModelWithNullableParameter<T>(int? parameter) where T : BaseViewModel
        {
            var viewModel = (T)Activator.CreateInstance(typeof(T), this, _carService, parameter)!;

            viewModel.SetVMManager(this);

            viewModel.Initialize();

            return viewModel;
        }

        /// <summary>
        /// Создает и инициализирует CarQRViewModel с DTO автомобиля.
        /// </summary>
        /// <param name="carDto">DTO автомобиля для которого создается QR-код.</param>
        /// <returns>Созданная и инициализированная CarQRViewModel.</returns>
        public ViewModels.CarQRViewModel CreateCarQRViewModel(BussinessLogic.Dto.CarQRDto carDto)
        {
            if (_qrService == null)
                throw new InvalidOperationException("IQRCodeService не инициализирован. Убедитесь что сервис зарегистрирован в DI.");

            var viewModel = new ViewModels.CarQRViewModel(this, _carService, _qrService, carDto);
            viewModel.SetVMManager(this);
            viewModel.Initialize();
            return viewModel;
        }

        /// <summary>
        /// Вызывается ViewModel при завершении инициализации.
        /// </summary>
        /// <param name="viewModel">Инициализированная ViewModel.</param>
        internal void OnViewModelInitialized(BaseViewModel viewModel)
        {
            ViewModelReady?.Invoke(this, new ViewModelReadyEventArgs(viewModel));
        }

        /// <summary>
        /// Очищает ресурсы ViewModel.
        /// </summary>
        /// <param name="viewModel">ViewModel для очистки.</param>
        public void CloseViewModel(BaseViewModel viewModel)
        {
        }
    }
}
