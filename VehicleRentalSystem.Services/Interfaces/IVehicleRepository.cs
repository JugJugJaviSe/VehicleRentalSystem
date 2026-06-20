using System;
using System.Collections.Generic;
using VehicleRentalSystem.Models.Models;

namespace VehicleRentalSystem.Services.Interfaces
{
    public interface IVehicleRepository
    {
        IEnumerable<Vehicle> GetAll();
        Vehicle GetById(Guid id);
        void Add(Vehicle vehicle);
        void AddRange(IEnumerable<Vehicle> vehicles);
        void Update(Vehicle vehicle);
        void Delete(Guid id);
    }
}
