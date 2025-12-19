using System;
using System.Collections.Generic;
using System.Windows.Input;
using BussinessLogic;
using Presenter.Commands;
using Presenter.ObservableDTO;

namespace Presenter.ViewModels
{
    /// <summary>
    /// ViewModel для редактирования автомобиля.
    /// </summary>
    public class CarEditViewModel : BaseViewModel
    {
        private readonly VMManager _vmManager;
        private readonly ICarService _carService;
        private readonly int? _carId;

        private string _brand = string.Empty;
        private string _model = string.Empty;
        private string _licensePlate = string.Empty;
        private int _year = DateTime.Now.Year;
        private int _mileage = 0;
        private decimal _rentalPricePerHour = 100m;
        private ObservableCarStatus _selectedStatus;

        /// <summary>
        /// Марка автомобиля.
        /// </summary>
        public string Brand
        {
            get => _brand;
            set => SetProperty(ref _brand, value);
        }

        /// <summary>
        /// Модель автомобиля.
        /// </summary>
        public string Model
        {
            get => _model;
            set => SetProperty(ref _model, value);
        }

        /// <summary>
        /// Государственный номерной знак автомобиля.
        /// </summary>
        public string LicensePlate
        {
            get => _licensePlate;
            set => SetProperty(ref _licensePlate, value);
        }

        /// <summary>
        /// Год выпуска автомобиля.
        /// </summary>
        public int Year
        {
            get => _year;
            set => SetProperty(ref _year, value);
        }

        /// <summary>
        /// Пробег автомобиля в километрах.
        /// </summary>
        public int Mileage
        {
            get => _mileage;
            set => SetProperty(ref _mileage, value);
        }

        /// <summary>
        /// Стоимость аренды автомобиля за час.
        /// </summary>
        public decimal RentalPricePerHour
        {
            get => _rentalPricePerHour;
            set => SetProperty(ref _rentalPricePerHour, value);
        }

        /// <summary>
        /// Выбранный статус автомобиля.
        /// </summary>
        public ObservableCarStatus SelectedStatus
        {
            get => _selectedStatus;
            set => SetProperty(ref _selectedStatus, value);
        }

        /// <summary>
        /// Список всех доступных статусов автомобиля.
        /// </summary>
        public List<ObservableCarStatus> AvailableStatuses { get; }

        /// <summary>
        /// Команда сохранения изменений.
        /// </summary>
        public ICommand SaveCommand { get; }

        /// <summary>
        /// Команда отмены изменений.
        /// </summary>
        public ICommand CancelCommand { get; }

        /// <summary>
        /// Результат диалога (true - сохранено, false - отменено).
        /// </summary>
        public bool DialogResult { get; private set; }

        /// <summary>
        /// Инициализирует новый экземпляр CarEditViewModel.
        /// </summary>
        /// <param name="vmManager">Менеджер ViewModels.</param>
        /// <param name="carService">Сервис для работы с автомобилями.</param>
        /// <param name="carId">Идентификатор автомобиля для редактирования (null для создания нового).</param>
        public CarEditViewModel(VMManager vmManager, ICarService carService, int? carId = null)
        {
            _vmManager = vmManager ?? throw new ArgumentNullException(nameof(vmManager));
            _carService = carService ?? throw new ArgumentNullException(nameof(carService));
            _carId = carId;

            // Инициализируем список статусов
            AvailableStatuses = ObservableCarStatus.GetAll();

            // Устанавливаем статус по умолчанию (Доступен)
            _selectedStatus = AvailableStatuses[0];

            SaveCommand = new RelayCommand(_ => Save());
            CancelCommand = new RelayCommand(_ => Cancel());
        }

        /// <summary>
        /// Инициализирует ViewModel, загружая данные автомобиля при редактировании.
        /// </summary>
        public override void Initialize()
        {
            if (_carId.HasValue)
            {
                LoadCar(_carId.Value);
            }
            base.Initialize();
        }

        /// <summary>
        /// Загружает данные автомобиля по идентификатору.
        /// </summary>
        /// <param name="carId">Идентификатор автомобиля.</param>
        private void LoadCar(int carId)
        {
            var car = _carService.GetCar(carId);
            if (car != null)
            {
                Brand = car.Brand;
                Model = car.Model;
                LicensePlate = car.LicensePlate;
                Year = car.Year;
                Mileage = car.Mileage;
                RentalPricePerHour = car.RentalPricePerHour;

                SelectedStatus = ObservableCarStatus.GetById((int)car.Status);
            }
        }

        /// <summary>
        /// Сохраняет изменения автомобиля.
        /// </summary>
        private void Save()
        {
            try
            {
                if (_carId.HasValue)
                {
                    _carService.UpdateCarDetails(_carId.Value, Brand, Model, LicensePlate, Year, Mileage, RentalPricePerHour, SelectedStatus.Id);
                }
                else
                {
                    _carService.CreateCar(Brand, Model, LicensePlate, Year, Mileage, RentalPricePerHour);
                }

                DialogResult = true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка сохранения: {ex.Message}");
                DialogResult = false;
            }
        }

        /// <summary>
        /// Отменяет редактирование автомобиля.
        /// </summary>
        private void Cancel()
        {
            DialogResult = false;
        }
    }
}
