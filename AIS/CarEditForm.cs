using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using BussinessLogic;

namespace AIS
{
    /// <summary>
    /// Форма добавления/редактирования автомобиля.
    /// Содержит поля ввода и базовую валидацию значений.
    /// </summary>
    public partial class CarEditForm : Form
    {
        /// <summary>
        /// Марка автомобиля.
        /// </summary>
        public string Brand { get; private set; } = string.Empty;
        /// <summary>
        /// Модель автомобиля.
        /// </summary>
        public string Model { get; private set; } = string.Empty;
        /// <summary>
        /// Гос. номер автомобиля.
        /// </summary>
        public string LicensePlate { get; private set; } = string.Empty;
        /// <summary>
        /// Год выпуска.
        /// </summary>
        public int Year { get; private set; }
        /// <summary>
        /// Пробег, км.
        /// </summary>
        public int Mileage { get; private set; }
        /// <summary>
        /// Стоимость аренды в час.
        /// </summary>
        public decimal Price { get; private set; }
        /// <summary>
        /// Статус автомобиля: 0 - Свободен, 1 - В аренде, 2 - На тех. обслуживании.
        /// </summary>
        public int Status { get; private set; }

        private readonly bool _isEditMode;

        /// <summary>
        /// Конструктор формы для добавления автомобиля.
        /// </summary>
        public CarEditForm()
        {
            InitializeComponent();
            _isEditMode = false;
            InitializeStatusComboBox();
            Text = "Добавление автомобиля";
        }

        /// <summary>
        /// Конструктор формы для редактирования автомобиля.
        /// </summary>
        /// <param name="brand">Марка.</param>
        /// <param name="model">Модель.</param>
        /// <param name="licensePlate">Гос. номер.</param>
        /// <param name="year">Год выпуска.</param>
        /// <param name="mileage">Пробег, км.</param>
        /// <param name="price">Цена аренды в час.</param>
        /// <param name="status">Статус (0 - Свободен, 1 - В аренде, 2 - На тех. обслуживании).</param>
        public CarEditForm(string brand, string model, string licensePlate, int year,
                          int mileage, decimal price, int status) : this()
        {
            _isEditMode = true;
            Text = "Редактирование автомобиля";

            textBoxBrand.Text = brand;
            textBoxModel.Text = model;
            textBoxLicensePlate.Text = licensePlate;
            numericUpDownYear.Value = year;
            numericUpDownMileage.Value = mileage;
            numericUpDownPrice.Value = price;
            comboBoxStatus.SelectedIndex = status;

            comboBoxStatus.Enabled = true;
        }

        /// <summary>
        /// Инициализирует значения комбобокса статуса.
        /// </summary>
        private void InitializeStatusComboBox()
        {
            comboBoxStatus.Items.AddRange(new object[] { "Свободен", "В аренде", "На тех. обслуживании" });
            comboBoxStatus.SelectedIndex = 0;
            comboBoxStatus.Enabled = !_isEditMode;
        }

        /// <summary>
        /// Валидирует ввод и сохраняет данные при нажатии ОК.
        /// </summary>
        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (ValidateChildren(ValidationConstraints.Enabled))
            {
                Brand = textBoxBrand.Text.Trim();
                Model = textBoxModel.Text.Trim();
                LicensePlate = textBoxLicensePlate.Text.Trim();
                Year = (int)numericUpDownYear.Value;
                Mileage = (int)numericUpDownMileage.Value;
                Price = numericUpDownPrice.Value;
                Status = comboBoxStatus.SelectedIndex;

                DialogResult = DialogResult.OK;
                Close();
            }
        }

        /// <summary>
        /// Отменяет изменения и закрывает форму.
        /// </summary>
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        /// <summary>
        /// Проверяет, что поле Марка не пустое.
        /// </summary>
        private void textBoxBrand_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxBrand.Text))
            {
                errorProvider.SetError(textBoxBrand, "Марка не может быть пустой");
                e.Cancel = true;
            }
            else
            {
                errorProvider.SetError(textBoxBrand, string.Empty);
            }
        }

        /// <summary>
        /// Проверяет, что поле Модель не пустое.
        /// </summary>
        private void textBoxModel_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxModel.Text))
            {
                errorProvider.SetError(textBoxModel, "Модель не может быть пустой");
                e.Cancel = true;
            }
            else
            {
                errorProvider.SetError(textBoxModel, string.Empty);
            }
        }

        /// <summary>
        /// Проверяет, что поле Гос. номер не пустое.
        /// </summary>
        private void textBoxLicensePlate_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxLicensePlate.Text))
            {
                errorProvider.SetError(textBoxLicensePlate, "Гос. номер не может быть пустым");
                e.Cancel = true;
            }
            else
            {
                errorProvider.SetError(textBoxLicensePlate, string.Empty);
            }
        }

        /// <summary>
        /// Проверяет, что год выпуска в допустимых пределах.
        /// </summary>
        private void numericUpDownYear_Validating(object sender, CancelEventArgs e)
        {
            if (numericUpDownYear.Value < 1900 || numericUpDownYear.Value > DateTime.Now.Year + 1)
            {
                errorProvider.SetError(numericUpDownYear, $"Год должен быть между 1900 и {DateTime.Now.Year + 1}");
                e.Cancel = true;
            }
            else
            {
                errorProvider.SetError(numericUpDownYear, string.Empty);
            }
        }

        /// <summary>
        /// Проверяет, что пробег не отрицательный.
        /// </summary>
        private void numericUpDownMileage_Validating(object sender, CancelEventArgs e)
        {
            if (numericUpDownMileage.Value < 0)
            {
                errorProvider.SetError(numericUpDownMileage, "Пробег не может быть отрицательным");
                e.Cancel = true;
            }
            else
            {
                errorProvider.SetError(numericUpDownMileage, string.Empty);
            }
        }

        /// <summary>
        /// Проверяет, что цена аренды положительная.
        /// </summary>
        private void numericUpDownPrice_Validating(object sender, CancelEventArgs e)
        {
            if (numericUpDownPrice.Value <= 0)
            {
                errorProvider.SetError(numericUpDownPrice, "Цена должна быть положительной");
                e.Cancel = true;
            }
            else
            {
                errorProvider.SetError(numericUpDownPrice, string.Empty);
            }
        }
    }
}
