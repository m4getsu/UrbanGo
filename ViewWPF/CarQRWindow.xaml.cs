using System.Windows;
using Microsoft.Win32;
using BussinessLogic.Dto;
using BussinessLogic.Services.QRCode;

namespace ViewWPF
{
    /// <summary>
    /// Логика взаимодействия для CarQRWindow.xaml
    /// </summary>
    public partial class CarQRWindow : Window
    {
        private IQRCodeService? _qrService;
        private CarQRDto? _carDto;

        /// <summary>
        /// Инициализирует новый экземпляр CarQRWindow.
        /// </summary>
        public CarQRWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Устанавливает зависимости для сохранения QR-кода.
        /// </summary>
        /// <param name="qrService">Сервис генерации QR-кодов.</param>
        /// <param name="carDto">DTO автомобиля для которого генерируется QR-код.</param>
        public void SetDependencies(IQRCodeService qrService, CarQRDto carDto)
        {
            _qrService = qrService;
            _carDto = carDto;
        }

        /// <summary>
        /// Обработчик нажатия кнопки сохранения QR-кода в файл.
        /// </summary>
        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            if (_qrService == null || _carDto == null)
            {
                MessageBox.Show(
                    "Ошибка: сервис QR-кодов не инициализирован",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            var saveDialog = new SaveFileDialog
            {
                Filter = "PNG файлы (*.png)|*.png",
                FileName = $"QR_{_carDto.LicensePlate}.png",
                Title = "Сохранить QR-код"
            };

            if (saveDialog.ShowDialog() == true)
            {
                try
                {
                    _qrService.SaveQRCodeToFile(_carDto, saveDialog.FileName, pixelsPerModule: 20);
                    MessageBox.Show(
                        $"QR-код успешно сохранён:\n{saveDialog.FileName}",
                        "Успех",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        $"Ошибка сохранения QR-кода:\n{ex.Message}",
                        "Ошибка",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
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
