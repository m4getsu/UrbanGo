using BussinessLogic;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AIS
{
    /// <summary>
    /// Главная форма приложения (WinForms) для управления автомобилями:
    /// отображение списка, поиск, добавление, редактирование, удаление,
    /// аренда и расчёт стоимости.
    /// </summary>
    public partial class MainForm : Form
    {
        private readonly ICarService _carService;

        private class CarDisplayItem
        {
            /// <summary>
            /// Уникальный идентификатор автомобиля.
            /// </summary>
            public int Id { get; set; }
            /// <summary>
            /// Марка автомобиля.
            /// </summary>
            public string Brand { get; set; } = string.Empty;
            /// <summary>
            /// Модель автомобиля.
            /// </summary>
            public string Model { get; set; } = string.Empty;
            /// <summary>
            /// Гос. номер автомобиля.
            /// </summary>
            public string LicensePlate { get; set; } = string.Empty;
            /// <summary>
            /// Год выпуска.
            /// </summary>
            public int Year { get; set; }
            /// <summary>
            /// Пробег, км.
            /// </summary>
            public int Mileage { get; set; }
            /// <summary>
            /// Стоимость аренды в час.
            /// </summary>
            public decimal RentalPricePerHour { get; set; }
            /// <summary>
            /// Статус в текстовом виде (англ. ключ enum).
            /// </summary>
            public string StatusText { get; set; } = string.Empty;
            /// <summary>
            /// Короткая строка для отображения (марка, модель, номер).
            /// </summary>
            public string DisplayText { get; set; } = string.Empty;
        }

        public MainForm(IRepository<Model.Car> repository, PromoService promoService)
        {
            InitializeComponent();
            _carService = new CarService(repository, promoService);
        }

        /// <summary>
        /// Загрузка тестовых данных
        /// </summary>


        /// <summary>
        /// Обработчик загрузки формы: настраивает таблицу и загружает данные.
        /// </summary>
        private void MainForm_Load(object sender, EventArgs e)
        {
            RefreshCarsList();
            ConfigureDataGridView();
        }

        /// <summary>
        /// Настраивает внешний вид и колонки DataGridView для списка автомобилей.
        /// </summary>
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

        /// <summary>
        /// Обновляет список автомобилей с учётом фильтра поиска и статистику.
        /// </summary>
        private void RefreshCarsList()
        {
            try
            {
                var carsForDisplay = _carService.GetCarsForDisplay();
                var displayItems = new List<CarDisplayItem>();

                foreach (var carObj in carsForDisplay)
                {
                    if (carObj != null)
                    {
                        var carType = carObj.GetType();
                        displayItems.Add(new CarDisplayItem
                        {
                            Id = (int)carType.GetProperty("Id").GetValue(carObj),
                            Brand = (string)carType.GetProperty("Brand").GetValue(carObj),
                            Model = (string)carType.GetProperty("Model").GetValue(carObj),
                            LicensePlate = (string)carType.GetProperty("LicensePlate").GetValue(carObj),
                            Year = (int)carType.GetProperty("Year").GetValue(carObj),
                            Mileage = (int)carType.GetProperty("Mileage").GetValue(carObj),
                            RentalPricePerHour = (decimal)carType.GetProperty("RentalPricePerHour").GetValue(carObj),
                            StatusText = (string)carType.GetProperty("StatusText").GetValue(carObj),
                            DisplayText = (string)carType.GetProperty("DisplayText").GetValue(carObj)
                        });
                    }
                }

                if (!string.IsNullOrWhiteSpace(textBoxSearch.Text))
                {
                    var searchTerm = textBoxSearch.Text.ToLower();
                    displayItems = displayItems.Where(c =>
                        c.Brand.ToLower().Contains(searchTerm) ||
                        c.Model.ToLower().Contains(searchTerm) ||
                        c.LicensePlate.ToLower().Contains(searchTerm)
                    ).ToList();
                }

                dataGridViewCars.DataSource = new BindingList<CarDisplayItem>(displayItems);
                UpdateStatusBar(displayItems);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"������ ��� �������� ������: {ex.Message}",
                    "������", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Пересчитывает и отображает агрегированную статистику по списку.
        /// </summary>
        /// <param name="cars">Текущие элементы, отображаемые в таблице.</param>
        private void UpdateStatusBar(List<CarDisplayItem> cars)
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

        /// <summary>
        /// Добавляет новый автомобиль через модальную форму.
        /// </summary>
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            using (var addForm = new CarEditForm())
            {
                if (addForm.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        _carService.CreateCar(
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

        /// <summary>
        /// Редактирует выбранный автомобиль через модальную форму.
        /// </summary>
        private void buttonEdit_Click(object sender, EventArgs e)
        {
            if (dataGridViewCars.CurrentRow?.DataBoundItem is CarDisplayItem selectedCar)
            {
                int carId = selectedCar.Id;

                var currentValues = _carService.GetCarValuesForEdit(carId);
                if (currentValues == null)
                {
                    MessageBox.Show("Автомобиль не найден!", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                using (var editForm = new CarEditForm(
                    (string)currentValues[0],  // марка
                    (string)currentValues[1],  // модель
                    (string)currentValues[2],  // номер
                    (int)currentValues[3],     // год
                    (int)currentValues[4],     // пробег
                    (decimal)currentValues[5], // цена
                    (int)currentValues[6]      // статус
                ))
                {
                    if (editForm.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            if (_carService.UpdateCarDetails(carId, editForm.Brand, editForm.Model,
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

        /// <summary>
        /// Удаляет выбранный автомобиль после подтверждения.
        /// </summary>
        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (dataGridViewCars.CurrentRow?.DataBoundItem is CarDisplayItem selectedCar)
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
                    if (_carService.DeleteCar(carId))
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

        /// <summary>
        /// Арендует выбранный автомобиль и обновляет список.
        /// </summary>
        private void buttonRent_Click(object sender, EventArgs e)
        {
            if (dataGridViewCars.CurrentRow?.DataBoundItem is CarDisplayItem selectedCar)
            {
                int carId = selectedCar.Id;

                if (_carService.RentCar(carId))
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

        /// <summary>
        /// Открывает форму расчёта стоимости для выбранного автомобиля.
        /// </summary>
        private void buttonCalculate_Click(object sender, EventArgs e)
        {
            if (dataGridViewCars.CurrentRow?.DataBoundItem is CarDisplayItem selectedCar)
            {
                int carId = selectedCar.Id;

                using (var calcForm = new CalculateCostForm(carId, _carService))
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

        /// <summary>
        /// Обновляет список автомобилей.
        /// </summary>
        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            RefreshCarsList();
        }

        /// <summary>
        /// Применяет фильтр по мере изменения текста поиска.
        /// </summary>
        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            RefreshCarsList();
        }

        /// <summary>
        /// Применяет фильтр поиска по кнопке.
        /// </summary>
        private void buttonSearch_Click(object sender, EventArgs e)
        {
            RefreshCarsList();
        }

        /// <summary>
        /// Включает/отключает кнопки действий в зависимости от выбранной строки.
        /// </summary>
        private void dataGridViewCars_SelectionChanged(object sender, EventArgs e)
        {
            bool hasSelection = dataGridViewCars.CurrentRow != null;
            buttonEdit.Enabled = hasSelection;
            buttonDelete.Enabled = hasSelection;
            buttonRent.Enabled = hasSelection;
            buttonCalculate.Enabled = hasSelection;
        }

        /// <summary>
        /// Запрашивает подтверждение перед закрытием приложения.
        /// </summary>
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