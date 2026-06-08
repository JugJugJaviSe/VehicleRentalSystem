using System.Collections.Generic;
using System.Linq;
using VehicleRentalSystem.Models.Models;
using VehicleRentalSystem.Services.Interfaces;

namespace VehicleRentalSystem.Services.Commands
{
    public class DeleteVehicleCommand : IUndoableCommand
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IRentalRecordRepository _rentalRecordRepository;
        private readonly Vehicle _vehicle;
        private readonly List<RentalRecord> _relatedRentalRecords;

        public DeleteVehicleCommand(
            IVehicleRepository vehicleRepository,
            IRentalRecordRepository rentalRecordRepository,
            Vehicle vehicle,
            IEnumerable<RentalRecord> relatedRentalRecords)
        {
            _vehicleRepository = vehicleRepository;
            _rentalRecordRepository = rentalRecordRepository;
            _vehicle = vehicle;
            _relatedRentalRecords = relatedRentalRecords.ToList();
        }

        public void Execute()
        {
            foreach (RentalRecord rentalRecord in _relatedRentalRecords)
            {
                _rentalRecordRepository.Delete(rentalRecord.Id);
            }

            _vehicleRepository.Delete(_vehicle.Id);
        }

        public void Undo()
        {
            _vehicleRepository.Add(_vehicle);

            foreach (RentalRecord rentalRecord in _relatedRentalRecords)
            {
                _rentalRecordRepository.Add(rentalRecord);
            }
        }
    }
}
