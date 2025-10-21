using Microsoft.EntityFrameworkCore;
using Model;

namespace DataAccessLayer
{
    /// <summary>
    /// Контекст базы данных для системы каршеринга.
    /// Предоставляет доступ к сущностям через Entity Framework Core.
    /// </summary>
    public class CarSharingContext : DbContext
    {
        /// <summary>
        /// Инициализирует новый экземпляр контекста с опциями подключения.
        /// </summary>
        /// <param name="options">Опции конфигурации для DbContext.</param>
        public CarSharingContext(DbContextOptions<CarSharingContext> options) : base(options)
        {
        }

        /// <summary>
        /// Коллекция автомобилей в базе данных.
        /// </summary>
        public DbSet<Car> Cars { get; set; }

        /// <summary>
        /// Строка подключения к базе данных SQL Server LocalDB.
        /// </summary>
        public static string ConnectionString => 
            "Server=(localdb)\\mssqllocaldb;Database=UrbanGoDB;Trusted_Connection=true;TrustServerCertificate=true;";
    }
}
