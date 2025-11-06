using System;

namespace BussinessLogic.Pricing
{
    /// <summary>
    /// Реализация стандартной стратегии расчета стоимости аренды.
    /// Рассчитывает базовую стоимость как произведение цены за час на количество часов.
    /// </summary>
    public class DefaultPricingStrategy : IPricingStrategy
    {
        /// <summary>
        /// Рассчитывает базовую стоимость аренды без применения скидок.
        /// </summary>
        /// <param name="pricePerHour">Стоимость аренды за час.</param>
        /// <param name="hours">Количество часов аренды.</param>
        /// <returns>Базовая стоимость аренды.</returns>
        public decimal CalculateBasePrice(decimal pricePerHour, int hours)
        {
            if (pricePerHour <= 0) throw new ArgumentException("Price per hour must be positive.");
            if (hours <= 0) throw new ArgumentException("Hours must be positive.");
            return pricePerHour * hours;
        }
    }
}
