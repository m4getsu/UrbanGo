using System;

namespace Shared
{
    /// <summary>
    /// Интерфейс представления для импорта автомобилей из файлов CSV/JSON.
    /// </summary>
    public interface ICarImportView
    {
        // События

        /// <summary>
        /// Событие выбора файла для импорта.
        /// Аргумент: путь к файлу
        /// </summary>
        event EventHandler<string> FileSelected;

        /// <summary>
        /// Событие валидации файла перед импортом.
        /// </summary>
        event EventHandler ValidateRequested;

        /// <summary>
        /// Событие импорта данных из файла.
        /// </summary>
        event EventHandler ImportRequested;

        /// <summary>
        /// Событие закрытия формы.
        /// </summary>
        event EventHandler CloseRequested;

        // Свойства

        /// <summary>
        /// Путь к выбранному файлу.
        /// </summary>
        string SelectedFilePath { get; }

        /// <summary>
        /// Формат импорта (CSV или JSON).
        /// </summary>
        string ImportFormat { get; }

        // Методы для отображения данных

        /// <summary>
        /// Отображает информацию о выбранном файле.
        /// </summary>
        /// <param name="fileName">Имя файла.</param>
        /// <param name="fileSize">Размер файла.</param>
        void DisplayFileInfo(string fileName, long fileSize);

        /// <summary>
        /// Отображает результаты валидации.
        /// </summary>
        /// <param name="isValid">Результат валидации.</param>
        /// <param name="message">Сообщение с результатами.</param>
        void DisplayValidationResult(bool isValid, string message);

        /// <summary>
        /// Отображает прогресс импорта.
        /// </summary>
        /// <param name="current">Текущее значение.</param>
        /// <param name="total">Общее количество.</param>
        void UpdateProgress(int current, int total);

        /// <summary>
        /// Отображает результат импорта.
        /// </summary>
        /// <param name="successCount">Успешно импортировано.</param>
        /// <param name="failedCount">Ошибок.</param>
        /// <param name="skippedCount">Пропущено.</param>
        /// <param name="errors">Список ошибок.</param>
        void DisplayImportResult(int successCount, int failedCount, int skippedCount, string errors);

        /// <summary>
        /// Включает/отключает кнопку импорта.
        /// </summary>
        /// <param name="enabled">True для включения.</param>
        void SetImportButtonEnabled(bool enabled);

        /// <summary>
        /// Отображает сообщение об ошибке.
        /// </summary>
        /// <param name="message">Текст ошибки.</param>
        void ShowError(string message);

        /// <summary>
        /// Закрывает представление с результатом OK.
        /// </summary>
        void CloseWithSuccess();
    }
}
