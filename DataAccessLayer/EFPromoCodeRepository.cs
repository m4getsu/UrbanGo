using Microsoft.EntityFrameworkCore;
using Model;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessLayer
{
    /// <summary>
    /// Реализация репозитория промокодов с использованием Entity Framework Core.
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
        /// Добавляет новый промокод в базу данных.
        /// </summary>
        /// <param name="entity">Промокод для добавления.</param>
        public void Add(PromoCode entity)
        {
            _context.Set<PromoCode>().Add(entity);
            _context.SaveChanges();
        }

        /// <summary>
        /// Удаляет промокод из базы данных по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор промокода для удаления.</param>
        public void Delete(int id)
        {
            var entity = _context.Set<PromoCode>().Find(id);
            if (entity != null)
            {
                _context.Set<PromoCode>().Remove(entity);
                _context.SaveChanges();
            }
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
        /// Обновляет существующий промокод в базе данных.
        /// </summary>
        /// <param name="entity">Промокод с обновленными данными.</param>
        public void Update(PromoCode entity)
        {
            var trackedEntity = _context.ChangeTracker.Entries<PromoCode>()
                .FirstOrDefault(e => e.Entity.Id == entity.Id);

            if (trackedEntity != null)
            {
                trackedEntity.CurrentValues.SetValues(entity);
            }
            else
            {
                _context.Entry(entity).State = EntityState.Modified;
            }

            _context.SaveChanges();
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

