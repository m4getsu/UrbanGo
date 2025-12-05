using System;
using System.Windows.Forms;
using Shared;

namespace AIS.Forms
{
    /// <summary>
    /// Форма для импорта автомобилей из CSV и JSON файлов.
    /// </summary>
    public partial class CarImportForm : Form, ICarImportView
    {
        public event EventHandler<string>? FileSelected;
        public event EventHandler? ValidateRequested;
        public event EventHandler? ImportRequested;
        public event EventHandler? CloseRequested;

        private string? _selectedFilePath;
        private BussinessLogic.Services.Import.ImportFormat _selectedFormat = BussinessLogic.Services.Import.ImportFormat.Csv;

        public string SelectedFilePath => _selectedFilePath ?? string.Empty;
        string ICarImportView.ImportFormat => _selectedFormat.ToString();

        /// <summary>
        /// Инициализирует новый экземпляр формы импорта для MVP.
        /// </summary>
        public CarImportForm()
        {
            InitializeComponent();
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

        private void btnValidate_Click(object sender, EventArgs e)
        {
            ValidateRequested?.Invoke(this, EventArgs.Empty);
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            ImportRequested?.Invoke(this, EventArgs.Empty);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            CloseRequested?.Invoke(this, EventArgs.Empty);
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }


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
