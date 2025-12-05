using System;
using Shared;
using BussinessLogic;

namespace Presenter
{
    /// <summary>
    /// Presenter для представления редактирования/создания автомобиля.
    /// </summary>
    public class CarEditPresenter
    {
        private readonly ICarEditView _view;
        private readonly ICarService _carService;
        private readonly int? _carId;

        /// <summary>
        /// Инициализирует presenter для редактирования существующего автомобиля.
        /// </summary>
        /// <param name="view">Представление редактирования.</param>
        /// <param name="carService">Сервис управления автомобилями.</param>
        /// <param name="carId">ID редактируемого автомобиля (null для создания нового).</param>
        public CarEditPresenter(ICarEditView view, ICarService carService, int? carId)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _carService = carService ?? throw new ArgumentNullException(nameof(carService));
            _carId = carId;

            SubscribeToViewEvents();

            if (_carId.HasValue)
            {
                LoadCarData(_carId.Value);
            }
        }

        /// <summary>
        /// Подписка на события представления.
        /// </summary>
        private void SubscribeToViewEvents()
        {
            _view.SaveRequested += OnSaveRequested;
            _view.CancelRequested += OnCancelRequested;
        }

        /// <summary>
        /// Загружает данные автомобиля для редактирования.
        /// </summary>
        private void LoadCarData(int carId)
        {
            var carValues = _carService.GetCarValuesForEdit(carId);

            if (carValues != null && carValues.Length >= 7)
            {
                _view.SetCarData(
                    brand: (string)carValues[0],
                    model: (string)carValues[1],
                    licensePlate: (string)carValues[2],
                    year: (int)carValues[3],
                    mileage: (int)carValues[4],
                    price: (decimal)carValues[5],
                    status: (int)carValues[6]
                );
            }
            else
            {
                _view.ShowFieldError("General", "Не удалось загрузить данные автомобиля");
                _view.CloseWithCancel();
            }
        }


        private void OnSaveRequested(object? sender, EventArgs e)
        {
            _view.ClearErrors();

            try
            {
                if (_carId.HasValue)
                {
                    bool success = _carService.UpdateCarDetails(
                        _carId.Value,
                        _view.Brand,
                        _view.Model,
                        _view.LicensePlate,
                        _view.Year,
                        _view.Mileage,
                        _view.Price,
                        _view.Status
                    );

                    if (success)
                    {
                        _view.CloseWithSuccess();
                    }
                    else
                    {
                        _view.ShowFieldError("General", "Не удалось обновить автомобиль. Проверьте уникальность гос. номера.");
                    }
                }
                else
                {
                    var car = _carService.CreateCar(
                        _view.Brand,
                        _view.Model,
                        _view.LicensePlate,
                        _view.Year,
                        _view.Mileage,
                        _view.Price
                    );

                    if (car != null)
                    {
                        _view.CloseWithSuccess();
                    }
                }
            }
            catch (ArgumentException ex)
            {
                _view.ShowFieldError("General", ex.Message);
            }
            catch (Exception ex)
            {
                _view.ShowFieldError("General", $"Неожиданная ошибка: {ex.Message}");
            }
        }

        private void OnCancelRequested(object? sender, EventArgs e)
        {
            _view.CloseWithCancel();
        }
    }
}
