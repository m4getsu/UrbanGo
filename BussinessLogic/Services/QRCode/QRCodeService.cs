using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using BussinessLogic.Dto;
using QRCoder;

namespace BussinessLogic.Services.QRCode
{
    /// <summary>
    /// Реализация сервиса генерации QR-кодов
    /// </summary>
    public class QRCodeService : IQRCodeService
    {
        /// <summary>
        /// Генерирует QR-код с информацией об автомобиле.
        /// </summary>
        /// <param name="carDto">Данные автомобиля для генерации QR-кода.</param>
        /// <param name="pixelsPerModule">Размер модуля QR-кода (по умолчанию 20).</param>
        /// <returns>Изображение QR-кода в формате PNG (массив байтов).</returns>
        public byte[] GenerateQRCode(CarQRDto carDto, int pixelsPerModule = 20)
        {
            if (carDto == null)
                throw new ArgumentNullException(nameof(carDto));

            var carInfo = FormatCarInfo(carDto);

            using (var qrGenerator = new QRCodeGenerator())
            {
                var qrCodeData = qrGenerator.CreateQrCode(carInfo, QRCodeGenerator.ECCLevel.Q);
                using (var qrCode = new QRCoder.QRCode(qrCodeData))
                {
                    using (var qrBitmap = qrCode.GetGraphic(pixelsPerModule))
                    {
                        return ConvertBitmapToByteArray(qrBitmap);
                    }
                }
            }
        }

        /// <summary>
        /// Сохраняет QR-код автомобиля в файл.
        /// </summary>
        /// <param name="carDto">Данные автомобиля для генерации QR-кода.</param>
        /// <param name="filePath">Путь к файлу для сохранения QR-кода.</param>
        /// <param name="pixelsPerModule">Размер модуля QR-кода (по умолчанию 20).</param>
        public void SaveQRCodeToFile(CarQRDto carDto, string filePath, int pixelsPerModule = 20)
        {
            if (carDto == null)
                throw new ArgumentNullException(nameof(carDto));
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("Путь к файлу не может быть пустым", nameof(filePath));

            var carInfo = FormatCarInfo(carDto);

            using (var qrGenerator = new QRCodeGenerator())
            {
                var qrCodeData = qrGenerator.CreateQrCode(carInfo, QRCodeGenerator.ECCLevel.Q);
                using (var qrCode = new QRCoder.QRCode(qrCodeData))
                {
                    using (var qrBitmap = qrCode.GetGraphic(pixelsPerModule))
                    {
                        qrBitmap.Save(filePath, ImageFormat.Png);
                    }
                }
            }
        }

        /// <summary>
        /// Форматирует информацию об автомобиле для отображения в QR-коде.
        /// </summary>
        /// <param name="carDto">Данные автомобиля для форматирования.</param>
        /// <returns>Форматированная строка с информацией об автомобиле.</returns>
        public string FormatCarInfo(CarQRDto carDto)
        {
            if (carDto == null)
                throw new ArgumentNullException(nameof(carDto));

            return $"UrbanGo - Аренда автомобилей\n" +
                   $"══════════════════════════════\n" +
                   $"Гос. номер: {carDto.LicensePlate}\n" +
                   $"Марка: {carDto.Brand}\n" +
                   $"Модель: {carDto.Model}\n" +
                   $"Год выпуска: {carDto.Year}\n" +
                   $"Пробег: {carDto.Mileage:N0} км\n" +
                   $"Цена/час: {carDto.RentalPricePerHour:N2} ₽\n" +
                   $"Статус: {carDto.Status}\n" +
                   $"══════════════════════════════\n" +
                   $"ID: {carDto.Id}";
        }

        /// <summary>
        /// Конвертирует Bitmap в массив байтов (PNG).
        /// </summary>
        /// <param name="bitmap">Bitmap для конвертации.</param>
        /// <returns>Массив байтов в формате PNG.</returns>
        private byte[] ConvertBitmapToByteArray(Bitmap bitmap)
        {
            using (var memoryStream = new MemoryStream())
            {
                bitmap.Save(memoryStream, ImageFormat.Png);
                return memoryStream.ToArray();
            }
        }
    }
}
