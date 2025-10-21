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


namespace AIS
{
    /// <summary>
    /// Форма расчёта стоимости аренды автомобиля за выбранное количество часов.
    /// </summary>
    public partial class CalculateCostForm : Form
    {
        private readonly ICarService _carService;
        private readonly int _carId;

        /// <summary>
        /// Создаёт форму расчёта и инициализирует данные автомобиля.
        /// </summary>
        /// <param name="carId">ID автомобиля для расчёта.</param>
        /// <param name="carService">Сервис автомобилей.</param>
        public CalculateCostForm(int carId, ICarService carService)
        {
            InitializeComponent();
            _carId = carId;
            _carService = carService;
            InitializeForm();
        }

        /// <summary>
        /// Загружает данные автомобиля и готовит элементы управления формы.
        /// </summary>
        private void InitializeForm()
        {
            var carInfo = _carService.GetCarForCalculation(_carId);
            if (carInfo == null)
            {
                MessageBox.Show("Автомобиль не найден!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }
            var carType = carInfo.GetType();
            labelCarInfo.Text = (string)carType.GetProperty("DisplayText").GetValue(carInfo);
            labelPricePerHour.Text = $"{(decimal)carType.GetProperty("RentalPricePerHour").GetValue(carInfo):C}/час";

            numericUpDownHours.Minimum = 1;
            numericUpDownHours.Maximum = 720;
            numericUpDownHours.Value = 1;
            CalculateCost();
        }

        /// <summary>
        /// Пересчитывает итоговую стоимость аренды по текущему числу часов.
        /// </summary>
        private void CalculateCost()
        {
            try
            {
                int hours = (int)numericUpDownHours.Value;
                decimal cost = _carService.CalculateRentalCost(_carId, hours);
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

        /// <summary>
        /// Обработчик изменения количества часов: запускает перерасчёт.
        /// </summary>
        private void numericUpDownHours_ValueChanged(object sender, EventArgs e)
        {
            CalculateCost();
        }

        /// <summary>
        /// Закрывает форму по нажатию кнопки ОК.
        /// </summary>
        private void buttonOK_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}