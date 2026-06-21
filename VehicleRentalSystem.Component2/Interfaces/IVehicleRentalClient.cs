using System;
using System.Collections.Generic;
using VehicleRentalSystem.Models.Models;

namespace VehicleRentalSystem.Component2.Interfaces
{
    public interface IVehicleRentalClient
    {
        bool UsedFallback { get; }

        List<Vehicle> GetAllVehicles();
        List<RentalRecord> GetRentalRecordsByVehicleAndMonth(Guid vehicleId, int year, int month);
    }
}
