using System.Collections.Generic;
using BussinessLogic;
using BussinessLogic.Dto;

namespace AIS.Controllers
{
	public class MainFormController
	{
		private readonly ICarService _carService;

		public MainFormController(ICarService carService)
		{
			_carService = carService;
		}

		public List<CarListItemDto> GetCarsForDisplay()
		{
			return _carService.GetCarsForDisplay();
		}

		public string GetCarDescription(int id) => _carService.GetCarDescription(id);
		public object[] GetCarValuesForEdit(int id) => _carService.GetCarValuesForEdit(id);
		public void CreateCar(string brand, string model, string plate, int year, int mileage, decimal price) => _carService.CreateCar(brand, model, plate, year, mileage, price);
		public bool UpdateCarDetails(int id, string brand, string model, string plate, int year, int mileage, decimal price, int status) => _carService.UpdateCarDetails(id, brand, model, plate, year, mileage, price, status);
		public bool DeleteCar(int id) => _carService.DeleteCar(id);
		public bool RentCar(int id) => _carService.RentCar(id);
		public List<string> GetAvailableCarsDescriptions() => _carService.GetAvailableCarsDescriptions();
		public decimal CalculateRentalCost(int carId, int hours, string promoCode = null) => _carService.CalculateRentalCost(carId, hours, promoCode);

		public CalculateCostFormController CreateCalculateCostFormController()
		{
			return new CalculateCostFormController(_carService);
		}
	}
}
