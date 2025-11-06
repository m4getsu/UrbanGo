namespace BussinessLogic
{
	/// <summary>
	/// Адаптер для использования PromoService через интерфейс IPromoService без изменения исходного класса.
	/// </summary>
	public sealed class PromoServiceAdapter : IPromoService
	{
		/// <summary>
		/// Получает внутренний экземпляр PromoService.
		/// </summary>
		public PromoService Inner { get; }

		/// <summary>
		/// Инициализирует новый экземпляр адаптера с внутренним сервисом.
		/// </summary>
		/// <param name="inner">Внутренний экземпляр PromoService.</param>
		public PromoServiceAdapter(PromoService inner)
		{
			Inner = inner;
		}

		/// <summary>
		/// Применяет промокод к исходной цене и возвращает итоговую стоимость.
		/// </summary>
		/// <param name="promoCode">Код промокода.</param>
		/// <param name="originalPrice">Исходная цена.</param>
		/// <returns>Итоговая стоимость после применения скидки.</returns>
		public decimal ApplyPromoCode(string promoCode, decimal originalPrice)
		{
			return Inner.ApplyPromoCode(promoCode, originalPrice);
		}
	}
}

