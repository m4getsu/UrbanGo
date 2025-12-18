using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using BussinessLogic;
using Presenter.Commands;
using Presenter.ObservableDTO;
using Presenter.ObservableDTO.Mappers;

namespace Presenter.ViewModels
{
    /// <summary>
    /// ViewModel для главного окна приложения.
    /// </summary>
    public class MainViewModel : BaseViewModel
    {
        private readonly VMManager _vmManager;
        private readonly ICarService _carService;
        private BindingList<ObservableCarListItemDto> _cars;
        private ObservableCarListItemDto? _selectedCar;
        private string _searchText = string.Empty;
        private string _statusBarText = "Готов";

        /// <summary>
        /// Предоставляет доступ к VMManager для создания дочерних ViewModel
        /// </summary>
        public VMManager VMManager => _vmManager;

        /// <summary>
        /// Предоставляет доступ к CarService для операций с автомобилями
        /// </summary>
        public ICarService CarService => _carService;

        public BindingList<ObservableCarListItemDto> Cars
        {
            get => _cars;
            set => SetProperty(ref _cars, value);
        }

        public ObservableCarListItemDto? SelectedCar
        {
            get => _selectedCar;
            set
            {
                if (SetProperty(ref _selectedCar, value))
                {
                    // Обновляем состояние команд
                    System.Windows.Input.CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        public string SearchText
        {
            get => _searchText;
            set => SetProperty(ref _searchText, value);
        }

        public string StatusBarText
        {
            get => _statusBarText;
            set => SetProperty(ref _statusBarText, value);
        }

        public ICommand LoadDataCommand { get; }
        public ICommand AddNewCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand RentCarCommand { get; }
        public ICommand CalculateCostCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand ImportCommand { get; }
        public ICommand ExportCommand { get; }

        public MainViewModel(VMManager vmManager, ICarService carService)
        {
            _vmManager = vmManager ?? throw new ArgumentNullException(nameof(vmManager));
            _carService = carService ?? throw new ArgumentNullException(nameof(carService));
            _cars = new BindingList<ObservableCarListItemDto>();

            LoadDataCommand = new RelayCommand(_ => LoadData());
            AddNewCommand = new RelayCommand(_ => AddNew());
            EditCommand = new RelayCommand(_ => Edit(), _ => SelectedCar != null);
            DeleteCommand = new RelayCommand(_ => Delete(), _ => SelectedCar != null);
            RentCarCommand = new RelayCommand(_ => RentCar(), _ => SelectedCar != null);
            CalculateCostCommand = new RelayCommand(_ => CalculateCost(), _ => SelectedCar != null);
            SearchCommand = new RelayCommand(_ => Search());
            ImportCommand = new RelayCommand(_ => Import());
            ExportCommand = new RelayCommand(_ => Export());
        }

        public override void Initialize()
        {
            LoadData();
            base.Initialize();
        }

        private void LoadData()
        {
            try
            {
                var carsDto = _carService.GetCarsForDisplay();
                var observableCars = CarListItemMapper.ToObservableList(carsDto);
                Cars = new BindingList<ObservableCarListItemDto>(observableCars);
                UpdateStatusBar();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка загрузки данных: {ex.Message}");
                StatusBarText = $"Ошибка: {ex.Message}";
            }
        }

        private void AddNew()
        {
            var editVm = _vmManager.CreateViewModelWithNullableParameter<CarEditViewModel>(null);
            LoadData();
        }

        private void Edit()
        {
            if (SelectedCar == null) return;

            var editVm = _vmManager.CreateViewModelWithParameter<CarEditViewModel>(SelectedCar.Id);
            LoadData();
        }

        private void Delete()
        {
            if (SelectedCar == null) return;

            try
            {
                _carService.DeleteCar(SelectedCar.Id);
                LoadData();
                SelectedCar = null;
                StatusBarText = "Автомобиль удален";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка удаления: {ex.Message}");
                StatusBarText = $"Ошибка удаления: {ex.Message}";
            }
        }

        private void RentCar()
        {
            if (SelectedCar == null) return;

            try
            {
                if (_carService.RentCar(SelectedCar.Id))
                {
                    LoadData();
                    StatusBarText = "Автомобиль арендован";
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка аренды: {ex.Message}");
                StatusBarText = $"Ошибка аренды: {ex.Message}";
            }
        }

        private void CalculateCost()
        {
            if (SelectedCar == null) return;

            var calcVm = _vmManager.CreateViewModelWithParameter<CalculateCostViewModel>(SelectedCar.Id);
        }

        private void Search()
        {
            try
            {
                var carsDto = _carService.GetCarsForDisplay();

                if (!string.IsNullOrWhiteSpace(SearchText))
                {
                    var search = SearchText.ToLower();
                    carsDto = carsDto.Where(c =>
                        c.Brand.ToLower().Contains(search) ||
                        c.Model.ToLower().Contains(search) ||
                        c.LicensePlate.ToLower().Contains(search)
                    ).ToList();
                }

                var observableCars = CarListItemMapper.ToObservableList(carsDto);
                Cars = new BindingList<ObservableCarListItemDto>(observableCars);
                UpdateStatusBar();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка поиска: {ex.Message}");
                StatusBarText = $"Ошибка поиска: {ex.Message}";
            }
        }

        private void Import()
        {
            var importVm = _vmManager.CreateViewModel<CarImportViewModel>();
        }

        private void Export()
        {
            // Export будет обработан в View через событие
            OnPropertyChanged(nameof(ExportCommand));
        }

        private void UpdateStatusBar()
        {
            int total = Cars.Count;
            int available = Cars.Count(c => c.StatusText == "Свободен");
            int rented = Cars.Count(c => c.StatusText == "В аренде");
            StatusBarText = $"Всего: {total} | Свободно: {available} | В аренде: {rented}";
        }
    }
}
