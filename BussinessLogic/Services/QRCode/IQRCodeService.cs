using BussinessLogic.Dto;

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
        /// <param name="carDto">Данные автомобиля для генерации QR-кода</param>
        /// <param name="pixelsPerModule">Размер модуля QR-кода (по умолчанию 20)</param>
        /// <returns>Изображение QR-кода в формате PNG (массив байтов)</returns>
        byte[] GenerateQRCode(CarQRDto carDto, int pixelsPerModule = 20);

        /// <summary>
        /// Сохраняет QR-код автомобиля в файл
        /// </summary>
        /// <param name="carDto">Данные автомобиля для генерации QR-кода</param>
        /// <param name="filePath">Путь к файлу для сохранения</param>
        /// <param name="pixelsPerModule">Размер модуля QR-кода (по умолчанию 20)</param>
        void SaveQRCodeToFile(CarQRDto carDto, string filePath, int pixelsPerModule = 20);

        /// <summary>
        /// Форматирует информацию об автомобиле для QR-кода
        /// </summary>
        /// <param name="carDto">Данные автомобиля</param>
        /// <returns>Форматированная строка с данными</returns>
        string FormatCarInfo(CarQRDto carDto);
    }
}
