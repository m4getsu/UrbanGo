using System;

namespace Shared
{
    /// <summary>
    /// Интерфейс представления для расчета стоимости аренды автомобиля.
    /// </summary>
    public interface ICalculateCostView
    {
        // События

        /// <summary>
        /// Событие загрузки представления.
        /// </summary>
        event EventHandler ViewLoaded;

        /// <summary>
        /// Событие изменения количества часов аренды.
        /// Аргумент: количество часов
        /// </summary>
        event EventHandler<int> HoursChanged;

        /// <summary>
        /// Событие применения промокода.
        /// Аргумент: код промокода
        /// </summary>
        event EventHandler<string> ApplyPromoCodeRequested;

        /// <summary>
        /// Событие закрытия формы.
        /// </summary>
        event EventHandler CloseRequested;

        /// <summary>
        /// Событие запроса отображения детализации расчета.
        /// </summary>
        event EventHandler ShowDetailsRequested;

        // Свойства

        /// <summary>
        /// ID автомобиля для расчета.
        /// </summary>
        int CarId { get; }

        /// <summary>
        /// Количество часов аренды.
        /// </summary>
        int Hours { get; }

        /// <summary>
        /// Введенный промокод.
        /// </summary>
        string PromoCode { get; }

        // Методы для отображения данных

        /// <summary>
        /// Отображает информацию об автомобиле.
        /// </summary>
        /// <param name="carInfo">Текстовое описание автомобиля.</param>
        /// <param name="pricePerHour">Цена за час.</param>
        void DisplayCarInfo(string carInfo, decimal pricePerHour);

        /// <summary>
        /// Отображает рассчитанную стоимость.
        /// </summary>
        /// <param name="totalCost">Общая стоимость аренды.</param>
        void DisplayTotalCost(decimal totalCost);

        /// <summary>
        /// Отображает информацию о примененной скидке.
        /// </summary>
        /// <param name="discountAmount">Сумма скидки.</param>
        /// <param name="discountPercent">Процент скидки.</param>
        void DisplayDiscountInfo(decimal discountAmount, decimal discountPercent);

        /// <summary>
        /// Очищает информацию о скидке.
        /// </summary>
        void ClearDiscountInfo();

        /// <summary>
        /// Отображает сообщение об ошибке.
        /// </summary>
        /// <param name="message">Текст ошибки.</param>
        void ShowError(string message);

        /// <summary>
        /// Закрывает представление.
        /// </summary>
        void Close();
    }
}
