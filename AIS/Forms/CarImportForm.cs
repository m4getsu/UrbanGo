using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using BussinessLogic.Services.Import;
using BussinessLogic.Services.Import.Models;

namespace AIS.Forms
{
    /// <summary>
    /// Форма для импорта автомобилей из CSV и JSON файлов.
    /// </summary>
    public partial class CarImportForm : Form
    {
        private readonly ICarImportService _importService;
        private string? _selectedFilePath;
        private ImportFormat _selectedFormat = ImportFormat.Csv;

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
            // Настройка начального состояния элементов управления
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
            }
        }

        private void rbFormat_CheckedChanged(object sender, EventArgs e)
        {
            if (rbCsv.Checked)
                _selectedFormat = ImportFormat.Csv;
            else if (rbJson.Checked)
                _selectedFormat = ImportFormat.Json;

            // Сбрасываем выбранный файл при смене формата
            _selectedFilePath = null;
            txtFilePath.Clear();
            txtResults.Clear();
            btnValidate.Enabled = false;
            btnImport.Enabled = false;
        }

        private async void btnValidate_Click(object sender, EventArgs e)
        {
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

                if (_selectedFormat == ImportFormat.Csv)
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

                    // Закрываем форму с положительным результатом
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
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
