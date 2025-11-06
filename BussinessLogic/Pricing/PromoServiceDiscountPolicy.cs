using BussinessLogic.Pricing;

namespace BussinessLogic
{
    public class PromoServiceDiscountPolicy : IDiscountPolicy
    {
        private readonly IPromoService _promoService;
        public PromoServiceDiscountPolicy(IPromoService promoService)
        {
            _promoService = promoService;
        }

        public decimal ApplyDiscount(string? promoCode, decimal basePrice)
        {
            if (string.IsNullOrWhiteSpace(promoCode))
                return basePrice;
            return _promoService.ApplyPromoCode(promoCode, basePrice);
        }
    }
}
