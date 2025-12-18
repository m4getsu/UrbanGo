using Model;

namespace BussinessLogic.Services.QRCode
{
    /// <summary>
    /// Сервис для генерации QR-кодов автомобилей
    /// </summary>
    public interface IQRCodeService
    {
        /// <summary>
        /// Генерирует QR-код с информацией об автомобиле
        /// </summary>
        /// <param name="car">Автомобиль для которого генерируется QR-код</param>
        /// <param name="pixelsPerModule">Размер модуля QR-кода (по умолчанию 20)</param>
        /// <returns>Изображение QR-кода в формате PNG (массив байтов)</returns>
        byte[] GenerateQRCode(Car car, int pixelsPerModule = 20);

        /// <summary>
        /// Сохраняет QR-код автомобиля в файл
        /// </summary>
        /// <param name="car">Автомобиль для которого генерируется QR-код</param>
        /// <param name="filePath">Путь к файлу для сохранения</param>
        /// <param name="pixelsPerModule">Размер модуля QR-кода (по умолчанию 20)</param>
        void SaveQRCodeToFile(Car car, string filePath, int pixelsPerModule = 20);

        /// <summary>
        /// Форматирует информацию об автомобиле для QR-кода
        /// </summary>
        /// <param name="car">Автомобиль</param>
        /// <returns>Форматированная строка с данными</returns>
        string FormatCarInfo(Car car);
    }
}
