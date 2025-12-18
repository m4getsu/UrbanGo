using System.Collections.Generic;
using System.Linq;
using BussinessLogic.Dto;

namespace Presenter.ObservableDTO.Mappers
{
    /// <summary>
    /// Mapper для конвертации между CarListItemDto и ObservableCarListItemDto.
    /// </summary>
    public static class CarListItemMapper
    {
        /// <summary>
        /// Конвертирует CarListItemDto в ObservableCarListItemDto.
        /// </summary>
        /// <param name="dto">DTO для конвертации.</param>
        /// <returns>Observable DTO или null.</returns>
        public static ObservableCarListItemDto? ToObservable(CarListItemDto? dto)
        {
            if (dto == null)
                return null;

            return new ObservableCarListItemDto
            {
                Id = dto.Id,
                Brand = dto.Brand,
                Model = dto.Model,
                LicensePlate = dto.LicensePlate,
                Year = dto.Year,
                Mileage = dto.Mileage,
                RentalPricePerHour = dto.RentalPricePerHour,
                StatusText = dto.StatusText,
                DisplayText = dto.DisplayText
            };
        }

        /// <summary>
        /// Конвертирует ObservableCarListItemDto в CarListItemDto.
        /// </summary>
        /// <param name="observableDto">Observable DTO для конвертации.</param>
        /// <returns>DTO или null.</returns>
        public static CarListItemDto? ToDTO(ObservableCarListItemDto? observableDto)
        {
            if (observableDto == null)
                return null;

            return new CarListItemDto
            {
                Id = observableDto.Id,
                Brand = observableDto.Brand,
                Model = observableDto.Model,
                LicensePlate = observableDto.LicensePlate,
                Year = observableDto.Year,
                Mileage = observableDto.Mileage,
                RentalPricePerHour = observableDto.RentalPricePerHour,
                StatusText = observableDto.StatusText,
                DisplayText = observableDto.DisplayText
            };
        }

        /// <summary>
        /// Конвертирует список CarListItemDto в список ObservableCarListItemDto.
        /// </summary>
        /// <param name="dtoList">Список DTO.</param>
        /// <returns>Список Observable DTO.</returns>
        public static List<ObservableCarListItemDto> ToObservableList(List<CarListItemDto>? dtoList)
        {
            if (dtoList == null)
                return new List<ObservableCarListItemDto>();

            return dtoList.Select(ToObservable).Where(x => x != null).ToList()!;
        }

        /// <summary>
        /// Конвертирует список ObservableCarListItemDto в список CarListItemDto.
        /// </summary>
        /// <param name="observableDtoList">Список Observable DTO.</param>
        /// <returns>Список DTO.</returns>
        public static List<CarListItemDto> ToDTOList(List<ObservableCarListItemDto>? observableDtoList)
        {
            if (observableDtoList == null)
                return new List<CarListItemDto>();

            return observableDtoList.Select(ToDTO).Where(x => x != null).ToList()!;
        }
    }
}
