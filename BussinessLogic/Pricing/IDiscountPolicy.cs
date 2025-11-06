namespace BussinessLogic.Pricing
{
    public interface IDiscountPolicy
    {
        decimal ApplyDiscount(string? promoCode, decimal basePrice);
    }
}
