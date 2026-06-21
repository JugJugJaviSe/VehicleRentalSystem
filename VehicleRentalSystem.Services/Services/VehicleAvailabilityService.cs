using System;
using System.Linq;
using VehicleRentalSystem.Models.Models;
using VehicleRentalSystem.Services.Interfaces;

namespace VehicleRentalSystem.Services.Services
{
    public class VehicleAvailabilityService : IVehicleAvailabilityService
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IRentalRecordRepository _rentalRecordRepository;
        private readonly IStateSimulationService _stateSimulationService;

        public VehicleAvailabilityService(
            IVehicleRepository vehicleRepository,
            IRentalRecordRepository rentalRecordRepository,
            IStateSimulationService stateSimulationService)
        {
            _vehicleRepository = vehicleRepository;
            _rentalRecordRepository = rentalRecordRepository;
            _stateSimulationService = stateSimulationService;
        }

        public void SynchronizeAvailability()
        {
            foreach (Vehicle vehicle in _vehicleRepository.GetAll())
            {
                vehicle.IsAvailable = IsVehicleAvailable(vehicle.Id);
            }
        }

        private bool IsVehicleAvailable(Guid vehicleId)
        {
            return !_rentalRecordRepository
                .GetAll()
                .Any(rentalRecord =>
                    rentalRecord.VehicleId == vehicleId
                    && _stateSimulationService.BlocksVehicleAvailability(rentalRecord.State));
        }
    }
}