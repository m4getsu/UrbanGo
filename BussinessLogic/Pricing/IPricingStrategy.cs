namespace BussinessLogic.Pricing
{
    public interface IPricingStrategy
    {
        decimal CalculateBasePrice(decimal pricePerHour, int hours);
    }
}
