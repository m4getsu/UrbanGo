using System.Collections.Generic;
using System.Linq;
using BussinessLogic.Dto;

namespace Presenter.ObservableDTO.Mappers
{
    /// <summary>
    /// Mapper для конвертации между CarDetailsDto и ObservableCarDetailsDto.
    /// </summary>
    public static class CarDetailsMapper
    {
        /// <summary>
        /// Конвертирует CarDetailsDto в ObservableCarDetailsDto.
        /// </summary>
        /// <param name="dto">DTO для конвертации.</param>
        /// <returns>Observable DTO или null.</returns>
        public static ObservableCarDetailsDto? ToObservable(CarDetailsDto? dto)
        {
            if (dto == null)
                return null;

            return new ObservableCarDetailsDto
            {
                Id = dto.Id,
                Brand = dto.Brand,
                Model = dto.Model,
                LicensePlate = dto.LicensePlate,
                Year = dto.Year,
                Mileage = dto.Mileage,
                RentalPricePerHour = dto.RentalPricePerHour,
                StatusText = dto.StatusText,
                Description = dto.Description
            };
        }

        /// <summary>
        /// Конвертирует ObservableCarDetailsDto в CarDetailsDto.
        /// </summary>
        /// <param name="observableDto">Observable DTO для конвертации.</param>
        /// <returns>DTO или null.</returns>
        public static CarDetailsDto? ToDTO(ObservableCarDetailsDto? observableDto)
        {
            if (observableDto == null)
                return null;

            return new CarDetailsDto
            {
                Id = observableDto.Id,
                Brand = observableDto.Brand,
                Model = observableDto.Model,
                LicensePlate = observableDto.LicensePlate,
                Year = observableDto.Year,
                Mileage = observableDto.Mileage,
                RentalPricePerHour = observableDto.RentalPricePerHour,
                StatusText = observableDto.StatusText,
                Description = observableDto.Description
            };
        }

        /// <summary>
        /// Конвертирует список CarDetailsDto в список ObservableCarDetailsDto.
        /// </summary>
        /// <param name="dtoList">Список DTO.</param>
        /// <returns>Список Observable DTO.</returns>
        public static List<ObservableCarDetailsDto> ToObservableList(List<CarDetailsDto>? dtoList)
        {
            if (dtoList == null)
                return new List<ObservableCarDetailsDto>();

            return dtoList.Select(ToObservable).Where(x => x != null).ToList()!;
        }

        /// <summary>
        /// Конвертирует список ObservableCarDetailsDto в список CarDetailsDto.
        /// </summary>
        /// <param name="observableDtoList">Список Observable DTO.</param>
        /// <returns>Список DTO.</returns>
        public static List<CarDetailsDto> ToDTOList(List<ObservableCarDetailsDto>? observableDtoList)
        {
            if (observableDtoList == null)
                return new List<CarDetailsDto>();

            return observableDtoList.Select(ToDTO).Where(x => x != null).ToList()!;
        }
    }
}
