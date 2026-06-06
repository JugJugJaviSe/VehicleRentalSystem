using VehicleRentalSystem.Models.Models;
using VehicleRentalSystem.Services.Interfaces;

namespace VehicleRentalSystem.Services.Commands
{
    public class UpdateVehicleCommand : IUndoableCommand
    {
        private readonly IVehicleRepository _repository;
        private readonly Vehicle _oldVehicle;
        private readonly Vehicle _newVehicle;

        public UpdateVehicleCommand(
            IVehicleRepository repository,
            Vehicle oldVehicle,
            Vehicle newVehicle)
        {
            _repository = repository;
            _oldVehicle = oldVehicle;
            _newVehicle = newVehicle;
        }

        public void Execute()
        {
            _repository.Update(_newVehicle);
        }

        public void Undo()
        {
            _repository.Update(_oldVehicle);
        }
    }
}
