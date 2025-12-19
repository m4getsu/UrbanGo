using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Shared;
using BussinessLogic;
using BussinessLogic.Services.Import;

namespace Presenter.Presenters
{
    /// <summary>
    /// Presenter для главного представления.
    /// Связывает IMainView с бизнес-сервисами, обрабатывает события и управляет бизнес-логикой.
    /// </summary>
    public class MainPresenter
    {
        private readonly IMainView _view;
        private readonly ICarService _carService;
        private readonly ICarImportService _exportService;
        private readonly Func<ICarEditView> _carEditViewFactory;
        private readonly Func<int, ICalculateCostView> _calculateCostViewFactory;
        private readonly Func<ICarImportView> _carImportViewFactory;

        /// <summary>
        /// Инициализирует presenter с представлением и сервисами.
        /// </summary>
        /// <param name="view">Главное представление.</param>
        /// <param name="carService">Сервис управления автомобилями.</param>
        /// <param name="exportService">Сервис экспорта данных.</param>
        /// <param name="carEditViewFactory">Фабрика для создания представления редактирования.</param>
        /// <param name="calculateCostViewFactory">Фабрика для создания представления расчета.</param>
        /// <param name="carImportViewFactory">Фабрика для создания представления импорта.</param>
        public MainPresenter(
            IMainView view,
            ICarService carService,
            ICarImportService exportService,
            Func<ICarEditView> carEditViewFactory,
            Func<int, ICalculateCostView> calculateCostViewFactory,
            Func<ICarImportView> carImportViewFactory)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _carService = carService ?? throw new ArgumentNullException(nameof(carService));
            _exportService = exportService ?? throw new ArgumentNullException(nameof(exportService));
            _carEditViewFactory = carEditViewFactory ?? throw new ArgumentNullException(nameof(carEditViewFactory));
            _calculateCostViewFactory = calculateCostViewFactory ?? throw new ArgumentNullException(nameof(calculateCostViewFactory));
            _carImportViewFactory = carImportViewFactory ?? throw new ArgumentNullException(nameof(carImportViewFactory));

            SubscribeToViewEvents();
        }

        /// <summary>
        /// Подписка на события представления.
        /// </summary>
        private void SubscribeToViewEvents()
        {
            _view.ViewLoaded += OnViewLoaded;
            _view.AddCarRequested += OnAddCarRequested;
            _view.EditCarRequested += OnEditCarRequested;
            _view.DeleteCarRequested += OnDeleteCarRequested;
            _view.RentCarRequested += OnRentCarRequested;
            _view.CalculateCostRequested += OnCalculateCostRequested;
            _view.RefreshRequested += OnRefreshRequested;
            _view.SearchTextChanged += OnSearchTextChanged;
            _view.ImportRequested += OnImportRequested;
            _view.ExportRequested += OnExportRequested;
        }



        private void OnViewLoaded(object? sender, EventArgs e)
        {
            LoadCarsList();
        }

        private void OnAddCarRequested(object? sender, EventArgs e)
        {
            var editView = _carEditViewFactory();
            var editPresenter = new CarEditPresenter(editView, _carService, null);

            if (editView is Form form)
            {
                form.ShowDialog();
            }

            LoadCarsList();
        }

        private void OnEditCarRequested(object? sender, int carId)
        {
            var carValues = _carService.GetCarValuesForEdit(carId);
            if (carValues == null)
            {
                _view.ShowError("Автомобиль не найден!");
                return;
            }

            var editView = _carEditViewFactory();
            var editPresenter = new CarEditPresenter(editView, _carService, carId);

            if (editView is Form form)
            {
                form.ShowDialog();
            }

            LoadCarsList();
        }

        private void OnDeleteCarRequested(object? sender, int carId)
        {
            var car = _carService.GetCar(carId);
            if (car == null)
            {
                _view.ShowError("Автомобиль не найден!");
                return;
            }

            if (_view.ConfirmAction($"Вы уверены, что хотите удалить автомобиль '{car.Brand} {car.Model}'?"))
            {
                if (_carService.DeleteCar(carId))
                {
                    _view.ShowInfo("Автомобиль успешно удален!");
                    LoadCarsList();
                }
                else
                {
                    _view.ShowError("Не удалось удалить автомобиль. Возможно, он арендован.");
                }
            }
        }

        private void OnRentCarRequested(object? sender, int carId)
        {
            if (_carService.RentCar(carId))
            {
                _view.ShowInfo("Автомобиль успешно арендован!");
                LoadCarsList();
            }
            else
            {
                _view.ShowError("Не удалось арендовать автомобиль. Возможно, он уже арендован или недоступен.");
            }
        }

        private void OnCalculateCostRequested(object? sender, int carId)
        {
            var calculateView = _calculateCostViewFactory(carId);
            var calculatePresenter = new CalculateCostPresenter(calculateView, _carService);

            if (calculateView is Form form)
            {
                form.ShowDialog();
            }
        }

        private void OnRefreshRequested(object? sender, EventArgs e)
        {
            LoadCarsList();
        }

        private void OnSearchTextChanged(object? sender, string searchText)
        {
            LoadCarsList(searchText);
        }

        private void OnImportRequested(object? sender, EventArgs e)
        {
            var importView = _carImportViewFactory();

            if (importView is Form form)
            {
                form.ShowDialog();
            }

            LoadCarsList();
        }

        private void OnExportRequested(object? sender, IEnumerable<int> carIds)
        {
            using (var dialog = new SaveFileDialog
            {
                Filter = "CSV файлы (*.csv)|*.csv|JSON файлы (*.json)|*.json",
                Title = "Экспорт автомобилей",
                FileName = $"cars_export_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}"
            })
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var carIdsList = carIds?.ToList();
                        int exportedCount;

                        if (dialog.FilterIndex == 1)
                        {
                            exportedCount = carIdsList != null && carIdsList.Any()
                                ? _exportService.ExportToCsv(carIdsList, dialog.FileName)
                                : _exportService.ExportToCsv(dialog.FileName);
                        }
                        else
                        {
                            exportedCount = carIdsList != null && carIdsList.Any()
                                ? _exportService.ExportToJson(carIdsList, dialog.FileName)
                                : _exportService.ExportToJson(dialog.FileName);
                        }

                        _view.ShowInfo($"Успешно экспортировано {exportedCount} автомобилей в файл:\n{dialog.FileName}");
                    }
                    catch (Exception ex)
                    {
                        _view.ShowError($"Ошибка при экспорте:\n{ex.Message}");
                    }
                }
            }
        }


        /// <summary>
        /// Загружает и отображает список автомобилей.
        /// </summary>
        private void LoadCarsList(string? searchText = null)
        {
            var cars = _carService.GetCarsForDisplay();

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                var search = searchText.ToLower();
                cars = cars.Where(c =>
                    c.Brand.ToLower().Contains(search) ||
                    c.Model.ToLower().Contains(search) ||
                    c.LicensePlate.ToLower().Contains(search)
                ).ToList();
            }

            _view.DisplayCars(cars);

            UpdateStatistics(cars);
        }

        /// <summary>
        /// Обновляет статистику на строке состояния.
        /// </summary>
        private void UpdateStatistics(List<BussinessLogic.Dto.CarListItemDto> cars)
        {
            int total = cars.Count;
            int available = cars.Count(c => c.StatusText == "Свободен");
            int rented = cars.Count(c => c.StatusText == "В аренде");
            int maintenance = cars.Count(c => c.StatusText == "На тех. обслуживании");

            _view.UpdateStatusBar(total, available, rented, maintenance);
        }
    }
}
