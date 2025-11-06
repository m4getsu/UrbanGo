using BussinessLogic.Pricing;

namespace BussinessLogic
{
    /// <summary>
    /// Реализация политики скидок с использованием сервиса промокодов.
    /// </summary>
    public class PromoServiceDiscountPolicy : IDiscountPolicy
    {
        private readonly IPromoService _promoService;

        /// <summary>
        /// Инициализирует новый экземпляр политики скидок с сервисом промокодов.
        /// </summary>
        /// <param name="promoService">Сервис для работы с промокодами.</param>
        public PromoServiceDiscountPolicy(IPromoService promoService)
        {
            _promoService = promoService;
        }

        /// <summary>
        /// Применяет скидку к базовой стоимости с использованием промокода.
        /// </summary>
        /// <param name="promoCode">Код промокода. Если null или пустой, скидка не применяется.</param>
        /// <param name="basePrice">Базовая стоимость до применения скидки.</param>
        /// <returns>Итоговая стоимость после применения скидки.</returns>
        public decimal ApplyDiscount(string? promoCode, decimal basePrice)
        {
            if (string.IsNullOrWhiteSpace(promoCode))
                return basePrice;
            return _promoService.ApplyPromoCode(promoCode, basePrice);
        }
    }
}
