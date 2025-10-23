using Dapper;
using Microsoft.Data.SqlClient;
using Model;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessLayer
{
    /// <summary>
    /// Реализация репозитория промокодов с использованием Dapper ORM.
    /// </summary>
    public class DapperPromoCodeRepository : IPromoCodeRepository
    {
        private readonly string _connectionString;

        /// <summary>
        /// Инициализирует новый экземпляр репозитория со строкой подключения.
        /// </summary>
        /// <param name="connectionString">Строка подключения к базе данных.</param>
        public DapperPromoCodeRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Добавляет новый промокод в базу данных.
        /// </summary>
        /// <param name="entity">Промокод для добавления.</param>
        public void Add(PromoCode entity)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Execute(@"
                INSERT INTO PromoCodes (Code, DiscountPercent, IsActive) 
                VALUES (@Code, @DiscountPercent, @IsActive)", entity);
        }

        /// <summary>
        /// Удаляет промокод из базы данных по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор промокода для удаления.</param>
        public void Delete(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Execute("DELETE FROM PromoCodes WHERE Id = @Id", new { Id = id });
        }

        /// <summary>
        /// Возвращает все промокоды из базы данных.
        /// </summary>
        /// <returns>Коллекция всех промокодов.</returns>
        public IEnumerable<PromoCode> ReadAll()
        {
            using var connection = new SqlConnection(_connectionString);
            return connection.Query<PromoCode>("SELECT * FROM PromoCodes");
        }

        /// <summary>
        /// Находит промокод в базе данных по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор промокода.</param>
        /// <returns>Найденный промокод или null, если не найден.</returns>
        public PromoCode ReadById(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            return connection.QueryFirstOrDefault<PromoCode>("SELECT * FROM PromoCodes WHERE Id = @Id", new { Id = id });
        }

        /// <summary>
        /// Обновляет существующий промокод в базе данных.
        /// </summary>
        /// <param name="entity">Промокод с обновленными данными.</param>
        public void Update(PromoCode entity)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Execute(@"
                UPDATE PromoCodes 
                SET Code = @Code, DiscountPercent = @DiscountPercent, IsActive = @IsActive 
                WHERE Id = @Id", entity);
        }

        /// <summary>
        /// Находит промокод по его коду.
        /// </summary>
        /// <param name="code">Код промокода.</param>
        /// <returns>Найденный промокод или null, если не найден.</returns>
        public PromoCode GetByCode(string code)
        {
            using var connection = new SqlConnection(_connectionString);
            return connection.QueryFirstOrDefault<PromoCode>("SELECT * FROM PromoCodes WHERE Code = @Code", new { Code = code });
        }
    }
}

