using System.Collections.Generic;
using VehicleRentalSystem.Component2.Interfaces;
using VehicleRentalSystem.Models.Models;

namespace VehicleRentalSystem.Component2.ViewModels
{
    public class RentalRecordGroupViewModelFactory : IRentalRecordGroupViewModelFactory
    {
        public RentalRecordGroupViewModel Create(IReadOnlyList<RentalRecord> records)
        {
            var firstRecord = records[0];
            var vehicleId = firstRecord.VehicleId.ToString();
            var rentalPeriod = firstRecord.RentalDate.ToString("yyyy-MM");
            return new RentalRecordGroupViewModel(vehicleId, rentalPeriod, records);
        }
    }
}
