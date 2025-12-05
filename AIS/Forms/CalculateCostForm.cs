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
using Shared;


namespace AIS
{
    /// <summary>
    /// Форма для расчета стоимости аренды автомобиля.
    /// </summary>
    public partial class CalculateCostForm : Form, ICalculateCostView
    {
        public event EventHandler? ViewLoaded;
        public event EventHandler<int>? HoursChanged;
        public event EventHandler<string>? ApplyPromoCodeRequested;
        public event EventHandler? CloseRequested;
        public event EventHandler? ShowDetailsRequested;

        private readonly int _carId;

        public int CarId => _carId;
        public int Hours => (int)numericUpDownHours.Value;
        public string PromoCode => txtPromoCode.Text.Trim();

        /// <summary>
        /// Инициализирует новый экземпляр формы расчета для MVP.
        /// </summary>
        /// <param name="carId">ID автомобиля для расчета.</param>
        public CalculateCostForm(int carId)
        {
            InitializeComponent();
            _carId = carId;

            numericUpDownHours.Minimum = 1;
            numericUpDownHours.Maximum = 720;
            numericUpDownHours.Value = 1;

            Load += (s, e) => ViewLoaded?.Invoke(this, EventArgs.Empty);
        }

        private void numericUpDownHours_ValueChanged(object sender, EventArgs e)
        {
            HoursChanged?.Invoke(this, Hours);
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            CloseRequested?.Invoke(this, EventArgs.Empty);
            Close();
        }

        private void btnApplyPromo_Click(object sender, EventArgs e)
        {
            string promoCode = txtPromoCode.Text.Trim();
            ApplyPromoCodeRequested?.Invoke(this, promoCode);
        }

        private void buttonDetails_Click(object sender, EventArgs e)
        {
            ShowDetailsRequested?.Invoke(this, EventArgs.Empty);
        }


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
    }
}

