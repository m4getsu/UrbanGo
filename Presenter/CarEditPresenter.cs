using System;
using Shared;
using Model;
using Presenter.Events;

namespace Presenter
{
    /// <summary>
    /// Presenter для представления редактирования/создания автомобиля.
    /// </summary>
    public class CarEditPresenter
    {
        private readonly ICarEditView _view;
        private readonly CarSharingModel _model;
        private readonly int? _carId;

        /// <summary>
        /// Инициализирует presenter для редактирования существующего автомобиля.
        /// </summary>
        /// <param name="view">Представление редактирования.</param>
        /// <param name="model">Модель системы.</param>
        /// <param name="carId">ID редактируемого автомобиля (null для создания нового).</param>
        public CarEditPresenter(ICarEditView view, CarSharingModel model, int? carId)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _model = model ?? throw new ArgumentNullException(nameof(model));
            _carId = carId;

            SubscribeToViewEvents();
            SubscribeToModelEvents();

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
        /// Подписка на события модели.
        /// </summary>
        private void SubscribeToModelEvents()
        {
            _model.ValidationPerformed += OnValidationPerformed;
            _model.ErrorOccurred += OnModelErrorOccurred;
        }

        /// <summary>
        /// Загружает данные автомобиля для редактирования.
        /// </summary>
        private void LoadCarData(int carId)
        {
            var carValues = _model.GetCarValuesForEdit(carId);

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
                    bool success = _model.UpdateCarDetails(
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
                    var car = _model.CreateCar(
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


        private void OnValidationPerformed(object? sender, ValidationEventArgs e)
        {
            if (!e.IsValid)
            {
                foreach (var error in e.Errors)
                {
                    _view.ShowFieldError("General", error);
                }
            }
        }

        private void OnModelErrorOccurred(object? sender, ModelEventArgs e)
        {
            _view.ShowFieldError("General", e.Message);
        }
    }
}
