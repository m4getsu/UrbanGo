using Microsoft.EntityFrameworkCore;
using Model;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessLayer
{
    /// <summary>
    /// Реализация репозитория с использованием Entity Framework Core.
    /// Обеспечивает доступ к данным через DbContext.
    /// </summary>
    /// <typeparam name="T">Тип доменного объекта, реализующего IDomainObject.</typeparam>
    public class EntityRepository<T> : IRepository<T> where T : class, IDomainObject
    {
        private readonly CarSharingContext _context;

        /// <summary>
        /// Инициализирует новый экземпляр репозитория с контекстом базы данных.
        /// </summary>
        /// <param name="context">Контекст Entity Framework для работы с базой данных.</param>
        public EntityRepository(CarSharingContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Добавляет новую сущность в базу данных.
        /// </summary>
        /// <param name="entity">Сущность для добавления.</param>
        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
            _context.SaveChanges();
        }

        /// <summary>
        /// Удаляет сущность из базы данных по её идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор сущности для удаления.</param>
        public void Delete(int id)
        {
            var entity = _context.Set<T>().Find(id);
            if (entity != null)
            {
                _context.Set<T>().Remove(entity);
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Возвращает все сущности из базы данных.
        /// </summary>
        /// <returns>Коллекция всех сущностей.</returns>
        public IEnumerable<T> ReadAll()
        {
            return _context.Set<T>().ToList();
        }

        /// <summary>
        /// Находит сущность в базе данных по её идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор сущности.</param>
        /// <returns>Найденная сущность или null, если не найдена.</returns>
        public T ReadById(int id)
        {
            return _context.Set<T>().Find(id);
        }

        /// <summary>
        /// Обновляет существующую сущность в базе данных.
        /// </summary>
        /// <param name="entity">Сущность с обновленными данными.</param>
        public void Update(T entity)
        {
            // Проверяем, отслеживается ли уже сущность с таким ID
            var trackedEntity = _context.ChangeTracker.Entries<T>()
                .FirstOrDefault(e => e.Entity.Id == entity.Id);

            if (trackedEntity != null)
            {
                // Если сущность уже отслеживается, обновляем её значения
                trackedEntity.CurrentValues.SetValues(entity);
            }
            else
            {
                // Если сущность не отслеживается, прикрепляем её как измененную
                _context.Entry(entity).State = EntityState.Modified;
            }

            _context.SaveChanges();
        }
    }
}
