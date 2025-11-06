using BussinessLogic;
using Ninject;

namespace AIS
{
	public class DependencyContainer
	{
		private readonly IKernel _kernel;
		public DependencyContainer(bool useEF, AppConfiguration configuration)
		{
			_kernel = new StandardKernel(new BussinessLogic.SimpleConfigModule(useEF, configuration.ConnectionString));
		}

		public ICarService CarService => _kernel.Get<ICarService>();
		public IPromoService PromoService => _kernel.Get<IPromoService>();
	}
}
