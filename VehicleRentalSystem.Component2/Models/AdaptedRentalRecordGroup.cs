using System;

namespace VehicleRentalSystem.Component2.Models
{
    public class AdaptedRentalRecordGroup
    {
        public string Key { get; set; }
        public Guid VehicleId { get; set; }
        public string RentalDateText { get; set; }
        public string RecordsText { get; set; }
        public int Count { get; set; }
    }
}
