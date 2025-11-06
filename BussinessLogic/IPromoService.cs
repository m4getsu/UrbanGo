using System;

namespace BussinessLogic
{
	/// <summary>
	/// Интерфейс сервиса промокодов для использования на уровне UI.
	/// </summary>
	public interface IPromoService
	{
		/// <summary>
		/// Применяет промокод к исходной цене и возвращает итоговую стоимость.
		/// </summary>
		/// <param name="promoCode">Код промокода.</param>
		/// <param name="originalPrice">Исходная цена.</param>
		/// <returns>Итоговая стоимость после применения скидки.</returns>
		decimal ApplyPromoCode(string promoCode, decimal originalPrice);
	}
}

