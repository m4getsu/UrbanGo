namespace BussinessLogic.Logging
{
	/// <summary>
	/// Определяет методы для записи сообщений в журнал.
	/// </summary>
	public interface ILogger
	{
		/// <summary>
		/// Записывает сообщение в журнал.
		/// </summary>
		/// <param name="message">Текст сообщения для записи.</param>
		void Log(string message);
	}
}

