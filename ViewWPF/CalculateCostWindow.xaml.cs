using System.Windows;
using Presenter.ViewModels;

namespace ViewWPF
{
    /// <summary>
    /// Логика взаимодействия для CalculateCostWindow.xaml
    /// </summary>
    public partial class CalculateCostWindow : Window
    {
        /// <summary>
        /// Инициализирует новый экземпляр CalculateCostWindow.
        /// </summary>
        public CalculateCostWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Обработчик нажатия кнопки OK (закрытие окна).
        /// </summary>
        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Обработчик нажатия кнопки показа детализации расчета стоимости.
        /// </summary>
        private void ButtonShowDetails_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is CalculateCostViewModel viewModel)
            {
                try
                {
                    string breakdown = viewModel.PricingDetails;
                    if (string.IsNullOrEmpty(breakdown))
                    {
                        // Получаем детализацию
                        viewModel.ShowDetailsCommand.Execute(null);
                        breakdown = viewModel.PricingDetails;
                    }

                    MessageBox.Show(
                        breakdown,
                        "Детализация расчета стоимости",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(
                        $"Ошибка получения детализации: {ex.Message}",
                        "Ошибка",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
        }
    }
}
