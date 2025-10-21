using System.Collections.Generic;
using Model;

namespace BussinessLogic
{
    /// <summary>
    /// Определяет методы для управления автомобилями в системе каршеринга.
    /// </summary>
    public interface ICarService
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
        /// Возвращает автомобиль по его уникальному идентификатору.
        /// </summary>
        /// <param name="id">ID автомобиля.</param>
        /// <returns>Найденный автомобиль или null, если не найден.</returns>
        Car GetCar(int id);

        /// <summary>
        /// Возвращает список всех автомобилей в системе.
        /// </summary>
        /// <returns>Список автомобилей.</returns>
        List<Car> GetAllCars();

        /// <summary>
        /// Обновляет данные существующего автомобиля.
        /// </summary>
        /// <param name="carToUpdate">Автомобиль с обновленными данными.</param>
        /// <returns>True, если обновление прошло успешно; иначе False.</returns>
        bool UpdateCar(Car carToUpdate);

        /// <summary>
        /// Удаляет автомобиль по его ID.
        /// </summary>
        /// <param name="id">ID автомобиля для удаления.</param>
        /// <returns>True, если удаление прошло успешно; иначе False.</returns>
        bool DeleteCar(int id);

        /// <summary>
        /// Возвращает список автомобилей, доступных для аренды.
        /// </summary>
        /// <returns>Список доступных автомобилей.</returns>
        List<Car> GetAvailableCars();

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
        /// <returns>Итоговая стоимость аренды.</returns>
        decimal CalculateRentalCost(int carId, int hours);

        /// <summary>
        /// Получает строковое представление автомобиля по ID.
        /// </summary>
        /// <param name="carId">ID автомобиля.</param>
        /// <returns>Строковое описание автомобиля или сообщение об ошибке.</returns>
        string GetCarDescription(int carId);

        /// <summary>
        /// Получает список строковых представлений всех автомобилей.
        /// </summary>
        /// <returns>Список строковых описаний автомобилей.</returns>
        List<string> GetAllCarsDescriptions();

        /// <summary>
        /// Получает список строковых представлений доступных автомобилей.
        /// </summary>
        /// <returns>Список строковых описаний доступных автомобилей.</returns>
        List<string> GetAvailableCarsDescriptions();

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
        /// Получает текущие значения автомобиля для редактирования.
        /// </summary>
        /// <param name="id">ID автомобиля.</param>
        /// <returns>Массив значений [brand, model, licensePlate, year, mileage, price, status] или null если не найден.</returns>
        object[] GetCarValuesForEdit(int id);

        /// <summary>
        /// Получает информацию об автомобиле для отображения в UI.
        /// </summary>
        /// <param name="carId">ID автомобиля.</param>
        /// <returns>Объект с данными для отображения или null.</returns>
        object GetCarForDisplay(int carId);

        /// <summary>
        /// Получает список автомобилей для отображения в UI.
        /// </summary>
        /// <returns>Список объектов для отображения.</returns>
        List<object> GetCarsForDisplay();

        /// <summary>
        /// Получает информацию об автомобиле для расчета стоимости.
        /// </summary>
        /// <param name="carId">ID автомобиля.</param>
        /// <returns>Объект с данными для расчета или null.</returns>
        object GetCarForCalculation(int carId);

    }
}
