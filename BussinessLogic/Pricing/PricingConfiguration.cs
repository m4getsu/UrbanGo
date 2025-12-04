using System;
using System.Collections.Generic;

namespace BussinessLogic.Pricing
{
    /// <summary>
    /// Конфигурация коэффициентов для динамического ценообразования.
    /// Содержит настройки для времени суток, дней недели, сезонов и праздников.
    /// </summary>
    public class PricingConfiguration
    {
        /// <summary>
        /// Коэффициенты времени суток (час -> множитель).
        /// Ключ: час (0-23), Значение: множитель (например, 1.5 означает +50%).
        /// </summary>
        public Dictionary<int, decimal> TimeOfDayMultipliers { get; set; }

        /// <summary>
        /// Коэффициенты дней недели.
        /// Ключ: день недели, Значение: множитель.
        /// </summary>
        public Dictionary<DayOfWeek, decimal> DayOfWeekMultipliers { get; set; }

        /// <summary>
        /// Коэффициенты продолжительности аренды (часы -> множитель).
        /// Ключ: минимальное количество часов для применения множителя, Значение: множитель.
        /// </summary>
        public Dictionary<int, decimal> DurationMultipliers { get; set; }

        /// <summary>
        /// Сезонные коэффициенты (месяц -> множитель).
        /// Ключ: номер месяца (1-12), Значение: множитель.
        /// </summary>
        public Dictionary<int, decimal> SeasonalMultipliers { get; set; }

        /// <summary>
        /// Праздничные дни (дата -> множитель).
        /// Ключ: дата праздника (только день и месяц имеют значение), Значение: множитель.
        /// </summary>
        public Dictionary<DateTime, decimal> HolidayMultipliers { get; set; }

        /// <summary>
        /// Возвращает конфигурацию по умолчанию с оптимальными коэффициентами для каршеринга.
        /// </summary>
        public static PricingConfiguration GetDefault()
        {
            return new PricingConfiguration
            {
                // Время суток: пиковые часы дороже, ночь дешевле
                TimeOfDayMultipliers = new Dictionary<int, decimal>
                {
                    { 0, 0.8m },  // Ночь (00:00-07:00): -20%
                    { 1, 0.8m },
                    { 2, 0.8m },
                    { 3, 0.8m },
                    { 4, 0.8m },
                    { 5, 0.8m },
                    { 6, 0.8m },
                    { 7, 1.5m },  // Утренний пик (07:00-09:00): +50%
                    { 8, 1.5m },
                    { 9, 1.2m },  // День (09:00-17:00): +20%
                    { 10, 1.2m },
                    { 11, 1.2m },
                    { 12, 1.2m },
                    { 13, 1.2m },
                    { 14, 1.2m },
                    { 15, 1.2m },
                    { 16, 1.2m },
                    { 17, 1.5m }, // Вечерний пик (17:00-19:00): +50%
                    { 18, 1.5m },
                    { 19, 1.1m }, // Вечер (19:00-22:00): +10%
                    { 20, 1.1m },
                    { 21, 1.1m },
                    { 22, 0.8m }, // Поздний вечер (22:00-00:00): -20%
                    { 23, 0.8m },
                },

                // Дни недели: выходные дороже
                DayOfWeekMultipliers = new Dictionary<DayOfWeek, decimal>
                {
                    { DayOfWeek.Monday, 1.0m },
                    { DayOfWeek.Tuesday, 1.0m },
                    { DayOfWeek.Wednesday, 1.0m },
                    { DayOfWeek.Thursday, 1.0m },
                    { DayOfWeek.Friday, 1.2m },    // Пятница: +20%
                    { DayOfWeek.Saturday, 1.3m },  // Суббота: +30%
                    { DayOfWeek.Sunday, 1.3m },    // Воскресенье: +30%
                },

                // Продолжительность: длительная аренда дешевле
                DurationMultipliers = new Dictionary<int, decimal>
                {
                    { 1, 1.1m },   // 1-2 часа: +10%
                    { 3, 1.0m },   // 3-7 часов: базовая цена
                    { 8, 0.95m },  // 8-23 часа: -5%
                    { 24, 0.9m },  // 24+ часов: -10%
                    { 72, 0.85m }, // 72+ часов (3 дня): -15%
                },

                // Сезоны: лето дороже, зима дешевле
                SeasonalMultipliers = new Dictionary<int, decimal>
                {
                    { 1, 0.9m },   // Январь: -10% (низкий сезон)
                    { 2, 0.9m },   // Февраль: -10%
                    { 3, 1.0m },   // Март: базовая цена
                    { 4, 1.0m },   // Апрель: базовая цена
                    { 5, 1.1m },   // Май: +10% (майские праздники)
                    { 6, 1.2m },   // Июнь: +20% (начало лета)
                    { 7, 1.3m },   // Июль: +30% (высокий сезон)
                    { 8, 1.3m },   // Август: +30% (высокий сезон)
                    { 9, 1.1m },   // Сентябрь: +10%
                    { 10, 1.0m },  // Октябрь: базовая цена
                    { 11, 0.95m }, // Ноябрь: -5%
                    { 12, 1.1m },  // Декабрь: +10% (новогодние праздники)
                },

                // Праздничные дни: значительное повышение цен
                HolidayMultipliers = new Dictionary<DateTime, decimal>
                {
                    // Новогодние праздники
                    { new DateTime(DateTime.Now.Year, 1, 1), 2.0m },  // Новый год: +100%
                    { new DateTime(DateTime.Now.Year, 1, 2), 1.8m },  // 2 января: +80%
                    { new DateTime(DateTime.Now.Year, 1, 3), 1.5m },  // 3 января: +50%
                    { new DateTime(DateTime.Now.Year, 1, 4), 1.5m },  // 4 января: +50%
                    { new DateTime(DateTime.Now.Year, 1, 5), 1.5m },  // 5 января: +50%
                    { new DateTime(DateTime.Now.Year, 1, 6), 1.5m },  // 6 января: +50%
                    { new DateTime(DateTime.Now.Year, 1, 7), 1.5m },  // 7 января: +50%
                    { new DateTime(DateTime.Now.Year, 1, 8), 1.3m },  // 8 января: +30%

                    // Другие праздники
                    { new DateTime(DateTime.Now.Year, 2, 23), 1.5m }, // 23 февраля: +50%
                    { new DateTime(DateTime.Now.Year, 3, 8), 1.5m },  // 8 марта: +50%
                    { new DateTime(DateTime.Now.Year, 5, 1), 1.5m },  // 1 мая: +50%
                    { new DateTime(DateTime.Now.Year, 5, 9), 1.5m },  // 9 мая: +50%
                    { new DateTime(DateTime.Now.Year, 6, 12), 1.3m }, // День России: +30%
                    { new DateTime(DateTime.Now.Year, 11, 4), 1.3m }, // День народного единства: +30%
                    { new DateTime(DateTime.Now.Year, 12, 31), 2.0m }, // Новый год: +100%
                }
            };
        }

        /// <summary>
        /// Создает пустую конфигурацию для ручного заполнения.
        /// </summary>
        public static PricingConfiguration CreateEmpty()
        {
            return new PricingConfiguration
            {
                TimeOfDayMultipliers = new Dictionary<int, decimal>(),
                DayOfWeekMultipliers = new Dictionary<DayOfWeek, decimal>(),
                DurationMultipliers = new Dictionary<int, decimal>(),
                SeasonalMultipliers = new Dictionary<int, decimal>(),
                HolidayMultipliers = new Dictionary<DateTime, decimal>()
            };
        }
    }
}
