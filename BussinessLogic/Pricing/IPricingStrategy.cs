namespace BussinessLogic.Pricing
{
    /// <summary>
    /// Определяет стратегию расчета базовой стоимости аренды.
    /// </summary>
    public interface IPricingStrategy
    {
        /// <summary>
        /// Рассчитывает базовую стоимость аренды без применения скидок.
        /// </summary>
        /// <param name="pricePerHour">Стоимость аренды за час.</param>
        /// <param name="hours">Количество часов аренды.</param>
        /// <returns>Базовая стоимость аренды.</returns>
        decimal CalculateBasePrice(decimal pricePerHour, int hours);
    }
}
