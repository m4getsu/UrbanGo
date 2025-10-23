using DataAccessLayer;
using Model;
using System;

namespace BussinessLogic
{
    /// <summary>
    /// Сервис для работы с промокодами в системе каршеринга.
    /// </summary>
    public class PromoService
    {
        private readonly IPromoCodeRepository _promoCodeRepository;

        /// <summary>
        /// Инициализирует новый экземпляр сервиса с репозиторием промокодов.
        /// </summary>
        /// <param name="promoCodeRepository">Репозиторий для работы с промокодами.</param>
        public PromoService(IPromoCodeRepository promoCodeRepository)
        {
            _promoCodeRepository = promoCodeRepository;
        }

        /// <summary>
        /// Применяет промокод к исходной цене и возвращает итоговую стоимость.
        /// </summary>
        /// <param name="promoCode">Код промокода.</param>
        /// <param name="originalPrice">Исходная цена.</param>
        /// <returns>Итоговая стоимость после применения скидки.</returns>
        /// <exception cref="ArgumentException">Выбрасывается, если промокод не найден или неактивен.</exception>
        public decimal ApplyPromoCode(string promoCode, decimal originalPrice)
        {
            if (string.IsNullOrWhiteSpace(promoCode))
                throw new ArgumentException("Код промокода не может быть пустым.", nameof(promoCode));

            if (originalPrice <= 0)
                throw new ArgumentException("Исходная цена должна быть положительной.", nameof(originalPrice));

            var promo = _promoCodeRepository.GetByCode(promoCode.Trim());
            
            if (promo == null)
                throw new ArgumentException("Промокод не найден.");

            if (!promo.IsActive)
                throw new ArgumentException("Промокод неактивен.");

            if (promo.DiscountPercent < 0 || promo.DiscountPercent > 100)
                throw new ArgumentException("Процент скидки должен быть от 0 до 100.");

            decimal discountAmount = originalPrice * (promo.DiscountPercent / 100);
            return originalPrice - discountAmount;
        }
    }
}

