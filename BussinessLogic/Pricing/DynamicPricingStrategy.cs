using System;
using System.Linq;
using System.Text;

namespace BussinessLogic.Pricing
{
    /// <summary>
    /// Реализация динамической стратегии ценообразования с конфигурируемыми коэффициентами.
    /// Цена зависит от времени суток, дня недели, сезона, праздников и продолжительности аренды.
    /// </summary>
    public class DynamicPricingStrategy : IPricingStrategy
    {
        private readonly PricingConfiguration _config;

        /// <summary>
        /// Инициализирует стратегию с указанной конфигурацией.
        /// </summary>
        /// <param name="config">Конфигурация коэффициентов. Если null, используется конфигурация по умолчанию.</param>
        public DynamicPricingStrategy(PricingConfiguration config = null)
        {
            _config = config ?? PricingConfiguration.GetDefault();
        }

        /// <summary>
        /// Рассчитывает базовую стоимость аренды с учетом динамических коэффициентов.
        /// </summary>
        /// <param name="pricePerHour">Базовая стоимость аренды за час.</param>
        /// <param name="hours">Количество часов аренды.</param>
        /// <returns>Динамическая стоимость с учетом всех коэффициентов.</returns>
        /// <exception cref="ArgumentException">Если цена или количество часов некорректны.</exception>
        public decimal CalculateBasePrice(decimal pricePerHour, int hours)
        {
            if (pricePerHour <= 0)
                throw new ArgumentException("Price per hour must be positive.", nameof(pricePerHour));
            if (hours <= 0)
                throw new ArgumentException("Hours must be positive.", nameof(hours));

            var now = DateTime.Now;
            var multiplier = 1.0m;

            // Приоритет: праздник > остальные коэффициенты
            var holidayMultiplier = GetHolidayMultiplier(now);
            if (holidayMultiplier > 1.0m)
            {
                // В праздники применяется только праздничный коэффициент
                multiplier = holidayMultiplier;
            }
            else
            {
                // Комбинируем все коэффициенты
                multiplier *= GetSeasonalMultiplier(now);
                multiplier *= GetDayOfWeekMultiplier(now);
                multiplier *= GetTimeOfDayMultiplier(now);
            }

            // Продолжительность применяется всегда
            multiplier *= GetDurationMultiplier(hours);

            // Расчет финальной цены
            var basePrice = pricePerHour * hours;
            var finalPrice = basePrice * multiplier;

            return Math.Round(finalPrice, 2);
        }

        /// <summary>
        /// Возвращает коэффициент в зависимости от времени суток.
        /// </summary>
        private decimal GetTimeOfDayMultiplier(DateTime time)
        {
            var hour = time.Hour;
            if (_config.TimeOfDayMultipliers.TryGetValue(hour, out var multiplier))
                return multiplier;
            return 1.0m;
        }

        /// <summary>
        /// Возвращает коэффициент в зависимости от дня недели.
        /// </summary>
        private decimal GetDayOfWeekMultiplier(DateTime time)
        {
            if (_config.DayOfWeekMultipliers.TryGetValue(time.DayOfWeek, out var multiplier))
                return multiplier;
            return 1.0m;
        }

        /// <summary>
        /// Возвращает коэффициент в зависимости от продолжительности аренды.
        /// Использует наибольший применимый порог.
        /// </summary>
        private decimal GetDurationMultiplier(int hours)
        {
            // Найти максимальный порог, который не превышает количество часов
            var applicableThresholds = _config.DurationMultipliers.Keys
                .Where(threshold => hours >= threshold)
                .OrderByDescending(t => t);

            var maxThreshold = applicableThresholds.FirstOrDefault();

            if (maxThreshold > 0 && _config.DurationMultipliers.TryGetValue(maxThreshold, out var multiplier))
                return multiplier;

            return 1.0m;
        }

        /// <summary>
        /// Возвращает сезонный коэффициент в зависимости от месяца.
        /// </summary>
        private decimal GetSeasonalMultiplier(DateTime time)
        {
            var month = time.Month;
            if (_config.SeasonalMultipliers.TryGetValue(month, out var multiplier))
                return multiplier;
            return 1.0m;
        }

        /// <summary>
        /// Возвращает праздничный коэффициент, если текущая дата является праздником.
        /// </summary>
        private decimal GetHolidayMultiplier(DateTime time)
        {
            var dateOnly = time.Date;
            if (_config.HolidayMultipliers.TryGetValue(dateOnly, out var multiplier))
                return multiplier;
            return 1.0m;
        }

