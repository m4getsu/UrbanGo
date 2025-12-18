using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Presenter.ObservableDTO
{
    /// <summary>
    /// Observable обертка для CarDetailsDto с поддержкой INotifyPropertyChanged.
    /// </summary>
    public class ObservableCarDetailsDto : INotifyPropertyChanged
    {
        private int _id;
        private string _brand = string.Empty;
        private string _model = string.Empty;
        private string _licensePlate = string.Empty;
        private int _year;
        private int _mileage;
        private decimal _rentalPricePerHour;
        private string _statusText = string.Empty;
        private string _description = string.Empty;

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
        /// Год выпуска автомобиля.
        /// </summary>
        public int Year
        {
            get => _year;
            set
            {
                if (_year != value)
                {
                    _year = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Пробег автомобиля в километрах.
        /// </summary>
        public int Mileage
        {
            get => _mileage;
            set
            {
                if (_mileage != value)
                {
                    _mileage = value;
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
        /// Текстовое представление статуса автомобиля.
        /// </summary>
        public string StatusText
        {
            get => _statusText;
            set
            {
                if (_statusText != value)
                {
                    _statusText = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Описание автомобиля.
        /// </summary>
        public string Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
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
