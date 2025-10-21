using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// Представляет автомобиль в системе каршеринга.
    /// </summary>
    public class Car
    {
        /// <summary>
        /// Уникальный идентификатор автомобиля.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Марка автомобиля (например, Toyota, Kia).
        /// </summary>
        public string Brand { get; set; } = string.Empty;

        /// <summary>
        /// Модель автомобиля (например, Camry, Rio).
        /// </summary>
        public string Model { get; set; } = string.Empty;

        /// <summary>
        /// Государственный номерной знак автомобиля.
        /// </summary>
        public string LicensePlate { get; set; } = string.Empty;

        /// <summary>
        /// Год выпуска автомобиля.
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// Текущий пробег автомобиля в километрах.
        /// </summary>
        public int Mileage { get; set; }

        /// <summary>
        /// Текущий статус автомобиля (доступен, арендован, на обслуживании).
        /// </summary>
        public CarStatus Status { get; set; }

        /// <summary>
        /// Стоимость аренды автомобиля за один час.
        /// </summary>
        public decimal RentalPricePerHour { get; set; }
    }
}
