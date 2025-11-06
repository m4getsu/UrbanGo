using System.Collections.Generic;
using BussinessLogic.Dto;

namespace BussinessLogic.Services
{
    /// <summary>
    /// Определяет методы для форматирования и отображения данных автомобилей в пользовательском интерфейсе.
    /// Следует принципу разделения интерфейсов (Interface Segregation Principle).
    /// </summary>
    public interface ICarDisplayService
    {
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
        /// Получает информацию об автомобиле для отображения в UI.
        /// </summary>
        /// <param name="carId">ID автомобиля.</param>
        /// <returns>Объект с данными для отображения или null.</returns>
        CarDetailsDto? GetCarForDisplay(int carId);

        /// <summary>
        /// Получает список автомобилей для отображения в UI.
        /// </summary>
        /// <returns>Список объектов для отображения.</returns>
        List<CarListItemDto> GetCarsForDisplay();

        /// <summary>
        /// Получает информацию об автомобиле для расчета стоимости.
        /// </summary>
        /// <param name="carId">ID автомобиля.</param>
        /// <returns>Объект с данными для расчета или null.</returns>
        CarForCalculationDto? GetCarForCalculation(int carId);
    }
}
