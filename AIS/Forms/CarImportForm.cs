using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using BussinessLogic.Services.Import;
using BussinessLogic.Services.Import.Models;
using Shared;

namespace AIS.Forms
{
    /// <summary>
    /// Форма для импорта автомобилей из CSV и JSON файлов.
    /// </summary>
    public partial class CarImportForm : Form, ICarImportView
    {
        // События для MVP
        public event EventHandler<string>? FileSelected;
        public event EventHandler? ValidateRequested;
        public event EventHandler? ImportRequested;
        public event EventHandler? CloseRequested;

        private readonly ICarImportService _importService;
        private string? _selectedFilePath;
        private BussinessLogic.Services.Import.ImportFormat _selectedFormat = BussinessLogic.Services.Import.ImportFormat.Csv;

        // Свойства интерфейса
        public string SelectedFilePath => _selectedFilePath ?? string.Empty;
        string ICarImportView.ImportFormat => _selectedFormat.ToString();

        /// <summary>
        /// Инициализирует новый экземпляр формы импорта.
        /// </summary>
        /// <param name="importService">Сервис импорта автомобилей.</param>
        public CarImportForm(ICarImportService importService)
        {
            InitializeComponent();
            _importService = importService ?? throw new ArgumentNullException(nameof(importService));
            InitializeFormControls();
        }

        private void InitializeFormControls()
        {
            btnValidate.Enabled = false;
            btnImport.Enabled = false;
            rbCsv.Checked = true;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using var dialog = new OpenFileDialog
            {
                Filter = rbCsv.Checked
                    ? "CSV файлы (*.csv)|*.csv|Все файлы (*.*)|*.*"
                    : "JSON файлы (*.json)|*.json|Все файлы (*.*)|*.*",
                Title = "Выберите файл для импорта автомобилей"
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                _selectedFilePath = dialog.FileName;
                txtFilePath.Text = _selectedFilePath;
                btnValidate.Enabled = true;
                btnImport.Enabled = false;
                txtResults.Clear();

                // Генерируем событие для MVP
                FileSelected?.Invoke(this, _selectedFilePath);
            }
        }

        private void rbFormat_CheckedChanged(object sender, EventArgs e)
        {
            if (rbCsv.Checked)
                _selectedFormat = BussinessLogic.Services.Import.ImportFormat.Csv;
            else if (rbJson.Checked)
                _selectedFormat = BussinessLogic.Services.Import.ImportFormat.Json;

            _selectedFilePath = null;
            txtFilePath.Clear();
            txtResults.Clear();
            btnValidate.Enabled = false;
            btnImport.Enabled = false;
        }

