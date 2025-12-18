using System;
using System.Collections.Generic;
using System.Linq;
using Presenter.Commands;

namespace Presenter.Services
{
    /// <summary>
    /// Сервис для управления Undo/Redo операциями
    /// </summary>
    public class UndoService
    {
        private readonly Stack<IUndoableCommand> _undoStack = new Stack<IUndoableCommand>();
        private readonly Stack<IUndoableCommand> _redoStack = new Stack<IUndoableCommand>();
        private const int MaxUndoSteps = 20; // Ограничение на количество отмен

        public bool CanUndo => _undoStack.Count > 0;
        public bool CanRedo => _redoStack.Count > 0;

        /// <summary>
        /// Событие изменения состояния Undo/Redo (для обновления UI)
        /// </summary>
        public event EventHandler? UndoRedoStateChanged;

        /// <summary>
        /// Выполняет команду и добавляет её в стек Undo
        /// </summary>
        public void ExecuteCommand(IUndoableCommand command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            command.Execute();
            _undoStack.Push(command);
            _redoStack.Clear(); // Очищаем redo при новом действии

            // Ограничиваем размер стека
            if (_undoStack.Count > MaxUndoSteps)
            {
                var list = _undoStack.ToList();
                list.RemoveAt(list.Count - 1);
                _undoStack.Clear();
                foreach (var cmd in list.AsEnumerable().Reverse())
                {
                    _undoStack.Push(cmd);
                }
            }

            OnUndoRedoStateChanged();
        }

        /// <summary>
        /// Отменяет последнюю команду
        /// </summary>
        public void Undo()
        {
            if (!CanUndo)
                throw new InvalidOperationException("Нет команд для отмены");

            var command = _undoStack.Pop();
            command.Undo();
            _redoStack.Push(command);

            OnUndoRedoStateChanged();
        }

        /// <summary>
        /// Повторяет последнюю отменённую команду
        /// </summary>
        public void Redo()
        {
            if (!CanRedo)
                throw new InvalidOperationException("Нет команд для повтора");

            var command = _redoStack.Pop();
            command.Execute();
            _undoStack.Push(command);

            OnUndoRedoStateChanged();
        }

        /// <summary>
        /// Получает описание команды для отмены
        /// </summary>
        public string GetUndoDescription()
        {
            return CanUndo ? $"Отменить: {_undoStack.Peek().Description}" : "Нечего отменять";
        }

        /// <summary>
        /// Получает описание команды для повтора
        /// </summary>
        public string GetRedoDescription()
        {
            return CanRedo ? $"Повторить: {_redoStack.Peek().Description}" : "Нечего повторять";
        }

        /// <summary>
        /// Очищает историю Undo/Redo
        /// </summary>
        public void Clear()
        {
            _undoStack.Clear();
            _redoStack.Clear();
            OnUndoRedoStateChanged();
        }

        private void OnUndoRedoStateChanged()
        {
            UndoRedoStateChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
