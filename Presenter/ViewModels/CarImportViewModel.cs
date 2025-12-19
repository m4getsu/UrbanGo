using System;
using System.Windows.Input;
using Presenter.Commands;
using BussinessLogic;
using BussinessLogic.Services.Import;
using BussinessLogic.Services.Import.Models;

namespace Presenter.ViewModels
{
    /// <summary>
    /// ViewModel для импорта автомобилей из файла.
    /// </summary>
    public class CarImportViewModel : BaseViewModel
    {
        private readonly VMManager _vmManager;
        private readonly ICarService _carService;
        private readonly ICarImportService _importService;

        private string _filePath = string.Empty;
        private bool _isCsvFormat = true;
        private string _results = string.Empty;
        private int _progressValue;

        public string FilePath
        {
            get => _filePath;
            set => SetProperty(ref _filePath, value);
        }

        public bool IsCsvFormat
        {
            get => _isCsvFormat;
            set => SetProperty(ref _isCsvFormat, value);
        }

        public bool IsJsonFormat
        {
            get => !_isCsvFormat;
            set => SetProperty(ref _isCsvFormat, !value);
        }

        public string Results
        {
            get => _results;
            set => SetProperty(ref _results, value);
        }

        public int ProgressValue
        {
            get => _progressValue;
            set => SetProperty(ref _progressValue, value);
        }

        public ICommand BrowseCommand { get; }
        public ICommand ValidateCommand { get; }
        public ICommand ImportCommand { get; }
        public ICommand CloseCommand { get; }

        public CarImportViewModel(VMManager vmManager, ICarService carService, ICarImportService importService)
        {
            _vmManager = vmManager ?? throw new ArgumentNullException(nameof(vmManager));
            _carService = carService ?? throw new ArgumentNullException(nameof(carService));
            _importService = importService ?? throw new ArgumentNullException(nameof(importService));

            BrowseCommand = new RelayCommand(_ => Browse());
            ValidateCommand = new RelayCommand(_ => Validate(), _ => !string.IsNullOrEmpty(FilePath));
            ImportCommand = new RelayCommand(_ => Import(), _ => !string.IsNullOrEmpty(FilePath));
            CloseCommand = new RelayCommand(_ => Close());
        }

        private void Browse()
        {
            Results = "Выберите файл для импорта...";
        }

        private void Validate()
        {
            try
            {
                Results = $"Проверка файла: {FilePath}\nФормат: {(IsCsvFormat ? "CSV" : "JSON")}\n\n";
                ProgressValue = 25;

                var format = IsCsvFormat ? ImportFormat.Csv : ImportFormat.Json;
                var validationResult = _importService.ValidateImportFile(FilePath, format);

                ProgressValue = 75;

                Results += $"Проверка завершена:\n";
                Results += $"Успешно: {validationResult.IsSuccess}\n";
                Results += $"Всего записей: {validationResult.TotalRecords}\n";
                Results += $"Успешных импортов: {validationResult.SuccessfulImports}\n";
                Results += $"Пропущено (дубликаты): {validationResult.SkippedRecords}\n";
                Results += $"Ошибок: {validationResult.FailedRecords}\n\n";

                if (validationResult.Errors.Count > 0)
                {
                    Results += "Ошибки:\n";
                    foreach (var error in validationResult.Errors)
                    {
                        Results += $"- {error}\n";
                    }
                }

                ProgressValue = 100;
            }
            catch (Exception ex)
            {
                Results = $"Ошибка при проверке файла:\n{ex.Message}";
                ProgressValue = 0;
            }
        }

        private void Import()
        {
            try
            {
                Results = $"Импорт из файла: {FilePath}\nФормат: {(IsCsvFormat ? "CSV" : "JSON")}\n\n";
                ProgressValue = 25;

                ImportResult importResult;
                if (IsCsvFormat)
                {
                    importResult = _importService.ImportFromCsv(FilePath);
                }
                else
                {
                    importResult = _importService.ImportFromJson(FilePath);
                }

                ProgressValue = 75;

                Results += $"Импорт завершен:\n";
                Results += $"Успешно: {importResult.IsSuccess}\n";
                Results += $"Всего записей: {importResult.TotalRecords}\n";
                Results += $"Импортировано записей: {importResult.SuccessfulImports}\n";
                Results += $"Пропущено (дубликаты): {importResult.SkippedRecords}\n";
                Results += $"Ошибок: {importResult.FailedRecords}\n\n";

                if (importResult.Errors.Count > 0)
                {
                    Results += "Ошибки:\n";
                    foreach (var error in importResult.Errors)
                    {
                        Results += $"- {error}\n";
                    }
                }

                ProgressValue = 100;
            }
            catch (Exception ex)
            {
                Results = $"Ошибка при импорте файла:\n{ex.Message}";
                ProgressValue = 0;
            }
        }

        private void Close()
        {
        }
    }
}
