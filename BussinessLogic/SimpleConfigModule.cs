using System;
using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Model;
using Ninject;
using Ninject.Modules;
using BussinessLogic.Logging;
using BussinessLogic.Pricing;
using BussinessLogic.Validation;
using BussinessLogic.Services;
using BussinessLogic.Services.Import;

namespace BussinessLogic
{
	/// <summary>
	/// Конфигурация Ninject: биндит репозитории и сервисы, поддерживает EF/Dapper и синглтоны.
	/// Обновлен для поддержки конкретных репозиториев и разделенных интерфейсов сервисов.
	/// </summary>
	public class SimpleConfigModule : NinjectModule
	{
		private readonly bool _useEF;
		private readonly string _connectionString;
		private readonly bool _useDynamicPricing;

		public SimpleConfigModule(bool useEF, string connectionString, bool useDynamicPricing = false)
		{
			_useEF = useEF;
			_connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
			_useDynamicPricing = useDynamicPricing;
		}

		public override void Load()
		{
			Bind<ILogger>().ToMethod(ctx => new FileLogger()).InSingletonScope();

			// Конфигурация и выбор стратегии ценообразования
			Bind<PricingConfiguration>().ToMethod(ctx => PricingConfiguration.GetDefault()).InSingletonScope();

			if (_useDynamicPricing)
			{
				// Динамическая стратегия с конфигурацией
				Bind<IPricingStrategy>().ToMethod(ctx =>
					new DynamicPricingStrategy(ctx.Kernel.Get<PricingConfiguration>())
				).InSingletonScope();
			}
			else
			{
				// Стандартная стратегия (цена × часы)
				Bind<IPricingStrategy>().To<DefaultPricingStrategy>().InSingletonScope();
			}

			Bind<IDiscountPolicy>().ToMethod(ctx => new PromoServiceDiscountPolicy(ctx.Kernel.Get<IPromoService>())).InSingletonScope();
			Bind<ICarValidator>().To<CarValidator>().InSingletonScope();

			if (_useEF)
			{
				var options = new DbContextOptionsBuilder<CarSharingContext>()
					.UseSqlServer(_connectionString)
					.Options;
				var context = new CarSharingContext(options);

				Bind<CarSharingContext>().ToConstant(context).InSingletonScope();
				Bind<IRepository<Car>>().ToConstant(new EntityRepository<Car>(context)).InSingletonScope();
				Bind<IPromoCodeRepository>().ToConstant(new EFPromoCodeRepository(context)).InSingletonScope();
			}
			else
			{
				Bind<IRepository<Car>>().ToConstant(new CarDapperRepository(_connectionString)).InSingletonScope();
				Bind<IPromoCodeRepository>().ToConstant(new DapperPromoCodeRepository(_connectionString)).InSingletonScope();
			}
			Bind<PromoService>().ToSelf().InSingletonScope();
			Bind<IPromoService>().ToMethod(ctx => new PromoServiceAdapter(ctx.Kernel.Get<PromoService>())).InSingletonScope();
			Bind<CarService>().ToSelf().InSingletonScope();
			Bind<ICarService>().ToMethod(ctx => ctx.Kernel.Get<CarService>()).InSingletonScope();
			Bind<ICarManagementService>().ToMethod(ctx => ctx.Kernel.Get<CarService>()).InSingletonScope();
			Bind<ICarQueryService>().ToMethod(ctx => ctx.Kernel.Get<CarService>()).InSingletonScope();
			Bind<ICarDisplayService>().ToMethod(ctx => ctx.Kernel.Get<CarService>()).InSingletonScope();
			Bind<ICarImportService>().To<CarImportService>().InSingletonScope();
		}
	}
}

