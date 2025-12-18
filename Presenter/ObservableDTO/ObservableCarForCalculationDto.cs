using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Presenter.ObservableDTO
{
    /// <summary>
    /// Observable обертка для CarForCalculationDto с поддержкой INotifyPropertyChanged.
    /// </summary>
    public class ObservableCarForCalculationDto : INotifyPropertyChanged
    {
        private int _id;
        private string _brand = string.Empty;
        private string _model = string.Empty;
        private string _licensePlate = string.Empty;
        private decimal _rentalPricePerHour;
        private string _displayText = string.Empty;

        /// <summary>
        /// Событие изменения свойства.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Идентификатор автомобиля.
        /// </summary>
        public int Id
        {
            get => _id;
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Марка автомобиля.
        /// </summary>
        public string Brand
        {
            get => _brand;
            set
            {
                if (_brand != value)
                {
                    _brand = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Модель автомобиля.
        /// </summary>
        public string Model
        {
            get => _model;
            set
            {
                if (_model != value)
                {
                    _model = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Государственный номерной знак автомобиля.
        /// </summary>
        public string LicensePlate
        {
            get => _licensePlate;
            set
            {
                if (_licensePlate != value)
                {
                    _licensePlate = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Стоимость аренды автомобиля за час.
        /// </summary>
        public decimal RentalPricePerHour
        {
            get => _rentalPricePerHour;
            set
            {
                if (_rentalPricePerHour != value)
                {
                    _rentalPricePerHour = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Текст для отображения в UI (краткое представление автомобиля).
        /// </summary>
        public string DisplayText
        {
            get => _displayText;
            set
            {
                if (_displayText != value)
                {
                    _displayText = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Вызывает событие PropertyChanged.
        /// </summary>
        /// <param name="propertyName">Имя свойства.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
