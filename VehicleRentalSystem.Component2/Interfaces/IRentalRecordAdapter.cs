using System;
using System.Collections.Generic;
using VehicleRentalSystem.Models.Models;

namespace VehicleRentalSystem.Component2.Interfaces
{
    public interface IRentalRecordAdapter
    {
        KeyValuePair<string, List<RentalRecord>> AdaptToDictionaryEntry(
            Guid vehicleId,
            int year,
            int month,
            IEnumerable<RentalRecord> rentalRecords);
    }
}
