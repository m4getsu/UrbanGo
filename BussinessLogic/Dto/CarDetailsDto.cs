namespace BussinessLogic.Dto
{
    public class CarDetailsDto
    {
        public int Id { get; set; }
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string LicensePlate { get; set; } = string.Empty;
        public int Year { get; set; }
        public int Mileage { get; set; }
        public decimal RentalPricePerHour { get; set; }
        public string StatusText { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
