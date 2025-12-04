using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BussinessLogic;
using BussinessLogic.Dto;
using AIS.Controllers;
using Shared;


namespace AIS
{
    /// <summary>
    /// Форма для расчета стоимости аренды автомобиля.
    /// </summary>
    public partial class CalculateCostForm : Form, ICalculateCostView
    {
        // События для MVP
        public event EventHandler? ViewLoaded;
        public event EventHandler<int>? HoursChanged;
        public event EventHandler<string>? ApplyPromoCodeRequested;
        public event EventHandler? CloseRequested;

        private readonly CalculateCostFormController _controller;
        private readonly int _carId;
        private string _currentPromoCode = null;

        // Свойства интерфейса
        public int CarId => _carId;
        public int Hours => (int)numericUpDownHours.Value;
        public string PromoCode => txtPromoCode.Text.Trim();

        /// <summary>
        /// Инициализирует новый экземпляр формы расчета с идентификатором автомобиля и контроллером.
        /// </summary>
        /// <param name="carId">ID автомобиля для расчета.</param>
        /// <param name="controller">Контроллер для взаимодействия с бизнес-логикой.</param>
        public CalculateCostForm(int carId, CalculateCostFormController controller)
        {
            InitializeComponent();
            _carId = carId;
            _controller = controller;
            InitializeForm();
        }

        private void InitializeForm()
        {
            var carInfo = _controller.GetCarForCalculation(_carId);
            if (carInfo == null)
            {
                MessageBox.Show("Автомобиль не найден!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }
            if (carInfo is CarForCalculationDto dto)
            {
                labelCarInfo.Text = dto.DisplayText;
                labelPricePerHour.Text = $"{dto.RentalPricePerHour:C}/час";
            }
            else
            {
                labelCarInfo.Text = "";
                labelPricePerHour.Text = "";
            }

            numericUpDownHours.Minimum = 1;
            numericUpDownHours.Maximum = 720;
            numericUpDownHours.Value = 1;
            CalculateCost();

            // Показать условия ценообразования, если используется динамическая стратегия
            UpdatePricingConditionsLabel();

            // Генерируем событие загрузки для MVP
            ViewLoaded?.Invoke(this, EventArgs.Empty);
        }

        private void CalculateCost()
        {
            try
            {
                int hours = (int)numericUpDownHours.Value;
                decimal cost = _controller.CalculateRentalCost(_carId, hours, _currentPromoCode);
                labelTotalCost.Text = cost.ToString("C");
                labelTotalCost.ForeColor = SystemColors.ControlText;

                // Обновить метку с условиями
                UpdatePricingConditionsLabel();
            }
            catch (ArgumentException ex)
            {
                labelTotalCost.Text = $"Ошибка: {ex.Message}";
                labelTotalCost.ForeColor = Color.Red;
            }
            catch (Exception ex)
            {
                labelTotalCost.Text = $"Неожиданная ошибка: {ex.Message}";
                labelTotalCost.ForeColor = Color.Red;
            }
        }

        /// <summary>
        /// Обновляет метку с текущими условиями ценообразования.
        /// </summary>
        private void UpdatePricingConditionsLabel()
        {
            if (_controller.IsDynamicPricingEnabled)
            {
                var conditions = _controller.GetCurrentConditions();
                if (!string.IsNullOrEmpty(conditions))
                {
                    this.Text = $"Расчет стоимости - {conditions}";
                }
            }
        }

        /// <summary>
        /// Показывает детальную информацию о расчете цены.
        /// </summary>
        private void ShowPricingBreakdown()
        {
            if (!_controller.IsDynamicPricingEnabled)
            {
                MessageBox.Show(
                    "Детализация доступна только для динамического ценообразования.\n\n" +
                    "Текущая стратегия: Стандартная (цена × часы)",
                    "Информация",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
                return;
            }

            int hours = (int)numericUpDownHours.Value;
            decimal cost = _controller.CalculateRentalCost(_carId, hours, _currentPromoCode);
            var breakdown = _controller.GetPricingBreakdown(hours);

            if (!string.IsNullOrEmpty(breakdown))
            {
                var message = breakdown + $"\n\n💰 Итоговая стоимость: {cost:C}";

                if (!string.IsNullOrEmpty(_currentPromoCode))
                {
                    message += $"\n🎟️  Применен промокод: {_currentPromoCode}";
                }

                MessageBox.Show(
                    message,
                    "Детализация расчета стоимости",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
        }

        private void numericUpDownHours_ValueChanged(object sender, EventArgs e)
        {
            // Генерируем событие для MVP
            HoursChanged?.Invoke(this, Hours);
            CalculateCost();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            // Генерируем событие для MVP
            CloseRequested?.Invoke(this, EventArgs.Empty);
            Close();
        }

        private void btnApplyPromo_Click(object sender, EventArgs e)
        {
            string promoCode = txtPromoCode.Text.Trim();

            // Генерируем событие для MVP
            ApplyPromoCodeRequested?.Invoke(this, promoCode);

            if (string.IsNullOrEmpty(promoCode))
            {
                MessageBox.Show("Введите промокод", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int hours = (int)numericUpDownHours.Value;
                decimal originalCost = _controller.CalculateRentalCost(_carId, hours);
                decimal discountedCost = _controller.CalculateRentalCost(_carId, hours, promoCode);

                if (discountedCost < originalCost)
                {
                    _currentPromoCode = promoCode;
                    decimal discountPercent = ((originalCost - discountedCost) / originalCost) * 100;
                    lblDiscountInfo.Text = $"Скидка {originalCost - discountedCost:F0}₽ применена";
                    lblDiscountInfo.ForeColor = Color.Green;
                    CalculateCost();
                }
                else
                {
                    MessageBox.Show("Промокод не найден", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Неожиданная ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonDetails_Click(object sender, EventArgs e)
        {
            ShowPricingBreakdown();
        }

        // Реализация методов ICalculateCostView

        /// <summary>
        /// Отображает информацию об автомобиле.
        /// </summary>
        public void DisplayCarInfo(string carInfo, decimal pricePerHour)
        {
            labelCarInfo.Text = carInfo;
            labelPricePerHour.Text = $"{pricePerHour:C}/час";
        }

        /// <summary>
        /// Отображает рассчитанную стоимость.
        /// </summary>
        public void DisplayTotalCost(decimal totalCost)
        {
            labelTotalCost.Text = totalCost.ToString("C");
            labelTotalCost.ForeColor = SystemColors.ControlText;
        }

        /// <summary>
        /// Отображает информацию о примененной скидке.
        /// </summary>
        public void DisplayDiscountInfo(decimal discountAmount, decimal discountPercent)
        {
            lblDiscountInfo.Text = $"Скидка {discountAmount:F0}₽ ({discountPercent:F0}%) применена";
            lblDiscountInfo.ForeColor = Color.Green;
        }

        /// <summary>
        /// Очищает информацию о скидке.
        /// </summary>
        public void ClearDiscountInfo()
        {
            lblDiscountInfo.Text = string.Empty;
        }

        /// <summary>
        /// Отображает сообщение об ошибке.
        /// </summary>
        public void ShowError(string message)
        {
            MessageBox.Show(message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // Close() уже есть в Form
    }
}

