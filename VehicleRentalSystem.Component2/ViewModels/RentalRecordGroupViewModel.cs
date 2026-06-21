using System.Collections.Generic;
using VehicleRentalSystem.Models.Models;

namespace VehicleRentalSystem.Component2.ViewModels
{
    public class RentalRecordGroupViewModel
    {
        public RentalRecordGroupViewModel(
            string vehicleId,
            string rentalPeriod,
            IReadOnlyList<RentalRecord> records)
        {
            VehicleId = vehicleId;
            RentalPeriod = rentalPeriod;
            Records = records;
        }

        public string VehicleId { get; }

        public string RentalPeriod { get; }

        public IReadOnlyList<RentalRecord> Records { get; }

        public int RecordCount
        {
            get { return Records.Count; }
        }
    }
}
