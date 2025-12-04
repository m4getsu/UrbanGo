using System;
using System.IO;
using System.Linq;
using Shared;
using Model;
using Presenter.Events;
using BussinessLogic.Services.Import;

namespace Presenter
{
    /// <summary>
    /// Presenter для представления импорта автомобилей.
    /// </summary>
    public class CarImportPresenter
    {
        private readonly ICarImportView _view;
        private readonly CarSharingModel _model;

        /// <summary>
        /// Инициализирует presenter для импорта.
        /// </summary>
        /// <param name="view">Представление импорта.</param>
        /// <param name="model">Модель системы.</param>
        public CarImportPresenter(ICarImportView view, CarSharingModel model)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _model = model ?? throw new ArgumentNullException(nameof(model));

            SubscribeToViewEvents();
            SubscribeToModelEvents();
        }

        /// <summary>
        /// Подписка на события представления.
        /// </summary>
        private void SubscribeToViewEvents()
        {
            _view.FileSelected += OnFileSelected;
            _view.ValidateRequested += OnValidateRequested;
            _view.ImportRequested += OnImportRequested;
            _view.CloseRequested += OnCloseRequested;
        }

        /// <summary>
        /// Подписка на события модели.
        /// </summary>
        private void SubscribeToModelEvents()
        {
            _model.ValidationPerformed += OnValidationPerformed;
            _model.ImportExportPerformed += OnImportExportPerformed;
            _model.ErrorOccurred += OnModelErrorOccurred;
        }

        // Обработчики событий View

        private void OnFileSelected(object? sender, string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
            {
                _view.ShowError("Файл не найден");
                return;
            }

            try
            {
                var fileInfo = new FileInfo(filePath);
                _view.DisplayFileInfo(fileInfo.Name, fileInfo.Length);
                _view.SetImportButtonEnabled(true);
            }
            catch (Exception ex)
            {
                _view.ShowError($"Ошибка чтения файла: {ex.Message}");
                _view.SetImportButtonEnabled(false);
            }
        }

        private void OnValidateRequested(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_view.SelectedFilePath))
            {
                _view.ShowError("Выберите файл для валидации");
                return;
            }

            try
            {
                var format = DetermineImportFormat(_view.SelectedFilePath);
                var result = _model.ValidateImportFile(_view.SelectedFilePath, format);

                bool isValid = result.FailedRecords == 0;
                string message = isValid
                    ? $"Валидация успешна!\nВсего записей: {result.TotalRecords}\nГотово к импорту: {result.TotalRecords}"
                    : $"Обнаружены ошибки!\nВсего записей: {result.TotalRecords}\nОшибок: {result.FailedRecords}\n\n" +
                      string.Join("\n", result.Errors.Take(5).Select(e => $"Строка {e.LineNumber}: {e.ErrorMessage}"));

                _view.DisplayValidationResult(isValid, message);
            }
            catch (Exception ex)
            {
                _view.ShowError($"Ошибка валидации: {ex.Message}");
            }
        }

        private void OnImportRequested(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_view.SelectedFilePath))
            {
                _view.ShowError("Выберите файл для импорта");
                return;
            }

            try
            {
                var format = DetermineImportFormat(_view.SelectedFilePath);
                var result = format == ImportFormat.Csv
                    ? _model.ImportFromCsv(_view.SelectedFilePath)
                    : _model.ImportFromJson(_view.SelectedFilePath);

                // Формируем список ошибок для отображения
                string errors = result.Errors.Any()
                    ? string.Join("\n", result.Errors.Take(10).Select(e => $"Строка {e.LineNumber}: {e.ErrorMessage}"))
                    : string.Empty;

                _view.DisplayImportResult(
                    result.SuccessfulImports,
                    result.FailedRecords,
                    result.SkippedRecords,
                    errors
                );

                if (result.SuccessfulImports > 0)
                {
                    _view.CloseWithSuccess();
                }
            }
            catch (Exception ex)
            {
                _view.ShowError($"Ошибка импорта: {ex.Message}");
            }
        }

        private void OnCloseRequested(object? sender, EventArgs e)
        {
            // Представление само закроется
        }

        // Обработчики событий Model

        private void OnValidationPerformed(object? sender, ValidationEventArgs e)
        {
            // Событие уже обработано в OnValidateRequested
        }

        private void OnImportExportPerformed(object? sender, ImportExportEventArgs e)
        {
            // Событие уже обработано в OnImportRequested
        }

        private void OnModelErrorOccurred(object? sender, ModelEventArgs e)
        {
            _view.ShowError(e.Message);
        }

        // Вспомогательные методы

        /// <summary>
        /// Определяет формат файла по расширению.
        /// </summary>
        private ImportFormat DetermineImportFormat(string filePath)
        {
            var extension = Path.GetExtension(filePath).ToLowerInvariant();
            return extension == ".csv" ? ImportFormat.Csv : ImportFormat.Json;
        }
    }
}
