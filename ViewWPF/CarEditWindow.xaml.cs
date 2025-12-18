using System.Windows;
using Presenter.ViewModels;

namespace ViewWPF
{
    /// <summary>
    /// Логика взаимодействия для CarEditWindow.xaml
    /// </summary>
    public partial class CarEditWindow : Window
    {
        /// <summary>
        /// Инициализирует новый экземпляр CarEditWindow.
        /// </summary>
        public CarEditWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Обработчик нажатия кнопки OK (сохранение изменений).
        /// </summary>
        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is CarEditViewModel viewModel && viewModel.DialogResult)
            {
                DialogResult = true;
                Close();
            }
        }

        /// <summary>
        /// Обработчик нажатия кнопки отмены.
        /// </summary>
        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
