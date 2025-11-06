namespace BussinessLogic.Validation
{
    public interface ICarValidator
    {
        void ValidateForCreate(string brand, string model, string licensePlate, int year, int mileage, decimal rentalPricePerHour);
        void ValidateForUpdate(string brand, string model, string licensePlate, int year, int mileage, decimal rentalPricePerHour, int status);
    }
}
