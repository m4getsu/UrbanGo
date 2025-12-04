using System;

namespace Presenter.Events
{
    /// <summary>
    /// Базовый класс аргументов событий модели.
    /// </summary>
    public class ModelEventArgs : EventArgs
    {
        /// <summary>
        /// Сообщение о событии.
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Время возникновения события.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.Now;

        public ModelEventArgs()
        {
        }

        public ModelEventArgs(string message)
        {
            Message = message;
        }
    }

    /// <summary>
    /// Аргументы события операции с автомобилем.
    /// </summary>
    public class CarOperationEventArgs : ModelEventArgs
    {
        /// <summary>
        /// ID автомобиля.
        /// </summary>
        public int CarId { get; set; }

        /// <summary>
        /// Тип операции (Create, Update, Delete, Rent, Calculate).
        /// </summary>
        public string OperationType { get; set; } = string.Empty;

        /// <summary>
        /// Была ли операция успешной.
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Дополнительные детали операции.
        /// </summary>
        public string Details { get; set; } = string.Empty;

        public CarOperationEventArgs()
        {
        }

        public CarOperationEventArgs(int carId, string operationType, bool isSuccess, string details = "")
        {
            CarId = carId;
            OperationType = operationType;
            IsSuccess = isSuccess;
            Details = details;
            Message = $"{operationType} car ID={carId}: {(isSuccess ? "Success" : "Failed")}";
        }
    }

    /// <summary>
    /// Аргументы события валидации данных.
    /// </summary>
    public class ValidationEventArgs : ModelEventArgs
    {
        /// <summary>
        /// Результат валидации.
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// Список ошибок валидации.
        /// </summary>
        public List<string> Errors { get; set; } = new List<string>();

        public ValidationEventArgs()
        {
        }

        public ValidationEventArgs(bool isValid, List<string> errors)
        {
            IsValid = isValid;
            Errors = errors;
            Message = isValid ? "Validation successful" : $"Validation failed: {string.Join(", ", errors)}";
        }
    }

    /// <summary>
    /// Аргументы события импорта/экспорта.
    /// </summary>
    public class ImportExportEventArgs : ModelEventArgs
    {
        /// <summary>
        /// Путь к файлу.
        /// </summary>
        public string FilePath { get; set; } = string.Empty;

        /// <summary>
        /// Тип операции (Import или Export).
        /// </summary>
        public string OperationType { get; set; } = string.Empty;

        /// <summary>
        /// Количество обработанных записей.
        /// </summary>
        public int ProcessedCount { get; set; }

        /// <summary>
        /// Количество успешных записей.
        /// </summary>
        public int SuccessCount { get; set; }

        /// <summary>
        /// Количество ошибок.
        /// </summary>
        public int ErrorCount { get; set; }

        public ImportExportEventArgs()
        {
        }

        public ImportExportEventArgs(string filePath, string operationType, int processedCount, int successCount, int errorCount)
        {
            FilePath = filePath;
            OperationType = operationType;
            ProcessedCount = processedCount;
            SuccessCount = successCount;
            ErrorCount = errorCount;
            Message = $"{operationType}: {successCount}/{processedCount} successful, {errorCount} errors";
        }
    }
}
