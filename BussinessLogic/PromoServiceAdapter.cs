namespace BussinessLogic
{
	/// <summary>
	/// Адаптер для использования PromoService через интерфейс IPromoService без изменения исходного класса.
	/// </summary>
	public sealed class PromoServiceAdapter : IPromoService
	{
		public PromoService Inner { get; }

		public PromoServiceAdapter(PromoService inner)
		{
			Inner = inner;
		}

		public decimal ApplyPromoCode(string promoCode, decimal originalPrice)
		{
			return Inner.ApplyPromoCode(promoCode, originalPrice);
		}
	}
}

