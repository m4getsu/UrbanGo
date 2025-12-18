namespace Presenter.Commands
{
    /// <summary>
    /// Интерфейс для команд, поддерживающих отмену (Undo/Redo)
    /// </summary>
    public interface IUndoableCommand
    {
        /// <summary>
        /// Выполняет команду
        /// </summary>
        void Execute();

        /// <summary>
        /// Отменяет выполненную команду
        /// </summary>
        void Undo();

        /// <summary>
        /// Описание команды для отображения пользователю
        /// </summary>
        string Description { get; }
    }
}
