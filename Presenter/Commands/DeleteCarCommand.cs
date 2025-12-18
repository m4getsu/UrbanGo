using System;
using System.ComponentModel;
using BussinessLogic;
using Model;
using Presenter.ObservableDTO;

namespace Presenter.Commands
{
    /// <summary>
    /// Команда удаления автомобиля с поддержкой Undo
    /// </summary>
    public class DeleteCarCommand : IUndoableCommand
    {
        private readonly ICarService _carService;
        private readonly BindingList<ObservableCarListItemDto> _cars;
        private readonly ObservableCarListItemDto _deletedCarDto;
        private readonly int _deletedIndex;
        private Car? _deletedCarEntity;

        public string Description => $"Удаление {_deletedCarDto.Brand} {_deletedCarDto.Model} ({_deletedCarDto.LicensePlate})";

        public DeleteCarCommand(
            ICarService carService,
            BindingList<ObservableCarListItemDto> cars,
            ObservableCarListItemDto carToDelete)
        {
            _carService = carService ?? throw new ArgumentNullException(nameof(carService));
            _cars = cars ?? throw new ArgumentNullException(nameof(cars));
            _deletedCarDto = carToDelete ?? throw new ArgumentNullException(nameof(carToDelete));
            _deletedIndex = cars.IndexOf(carToDelete);
        }

        public void Execute()
        {
            // Сохраняем полную сущность перед удалением
            _deletedCarEntity = _carService.GetCar(_deletedCarDto.Id);

            if (_deletedCarEntity == null)
                throw new InvalidOperationException($"Автомобиль с ID {_deletedCarDto.Id} не найден");

            // Удаляем из БД
            _carService.DeleteCar(_deletedCarDto.Id);

            // Удаляем из коллекции UI
            _cars.Remove(_deletedCarDto);
        }

        public void Undo()
        {
            if (_deletedCarEntity == null)
                throw new InvalidOperationException("Невозможно отменить: данные автомобиля не сохранены");

            // Восстанавливаем в БД
            _carService.CreateCar(
                _deletedCarEntity.Brand,
                _deletedCarEntity.Model,
                _deletedCarEntity.LicensePlate,
                _deletedCarEntity.Year,
                _deletedCarEntity.Mileage,
                _deletedCarEntity.RentalPricePerHour);

            // Восстанавливаем в коллекцию на прежнюю позицию
            if (_deletedIndex >= 0 && _deletedIndex <= _cars.Count)
            {
                _cars.Insert(_deletedIndex, _deletedCarDto);
            }
            else
            {
                _cars.Add(_deletedCarDto);
            }
        }
    }
}
