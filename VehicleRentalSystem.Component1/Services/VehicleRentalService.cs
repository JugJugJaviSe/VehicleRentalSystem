using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using VehicleRentalSystem.Models.Models;
using VehicleRentalSystem.Services.Interfaces;
using VehicleRentalSystem.WcfService.Contracts;

namespace VehicleRentalSystem.Component1.Services
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class VehicleRentalService : IVehicleRentalService
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IRentalRecordRepository _rentalRecordRepository;

        public VehicleRentalService(IVehicleRepository vehicleRepository, IRentalRecordRepository rentalRecordRepository)
        {
            _vehicleRepository = vehicleRepository;
            _rentalRecordRepository = rentalRecordRepository;
        }

        public List<Vehicle> GetAllVehicles()
        {
            return _vehicleRepository.GetAll().ToList();
        }

        public List<RentalRecord> GetRentalRecordsByVehicleAndMonth(Guid vehicleId, int year, int month)
        {
            return _rentalRecordRepository.GetAll()
                .Where(r => r.VehicleId == vehicleId
                         && r.RentalDate.Year == year
                         && r.RentalDate.Month == month)
                .ToList();
        }
    }
}
