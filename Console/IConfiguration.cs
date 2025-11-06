namespace ConsoleApp
{
    /// <summary>
    /// Определяет интерфейс конфигурации приложения.
    /// </summary>
    public interface IConfiguration
    {
        /// <summary>
        /// Получает строку подключения к базе данных.
        /// </summary>
        string ConnectionString { get; }
    }
}
