using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Model;
using BussinessLogic.Logging;
using BussinessLogic.Pricing;
using BussinessLogic.Validation;

namespace BussinessLogic
{
	/// <summary>
	/// Композиционный фабричный класс для создания сервисов, изолируя UI от DataAccessLayer.
	/// </summary>
	public static class ServiceFactory
	{
		/// <summary>
		/// Создает сервисы для работы с автомобилями и промокодами, используя выбранный ORM.
		/// </summary>
		/// <param name="useEF">True для использования Entity Framework, False для Dapper.</param>
		/// <returns>Кортеж из сервиса автомобилей и сервиса промокодов.</returns>
		public static (ICarService carService, IPromoService promoService) CreateServices(bool useEF)
		{
			string connectionString = "Server=(localdb)\\mssqllocaldb;Database=UrbanGoDB;Trusted_Connection=true;TrustServerCertificate=true;";

			if (useEF)
			{
				var options = new DbContextOptionsBuilder<CarSharingContext>()
					.UseSqlServer(connectionString)
					.Options;
				var context = new CarSharingContext(options);

				IRepository<Car> carRepository = new EntityRepository<Car>(context);
				IPromoCodeRepository promoRepository = new EFPromoCodeRepository(context);

				var concretePromo = new PromoService(promoRepository);
				IPromoService promoService = new PromoServiceAdapter(concretePromo);
				var logger = new FileLogger();
				var pricing = new DefaultPricingStrategy();
				var discount = new PromoServiceDiscountPolicy(promoService);
				var validator = new CarValidator();
				var carService = new CarService(carRepository, pricing, discount, logger, validator);
				return (carService, promoService);
			}
			else
			{
				IRepository<Car> carRepository = new DapperRepository<Car>(connectionString);
				IPromoCodeRepository promoRepository = new DapperPromoCodeRepository(connectionString);

				var concretePromo = new PromoService(promoRepository);
				IPromoService promoService = new PromoServiceAdapter(concretePromo);
				var logger = new FileLogger();
				var pricing = new DefaultPricingStrategy();
				var discount = new PromoServiceDiscountPolicy(promoService);
				var validator = new CarValidator();
				var carService = new CarService(carRepository, pricing, discount, logger, validator);
				return (carService, promoService);
			}
		}
	}
}

