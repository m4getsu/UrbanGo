using System.Collections.Generic;
using Model;

namespace DataAccessLayer
{
    /// <summary>
    /// Базовый интерфейс репозитория для работы с доменными объектами.
    /// Определяет стандартные операции CRUD для сущностей.
    /// </summary>
    /// <typeparam name="T">Тип доменного объекта, реализующего IDomainObject.</typeparam>
    // Интерфейс только для чтения
    public interface IReadRepository<T> where T : IDomainObject
    {
        /// <summary>
        /// Возвращает все сущности из репозитория.
        /// </summary>
        /// <returns>Коллекция всех сущностей.</returns>
        IEnumerable<T> ReadAll();

        /// <summary>
        /// Находит сущность по её идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор сущности.</param>
        /// <returns>Найденная сущность или null, если не найдена.</returns>
        T ReadById(int id);
    }

    /// <summary>
    /// Интерфейс репозитория для операций записи с доменными объектами.
    /// Определяет методы для создания, обновления и удаления сущностей.
    /// </summary>
    /// <typeparam name="T">Тип доменного объекта, реализующего IDomainObject.</typeparam>
    public interface IWriteRepository<T> where T : IDomainObject
    {
        /// <summary>
        /// Добавляет новую сущность в репозиторий.
        /// </summary>
        /// <param name="entity">Сущность для добавления.</param>
        void Add(T entity);

        /// <summary>
        /// Обновляет существующую сущность в репозитории.
        /// </summary>
        /// <param name="entity">Сущность с обновленными данными.</param>
        void Update(T entity);

        /// <summary>
        /// Удаляет сущность по её идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор сущности для удаления.</param>
        void Delete(int id);
    }

    /// <summary>
    /// Полный интерфейс репозитория, объединяющий операции чтения и записи.
    /// Представляет собой фасад для полного набора CRUD операций.
    /// </summary>
    /// <typeparam name="T">Тип доменного объекта, реализующего IDomainObject.</typeparam>
    public interface IRepository<T> : IReadRepository<T>, IWriteRepository<T> where T : IDomainObject
    {
    }
}


