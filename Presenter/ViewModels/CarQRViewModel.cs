using System;
using System.IO;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using BussinessLogic;
using BussinessLogic.Dto;
using BussinessLogic.Services.QRCode;
using Presenter.Commands;

namespace Presenter.ViewModels
{
    /// <summary>
    /// ViewModel для отображения QR-кода автомобиля
    /// </summary>
    public class CarQRViewModel : BaseViewModel
    {
        private readonly VMManager _vmManager;
        private readonly ICarService _carService;
        private readonly IQRCodeService _qrService;
        private readonly CarQRDto _carDto;

        private BitmapImage? _qrCodeImage;
        private string _carInfo = string.Empty;

        public CarQRViewModel(VMManager vmManager, ICarService carService, IQRCodeService qrService, CarQRDto carDto)
        {
            _vmManager = vmManager ?? throw new ArgumentNullException(nameof(vmManager));
            _carService = carService ?? throw new ArgumentNullException(nameof(carService));
            _qrService = qrService ?? throw new ArgumentNullException(nameof(qrService));
            _carDto = carDto ?? throw new ArgumentNullException(nameof(carDto));

            SaveCommand = new RelayCommand(_ => SaveQRCode());
            CloseCommand = new RelayCommand(_ => { }); 
        }

        public BitmapImage? QRCodeImage
        {
            get => _qrCodeImage;
            set => SetProperty(ref _qrCodeImage, value);
        }

        public string CarInfo
        {
            get => _carInfo;
            set => SetProperty(ref _carInfo, value);
        }

        public string WindowTitle => $"QR-код автомобиля {_carDto.LicensePlate}";

        public ICommand SaveCommand { get; }
        public ICommand CloseCommand { get; }

        /// <summary>
        /// Получает DTO автомобиля для которого создан QR-код
        /// </summary>
        public CarQRDto GetCarDto() => _carDto;

        public override void Initialize()
        {
            GenerateQRCode();
            base.Initialize();
        }

        private void GenerateQRCode()
        {
            try
            {
                CarInfo = _qrService.FormatCarInfo(_carDto);

                var qrCodeBytes = _qrService.GenerateQRCode(_carDto, pixelsPerModule: 15);

                QRCodeImage = ConvertByteArrayToBitmapImage(qrCodeBytes);
            }
            catch (Exception ex)
            {
                CarInfo = $"Ошибка генерации QR-кода:\n{ex.Message}";
            }
        }

        private void SaveQRCode()
        {
        }

        /// <summary>
        /// Конвертирует массив байтов (PNG) в BitmapImage для WPF
        /// </summary>
        private BitmapImage ConvertByteArrayToBitmapImage(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0)
                throw new ArgumentException("Данные изображения пусты", nameof(imageData));

            using (var memoryStream = new MemoryStream(imageData))
            {
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.EndInit();
                bitmapImage.Freeze(); 

                return bitmapImage;
            }
        }
    }
}
