using System.Windows;
using Microsoft.Win32;
using Presenter.ViewModels;

namespace ViewWPF
{
    /// <summary>
    /// Логика взаимодействия для CarImportWindow.xaml
    /// </summary>
    public partial class CarImportWindow : Window
    {
        /// <summary>
        /// Инициализирует новый экземпляр CarImportWindow.
        /// </summary>
        public CarImportWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Обработчик нажатия кнопки выбора файла для импорта.
        /// </summary>
        private void ButtonBrowse_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is CarImportViewModel viewModel)
            {
                var dialog = new OpenFileDialog
                {
                    Filter = viewModel.IsCsvFormat
                        ? "CSV файлы (*.csv)|*.csv|Все файлы (*.*)|*.*"
                        : "JSON файлы (*.json)|*.json|Все файлы (*.*)|*.*",
                    Title = "Выберите файл для импорта автомобилей"
                };

                if (dialog.ShowDialog() == true)
                {
                    viewModel.FilePath = dialog.FileName;
                }
            }
        }

        /// <summary>
        /// Обработчик нажатия кнопки закрытия окна.
        /// </summary>
        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
