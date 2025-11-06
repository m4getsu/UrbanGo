using Model;

namespace BussinessLogic.Services
{
    /// <summary>
    /// Определяет методы для управления автомобилями (CRUD и бизнес-операции).
    /// Следует принципу разделения интерфейсов (Interface Segregation Principle).
    /// </summary>
    public interface ICarManagementService
    {
        /// <summary>
        /// Создает новый автомобиль и добавляет его в систему.
        /// </summary>
        /// <param name="brand">Марка автомобиля (например, Toyota).</param>
        /// <param name="model">Модель автомобиля (например, Camry).</param>
        /// <param name="licensePlate">Гос. номер автомобиля.</param>
        /// <param name="year">Год выпуска.</param>
        /// <param name="mileage">Текущий пробег, км.</param>
        /// <param name="rentalPricePerHour">Стоимость аренды за час.</param>
        /// <returns>Созданный объект автомобиля.</returns>
        Car CreateCar(string brand, string model, string licensePlate, int year, int mileage, decimal rentalPricePerHour);

        /// <summary>
        /// Обновляет данные существующего автомобиля.
        /// </summary>
        /// <param name="carToUpdate">Автомобиль с обновленными данными.</param>
        /// <returns>True, если обновление прошло успешно; иначе False.</returns>
        bool UpdateCar(Car carToUpdate);

        /// <summary>
        /// Обновляет автомобиль используя отдельные параметры вместо объекта Car.
        /// </summary>
        /// <param name="id">ID автомобиля.</param>
        /// <param name="brand">Марка автомобиля.</param>
        /// <param name="model">Модель автомобиля.</param>
        /// <param name="licensePlate">Государственный номер.</param>
        /// <param name="year">Год выпуска.</param>
        /// <param name="mileage">Пробег.</param>
        /// <param name="rentalPricePerHour">Цена аренды в час.</param>
        /// <param name="status">Статус автомобиля.</param>
        /// <returns>True, если обновление прошло успешно, иначе False.</returns>
        bool UpdateCarDetails(int id, string brand, string model, string licensePlate, int year, int mileage, decimal rentalPricePerHour, int status);

        /// <summary>
        /// Удаляет автомобиль по его ID.
        /// </summary>
        /// <param name="id">ID автомобиля для удаления.</param>
        /// <returns>True, если удаление прошло успешно; иначе False.</returns>
        bool DeleteCar(int id);

        /// <summary>
        /// Выполняет аренду автомобиля по его ID.
        /// </summary>
        /// <param name="carId">ID автомобиля.</param>
        /// <returns>True, если аренда прошла успешно; иначе False.</returns>
        bool RentCar(int carId);

        /// <summary>
        /// Рассчитывает стоимость аренды автомобиля на указанное количество часов.
        /// </summary>
        /// <param name="carId">ID автомобиля.</param>
        /// <param name="hours">Количество часов аренды.</param>
        /// <param name="promoCode">Необязательный промокод для применения скидки.</param>
        /// <returns>Итоговая стоимость аренды.</returns>
        decimal CalculateRentalCost(int carId, int hours, string promoCode = null);
    }
}
