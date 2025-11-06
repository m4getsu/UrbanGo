using BussinessLogic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BussinessLogic.Dto;
using AIS.Controllers;

namespace AIS
{
    public partial class MainForm : Form
    {
        private readonly MainFormController _controller;

        public MainForm(MainFormController controller)
        {
            InitializeComponent();
            _controller = controller;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            RefreshCarsList();
            ConfigureDataGridView();
        }

        private void ConfigureDataGridView()
        {
            dataGridViewCars.AutoGenerateColumns = false;
            dataGridViewCars.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewCars.MultiSelect = false;
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

        private void RefreshCarsList()
        {
            try
            {
                var carsForDisplay = _controller.GetCarsForDisplay();
                var items = new List<CarListItemDto>(carsForDisplay ?? Enumerable.Empty<CarListItemDto>());

                if (!string.IsNullOrWhiteSpace(textBoxSearch.Text))
                {
                    var searchTerm = textBoxSearch.Text.ToLower();
                    items = items.Where(c =>
                        c.Brand.ToLower().Contains(searchTerm) ||
                        c.Model.ToLower().Contains(searchTerm) ||
                        c.LicensePlate.ToLower().Contains(searchTerm)
                    ).ToList();
                }

                dataGridViewCars.DataSource = new BindingList<CarListItemDto>(items);
                UpdateStatusBar(items);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении списка: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateStatusBar(List<CarListItemDto> cars)
        {
            try
            {
                int total = cars.Count;
                int availableCount = cars.Count(c => c.StatusText == "Свободен");
                int rentedCount = cars.Count(c => c.StatusText == "В аренде");
                int maintenanceCount = cars.Count(c => c.StatusText == "На тех. обслуживании");

                toolStripStatusLabel.Text =
                    $"Всего: {total} | Свбободных: {availableCount} | В аренде:  {rentedCount} | На тех. обслуживании: {maintenanceCount}";
            }
            catch (Exception ex)
            {
                toolStripStatusLabel.Text = $"Ошибка: {ex.Message}";
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            using (var addForm = new CarEditForm())
            {
                if (addForm.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        _controller.CreateCar(
                            addForm.Brand,
                            addForm.Model,
                            addForm.LicensePlate,
                            addForm.Year,
                            addForm.Mileage,
                            addForm.Price
                        );
                        RefreshCarsList();
                        MessageBox.Show("Автомобиль успешно добавлен!", "Успех",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (ArgumentException ex)
                    {
                        MessageBox.Show($"Ошибка при добавлении: {ex.Message}", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            if (dataGridViewCars.CurrentRow?.DataBoundItem is CarListItemDto selectedCar)
            {
                int carId = selectedCar.Id;

                var currentValues = _controller.GetCarValuesForEdit(carId);
                if (currentValues == null)
                {
                    MessageBox.Show("Автомобиль не найден!", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                using (var editForm = new CarEditForm(
                    (string)currentValues[0],
                    (string)currentValues[1],
                    (string)currentValues[2],
                    (int)currentValues[3],
                    (int)currentValues[4],
                    (decimal)currentValues[5],
                    (int)currentValues[6]
                ))
                {
                    if (editForm.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            if (_controller.UpdateCarDetails(carId, editForm.Brand, editForm.Model,
                                editForm.LicensePlate, editForm.Year, editForm.Mileage, editForm.Price, editForm.Status))
                            {
                                RefreshCarsList();
                                MessageBox.Show("Данные автомобиля успешно обновлены!", "Успех",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show("Не удалось обновить автомобиль. Проверьте уникальность гос. номера.",
                                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        catch (ArgumentException ex)
                        {
                            MessageBox.Show($"Ошибка при обновлении: {ex.Message}", "Ошибка",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите автомобиль для редактирования.", "Информация",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (dataGridViewCars.CurrentRow?.DataBoundItem is CarListItemDto selectedCar)
            {
                int carId = selectedCar.Id;
                string carInfo = $"{selectedCar.Brand} {selectedCar.Model}";

                var result = MessageBox.Show(
                   $"Вы уверены, что хотите удалить автомобиль '{carInfo}'?",
                   "Подтверждение удаления",
                   MessageBoxButtons.YesNo,
                   MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    if (_controller.DeleteCar(carId))
                    {
                        RefreshCarsList();
                        MessageBox.Show("Автомобиль успешно удален!", "Успех",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Не удалось удалить автомобиль. Возможно, он арендован.",
                            "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите автомобиль для удаления.", "Информация",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void buttonRent_Click(object sender, EventArgs e)
        {
            if (dataGridViewCars.CurrentRow?.DataBoundItem is CarListItemDto selectedCar)
            {
                int carId = selectedCar.Id;

                if (_controller.RentCar(carId))
                {
                    RefreshCarsList();
                    MessageBox.Show("Автомобиль успешно арендован!", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Не удалось арендовать автомобиль. Возможно, он уже арендован или недоступен.",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите автомобиль для аренды.", "Информация",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void buttonCalculate_Click(object sender, EventArgs e)
        {
            if (dataGridViewCars.CurrentRow?.DataBoundItem is CarListItemDto selectedCar)
            {
                int carId = selectedCar.Id;

                using (var calcForm = new CalculateCostForm(carId, _controller.CreateCalculateCostFormController()))
                {
                    calcForm.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите автомобиль для расчета стоимости.", "Информация",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            RefreshCarsList();
        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            RefreshCarsList();
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            RefreshCarsList();
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
    }
}

