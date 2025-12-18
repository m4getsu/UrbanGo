using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Presenter
{
    /// <summary>
    /// Базовый класс для всех ViewModel с поддержкой INotifyPropertyChanged.
    /// </summary>
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        private VMManager? _vmManager;

        /// <summary>
        /// Событие изменения свойства.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Устанавливает ссылку на VMManager.
        /// </summary>
        /// <param name="vmManager">Менеджер ViewModel.</param>
        internal void SetVMManager(VMManager vmManager)
        {
            _vmManager = vmManager ?? throw new ArgumentNullException(nameof(vmManager));
        }

        /// <summary>
        /// Вызывает событие PropertyChanged.
        /// </summary>
        /// <param name="propertyName">Имя свойства.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Устанавливает значение свойства и вызывает PropertyChanged при изменении.
        /// </summary>
        /// <typeparam name="T">Тип свойства.</typeparam>
        /// <param name="field">Ссылка на backing field.</param>
        /// <param name="value">Новое значение.</param>
        /// <param name="propertyName">Имя свойства.</param>
        /// <returns>True если значение изменилось, иначе False.</returns>
        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;

            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// Инициализирует ViewModel. Вызывается VMManager после создания.
        /// </summary>
        public virtual void Initialize()
        {
            OnInitialized();
        }

        /// <summary>
        /// Уведомляет VMManager о завершении инициализации.
        /// </summary>
        protected void OnInitialized()
        {
            _vmManager?.OnViewModelInitialized(this);
        }
    }
}
