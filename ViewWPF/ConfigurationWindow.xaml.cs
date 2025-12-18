using System.Windows;

namespace ViewWPF
{
    /// <summary>
    /// Окно выбора конфигурации приложения
    /// </summary>
    public partial class ConfigurationWindow : Window
    {
        /// <summary>
        /// Использовать Entity Framework для работы с БД.
        /// </summary>
        public bool UseEntityFramework { get; private set; }

        /// <summary>
        /// Использовать динамическое ценообразование.
        /// </summary>
        public bool UseDynamicPricing { get; private set; }

        /// <summary>
        /// Инициализирует новый экземпляр ConfigurationWindow.
        /// </summary>
        public ConfigurationWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Обработчик нажатия кнопки OK (сохранение выбранных настроек).
        /// </summary>
        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            UseEntityFramework = radioEF.IsChecked == true;
            UseDynamicPricing = radioDynamic.IsChecked == true;
            DialogResult = true;
            Close();
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