        /// <summary>
        /// Возвращает детальное описание примененных коэффициентов для отображения пользователю.
        /// </summary>
        /// <param name="hours">Количество часов аренды.</param>
        /// <returns>Текстовое описание всех примененных коэффициентов.</returns>
        public string GetDetailedMultiplierBreakdown(int hours)
        {
            var now = DateTime.Now;
            var sb = new StringBuilder();

            sb.AppendLine("📊 Детализация динамического ценообразования:");
            sb.AppendLine();

            var holidayMult = GetHolidayMultiplier(now);
            if (holidayMult > 1.0m)
            {
                sb.AppendLine($"🎉 Праздничный день: {FormatMultiplier(holidayMult)}");
                sb.AppendLine("   (В праздники применяется только праздничный коэффициент)");
            }
            else
            {
                var seasonMult = GetSeasonalMultiplier(now);
                sb.AppendLine($"🌤️  Сезон ({GetMonthName(now.Month)}): {FormatMultiplier(seasonMult)}");

                var dayMult = GetDayOfWeekMultiplier(now);
                sb.AppendLine($"📅 День недели ({GetDayName(now.DayOfWeek)}): {FormatMultiplier(dayMult)}");

                var timeMult = GetTimeOfDayMultiplier(now);
                sb.AppendLine($"🕐 Время суток ({now:HH:mm}): {FormatMultiplier(timeMult)}");
            }

            var durationMult = GetDurationMultiplier(hours);
            sb.AppendLine($"⏱️  Продолжительность ({hours}ч): {FormatMultiplier(durationMult)}");

            // Итоговый коэффициент
            var totalMultiplier = CalculateTotalMultiplier(hours);
            sb.AppendLine();
            sb.AppendLine($"📈 Итоговый коэффициент: {totalMultiplier:F2}x ({FormatMultiplier(totalMultiplier)})");

            return sb.ToString();
        }

        /// <summary>
        /// Вычисляет итоговый коэффициент для текущего момента времени.
        /// </summary>
        private decimal CalculateTotalMultiplier(int hours)
        {
            var now = DateTime.Now;
            var multiplier = 1.0m;

            var holidayMultiplier = GetHolidayMultiplier(now);
            if (holidayMultiplier > 1.0m)
            {
                multiplier = holidayMultiplier;
            }
            else
            {
                multiplier *= GetSeasonalMultiplier(now);
                multiplier *= GetDayOfWeekMultiplier(now);
                multiplier *= GetTimeOfDayMultiplier(now);
            }

            multiplier *= GetDurationMultiplier(hours);
            return multiplier;
        }

        /// <summary>
        /// Форматирует множитель в процентный вид для отображения.
        /// </summary>
        private string FormatMultiplier(decimal multiplier)
        {
            var percent = (multiplier - 1) * 100;
            if (percent > 0)
                return $"+{percent:F0}%";
            else if (percent < 0)
                return $"{percent:F0}%";
            else
                return "без изменений";
        }

        /// <summary>
        /// Возвращает название месяца на русском языке.
        /// </summary>
        private string GetMonthName(int month)
        {
            var months = new[]
            {
                "Январь", "Февраль", "Март", "Апрель", "Май", "Июнь",
                "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"
            };
            return months[month - 1];
        }

        /// <summary>
        /// Возвращает название дня недели на русском языке.
        /// </summary>
        private string GetDayName(DayOfWeek day)
        {
            var days = new System.Collections.Generic.Dictionary<DayOfWeek, string>
            {
                { DayOfWeek.Monday, "Понедельник" },
                { DayOfWeek.Tuesday, "Вторник" },
                { DayOfWeek.Wednesday, "Среда" },
                { DayOfWeek.Thursday, "Четверг" },
                { DayOfWeek.Friday, "Пятница" },
                { DayOfWeek.Saturday, "Суббота" },
                { DayOfWeek.Sunday, "Воскресенье" },
            };
            return days[day];
        }

        /// <summary>
        /// Возвращает краткое описание текущих условий ценообразования.
        /// </summary>
        public string GetCurrentConditionsDescription()
        {
            var now = DateTime.Now;
            var holidayMult = GetHolidayMultiplier(now);

            if (holidayMult > 1.0m)
            {
                return $"🎉 Праздничный день ({FormatMultiplier(holidayMult)})";
            }

            var timeMult = GetTimeOfDayMultiplier(now);
            var dayMult = GetDayOfWeekMultiplier(now);
            var seasonMult = GetSeasonalMultiplier(now);

            var conditions = new System.Collections.Generic.List<string>();

            if (timeMult > 1.0m)
                conditions.Add($"Пиковое время ({FormatMultiplier(timeMult)})");
            else if (timeMult < 1.0m)
                conditions.Add($"Ночная скидка ({FormatMultiplier(timeMult)})");

            if (dayMult > 1.0m)
                conditions.Add($"Выходные ({FormatMultiplier(dayMult)})");

            if (seasonMult > 1.0m)
                conditions.Add($"Высокий сезон ({FormatMultiplier(seasonMult)})");
            else if (seasonMult < 1.0m)
                conditions.Add($"Низкий сезон ({FormatMultiplier(seasonMult)})");

            return conditions.Count > 0
                ? "🔥 " + string.Join(" • ", conditions)
                : "💵 Базовые цены";
        }
    }
}
