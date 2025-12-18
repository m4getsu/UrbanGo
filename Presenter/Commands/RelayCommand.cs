using System;
using System.Windows.Input;

namespace Presenter.Commands
{
    /// <summary>
    /// Реализация ICommand для использования в MVVM.
    /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly Action<object?> _execute;
        private readonly Predicate<object?>? _canExecute;

        /// <summary>
        /// Событие изменения возможности выполнения команды.
        /// </summary>
        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        /// <summary>
        /// Инициализирует новую команду.
        /// </summary>
        /// <param name="execute">Действие для выполнения.</param>
        /// <param name="canExecute">Предикат проверки возможности выполнения.</param>
        public RelayCommand(Action<object?> execute, Predicate<object?>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        /// <summary>
        /// Определяет, может ли команда выполниться.
        /// </summary>
        /// <param name="parameter">Параметр команды.</param>
        /// <returns>True если команда может выполниться, иначе False.</returns>
        public bool CanExecute(object? parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        /// <summary>
        /// Выполняет команду.
        /// </summary>
        /// <param name="parameter">Параметр команды.</param>
        public void Execute(object? parameter)
        {
            _execute(parameter);
        }
    }
}
