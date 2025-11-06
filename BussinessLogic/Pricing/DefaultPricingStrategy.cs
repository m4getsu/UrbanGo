using System;

namespace BussinessLogic.Pricing
{
    public class DefaultPricingStrategy : IPricingStrategy
    {
        public decimal CalculateBasePrice(decimal pricePerHour, int hours)
        {
            if (pricePerHour <= 0) throw new ArgumentException("Price per hour must be positive.");
            if (hours <= 0) throw new ArgumentException("Hours must be positive.");
            return pricePerHour * hours;
        }
    }
}
