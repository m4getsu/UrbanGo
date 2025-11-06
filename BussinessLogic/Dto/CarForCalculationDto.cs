namespace BussinessLogic.Dto
{
    /// <summary>
    /// Объект передачи данных для расчета стоимости аренды автомобиля.
    /// Содержит минимальный набор данных, необходимых для расчета.
    /// </summary>
    public class CarForCalculationDto
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
        /// Стоимость аренды автомобиля за один час.
        /// </summary>
        public decimal RentalPricePerHour { get; set; }

        /// <summary>
        /// Форматированный текст для отображения в пользовательском интерфейсе.
        /// </summary>
        public string DisplayText { get; set; } = string.Empty;
    }
}
