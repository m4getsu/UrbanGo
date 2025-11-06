namespace BussinessLogic.Pricing
{
    /// <summary>
    /// Определяет политику применения скидок к базовой стоимости.
    /// </summary>
    public interface IDiscountPolicy
    {
        /// <summary>
        /// Применяет скидку к базовой стоимости с использованием промокода.
        /// </summary>
        /// <param name="promoCode">Код промокода. Может быть null.</param>
        /// <param name="basePrice">Базовая стоимость до применения скидки.</param>
        /// <returns>Итоговая стоимость после применения скидки.</returns>
        decimal ApplyDiscount(string? promoCode, decimal basePrice);
    }
}
