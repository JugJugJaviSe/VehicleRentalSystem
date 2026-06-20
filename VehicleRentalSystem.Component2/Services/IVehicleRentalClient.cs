using System;
using System.Collections.Generic;
using VehicleRentalSystem.Models.Models;

namespace VehicleRentalSystem.Component2.Services
{
    public interface IVehicleRentalClient
    {
        List<Vehicle> GetAllVehicles();
        List<RentalRecord> GetRentalRecordsByVehicleAndMonth(Guid vehicleId, int year, int month);
    }
}
