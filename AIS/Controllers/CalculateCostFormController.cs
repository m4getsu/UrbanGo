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
		private readonly BussinessLogic.Pricing.IPricingStrategy _pricingStrategy;

		/// <summary>
		/// Инициализирует новый экземпляр контроллера с сервисом автомобилей.
		/// </summary>
		/// <param name="carService">Сервис для работы с автомобилями.</param>
		public CalculateCostFormController(ICarService carService)
		{
			_carService = carService;

			// Получаем стратегию ценообразования из CarService через рефлексию
			// Это необходимо для отображения детализации расчета
			var carServiceType = carService.GetType();
			var strategyField = carServiceType.GetField("_pricingStrategy",
				System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

			if (strategyField != null)
			{
				_pricingStrategy = strategyField.GetValue(carService) as BussinessLogic.Pricing.IPricingStrategy;
			}
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

		/// <summary>
		/// Возвращает детализацию расчета цены, если используется динамическая стратегия.
		/// </summary>
		/// <param name="hours">Количество часов аренды.</param>
		/// <returns>Текстовое описание примененных коэффициентов или null.</returns>
		public string GetPricingBreakdown(int hours)
		{
			if (_pricingStrategy is BussinessLogic.Pricing.DynamicPricingStrategy dynamicStrategy)
			{
				return dynamicStrategy.GetDetailedMultiplierBreakdown(hours);
			}
			return null;
		}

		/// <summary>
		/// Возвращает краткое описание условий ценообразования.
		/// </summary>
		/// <returns>Описание условий или null.</returns>
		public string GetCurrentConditions()
		{
			if (_pricingStrategy is BussinessLogic.Pricing.DynamicPricingStrategy dynamicStrategy)
			{
				return dynamicStrategy.GetCurrentConditionsDescription();
			}
			return null;
		}

		/// <summary>
		/// Проверяет, используется ли динамическое ценообразование.
		/// </summary>
		public bool IsDynamicPricingEnabled => _pricingStrategy is BussinessLogic.Pricing.DynamicPricingStrategy;
	}
}
