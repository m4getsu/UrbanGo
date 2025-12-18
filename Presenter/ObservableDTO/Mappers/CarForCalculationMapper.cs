using System.Collections.Generic;
using System.Linq;
using BussinessLogic.Dto;

namespace Presenter.ObservableDTO.Mappers
{
    /// <summary>
    /// Mapper для конвертации между CarForCalculationDto и ObservableCarForCalculationDto.
    /// </summary>
    public static class CarForCalculationMapper
    {
        /// <summary>
        /// Конвертирует CarForCalculationDto в ObservableCarForCalculationDto.
        /// </summary>
        /// <param name="dto">DTO для конвертации.</param>
        /// <returns>Observable DTO или null.</returns>
        public static ObservableCarForCalculationDto? ToObservable(CarForCalculationDto? dto)
        {
            if (dto == null)
                return null;

            return new ObservableCarForCalculationDto
            {
                Id = dto.Id,
                Brand = dto.Brand,
                Model = dto.Model,
                LicensePlate = dto.LicensePlate,
                RentalPricePerHour = dto.RentalPricePerHour,
                DisplayText = dto.DisplayText
            };
        }

        /// <summary>
        /// Конвертирует ObservableCarForCalculationDto в CarForCalculationDto.
        /// </summary>
        /// <param name="observableDto">Observable DTO для конвертации.</param>
        /// <returns>DTO или null.</returns>
        public static CarForCalculationDto? ToDTO(ObservableCarForCalculationDto? observableDto)
        {
            if (observableDto == null)
                return null;

            return new CarForCalculationDto
            {
                Id = observableDto.Id,
                Brand = observableDto.Brand,
                Model = observableDto.Model,
                LicensePlate = observableDto.LicensePlate,
                RentalPricePerHour = observableDto.RentalPricePerHour,
                DisplayText = observableDto.DisplayText
            };
        }

        /// <summary>
        /// Конвертирует список CarForCalculationDto в список ObservableCarForCalculationDto.
        /// </summary>
        /// <param name="dtoList">Список DTO.</param>
        /// <returns>Список Observable DTO.</returns>
        public static List<ObservableCarForCalculationDto> ToObservableList(List<CarForCalculationDto>? dtoList)
        {
            if (dtoList == null)
                return new List<ObservableCarForCalculationDto>();

            return dtoList.Select(ToObservable).Where(x => x != null).ToList()!;
        }

        /// <summary>
        /// Конвертирует список ObservableCarForCalculationDto в список CarForCalculationDto.
        /// </summary>
        /// <param name="observableDtoList">Список Observable DTO.</param>
        /// <returns>Список DTO.</returns>
        public static List<CarForCalculationDto> ToDTOList(List<ObservableCarForCalculationDto>? observableDtoList)
        {
            if (observableDtoList == null)
                return new List<CarForCalculationDto>();

            return observableDtoList.Select(ToDTO).Where(x => x != null).ToList()!;
        }
    }
}
