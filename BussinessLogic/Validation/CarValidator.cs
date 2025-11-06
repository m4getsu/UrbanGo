using System;

namespace BussinessLogic.Validation
{
    /// <summary>
    /// Реализация валидатора для проверки данных автомобилей.
    /// </summary>
    public class CarValidator : ICarValidator
    {
        /// <summary>
        /// Проверяет корректность данных при создании автомобиля.
        /// </summary>
        /// <param name="brand">Марка автомобиля.</param>
        /// <param name="model">Модель автомобиля.</param>
        /// <param name="licensePlate">Государственный номер.</param>
        /// <param name="year">Год выпуска.</param>
        /// <param name="mileage">Пробег.</param>
        /// <param name="rentalPricePerHour">Стоимость аренды за час.</param>
        public void ValidateForCreate(string brand, string model, string licensePlate, int year, int mileage, decimal rentalPricePerHour)
        {
            if (string.IsNullOrWhiteSpace(brand))
                throw new ArgumentException("Марка автомобиля не может быть пустой.", nameof(brand));
            if (string.IsNullOrWhiteSpace(model))
                throw new ArgumentException("Модель автомобиля не может быть пустой.", nameof(model));
            if (string.IsNullOrWhiteSpace(licensePlate))
                throw new ArgumentException("Государственный номер не может быть пустым.", nameof(licensePlate));
            if (year < 1900 || year > DateTime.Now.Year + 1)
                throw new ArgumentException($"Год выпуска должен быть между 1900 и {DateTime.Now.Year + 1}.", nameof(year));
            if (mileage < 0)
                throw new ArgumentException("Пробег не может быть отрицательным.", nameof(mileage));
            if (rentalPricePerHour <= 0)
                throw new ArgumentException("Стоимость аренды должна быть положительной.", nameof(rentalPricePerHour));
        }

        /// <summary>
        /// Проверяет корректность данных при обновлении автомобиля.
        /// </summary>
        /// <param name="brand">Марка автомобиля.</param>
        /// <param name="model">Модель автомобиля.</param>
        /// <param name="licensePlate">Государственный номер.</param>
        /// <param name="year">Год выпуска.</param>
        /// <param name="mileage">Пробег.</param>
        /// <param name="rentalPricePerHour">Стоимость аренды за час.</param>
        /// <param name="status">Статус автомобиля.</param>
        public void ValidateForUpdate(string brand, string model, string licensePlate, int year, int mileage, decimal rentalPricePerHour, int status)
        {
            ValidateForCreate(brand, model, licensePlate, year, mileage, rentalPricePerHour);
            if (status < 0 || status > 2)
                throw new ArgumentException("Некорректный статус. Допустимо: 0 - Cвободен, 1 - В аренде, 2 - На обслуживании.", nameof(status));
        }
    }
}
