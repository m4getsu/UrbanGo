using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// Представляет возможные статусы автомобиля в системе каршеринга.
    /// </summary>
    public enum CarStatus
    {
        /// <summary>
        /// Автомобиль доступен для аренды.
        /// </summary>
        Available,

        /// <summary>
        /// Автомобиль арендован.
        /// </summary>
        Rented,

        /// <summary>
        /// Автомобиль на техническом обслуживании.
        /// </summary>
        UnderMaintenance
    }
}
