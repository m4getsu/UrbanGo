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
    public partial class CarEditForm : Form
    {
        public string Brand { get; private set; } = string.Empty;
        public string Model { get; private set; } = string.Empty;
        public string LicensePlate { get; private set; } = string.Empty;
        public int Year { get; private set; }
        public int Mileage { get; private set; }
        public decimal Price { get; private set; }
        public int Status { get; private set; }

        private readonly bool _isEditMode;

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

                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
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
    }
}

