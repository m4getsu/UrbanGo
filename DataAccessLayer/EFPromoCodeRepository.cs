using Microsoft.EntityFrameworkCore;
using Model;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessLayer
{
    /// <summary>
    /// Реализация репозитория промокодов с использованием Entity Framework Core.
    /// Реализует только операции чтения, так как промокоды создаются администратором.
    /// </summary>
    public class EFPromoCodeRepository : IPromoCodeRepository
    {
        private readonly CarSharingContext _context;

        /// <summary>
        /// Инициализирует новый экземпляр репозитория с контекстом базы данных.
        /// </summary>
        /// <param name="context">Контекст Entity Framework для работы с базой данных.</param>
        public EFPromoCodeRepository(CarSharingContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Возвращает все промокоды из базы данных.
        /// </summary>
        /// <returns>Коллекция всех промокодов.</returns>
        public IEnumerable<PromoCode> ReadAll()
        {
            return _context.Set<PromoCode>().ToList();
        }

        /// <summary>
        /// Находит промокод в базе данных по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор промокода.</param>
        /// <returns>Найденный промокод или null, если не найден.</returns>
        public PromoCode ReadById(int id)
        {
            return _context.Set<PromoCode>().Find(id);
        }

        /// <summary>
        /// Находит промокод по его коду.
        /// </summary>
        /// <param name="code">Код промокода.</param>
        /// <returns>Найденный промокод или null, если не найден.</returns>
        public PromoCode GetByCode(string code)
        {
            return _context.Set<PromoCode>()
                .FirstOrDefault(p => p.Code == code);
        }
    }
}


