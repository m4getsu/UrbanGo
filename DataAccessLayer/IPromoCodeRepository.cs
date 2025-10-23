using Model;

namespace DataAccessLayer
{
    /// <summary>
    /// Интерфейс репозитория для работы с промокодами.
    /// </summary>
    public interface IPromoCodeRepository : IRepository<PromoCode>
    {
        /// <summary>
        /// Находит промокод по его коду.
        /// </summary>
        /// <param name="code">Код промокода.</param>
        /// <returns>Найденный промокод или null, если не найден.</returns>
        PromoCode GetByCode(string code);
    }
}

