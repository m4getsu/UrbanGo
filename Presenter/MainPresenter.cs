using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Shared;
using Model;
using Presenter.Events;

namespace Presenter
{
    /// <summary>
    /// Presenter для главного представления.
    /// Связывает IMainView с CarSharingModel, обрабатывает события и управляет бизнес-логикой.
    /// </summary>
    public class MainPresenter
    {
        private readonly IMainView _view;
        private readonly CarSharingModel _model;
        private readonly Func<ICarEditView> _carEditViewFactory;
        private readonly Func<int, ICalculateCostView> _calculateCostViewFactory;
        private readonly Func<ICarImportView> _carImportViewFactory;

        /// <summary>
        /// Инициализирует presenter с представлением и моделью.
        /// </summary>
        /// <param name="view">Главное представление.</param>
        /// <param name="model">Модель системы.</param>
        /// <param name="carEditViewFactory">Фабрика для создания представления редактирования.</param>
        /// <param name="calculateCostViewFactory">Фабрика для создания представления расчета.</param>
        /// <param name="carImportViewFactory">Фабрика для создания представления импорта.</param>
        public MainPresenter(
            IMainView view,
            CarSharingModel model,
            Func<ICarEditView> carEditViewFactory,
            Func<int, ICalculateCostView> calculateCostViewFactory,
            Func<ICarImportView> carImportViewFactory)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _model = model ?? throw new ArgumentNullException(nameof(model));
            _carEditViewFactory = carEditViewFactory ?? throw new ArgumentNullException(nameof(carEditViewFactory));
            _calculateCostViewFactory = calculateCostViewFactory ?? throw new ArgumentNullException(nameof(calculateCostViewFactory));
            _carImportViewFactory = carImportViewFactory ?? throw new ArgumentNullException(nameof(carImportViewFactory));

            SubscribeToViewEvents();
            SubscribeToModelEvents();
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

        /// <summary>
        /// Подписка на события модели.
        /// </summary>
        private void SubscribeToModelEvents()
        {
            _model.CarOperationPerformed += OnCarOperationPerformed;
            _model.ErrorOccurred += OnModelErrorOccurred;
            _model.ImportExportPerformed += OnImportExportPerformed;
        }



        private void OnViewLoaded(object? sender, EventArgs e)
        {
            LoadCarsList();
        }

        private void OnAddCarRequested(object? sender, EventArgs e)
        {
            var editView = _carEditViewFactory();
            var editPresenter = new CarEditPresenter(editView, _model, null);

            if (editView is Form form)
            {
                form.ShowDialog();
            }

            LoadCarsList();
        }

        private void OnEditCarRequested(object? sender, int carId)
        {
            var carValues = _model.GetCarValuesForEdit(carId);
            if (carValues == null)
            {
                _view.ShowError("Автомобиль не найден!");
                return;
            }

            var editView = _carEditViewFactory();
            var editPresenter = new CarEditPresenter(editView, _model, carId);

            if (editView is Form form)
            {
                form.ShowDialog();
            }

            LoadCarsList();
        }

        private void OnDeleteCarRequested(object? sender, int carId)
        {
            var car = _model.GetCar(carId);
            if (car == null)
            {
                _view.ShowError("Автомобиль не найден!");
                return;
            }

            if (_view.ConfirmAction($"Вы уверены, что хотите удалить автомобиль '{car.Brand} {car.Model}'?"))
            {
                if (_model.DeleteCar(carId))
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
            if (_model.RentCar(carId))
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
            var calculatePresenter = new CalculateCostPresenter(calculateView, _model);

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
            var importPresenter = new CarImportPresenter(importView, _model);

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
                                ? _model.ExportToCsv(carIdsList, dialog.FileName)
                                : _model.ExportToCsv(dialog.FileName);
                        }
                        else
                        {
                            exportedCount = carIdsList != null && carIdsList.Any()
                                ? _model.ExportToJson(carIdsList, dialog.FileName)
                                : _model.ExportToJson(dialog.FileName);
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


        private void OnCarOperationPerformed(object? sender, CarOperationEventArgs e)
        {
            if (!e.IsSuccess)
            {
                _view.ShowError($"Операция '{e.OperationType}' не выполнена: {e.Details}");
            }
        }

        private void OnModelErrorOccurred(object? sender, ModelEventArgs e)
        {
            _view.ShowError(e.Message);
        }

        private void OnImportExportPerformed(object? sender, ImportExportEventArgs e)
        {
            _view.ShowInfo(e.Message);
        }


        /// <summary>
        /// Загружает и отображает список автомобилей.
        /// </summary>
        private void LoadCarsList(string? searchText = null)
        {
            var cars = _model.GetCarsForDisplay();

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
