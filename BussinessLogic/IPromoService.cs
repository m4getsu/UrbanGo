using System;

namespace BussinessLogic
{
	/// <summary>
	/// Интерфейс сервиса промокодов для использования на уровне UI.
	/// </summary>
	public interface IPromoService
	{
		decimal ApplyPromoCode(string promoCode, decimal originalPrice);
	}
}

