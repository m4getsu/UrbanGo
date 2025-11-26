using Model;
namespace BussinessLogic.Dto
{
    /// <summary>
    /// DTO для импорта данных автомобиля из внешнего источника.
    /// </summary>
    internal class CarImportDto
    {
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string LicensePlate { get; set; } = string.Empty;
        public int Year { get; set; }
        public int Mileage { get; set; }
        public CarStatus Status { get; set; }
        public decimal RentalPricePerHour { get; set; }
    }
}
