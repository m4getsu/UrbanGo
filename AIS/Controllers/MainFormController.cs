using System.Collections.Generic;
using BussinessLogic;
using BussinessLogic.Dto;

namespace AIS.Controllers
{
	/// <summary>
	/// Контроллер главной формы приложения.
	/// Обеспечивает взаимодействие между UI и бизнес-логикой для операций с автомобилями.
	/// </summary>
	public class MainFormController
	{
		private readonly ICarService _carService;

		/// <summary>
		/// Инициализирует новый экземпляр контроллера с сервисом автомобилей.
		/// </summary>
		/// <param name="carService">Сервис для работы с автомобилями.</param>
		public MainFormController(ICarService carService)
		{
			_carService = carService;
		}

		/// <summary>
		/// Получает список автомобилей для отображения в UI.
		/// </summary>
		/// <returns>Список объектов с данными для отображения.</returns>
		public List<CarListItemDto> GetCarsForDisplay()
		{
			return _carService.GetCarsForDisplay();
		}

		/// <summary>
		/// Получает строковое описание автомобиля.
		/// </summary>
		/// <param name="id">ID автомобиля.</param>
		/// <returns>Строковое описание автомобиля.</returns>
		public string GetCarDescription(int id) => _carService.GetCarDescription(id);

		/// <summary>
		/// Получает текущие значения автомобиля для редактирования.
		/// </summary>
		/// <param name="id">ID автомобиля.</param>
		/// <returns>Массив значений автомобиля.</returns>
		public object[] GetCarValuesForEdit(int id) => _carService.GetCarValuesForEdit(id);

		/// <summary>
		/// Создает новый автомобиль.
		/// </summary>
		/// <param name="brand">Марка автомобиля.</param>
		/// <param name="model">Модель автомобиля.</param>
		/// <param name="plate">Государственный номер.</param>
		/// <param name="year">Год выпуска.</param>
		/// <param name="mileage">Пробег.</param>
		/// <param name="price">Стоимость аренды за час.</param>
		public void CreateCar(string brand, string model, string plate, int year, int mileage, decimal price) => _carService.CreateCar(brand, model, plate, year, mileage, price);

		/// <summary>
		/// Обновляет данные автомобиля.
		/// </summary>
		/// <param name="id">ID автомобиля.</param>
		/// <param name="brand">Марка автомобиля.</param>
		/// <param name="model">Модель автомобиля.</param>
		/// <param name="plate">Государственный номер.</param>
		/// <param name="year">Год выпуска.</param>
		/// <param name="mileage">Пробег.</param>
		/// <param name="price">Стоимость аренды за час.</param>
		/// <param name="status">Статус автомобиля.</param>
		/// <returns>True, если обновление прошло успешно.</returns>
		public bool UpdateCarDetails(int id, string brand, string model, string plate, int year, int mileage, decimal price, int status) => _carService.UpdateCarDetails(id, brand, model, plate, year, mileage, price, status);

		/// <summary>
		/// Удаляет автомобиль.
		/// </summary>
		/// <param name="id">ID автомобиля.</param>
		/// <returns>True, если удаление прошло успешно.</returns>
		public bool DeleteCar(int id) => _carService.DeleteCar(id);

		/// <summary>
		/// Выполняет аренду автомобиля.
		/// </summary>
		/// <param name="id">ID автомобиля.</param>
		/// <returns>True, если аренда прошла успешно.</returns>
		public bool RentCar(int id) => _carService.RentCar(id);

		/// <summary>
		/// Получает список строковых описаний доступных автомобилей.
		/// </summary>
		/// <returns>Список строковых описаний.</returns>
		public List<string> GetAvailableCarsDescriptions() => _carService.GetAvailableCarsDescriptions();

		/// <summary>
		/// Рассчитывает стоимость аренды автомобиля.
		/// </summary>
		/// <param name="carId">ID автомобиля.</param>
		/// <param name="hours">Количество часов аренды.</param>
		/// <param name="promoCode">Необязательный промокод.</param>
		/// <returns>Итоговая стоимость аренды.</returns>
		public decimal CalculateRentalCost(int carId, int hours, string promoCode = null) => _carService.CalculateRentalCost(carId, hours, promoCode);

		/// <summary>
		/// Создает контроллер для формы расчета стоимости.
		/// </summary>
		/// <returns>Новый экземпляр контроллера формы расчета.</returns>
		public CalculateCostFormController CreateCalculateCostFormController()
		{
			return new CalculateCostFormController(_carService);
		}
	}
}
