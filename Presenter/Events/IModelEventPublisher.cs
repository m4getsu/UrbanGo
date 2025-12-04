using System;

namespace Presenter.Events
{
    /// <summary>
    /// Интерфейс для публикации событий модели.
    /// Presenter подписывается на эти события для получения уведомлений от Model.
    /// </summary>
    public interface IModelEventPublisher
    {
        /// <summary>
        /// Событие выполнения операции с автомобилем (создание, обновление, удаление, аренда).
        /// </summary>
        event EventHandler<CarOperationEventArgs> CarOperationPerformed;

        /// <summary>
        /// Событие валидации данных.
        /// </summary>
        event EventHandler<ValidationEventArgs> ValidationPerformed;

        /// <summary>
        /// Событие импорта/экспорта данных.
        /// </summary>
        event EventHandler<ImportExportEventArgs> ImportExportPerformed;

        /// <summary>
        /// Событие ошибки в модели.
        /// </summary>
        event EventHandler<ModelEventArgs> ErrorOccurred;

        /// <summary>
        /// Событие информационного сообщения от модели.
        /// </summary>
        event EventHandler<ModelEventArgs> InfoMessage;
    }
}
