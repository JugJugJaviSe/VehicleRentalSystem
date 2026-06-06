using VehicleRentalSystem.Models.Models;
using VehicleRentalSystem.Services.Interfaces;

namespace VehicleRentalSystem.Services.Commands
{
    public class DeleteVehicleCommand : IUndoableCommand
    {
        private readonly IVehicleRepository _repository;
        private readonly Vehicle _vehicle;

        public DeleteVehicleCommand(IVehicleRepository repository, Vehicle vehicle)
        {
            _repository = repository;
            _vehicle = vehicle;
        }

        public void Execute()
        {
            _repository.Delete(_vehicle.Id);
        }

        public void Undo()
        {
            _repository.Add(_vehicle);
        }
    }
}
