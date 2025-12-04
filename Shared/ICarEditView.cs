using System;

namespace Shared
{
    /// <summary>
    /// Интерфейс представления для редактирования/создания автомобиля.
    /// Используется как для добавления нового автомобиля, так и для редактирования существующего.
    /// </summary>
    public interface ICarEditView
    {
        // События

        /// <summary>
        /// Событие сохранения данных автомобиля.
        /// </summary>
        event EventHandler SaveRequested;

        /// <summary>
        /// Событие отмены редактирования.
        /// </summary>
        event EventHandler CancelRequested;

        // Свойства для получения введенных данных

        /// <summary>
        /// Марка автомобиля.
        /// </summary>
        string Brand { get; }

        /// <summary>
        /// Модель автомобиля.
        /// </summary>
        string Model { get; }

        /// <summary>
        /// Государственный номер.
        /// </summary>
        string LicensePlate { get; }

        /// <summary>
        /// Год выпуска.
        /// </summary>
        int Year { get; }

        /// <summary>
        /// Пробег автомобиля.
        /// </summary>
        int Mileage { get; }

        /// <summary>
        /// Цена аренды за час.
        /// </summary>
        decimal Price { get; }

        /// <summary>
        /// Статус автомобиля (0 - Доступен, 1 - В аренде, 2 - На обслуживании).
        /// </summary>
        int Status { get; }

        /// <summary>
        /// Режим редактирования (true) или создания (false).
        /// </summary>
        bool IsEditMode { get; }

        // Методы для управления представлением

        /// <summary>
        /// Устанавливает данные автомобиля для редактирования.
        /// </summary>
        /// <param name="brand">Марка.</param>
        /// <param name="model">Модель.</param>
        /// <param name="licensePlate">Гос. номер.</param>
        /// <param name="year">Год.</param>
        /// <param name="mileage">Пробег.</param>
        /// <param name="price">Цена.</param>
        /// <param name="status">Статус.</param>
        void SetCarData(string brand, string model, string licensePlate, int year, int mileage, decimal price, int status);

        /// <summary>
        /// Закрывает представление с результатом OK.
        /// </summary>
        void CloseWithSuccess();

        /// <summary>
        /// Закрывает представление с результатом Cancel.
        /// </summary>
        void CloseWithCancel();

        /// <summary>
        /// Отображает ошибку валидации для поля.
        /// </summary>
        /// <param name="fieldName">Имя поля.</param>
        /// <param name="errorMessage">Сообщение об ошибке.</param>
        void ShowFieldError(string fieldName, string errorMessage);

        /// <summary>
        /// Очищает все ошибки валидации.
        /// </summary>
        void ClearErrors();
    }
}
