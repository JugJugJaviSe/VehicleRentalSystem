using System;
using VehicleRentalSystem.Models.Enums;

namespace VehicleRentalSystem.Models.Models
{
    public class Vehicle
    {
        public Guid Id { get; set; }
        public string LicensePlate { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int ProductionYear { get; set; }
        public FuelType FuelType { get; set; }
        public bool IsAvailable { get; set; } = true;
    }
}
