using System;
using VehicleRentalSystem.Models.Enums;

namespace VehicleRentalSystem.Models.Models
{
    public class RentalRecord
    {
        public Guid Id { get; set; }
        public Guid VehicleId { get; set; }
        public DateTime RentalDate { get; set; }
        public double DurationDays { get; set; }
        public double TotalCost { get; set; }
        public double MileageDriven { get; set; }
        public RentalState State { get; set; }
    }
}
