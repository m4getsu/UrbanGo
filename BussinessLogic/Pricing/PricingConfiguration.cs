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
                TimeOfDayMultipliers = new Dictionary<int, decimal>
                {
                    { 0, 0.8m },  
                    { 1, 0.8m },
                    { 2, 0.8m },
                    { 3, 0.8m },
                    { 4, 0.8m },
                    { 5, 0.8m },
                    { 6, 0.8m },
                    { 7, 1.5m },  
                    { 8, 1.5m },
                    { 9, 1.2m },  
                    { 10, 1.2m },
                    { 11, 1.2m },
                    { 12, 1.2m },
                    { 13, 1.2m },
                    { 14, 1.2m },
                    { 15, 1.2m },
                    { 16, 1.2m },
                    { 17, 1.5m }, 
                    { 18, 1.5m },
                    { 19, 1.1m }, 
                    { 20, 1.1m },
                    { 21, 1.1m },
                    { 22, 0.8m }, 
                    { 23, 0.8m },
                },

                DayOfWeekMultipliers = new Dictionary<DayOfWeek, decimal>
                {
                    { DayOfWeek.Monday, 1.0m },
                    { DayOfWeek.Tuesday, 1.0m },
                    { DayOfWeek.Wednesday, 1.0m },
                    { DayOfWeek.Thursday, 1.0m },
                    { DayOfWeek.Friday, 1.2m },    
                    { DayOfWeek.Saturday, 1.3m },  
                    { DayOfWeek.Sunday, 1.3m },    
                },

                DurationMultipliers = new Dictionary<int, decimal>
                {
                    { 1, 1.1m },   
                    { 3, 1.0m },   
                    { 8, 0.95m },  
                    { 24, 0.9m },  
                    { 72, 0.85m },
                },

                SeasonalMultipliers = new Dictionary<int, decimal>
                {
                    { 1, 0.9m },  
                    { 2, 0.9m },   
                    { 3, 1.0m },   
                    { 4, 1.0m },   
                    { 5, 1.1m },   
                    { 6, 1.2m },   
                    { 7, 1.3m },   
                    { 8, 1.3m },   
                    { 9, 1.1m },   
                    { 10, 1.0m },  
                    { 11, 0.95m }, 
                    { 12, 1.1m },  
                },

                HolidayMultipliers = new Dictionary<DateTime, decimal>
                {
                    { new DateTime(DateTime.Now.Year, 1, 1), 2.0m },  
                    { new DateTime(DateTime.Now.Year, 1, 2), 1.8m },  
                    { new DateTime(DateTime.Now.Year, 1, 3), 1.5m }, 
                    { new DateTime(DateTime.Now.Year, 1, 4), 1.5m },  
                    { new DateTime(DateTime.Now.Year, 1, 5), 1.5m },  
                    { new DateTime(DateTime.Now.Year, 1, 6), 1.5m },  
                    { new DateTime(DateTime.Now.Year, 1, 7), 1.5m },  
                    { new DateTime(DateTime.Now.Year, 1, 8), 1.3m },  

                    { new DateTime(DateTime.Now.Year, 2, 23), 1.5m }, 
                    { new DateTime(DateTime.Now.Year, 3, 8), 1.5m },  
                    { new DateTime(DateTime.Now.Year, 5, 1), 1.5m },  
                    { new DateTime(DateTime.Now.Year, 5, 9), 1.5m },  
                    { new DateTime(DateTime.Now.Year, 6, 12), 1.3m }, 
                    { new DateTime(DateTime.Now.Year, 11, 4), 1.3m }, 
                    { new DateTime(DateTime.Now.Year, 12, 31), 2.0m }, 
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
