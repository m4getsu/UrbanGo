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


namespace AIS
{
    /// <summary>
    /// Форма для расчета стоимости аренды автомобиля.
    /// </summary>
    public partial class CalculateCostForm : Form
    {
        private readonly CalculateCostFormController _controller;
        private readonly int _carId;
        private string _currentPromoCode = null;

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
        }

        private void CalculateCost()
        {
            try
            {
                int hours = (int)numericUpDownHours.Value;
                decimal cost = _controller.CalculateRentalCost(_carId, hours, _currentPromoCode);
                labelTotalCost.Text = cost.ToString("C");
                labelTotalCost.ForeColor = SystemColors.ControlText;
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

        private void numericUpDownHours_ValueChanged(object sender, EventArgs e)
        {
            CalculateCost();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnApplyPromo_Click(object sender, EventArgs e)
        {
            string promoCode = txtPromoCode.Text.Trim();
            
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
    }
}

