using BussinessLogic;
using BussinessLogic.Dto;

namespace AIS.Controllers
{
	/// <summary>
	/// Контроллер формы расчета стоимости аренды.
	/// Обеспечивает взаимодействие между UI и бизнес-логикой для расчета стоимости.
	/// </summary>
	public class CalculateCostFormController
	{
		private readonly ICarService _carService;

		/// <summary>
		/// Инициализирует новый экземпляр контроллера с сервисом автомобилей.
		/// </summary>
		/// <param name="carService">Сервис для работы с автомобилями.</param>
		public CalculateCostFormController(ICarService carService)
		{
			_carService = carService;
		}

		/// <summary>
		/// Получает информацию об автомобиле для расчета стоимости.
		/// </summary>
		/// <param name="carId">ID автомобиля.</param>
		/// <returns>Объект с данными для расчета.</returns>
		public CarForCalculationDto GetCarForCalculation(int carId) => _carService.GetCarForCalculation(carId);

		/// <summary>
		/// Рассчитывает стоимость аренды автомобиля.
		/// </summary>
		/// <param name="carId">ID автомобиля.</param>
		/// <param name="hours">Количество часов аренды.</param>
		/// <param name="promoCode">Необязательный промокод.</param>
		/// <returns>Итоговая стоимость аренды.</returns>
		public decimal CalculateRentalCost(int carId, int hours, string promoCode = null) => _carService.CalculateRentalCost(carId, hours, promoCode);
	}
}
