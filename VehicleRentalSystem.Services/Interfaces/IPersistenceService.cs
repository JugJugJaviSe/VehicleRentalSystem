using System.Collections.Generic;
using VehicleRentalSystem.Models.Models;

namespace VehicleRentalSystem.Services.Interfaces
{
    public interface IPersistenceService
    {
        void SaveVehicles(IEnumerable<Vehicle> vehicles, string filePath);

        List<Vehicle> LoadVehicles(string filePath);

        void SaveRentalRecords(IEnumerable<RentalRecord> rentalRecords, string filePath);

        List<RentalRecord> LoadRentalRecords(string filePath);
    }
}