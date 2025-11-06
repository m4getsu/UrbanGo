using BussinessLogic;
using Ninject;

namespace ConsoleApp
{
    /// <summary>
    /// Контейнер для управления зависимостями с использованием Ninject.
    /// Предоставляет доступ к сервисам приложения.
    /// </summary>
    public class DependencyContainer
    {
        private IKernel _kernel;

        /// <summary>
        /// Инициализирует новый экземпляр контейнера зависимостей с выбранным провайдером данных.
        /// </summary>
        /// <param name="config">Конфигурация приложения со строкой подключения.</param>
        /// <param name="useEF">True для использования Entity Framework, False для Dapper.</param>
        public DependencyContainer(IConfiguration config, bool useEF)
        {
            _kernel = new StandardKernel(new BussinessLogic.SimpleConfigModule(useEF, config.ConnectionString));
        }

        /// <summary>
        /// Получает сервис для работы с автомобилями.
        /// </summary>
        public ICarService CarService => _kernel.Get<ICarService>();

        /// <summary>
        /// Получает сервис для работы с промокодами.
        /// </summary>
        public IPromoService PromoService => _kernel.Get<IPromoService>();
    }
}
