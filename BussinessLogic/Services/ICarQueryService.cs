using System.Collections.Generic;
using Model;

namespace BussinessLogic.Services
{
    /// <summary>
    /// Определяет методы для запросов данных об автомобилях.
    /// Следует принципу разделения интерфейсов (Interface Segregation Principle).
    /// </summary>
    public interface ICarQueryService
    {
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
        /// Возвращает список автомобилей, доступных для аренды.
        /// </summary>
        /// <returns>Список доступных автомобилей.</returns>
        List<Car> GetAvailableCars();

        /// <summary>
        /// Получает текущие значения автомобиля для редактирования.
        /// </summary>
        /// <param name="id">ID автомобиля.</param>
        /// <returns>Массив значений [brand, model, licensePlate, year, mileage, price, status] или null если не найден.</returns>
        object[] GetCarValuesForEdit(int id);
    }
}
