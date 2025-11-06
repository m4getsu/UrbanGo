using Model;

namespace DataAccessLayer
{
    /// <summary>
    /// Интерфейс репозитория для работы с промокодами.
    /// Наследует только от IReadRepository, так как промокоды создаются администратором
    /// и не изменяются через приложение.
    /// </summary>
    public interface IPromoCodeRepository : IReadRepository<PromoCode>
    {
        /// <summary>
        /// Находит промокод по его коду.
        /// </summary>
        /// <param name="code">Код промокода.</param>
        /// <returns>Найденный промокод или null, если не найден.</returns>
        PromoCode GetByCode(string code);
    }
}

