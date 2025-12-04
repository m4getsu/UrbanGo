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
using Shared;

namespace AIS
{
    /// <summary>
    /// Форма для создания и редактирования автомобилей.
    /// </summary>
    public partial class CarEditForm : Form, ICarEditView
    {
        // События для MVP
        public event EventHandler? SaveRequested;
        public event EventHandler? CancelRequested;

        /// <summary>
        /// Получает марку автомобиля.
        /// </summary>
        public string Brand { get; private set; } = string.Empty;

        /// <summary>
        /// Получает модель автомобиля.
        /// </summary>
        public string Model { get; private set; } = string.Empty;

        /// <summary>
        /// Получает государственный номер автомобиля.
        /// </summary>
        public string LicensePlate { get; private set; } = string.Empty;

        /// <summary>
        /// Получает год выпуска автомобиля.
        /// </summary>
        public int Year { get; private set; }

        /// <summary>
        /// Получает пробег автомобиля.
        /// </summary>
        public int Mileage { get; private set; }

        /// <summary>
        /// Получает стоимость аренды за час.
        /// </summary>
        public decimal Price { get; private set; }

        /// <summary>
        /// Получает статус автомобиля.
        /// </summary>
        public int Status { get; private set; }

        /// <summary>
        /// Режим редактирования или создания.
        /// </summary>
        public bool IsEditMode => _isEditMode;

        private readonly bool _isEditMode;

        /// <summary>
        /// Инициализирует новый экземпляр формы для создания автомобиля.
        /// </summary>
        public CarEditForm()
        {
            InitializeComponent();
            _isEditMode = false;
            InitializeStatusComboBox();
            Text = "Добавление автомобиля";
        }

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

        private void InitializeStatusComboBox()
        {
            comboBoxStatus.Items.AddRange(new object[] { "Свободен", "В аренде", "На тех. обслуживании" });
            comboBoxStatus.SelectedIndex = 0;
            comboBoxStatus.Enabled = !_isEditMode;
        }

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

                // Генерируем событие для MVP
                SaveRequested?.Invoke(this, EventArgs.Empty);

                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            // Генерируем событие для MVP
            CancelRequested?.Invoke(this, EventArgs.Empty);

            DialogResult = DialogResult.Cancel;
            Close();
        }

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

        // Реализация методов ICarEditView

        /// <summary>
        /// Устанавливает данные автомобиля для редактирования.
        /// </summary>
        public void SetCarData(string brand, string model, string licensePlate, int year, int mileage, decimal price, int status)
        {
            textBoxBrand.Text = brand;
            textBoxModel.Text = model;
            textBoxLicensePlate.Text = licensePlate;
            numericUpDownYear.Value = year;
            numericUpDownMileage.Value = mileage;
            numericUpDownPrice.Value = price;
            comboBoxStatus.SelectedIndex = status;
        }

        /// <summary>
        /// Закрывает представление с результатом OK.
        /// </summary>
        public void CloseWithSuccess()
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        /// <summary>
        /// Закрывает представление с результатом Cancel.
        /// </summary>
        public void CloseWithCancel()
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        /// <summary>
        /// Отображает ошибку валидации для поля.
        /// </summary>
        public void ShowFieldError(string fieldName, string errorMessage)
        {
            Control? control = fieldName.ToLower() switch
            {
                "brand" => textBoxBrand,
                "model" => textBoxModel,
                "licenseplate" => textBoxLicensePlate,
                "year" => numericUpDownYear,
                "mileage" => numericUpDownMileage,
                "price" => numericUpDownPrice,
                _ => null
            };

            if (control != null)
            {
                errorProvider.SetError(control, errorMessage);
            }
        }

        /// <summary>
        /// Очищает все ошибки валидации.
        /// </summary>
        public void ClearErrors()
        {
            errorProvider.Clear();
        }
    }
}

