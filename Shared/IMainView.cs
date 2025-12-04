using System;
using System.Collections.Generic;

namespace Shared
{
    /// <summary>
    /// Интерфейс главного представления (главная форма приложения).
    /// Отображает список автомобилей и предоставляет события для взаимодействия пользователя.
    /// </summary>
    public interface IMainView
    {
        // События, на которые подписывается Presenter

        /// <summary>
        /// Событие загрузки представления.
        /// </summary>
        event EventHandler ViewLoaded;

        /// <summary>
        /// Событие добавления нового автомобиля.
        /// </summary>
        event EventHandler AddCarRequested;

        /// <summary>
        /// Событие редактирования автомобиля.
        /// Аргумент: ID автомобиля
        /// </summary>
        event EventHandler<int> EditCarRequested;

        /// <summary>
        /// Событие удаления автомобиля.
        /// Аргумент: ID автомобиля
        /// </summary>
        event EventHandler<int> DeleteCarRequested;

        /// <summary>
        /// Событие аренды автомобиля.
        /// Аргумент: ID автомобиля
        /// </summary>
        event EventHandler<int> RentCarRequested;

        /// <summary>
        /// Событие расчета стоимости аренды.
        /// Аргумент: ID автомобиля
        /// </summary>
        event EventHandler<int> CalculateCostRequested;

        /// <summary>
        /// Событие обновления списка автомобилей.
        /// </summary>
        event EventHandler RefreshRequested;

        /// <summary>
        /// Событие изменения поискового запроса.
        /// Аргумент: текст поиска
        /// </summary>
        event EventHandler<string> SearchTextChanged;

        /// <summary>
        /// Событие импорта автомобилей.
        /// </summary>
        event EventHandler ImportRequested;

        /// <summary>
        /// Событие экспорта автомобилей.
        /// Аргумент: список выбранных ID (null если экспорт всех)
        /// </summary>
        event EventHandler<IEnumerable<int>> ExportRequested;

        // Методы для отображения данных (вызываются Presenter)

        /// <summary>
        /// Отображает список автомобилей.
        /// </summary>
        /// <param name="cars">Список автомобилей для отображения.</param>
        void DisplayCars(IEnumerable<object> cars);

        /// <summary>
        /// Обновляет строку состояния с статистикой.
        /// </summary>
        /// <param name="total">Общее количество.</param>
        /// <param name="available">Доступно.</param>
        /// <param name="rented">В аренде.</param>
        /// <param name="maintenance">На обслуживании.</param>
        void UpdateStatusBar(int total, int available, int rented, int maintenance);

        /// <summary>
        /// Отображает сообщение об ошибке.
        /// </summary>
        /// <param name="message">Текст сообщения.</param>
        void ShowError(string message);

        /// <summary>
        /// Отображает информационное сообщение.
        /// </summary>
        /// <param name="message">Текст сообщения.</param>
        void ShowInfo(string message);

        /// <summary>
        /// Запрашивает подтверждение у пользователя.
        /// </summary>
        /// <param name="message">Текст запроса.</param>
        /// <returns>True если пользователь подтвердил, иначе False.</returns>
        bool ConfirmAction(string message);

        /// <summary>
        /// Получает ID выбранного автомобиля.
        /// </summary>
        /// <returns>ID выбранного автомобиля или null.</returns>
        int? GetSelectedCarId();

        /// <summary>
        /// Получает список ID выбранных автомобилей (для экспорта).
        /// </summary>
        /// <returns>Список ID или пустая коллекция.</returns>
        IEnumerable<int> GetSelectedCarIds();
    }
}
