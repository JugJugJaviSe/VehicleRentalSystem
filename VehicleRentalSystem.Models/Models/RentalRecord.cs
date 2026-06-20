using System;
using System.Runtime.Serialization;
using VehicleRentalSystem.Models.Enums;

namespace VehicleRentalSystem.Models.Models
{
    [DataContract]
    public class RentalRecord
    {
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public Guid VehicleId { get; set; }
        [DataMember]
        public DateTime RentalDate { get; set; }
        [DataMember]
        public double DurationDays { get; set; }
        [DataMember]
        public double TotalCost { get; set; }
        [DataMember]
        public double MileageDriven { get; set; }
        [DataMember]
        public RentalState State { get; set; }
    }
}
