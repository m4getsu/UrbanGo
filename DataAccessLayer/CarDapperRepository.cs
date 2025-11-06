using Dapper;
using Microsoft.Data.SqlClient;
using Model;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessLayer
{
    /// <summary>
    /// Реализация репозитория для автомобилей с использованием Dapper ORM.
    /// Обеспечивает доступ к данным через прямые SQL-запросы к таблице Cars.
    /// </summary>
    public class CarDapperRepository : IRepository<Car>, IReadRepository<Car>, IWriteRepository<Car>
    {
        private readonly string _connectionString;

        /// <summary>
        /// Инициализирует новый экземпляр репозитория со строкой подключения.
        /// </summary>
        /// <param name="connectionString">Строка подключения к базе данных.</param>
        public CarDapperRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Добавляет новый автомобиль в базу данных.
        /// </summary>
        /// <param name="entity">Автомобиль для добавления.</param>
        public void Add(Car entity)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Execute(@"
                INSERT INTO Cars (Brand, Model, LicensePlate, Year, Mileage, Status, RentalPricePerHour)
                VALUES (@Brand, @Model, @LicensePlate, @Year, @Mileage, @Status, @RentalPricePerHour)", entity);
        }

        /// <summary>
        /// Удаляет автомобиль из базы данных по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор автомобиля для удаления.</param>
        public void Delete(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Execute("DELETE FROM Cars WHERE Id = @Id", new { Id = id });
        }

        /// <summary>
        /// Возвращает все автомобили из базы данных.
        /// </summary>
        /// <returns>Коллекция всех автомобилей.</returns>
        public IEnumerable<Car> ReadAll()
        {
            using var connection = new SqlConnection(_connectionString);
            return connection.Query<Car>("SELECT * FROM Cars");
        }

        /// <summary>
        /// Находит автомобиль в базе данных по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор автомобиля.</param>
        /// <returns>Найденный автомобиль или null, если не найден.</returns>
        public Car ReadById(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            return connection.QueryFirstOrDefault<Car>("SELECT * FROM Cars WHERE Id = @Id", new { Id = id });
        }

        /// <summary>
        /// Обновляет существующий автомобиль в базе данных.
        /// </summary>
        /// <param name="entity">Автомобиль с обновленными данными.</param>
        public void Update(Car entity)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Execute(@"
                UPDATE Cars
                SET Brand = @Brand, Model = @Model, LicensePlate = @LicensePlate,
                    Year = @Year, Mileage = @Mileage, Status = @Status, RentalPricePerHour = @RentalPricePerHour
                WHERE Id = @Id", entity);
        }
    }
}