        private async void btnValidate_Click(object sender, EventArgs e)
        {
            // Генерируем событие для MVP
            ValidateRequested?.Invoke(this, EventArgs.Empty);

            if (string.IsNullOrEmpty(_selectedFilePath))
            {
                MessageBox.Show("Выберите файл для проверки", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!File.Exists(_selectedFilePath))
            {
                MessageBox.Show("Указанный файл не существует", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            btnValidate.Enabled = false;
            btnImport.Enabled = false;
            btnBrowse.Enabled = false;
            progressBar.Style = ProgressBarStyle.Marquee;
            txtResults.Text = "Проверка файла...";

            try
            {
                var result = await Task.Run(() => _importService.ValidateImportFile(_selectedFilePath, _selectedFormat));

                DisplayValidationResult(result);

                if (result.TotalRecords > 0 && result.FailedRecords < result.TotalRecords)
                {
                    btnImport.Enabled = true;
                }

                if (result.FailedRecords > 0)
                {
                    MessageBox.Show(
                        $"Найдены ошибки в {result.FailedRecords} записях из {result.TotalRecords}.\n\n" +
                        $"Успешно пройдут валидацию: {result.SuccessfulImports}\n" +
                        $"Будут пропущены (дубликаты): {result.SkippedRecords}\n\n" +
                        $"Проверьте детали ошибок ниже.",
                        "Предупреждение",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                }
                else if (result.TotalRecords > 0)
                {
                    MessageBox.Show(
                        $"Файл прошел проверку!\n\n" +
                        $"Готово к импорту: {result.SuccessfulImports} записей\n" +
                        $"Будут пропущены (дубликаты): {result.SkippedRecords}",
                        "Проверка пройдена",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
                else
                {
                    MessageBox.Show(
                        "Файл не содержит данных для импорта",
                        "Информация",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show($"Файл не найден: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtResults.Text = $"Ошибка: {ex.Message}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка проверки файла:\n{ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtResults.Text = $"Критическая ошибка: {ex.Message}";
            }
            finally
            {
                progressBar.Style = ProgressBarStyle.Continuous;
                btnValidate.Enabled = true;
                btnBrowse.Enabled = true;
            }
        }

        private async void btnImport_Click(object sender, EventArgs e)
        {
            // Генерируем событие для MVP
            ImportRequested?.Invoke(this, EventArgs.Empty);

            if (string.IsNullOrEmpty(_selectedFilePath))
            {
                MessageBox.Show("Выберите файл для импорта", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirmResult = MessageBox.Show(
                "Вы уверены, что хотите импортировать данные из выбранного файла?\n\n" +
                "Автомобили с дубликатами госномеров будут пропущены.",
                "Подтверждение импорта",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirmResult != DialogResult.Yes)
                return;

            btnValidate.Enabled = false;
            btnImport.Enabled = false;
            btnBrowse.Enabled = false;
            progressBar.Style = ProgressBarStyle.Marquee;
            txtResults.Text = "Выполняется импорт данных...";

            try
            {
                ImportResult result;

                if (_selectedFormat == BussinessLogic.Services.Import.ImportFormat.Csv)
                    result = await Task.Run(() => _importService.ImportFromCsv(_selectedFilePath));
                else
                    result = await Task.Run(() => _importService.ImportFromJson(_selectedFilePath));

                DisplayImportResult(result);

                if (result.SuccessfulImports > 0)
                {
                    var messageText = $"Импорт завершен!\n\n" +
                                     $"Успешно импортировано: {result.SuccessfulImports}\n" +
                                     $"Пропущено (дубликаты): {result.SkippedRecords}\n" +
                                     $"Ошибок: {result.FailedRecords}";

                    MessageBox.Show(
                        messageText,
                        "Результат импорта",
                        MessageBoxButtons.OK,
                        result.FailedRecords > 0 ? MessageBoxIcon.Warning : MessageBoxIcon.Information
                    );

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show(
                        "Не удалось импортировать ни одной записи.\n" +
                        "Все записи либо содержат ошибки, либо являются дубликатами.\n\n" +
                        "Проверьте файл и повторите попытку.",
                        "Импорт не выполнен",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show($"Файл не найден: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtResults.Text = $"Ошибка: {ex.Message}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Критическая ошибка импорта:\n{ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtResults.Text = $"Критическая ошибка: {ex.Message}";
            }
            finally
            {
                progressBar.Style = ProgressBarStyle.Continuous;
                btnValidate.Enabled = true;
                btnBrowse.Enabled = true;
            }
        }

        private void DisplayValidationResult(ImportResult result)
        {
            txtResults.Clear();
            txtResults.AppendText("=== РЕЗУЛЬТАТЫ ПРОВЕРКИ ФАЙЛА ===\r\n\r\n");
            txtResults.AppendText(result.GetSummary());

            if (result.Errors.Count > 0)
            {
                txtResults.AppendText("\r\n\r\n=== НАЙДЕННЫЕ ПРОБЛЕМЫ ===\r\n");

                int displayCount = Math.Min(result.Errors.Count, 50);
                for (int i = 0; i < displayCount; i++)
                {
                    txtResults.AppendText($"\r\n{result.Errors[i]}");
                }

                if (result.Errors.Count > 50)
                {
                    txtResults.AppendText($"\r\n\r\n... и еще {result.Errors.Count - 50} проблем(ы).");
                    txtResults.AppendText("\r\nИсправьте первые ошибки и проверьте файл повторно.");
                }
            }
            else
            {
                txtResults.AppendText("\r\n\r\n✓ Все записи прошли проверку!");
            }
        }

        private void DisplayImportResult(ImportResult result)
        {
            txtResults.Clear();
            txtResults.AppendText("=== РЕЗУЛЬТАТЫ ИМПОРТА ===\r\n\r\n");
            txtResults.AppendText(result.GetSummary());

            if (result.Errors.Count > 0)
            {
                txtResults.AppendText("\r\n\r\n=== ОШИБКИ И ПРЕДУПРЕЖДЕНИЯ ===\r\n");

                int displayCount = Math.Min(result.Errors.Count, 50);
                for (int i = 0; i < displayCount; i++)
                {
                    txtResults.AppendText($"\r\n{result.Errors[i]}");
                }

                if (result.Errors.Count > 50)
                {
                    txtResults.AppendText($"\r\n\r\n... и еще {result.Errors.Count - 50} записей с проблемами.");
                }
            }

            if (result.SuccessfulImports > 0)
            {
                txtResults.AppendText($"\r\n\r\n✓ Успешно импортировано автомобилей: {result.SuccessfulImports}");
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            // Генерируем событие для MVP
            CloseRequested?.Invoke(this, EventArgs.Empty);

            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        // Реализация методов ICarImportView

        /// <summary>
        /// Отображает информацию о выбранном файле.
        /// </summary>
        public void DisplayFileInfo(string fileName, long fileSize)
        {
            txtFilePath.Text = fileName;
            txtResults.Text = $"Файл: {Path.GetFileName(fileName)}\r\nРазмер: {fileSize / 1024} КБ";
        }

        /// <summary>
        /// Отображает результаты валидации.
        /// </summary>
        public void DisplayValidationResult(bool isValid, string message)
        {
            txtResults.Text = message;
            btnImport.Enabled = isValid;
        }

        /// <summary>
        /// Отображает прогресс импорта.
        /// </summary>
        public void UpdateProgress(int current, int total)
        {
            if (progressBar.Style != ProgressBarStyle.Continuous)
                progressBar.Style = ProgressBarStyle.Continuous;

            progressBar.Maximum = total;
            progressBar.Value = Math.Min(current, total);
        }

        /// <summary>
        /// Отображает результат импорта.
        /// </summary>
        public void DisplayImportResult(int successCount, int failedCount, int skippedCount, string errors)
        {
            txtResults.Clear();
            txtResults.AppendText("=== РЕЗУЛЬТАТЫ ИМПОРТА ===\r\n\r\n");
            txtResults.AppendText($"✓ Успешно импортировано: {successCount}\r\n");
            txtResults.AppendText($"⊘ Пропущено (дубликаты): {skippedCount}\r\n");
            txtResults.AppendText($"✗ Ошибок: {failedCount}\r\n");

            if (!string.IsNullOrEmpty(errors))
            {
                txtResults.AppendText("\r\n=== ОШИБКИ ===\r\n");
                txtResults.AppendText(errors);
            }
        }

        /// <summary>
        /// Включает/отключает кнопку импорта.
        /// </summary>
        public void SetImportButtonEnabled(bool enabled)
        {
            btnImport.Enabled = enabled;
        }

        /// <summary>
        /// Отображает сообщение об ошибке.
        /// </summary>
        public void ShowError(string message)
        {
            MessageBox.Show(message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Закрывает представление с результатом OK.
        /// </summary>
        public void CloseWithSuccess()
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
