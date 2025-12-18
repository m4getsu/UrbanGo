using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Model;
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
        /// <param name="car">Автомобиль для которого генерируется QR-код.</param>
        /// <param name="pixelsPerModule">Размер модуля QR-кода (по умолчанию 20).</param>
        /// <returns>Изображение QR-кода в формате PNG (массив байтов).</returns>
        public byte[] GenerateQRCode(Car car, int pixelsPerModule = 20)
        {
            if (car == null)
                throw new ArgumentNullException(nameof(car));

            var carInfo = FormatCarInfo(car);

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
        /// <param name="car">Автомобиль для которого генерируется QR-код.</param>
        /// <param name="filePath">Путь к файлу для сохранения QR-кода.</param>
        /// <param name="pixelsPerModule">Размер модуля QR-кода (по умолчанию 20).</param>
        public void SaveQRCodeToFile(Car car, string filePath, int pixelsPerModule = 20)
        {
            if (car == null)
                throw new ArgumentNullException(nameof(car));
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("Путь к файлу не может быть пустым", nameof(filePath));

            var carInfo = FormatCarInfo(car);

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
        /// <param name="car">Автомобиль для форматирования.</param>
        /// <returns>Форматированная строка с информацией об автомобиле.</returns>
        public string FormatCarInfo(Car car)
        {
            if (car == null)
                throw new ArgumentNullException(nameof(car));

            return $"UrbanGo - Аренда автомобилей\n" +
                   $"══════════════════════════════\n" +
                   $"Гос. номер: {car.LicensePlate}\n" +
                   $"Марка: {car.Brand}\n" +
                   $"Модель: {car.Model}\n" +
                   $"Год выпуска: {car.Year}\n" +
                   $"Пробег: {car.Mileage:N0} км\n" +
                   $"Цена/час: {car.RentalPricePerHour:N2} ₽\n" +
                   $"Статус: {car.Status}\n" +
                   $"══════════════════════════════\n" +
                   $"ID: {car.Id}";
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
