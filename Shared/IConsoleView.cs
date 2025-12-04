using System;
using System.Collections.Generic;

namespace Shared
{
    /// <summary>
    /// Интерфейс консольного представления для работы с системой через консоль.
    /// </summary>
    public interface IConsoleView
    {
        // События пользовательских действий

        /// <summary>
        /// Событие выбора пункта меню.
        /// Аргумент: номер пункта меню
        /// </summary>
        event EventHandler<int> MenuItemSelected;

        /// <summary>
        /// Событие выхода из приложения.
        /// </summary>
        event EventHandler ExitRequested;

        // Методы для отображения данных

        /// <summary>
        /// Отображает главное меню.
        /// </summary>
        void DisplayMainMenu();

        /// <summary>
        /// Отображает список автомобилей в консоли.
        /// </summary>
        /// <param name="cars">Список автомобилей.</param>
        void DisplayCarsList(IEnumerable<object> cars);

        /// <summary>
        /// Отображает детальную информацию об автомобиле.
        /// </summary>
        /// <param name="carDetails">Данные автомобиля.</param>
        void DisplayCarDetails(object carDetails);

        /// <summary>
        /// Запрашивает у пользователя ввод строки.
        /// </summary>
        /// <param name="prompt">Текст запроса.</param>
        /// <returns>Введенная строка.</returns>
        string GetStringInput(string prompt);

        /// <summary>
        /// Запрашивает у пользователя ввод целого числа.
        /// </summary>
        /// <param name="prompt">Текст запроса.</param>
        /// <returns>Введенное число.</returns>
        int GetIntInput(string prompt);

        /// <summary>
        /// Запрашивает у пользователя ввод десятичного числа.
        /// </summary>
        /// <param name="prompt">Текст запроса.</param>
        /// <returns>Введенное число.</returns>
        decimal GetDecimalInput(string prompt);

        /// <summary>
        /// Запрашивает подтверждение у пользователя (Y/N).
        /// </summary>
        /// <param name="message">Текст запроса.</param>
        /// <returns>True если подтверждено.</returns>
        bool GetConfirmation(string message);

        /// <summary>
        /// Отображает сообщение об успехе.
        /// </summary>
        /// <param name="message">Текст сообщения.</param>
        void ShowSuccess(string message);

        /// <summary>
        /// Отображает сообщение об ошибке.
        /// </summary>
        /// <param name="message">Текст ошибки.</param>
        void ShowError(string message);

        /// <summary>
        /// Отображает информационное сообщение.
        /// </summary>
        /// <param name="message">Текст сообщения.</param>
        void ShowInfo(string message);

        /// <summary>
        /// Очищает экран консоли.
        /// </summary>
        void Clear();

        /// <summary>
        /// Ожидает нажатия клавиши пользователем.
        /// </summary>
        void WaitForKeyPress();
    }
}
