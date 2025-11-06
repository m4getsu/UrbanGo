using BussinessLogic;
using Ninject;

namespace AIS
{
	/// <summary>
	/// Контейнер для управления зависимостями с использованием Ninject.
	/// Предоставляет доступ к сервисам приложения.
	/// </summary>
	public class DependencyContainer
	{
		private readonly IKernel _kernel;

		/// <summary>
		/// Инициализирует новый экземпляр контейнера зависимостей с выбранным провайдером данных.
		/// </summary>
		/// <param name="useEF">True для использования Entity Framework, False для Dapper.</param>
		/// <param name="configuration">Конфигурация приложения со строкой подключения.</param>
		public DependencyContainer(bool useEF, AppConfiguration configuration)
		{
			_kernel = new StandardKernel(new BussinessLogic.SimpleConfigModule(useEF, configuration.ConnectionString));
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
