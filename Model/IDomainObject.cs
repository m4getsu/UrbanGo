using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// Базовый интерфейс доменной сущности с идентификатором.
    /// Определяет минимальный контракт для всех доменных объектов.
    /// </summary>
    public interface IDomainObject
    {
        /// <summary>
        /// Уникальный идентификатор доменной сущности.
        /// </summary>
        int Id { get; set; }
    }
}


