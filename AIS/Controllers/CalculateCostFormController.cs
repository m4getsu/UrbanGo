using BussinessLogic;
using BussinessLogic.Dto;

namespace AIS.Controllers
{
	public class CalculateCostFormController
	{
		private readonly ICarService _carService;
		public CalculateCostFormController(ICarService carService)
		{
			_carService = carService;
		}
		public CarForCalculationDto GetCarForCalculation(int carId) => _carService.GetCarForCalculation(carId);
		public decimal CalculateRentalCost(int carId, int hours, string promoCode = null) => _carService.CalculateRentalCost(carId, hours, promoCode);
	}
}
