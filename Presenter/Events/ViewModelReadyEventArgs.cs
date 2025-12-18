using System;

namespace Presenter.Events
{
    /// <summary>
    /// Аргументы события готовности ViewModel.
    /// </summary>
    public class ViewModelReadyEventArgs : EventArgs
    {
        /// <summary>
        /// Готовая ViewModel.
        /// </summary>
        public BaseViewModel ViewModel { get; }

        /// <summary>
        /// Инициализирует новый экземпляр ViewModelReadyEventArgs.
        /// </summary>
        /// <param name="viewModel">Готовая ViewModel.</param>
        public ViewModelReadyEventArgs(BaseViewModel viewModel)
        {
            ViewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
        }
    }
}
