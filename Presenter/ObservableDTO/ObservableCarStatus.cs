using System.Collections.Generic;

namespace Presenter.ObservableDTO
{
    /// <summary>
    /// Observable класс для представления статуса автомобиля в UI.
    /// </summary>
    public class ObservableCarStatus
    {
        /// <summary>
        /// Идентификатор статуса.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Название статуса для отображения.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Возвращает список всех доступных статусов автомобиля.
        /// </summary>
        /// <returns>Список статусов.</returns>
        public static List<ObservableCarStatus> GetAll()
        {
            return new List<ObservableCarStatus>
            {
                new ObservableCarStatus { Id = 1, Name = "Доступен" },
                new ObservableCarStatus { Id = 2, Name = "В использовании" },
                new ObservableCarStatus { Id = 3, Name = "На обслуживании" },
                new ObservableCarStatus { Id = 4, Name = "Вне эксплуатации" }
            };
        }

        /// <summary>
        /// Возвращает статус по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор статуса.</param>
        /// <returns>Объект статуса или статус "Доступен" по умолчанию.</returns>
        public static ObservableCarStatus GetById(int id)
        {
            var statuses = GetAll();
            return statuses.Find(s => s.Id == id) ?? statuses[0];
        }
    }
}
