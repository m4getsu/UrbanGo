namespace BussinessLogic.Services.Import.Models
{
    /// <summary>
    /// Результат операции импорта автомобилей из внешнего источника.
    /// </summary>
    public class ImportResult
    {
        /// <summary>
        /// Общее количество записей в файле.
        /// </summary>
        public int TotalRecords { get; set; }

        /// <summary>
        /// Количество успешно импортированных записей.
        /// </summary>
        public int SuccessfulImports { get; set; }

        /// <summary>
        /// Количество пропущенных записей (дубликаты по госномеру).
        /// </summary>
        public int SkippedRecords { get; set; }

        /// <summary>
        /// Количество записей с ошибками валидации или сохранения.
        /// </summary>
        public int FailedRecords { get; set; }

        /// <summary>
        /// Список ошибок импорта с номерами строк и описанием проблем.
        /// </summary>
        public List<ImportError> Errors { get; set; } = new List<ImportError>();

        /// <summary>
        /// Признак успешности операции (нет критических ошибок).
        /// </summary>
        public bool IsSuccess => FailedRecords == 0;

        /// <summary>
        /// Возвращает текстовое представление результата импорта.
        /// </summary>
        /// <returns>Форматированная строка с итогами импорта.</returns>
        public string GetSummary()
        {
            return $"Импорт завершен.\n" +
                   $"Всего записей: {TotalRecords}\n" +
                   $"Успешно: {SuccessfulImports}\n" +
                   $"Пропущено (дубликаты): {SkippedRecords}\n" +
                   $"Ошибок: {FailedRecords}";
        }
    }

    /// <summary>
    /// Информация об ошибке при импорте конкретной записи.
    /// </summary>
    public class ImportError
    {
        /// <summary>
        /// Номер строки в файле, где произошла ошибка (0 для общих ошибок файла).
        /// </summary>
        public int LineNumber { get; set; }

        /// <summary>
        /// Текстовое описание ошибки.
        /// </summary>
        public string ErrorMessage { get; set; } = string.Empty;

        /// <summary>
        /// Исходные данные строки, вызвавшей ошибку.
        /// </summary>
        public string RawData { get; set; } = string.Empty;

        /// <summary>
        /// Возвращает строковое представление ошибки.
        /// </summary>
        /// <returns>Форматированная строка с номером строки и описанием ошибки.</returns>
        public override string ToString()
        {
            return $"Строка {LineNumber}: {ErrorMessage}";
        }
    }
}
