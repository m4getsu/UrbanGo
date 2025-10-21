using Dapper;
using Microsoft.Data.SqlClient;
using Model;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessLayer
{
    /// <summary>
    /// Реализация репозитория с использованием Dapper ORM.
    /// Обеспечивает доступ к данным через прямые SQL-запросы.
    /// </summary>
    /// <typeparam name="T">Тип доменного объекта, реализующего IDomainObject.</typeparam>
    public class DapperRepository<T> : IRepository<T> where T : class, IDomainObject
    {
        private readonly string _connectionString;

        /// <summary>
        /// Инициализирует новый экземпляр репозитория со строкой подключения.
        /// </summary>
        /// <param name="connectionString">Строка подключения к базе данных.</param>
        public DapperRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Добавляет новую сущность в базу данных.
        /// </summary>
        /// <param name="entity">Сущность для добавления.</param>
        public void Add(T entity)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Execute(@"
                INSERT INTO Cars (Brand, Model, LicensePlate, Year, Mileage, Status, RentalPricePerHour) 
                VALUES (@Brand, @Model, @LicensePlate, @Year, @Mileage, @Status, @RentalPricePerHour)", entity);
        }

        /// <summary>
        /// Удаляет сущность из базы данных по её идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор сущности для удаления.</param>
        public void Delete(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Execute("DELETE FROM Cars WHERE Id = @Id", new { Id = id });
        }

        /// <summary>
        /// Возвращает все сущности из базы данных.
        /// </summary>
        /// <returns>Коллекция всех сущностей.</returns>
        public IEnumerable<T> ReadAll()
        {
            using var connection = new SqlConnection(_connectionString);
            return connection.Query<T>("SELECT * FROM Cars");
        }

        /// <summary>
        /// Находит сущность в базе данных по её идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор сущности.</param>
        /// <returns>Найденная сущность или null, если не найдена.</returns>
        public T ReadById(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            return connection.QueryFirstOrDefault<T>("SELECT * FROM Cars WHERE Id = @Id", new { Id = id });
        }

        /// <summary>
        /// Обновляет существующую сущность в базе данных.
        /// </summary>
        /// <param name="entity">Сущность с обновленными данными.</param>
        public void Update(T entity)
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
