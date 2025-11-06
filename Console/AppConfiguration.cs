namespace ConsoleApp
{
    /// <summary>
    /// Конфигурация консольного приложения, содержащая настройки подключения к базе данных.
    /// </summary>
    public class AppConfiguration : IConfiguration
    {
        /// <summary>
        /// Получает строку подключения к базе данных.
        /// </summary>
        public string ConnectionString { get; }

        /// <summary>
        /// Инициализирует новый экземпляр конфигурации со строкой подключения.
        /// </summary>
        /// <param name="connectionString">Строка подключения. Если null, используется значение по умолчанию для LocalDB.</param>
        public AppConfiguration(string? connectionString = null)
        {
            ConnectionString = connectionString ?? "Server=(localdb)\\mssqllocaldb;Database=UrbanGoDB;Trusted_Connection=true;TrustServerCertificate=true;";
        }
    }
}
