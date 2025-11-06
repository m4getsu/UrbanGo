namespace BussinessLogic.Validation
{
    /// <summary>
    /// Определяет методы для валидации данных автомобилей.
    /// </summary>
    public interface ICarValidator
    {
        /// <summary>
        /// Проверяет корректность данных при создании автомобиля.
        /// </summary>
        /// <param name="brand">Марка автомобиля.</param>
        /// <param name="model">Модель автомобиля.</param>
        /// <param name="licensePlate">Государственный номер.</param>
        /// <param name="year">Год выпуска.</param>
        /// <param name="mileage">Пробег.</param>
        /// <param name="rentalPricePerHour">Стоимость аренды за час.</param>
        void ValidateForCreate(string brand, string model, string licensePlate, int year, int mileage, decimal rentalPricePerHour);

        /// <summary>
        /// Проверяет корректность данных при обновлении автомобиля.
        /// </summary>
        /// <param name="brand">Марка автомобиля.</param>
        /// <param name="model">Модель автомобиля.</param>
        /// <param name="licensePlate">Государственный номер.</param>
        /// <param name="year">Год выпуска.</param>
        /// <param name="mileage">Пробег.</param>
        /// <param name="rentalPricePerHour">Стоимость аренды за час.</param>
        /// <param name="status">Статус автомобиля.</param>
        void ValidateForUpdate(string brand, string model, string licensePlate, int year, int mileage, decimal rentalPricePerHour, int status);
    }
}
