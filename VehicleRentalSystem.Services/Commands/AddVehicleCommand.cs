using VehicleRentalSystem.Models.Models;
using VehicleRentalSystem.Services.Interfaces;

namespace VehicleRentalSystem.Services.Commands
{
    public class AddVehicleCommand : IUndoableCommand
    {
        private readonly IVehicleRepository _repository;
        private readonly Vehicle _vehicle;

        public AddVehicleCommand(IVehicleRepository repository, Vehicle vehicle)
        {
            _repository = repository;
            _vehicle = vehicle;
        }

        public void Execute()
        {
            _repository.Add(_vehicle);
        }

        public void Undo()
        {
            _repository.Delete(_vehicle.Id);
        }
    }
}
