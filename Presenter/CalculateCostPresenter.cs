using System;
using Shared;
using Model;
using Presenter.Events;

namespace Presenter
{
    /// <summary>
    /// Presenter для представления расчета стоимости аренды.
    /// </summary>
    public class CalculateCostPresenter
    {
        private readonly ICalculateCostView _view;
        private readonly CarSharingModel _model;
        private string? _currentPromoCode;

        /// <summary>
        /// Инициализирует presenter для расчета стоимости.
        /// </summary>
        /// <param name="view">Представление расчета.</param>
        /// <param name="model">Модель системы.</param>
        public CalculateCostPresenter(ICalculateCostView view, CarSharingModel model)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _model = model ?? throw new ArgumentNullException(nameof(model));

            SubscribeToViewEvents();
            SubscribeToModelEvents();
        }

        /// <summary>
        /// Подписка на события представления.
        /// </summary>
        private void SubscribeToViewEvents()
        {
            _view.ViewLoaded += OnViewLoaded;
            _view.HoursChanged += OnHoursChanged;
            _view.ApplyPromoCodeRequested += OnApplyPromoCodeRequested;
            _view.CloseRequested += OnCloseRequested;
            _view.ShowDetailsRequested += OnShowDetailsRequested;
        }

        /// <summary>
        /// Подписка на события модели.
        /// </summary>
        private void SubscribeToModelEvents()
        {
            _model.ErrorOccurred += OnModelErrorOccurred;
        }


        private void OnViewLoaded(object? sender, EventArgs e)
        {
            LoadCarInfo();
            CalculateAndDisplayCost();
        }

        private void OnHoursChanged(object? sender, int hours)
        {
            CalculateAndDisplayCost();
        }

        private void OnApplyPromoCodeRequested(object? sender, string promoCode)
        {
            if (string.IsNullOrWhiteSpace(promoCode))
            {
                _view.ShowError("Введите промокод");
                return;
            }

            try
            {
                decimal originalCost = _model.CalculateRentalCost(_view.CarId, _view.Hours);
                decimal discountedCost = _model.CalculateRentalCost(_view.CarId, _view.Hours, promoCode);

                if (discountedCost < originalCost)
                {
                    _currentPromoCode = promoCode;
                    decimal discountAmount = originalCost - discountedCost;
                    decimal discountPercent = (discountAmount / originalCost) * 100;

                    _view.DisplayDiscountInfo(discountAmount, discountPercent);
                    _view.DisplayTotalCost(discountedCost);
                }
                else
                {
                    _view.ShowError("Промокод не найден или недействителен");
                    _view.ClearDiscountInfo();
                }
            }
            catch (ArgumentException ex)
            {
                _view.ShowError(ex.Message);
            }
            catch (Exception ex)
            {
                _view.ShowError($"Ошибка применения промокода: {ex.Message}");
            }
        }

        private void OnCloseRequested(object? sender, EventArgs e)
        {
            _view.Close();
        }

        private void OnShowDetailsRequested(object? sender, EventArgs e)
        {
            try
            {
                string breakdown = _model.GetPricingBreakdown(_view.Hours);
                System.Windows.Forms.MessageBox.Show(
                    breakdown,
                    "Детализация расчета стоимости",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                _view.ShowError($"Ошибка получения детализации: {ex.Message}");
            }
        }


        private void OnModelErrorOccurred(object? sender, ModelEventArgs e)
        {
            _view.ShowError(e.Message);
        }


        /// <summary>
        /// Загружает информацию об автомобиле.
        /// </summary>
        private void LoadCarInfo()
        {
            try
            {
                var carInfo = _model.GetCarForCalculation(_view.CarId);

                if (carInfo != null)
                {
                    _view.DisplayCarInfo(carInfo.DisplayText, carInfo.RentalPricePerHour);
                }
                else
                {
                    _view.ShowError("Автомобиль не найден");
                    _view.Close();
                }
            }
            catch (Exception ex)
            {
                _view.ShowError($"Ошибка загрузки данных автомобиля: {ex.Message}");
                _view.Close();
            }
        }

        /// <summary>
        /// Рассчитывает и отображает стоимость аренды.
        /// </summary>
        private void CalculateAndDisplayCost()
        {
            try
            {
                decimal cost = _model.CalculateRentalCost(_view.CarId, _view.Hours, _currentPromoCode);
                _view.DisplayTotalCost(cost);
            }
            catch (ArgumentException ex)
            {
                _view.ShowError($"Ошибка расчета: {ex.Message}");
            }
            catch (Exception ex)
            {
                _view.ShowError($"Неожиданная ошибка: {ex.Message}");
            }
        }
    }
}
