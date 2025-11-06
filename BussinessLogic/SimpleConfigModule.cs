using System;
using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Model;
using Ninject;
using Ninject.Modules;
using BussinessLogic.Logging;
using BussinessLogic.Pricing;
using BussinessLogic.Validation;

namespace BussinessLogic
{
	/// <summary>
	/// Конфигурация Ninject: биндит репозитории и сервисы, поддерживает EF/Dapper и синглтоны.
	/// </summary>
	public class SimpleConfigModule : NinjectModule
	{
		private readonly bool _useEF;
		private readonly string _connectionString;

		public SimpleConfigModule(bool useEF, string connectionString)
		{
			_useEF = useEF;
			_connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
		}

		public override void Load()
		{
			// Логгер
			Bind<ILogger>().ToMethod(ctx => new FileLogger()).InSingletonScope();

			// Стратегии ценообразования и скидок (расширяемость OCP)
			Bind<IPricingStrategy>().To<DefaultPricingStrategy>().InSingletonScope();
			Bind<IDiscountPolicy>().ToMethod(ctx => new PromoServiceDiscountPolicy(ctx.Kernel.Get<IPromoService>())).InSingletonScope();

			// Валидаторы
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
				Bind<IRepository<Car>>().ToConstant(new DapperRepository<Car>(_connectionString)).InSingletonScope();
				Bind<IPromoCodeRepository>().ToConstant(new DapperPromoCodeRepository(_connectionString)).InSingletonScope();
			}

			// Сервисы
			Bind<PromoService>().ToSelf().InSingletonScope();
			Bind<IPromoService>().ToMethod(ctx => new PromoServiceAdapter(ctx.Kernel.Get<PromoService>())).InSingletonScope();
			Bind<ICarService>().To<CarService>().InSingletonScope();
		}
	}
}

