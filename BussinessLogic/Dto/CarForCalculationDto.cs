namespace BussinessLogic.Dto
{
    public class CarForCalculationDto
    {
        public int Id { get; set; }
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string LicensePlate { get; set; } = string.Empty;
        public decimal RentalPricePerHour { get; set; }
        public string DisplayText { get; set; } = string.Empty;
    }
}
