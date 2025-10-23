using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// Представляет промокод в системе каршеринга.
    /// </summary>
    public class PromoCode : IDomainObject
    {
        /// <summary>
        /// Уникальный идентификатор промокода.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Уникальный код промокода.
        /// </summary>
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// Процент скидки (от 0 до 100).
        /// </summary>
        public decimal DiscountPercent { get; set; }

        /// <summary>
        /// Активен ли промокод.
        /// </summary>
        public bool IsActive { get; set; }
    }
}

