using BussinessLogic;
using Ninject;

namespace ConsoleApp
{
    public class DependencyContainer
    {
        private IKernel _kernel;
        public DependencyContainer(IConfiguration config, bool useEF)
        {
            _kernel = new StandardKernel(new BussinessLogic.SimpleConfigModule(useEF, config.ConnectionString));
        }

        public ICarService CarService => _kernel.Get<ICarService>();
        public IPromoService PromoService => _kernel.Get<IPromoService>();
    }
}
