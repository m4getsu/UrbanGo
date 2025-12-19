using System;
using System.Windows.Input;
using BussinessLogic;
using Presenter.Commands;

namespace Presenter.ViewModels
{
    /// <summary>
    /// ViewModel для расчета стоимости аренды.
    /// </summary>
    public class CalculateCostViewModel : BaseViewModel
    {
        private readonly VMManager _vmManager;
        private readonly ICarService _carService;
        private readonly int _carId;

        private string _carInfo = string.Empty;
        private decimal _pricePerHour;
        private int _hours = 1;
        private decimal _totalCost;
        private string _promoCode = string.Empty;
        private string _discountInfo = string.Empty;
        private string _pricingDetails = string.Empty;

        public string CarInfo
        {
            get => _carInfo;
            set => SetProperty(ref _carInfo, value);
        }

        public decimal PricePerHour
        {
            get => _pricePerHour;
            set => SetProperty(ref _pricePerHour, value);
        }

        public int Hours
        {
            get => _hours;
            set
            {
                if (SetProperty(ref _hours, value))
                {
                    CalculateCost();
                }
            }
        }

        public decimal TotalCost
        {
            get => _totalCost;
            set => SetProperty(ref _totalCost, value);
        }

        public string PromoCode
        {
            get => _promoCode;
            set => SetProperty(ref _promoCode, value);
        }

        public string DiscountInfo
        {
            get => _discountInfo;
            set => SetProperty(ref _discountInfo, value);
        }

        public string PricingDetails
        {
            get => _pricingDetails;
            set => SetProperty(ref _pricingDetails, value);
        }

        public ICommand ApplyPromoCommand { get; }
        public ICommand ShowDetailsCommand { get; }
        public ICommand CloseCommand { get; }

        public CalculateCostViewModel(VMManager vmManager, ICarService carService, int carId)
        {
            _vmManager = vmManager ?? throw new ArgumentNullException(nameof(vmManager));
            _carService = carService ?? throw new ArgumentNullException(nameof(carService));
            _carId = carId;

            ApplyPromoCommand = new RelayCommand(_ => ApplyPromo());
            ShowDetailsCommand = new RelayCommand(_ => ShowDetails());
            CloseCommand = new RelayCommand(_ => Close());
        }

        public override void Initialize()
        {
            LoadCarInfo();
            CalculateCost();
            base.Initialize();
        }

        private void LoadCarInfo()
        {
            var carDto = _carService.GetCarForCalculation(_carId);
            if (carDto != null)
            {
                CarInfo = $"{carDto.Brand} {carDto.Model} ({carDto.LicensePlate})";
                PricePerHour = carDto.RentalPricePerHour;
            }
        }

        private void CalculateCost()
        {
            try
            {
                TotalCost = _carService.CalculateRentalCost(_carId, Hours, string.IsNullOrWhiteSpace(PromoCode) ? null : PromoCode);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка расчета: {ex.Message}");
            }
        }

        private void ApplyPromo()
        {
            CalculateCost();
            if (!string.IsNullOrWhiteSpace(PromoCode))
            {
                DiscountInfo = "Промокод применен!";
            }
        }

        private void ShowDetails()
        {
            try
            {
                PricingDetails = _carService.GetPricingBreakdown(Hours);
            }
            catch (Exception ex)
            {
                PricingDetails = $"Ошибка получения деталей: {ex.Message}";
            }
        }

        private void Close()
        {
        }
    }
}
