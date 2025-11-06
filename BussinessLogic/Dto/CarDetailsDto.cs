namespace BussinessLogic.Dto
{
    /// <summary>
    /// Объект передачи данных для отображения детальной информации об автомобиле.
    /// Содержит все данные автомобиля и форматированное описание.
    /// </summary>
    public class CarDetailsDto
    {
        /// <summary>
        /// Уникальный идентификатор автомобиля.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Марка автомобиля.
        /// </summary>
        public string Brand { get; set; } = string.Empty;

        /// <summary>
        /// Модель автомобиля.
        /// </summary>
        public string Model { get; set; } = string.Empty;

        /// <summary>
        /// Государственный номерной знак автомобиля.
        /// </summary>
        public string LicensePlate { get; set; } = string.Empty;

        /// <summary>
        /// Год выпуска автомобиля.
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// Текущий пробег автомобиля в километрах.
        /// </summary>
        public int Mileage { get; set; }

        /// <summary>
        /// Стоимость аренды автомобиля за один час.
        /// </summary>
        public decimal RentalPricePerHour { get; set; }

        /// <summary>
        /// Текстовое представление статуса автомобиля.
        /// </summary>
        public string StatusText { get; set; } = string.Empty;

        /// <summary>
        /// Полное форматированное описание автомобиля.
        /// </summary>
        public string Description { get; set; } = string.Empty;
    }
}
