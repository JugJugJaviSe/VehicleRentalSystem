using System;
using System.Runtime.Serialization;
using VehicleRentalSystem.Models.Enums;

namespace VehicleRentalSystem.Models.Models
{
    [DataContract]
    public class Vehicle
    {
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public string LicensePlate { get; set; }
        [DataMember]
        public string Brand { get; set; }
        [DataMember]
        public string Model { get; set; }
        [DataMember]
        public int ProductionYear { get; set; }
        [DataMember]
        public FuelType FuelType { get; set; }
        [DataMember]
        public bool IsAvailable { get; set; } = true;
    }
}
