using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BussinessLogic.Dto;
using Shared;

namespace AIS
{
    /// <summary>
    /// Главная форма приложения для управления автомобилями.
    /// Реализует MVP паттерн через интерфейс IMainView.
    /// </summary>
    public partial class MainForm : Form, IMainView
    {
        public event EventHandler ViewLoaded;
        public event EventHandler AddCarRequested;
        public event EventHandler<int> EditCarRequested;
        public event EventHandler<int> DeleteCarRequested;
        public event EventHandler<int> RentCarRequested;
        public event EventHandler<int> CalculateCostRequested;
        public event EventHandler RefreshRequested;
        public event EventHandler<string> SearchTextChanged;
        public event EventHandler ImportRequested;
        public event EventHandler<IEnumerable<int>> ExportRequested;

        private void MainForm_Load(object sender, EventArgs e)
        {
            ConfigureDataGridView();
            UpdatePricingStrategyLabel();
            ViewLoaded?.Invoke(this, EventArgs.Empty); 
        }

        private void ConfigureDataGridView()
        {
            dataGridViewCars.AutoGenerateColumns = false;
            dataGridViewCars.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewCars.MultiSelect = true;
            dataGridViewCars.ReadOnly = true;
            dataGridViewCars.AllowUserToAddRows = false;
            dataGridViewCars.AllowUserToDeleteRows = false;
            dataGridViewCars.RowHeadersVisible = false;

            dataGridViewCars.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.LightGray,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Alignment = DataGridViewContentAlignment.MiddleCenter
            };

            dataGridViewCars.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.LightGray
            };

            dataGridViewCars.Columns.Clear();

            dataGridViewCars.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Id",
                HeaderText = "ID",
                Width = 50,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            dataGridViewCars.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Brand",
                HeaderText = "Марка",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleLeft }
            });

            dataGridViewCars.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Model",
                HeaderText = "Модель",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleLeft }
            });

            dataGridViewCars.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "LicensePlate",
                HeaderText = "Гос. номер",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            dataGridViewCars.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Year",
                HeaderText = "Год",
                Width = 60,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            dataGridViewCars.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "StatusText",
                HeaderText = "Статус",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            dataGridViewCars.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Mileage",
                HeaderText = "Пробег",
                Width = 80,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight }
            });

            dataGridViewCars.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "RentalPricePerHour",
                HeaderText = "Цена/час",
                Width = 90,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Format = "C2",
                    Alignment = DataGridViewContentAlignment.MiddleRight
                }
            });
        }


        private void buttonAdd_Click(object sender, EventArgs e)
        {
            AddCarRequested?.Invoke(this, EventArgs.Empty);
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            var carId = GetSelectedCarId();
            if (carId.HasValue)
                EditCarRequested?.Invoke(this, carId.Value);
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            var carId = GetSelectedCarId();
            if (carId.HasValue)
                DeleteCarRequested?.Invoke(this, carId.Value);
        }

        private void buttonRent_Click(object sender, EventArgs e)
        {
            var carId = GetSelectedCarId();
            if (carId.HasValue)
                RentCarRequested?.Invoke(this, carId.Value);
        }

        private void buttonCalculate_Click(object sender, EventArgs e)
        {
            var carId = GetSelectedCarId();
            if (carId.HasValue)
                CalculateCostRequested?.Invoke(this, carId.Value);
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            RefreshRequested?.Invoke(this, EventArgs.Empty);
        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            SearchTextChanged?.Invoke(this, textBoxSearch.Text);
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            SearchTextChanged?.Invoke(this, textBoxSearch.Text);
        }

        private void dataGridViewCars_SelectionChanged(object sender, EventArgs e)
        {
            bool hasSelection = dataGridViewCars.CurrentRow != null;
            buttonEdit.Enabled = hasSelection;
            buttonDelete.Enabled = hasSelection;
            buttonRent.Enabled = hasSelection;
            buttonCalculate.Enabled = hasSelection;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            var result = MessageBox.Show(
                "Вы уверены, что хотите выйти?",
                "Подтверждение выхода",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
            {
                e.Cancel = true;
            }
        }

        private void buttonImport_Click(object sender, EventArgs e)
        {
            ImportRequested?.Invoke(this, EventArgs.Empty);
        }

        private void buttonExport_Click(object sender, EventArgs e)
        {
            var selectedIds = GetSelectedCarIds();
            ExportRequested?.Invoke(this, selectedIds);
        }

        private void UpdatePricingStrategyLabel()
        {
        }


        /// <summary>
        /// Конструктор без параметров для MVP.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Отображает список автомобилей.
        /// </summary>
        public void DisplayCars(IEnumerable<object> cars)
        {
            if (cars is IEnumerable<CarListItemDto> carDtos)
            {
                dataGridViewCars.DataSource = new BindingList<CarListItemDto>(carDtos.ToList());
            }
        }

        /// <summary>
        /// Обновляет строку состояния.
        /// </summary>
        public void UpdateStatusBar(int total, int available, int rented, int maintenance)
        {
            toolStripStatusLabel.Text =
                $"Всего: {total} | Свободных: {available} | В аренде: {rented} | На тех. обслуживании: {maintenance}";
        }

        /// <summary>
        /// Показывает сообщение об ошибке.
        /// </summary>
        public void ShowError(string message)
        {
            MessageBox.Show(message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Показывает информационное сообщение.
        /// </summary>
        public void ShowInfo(string message)
        {
            MessageBox.Show(message, "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Запрашивает подтверждение у пользователя.
        /// </summary>
        public bool ConfirmAction(string message)
        {
            return MessageBox.Show(message, "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }

        /// <summary>
        /// Получает ID выбранного автомобиля.
        /// </summary>
        public int? GetSelectedCarId()
        {
            if (dataGridViewCars.CurrentRow?.DataBoundItem is CarListItemDto car)
                return car.Id;
            return null;
        }

        /// <summary>
        /// Получает список ID выбранных автомобилей.
        /// </summary>
        public IEnumerable<int> GetSelectedCarIds()
        {
            return dataGridViewCars.SelectedRows
                .Cast<DataGridViewRow>()
                .Where(r => r.DataBoundItem is CarListItemDto)
                .Select(r => ((CarListItemDto)r.DataBoundItem).Id)
                .ToList();
        }
    }
}

