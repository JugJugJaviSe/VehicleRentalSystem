using System;
using System.Collections.Generic;
using VehicleRentalSystem.Models.Models;
using VehicleRentalSystem.Services.Interfaces;

namespace VehicleRentalSystem.Services.Repositories
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly List<Vehicle> _vehicles;

        public VehicleRepository()
        {
            _vehicles = new List<Vehicle>();
        }

        public IEnumerable<Vehicle> GetAll()
        {
            return _vehicles;
        }

        public Vehicle GetById(Guid id)
        {
            return _vehicles.Find(vehicle => vehicle.Id == id);
        }

        public void Add(Vehicle vehicle)
        {
            _vehicles.Add(vehicle);
        }

        public void AddRange(IEnumerable<Vehicle> vehicles)
        {
            _vehicles.AddRange(vehicles);
        }

        public void Update(Vehicle vehicle)
        {
            int index = _vehicles.FindIndex(existingVehicle => existingVehicle.Id == vehicle.Id);

            if (index >= 0)
            {
                _vehicles[index] = vehicle;
            }
        }

        public void Delete(Guid id)
        {
            Vehicle vehicle = GetById(id);

            if (vehicle != null)
            {
                _vehicles.Remove(vehicle);
            }
        }
    }
}
