using BussinessLogic.Services.Import.Models;

namespace BussinessLogic.Services.Import
{
    /// <summary>
    /// Сервис для импорта и экспорта автомобилей из/в внешние источники данных.
    /// </summary>
    public interface ICarImportService
    {
        /// <summary>
        /// Импортирует автомобили из CSV файла.
        /// </summary>
        /// <param name="filePath">Полный путь к CSV файлу.</param>
        /// <returns>Результат импорта с детальной информацией о количестве обработанных записей и ошибках.</returns>
        /// <exception cref="FileNotFoundException">Если указанный файл не существует.</exception>
        ImportResult ImportFromCsv(string filePath);

        /// <summary>
        /// Импортирует автомобили из JSON файла.
        /// </summary>
        /// <param name="filePath">Полный путь к JSON файлу.</param>
        /// <returns>Результат импорта с детальной информацией о количестве обработанных записей и ошибках.</returns>
        /// <exception cref="FileNotFoundException">Если указанный файл не существует.</exception>
        ImportResult ImportFromJson(string filePath);

        /// <summary>
        /// Проверяет валидность данных для импорта без фактического сохранения в базу данных.
        /// </summary>
        /// <param name="filePath">Полный путь к файлу для проверки.</param>
        /// <param name="format">Формат файла (CSV или JSON).</param>
        /// <returns>Результат валидации с информацией о найденных ошибках.</returns>
        /// <exception cref="FileNotFoundException">Если указанный файл не существует.</exception>
        ImportResult ValidateImportFile(string filePath, ImportFormat format);

        /// <summary>
        /// Экспортирует все автомобили из базы данных в CSV файл.
        /// </summary>
        /// <param name="filePath">Полный путь к CSV файлу для сохранения.</param>
        /// <returns>Количество экспортированных записей.</returns>
        int ExportToCsv(string filePath);

        /// <summary>
        /// Экспортирует все автомобили из базы данных в JSON файл.
        /// </summary>
        /// <param name="filePath">Полный путь к JSON файлу для сохранения.</param>
        /// <returns>Количество экспортированных записей.</returns>
        int ExportToJson(string filePath);

        /// <summary>
        /// Экспортирует выбранные автомобили в CSV файл.
        /// </summary>
        /// <param name="carIds">Идентификаторы автомобилей для экспорта.</param>
        /// <param name="filePath">Полный путь к CSV файлу для сохранения.</param>
        /// <returns>Количество экспортированных записей.</returns>
        int ExportToCsv(IEnumerable<int> carIds, string filePath);

        /// <summary>
        /// Экспортирует выбранные автомобили в JSON файл.
        /// </summary>
        /// <param name="carIds">Идентификаторы автомобилей для экспорта.</param>
        /// <param name="filePath">Полный путь к JSON файлу для сохранения.</param>
        /// <returns>Количество экспортированных записей.</returns>
        int ExportToJson(IEnumerable<int> carIds, string filePath);
    }

    /// <summary>
    /// Формат файла для операций импорта.
    /// </summary>
    public enum ImportFormat
    {
        /// <summary>
        /// Comma-Separated Values (CSV) формат.
        /// </summary>
        Csv,

        /// <summary>
        /// JavaScript Object Notation (JSON) формат.
        /// </summary>
        Json
    }
}
