using System;
using System.Collections.Generic;
using System.Linq;
using VehicleRentalSystem.Component2.Helpers;
using VehicleRentalSystem.Component2.Interfaces;
using VehicleRentalSystem.Models.Models;

namespace VehicleRentalSystem.Component2.Adapters
{
    public class RentalRecordDictionaryAdapter : IRentalRecordAdapter
    {
        public KeyValuePair<string, List<RentalRecord>> AdaptToDictionaryEntry(
            Guid vehicleId,
            int year,
            int month,
            IEnumerable<RentalRecord> rentalRecords)
        {
            var key = RentalRecordCacheKeyBuilder.Build(vehicleId, year, month);
            var records = rentalRecords
                .Where(record => record.VehicleId == vehicleId
                    && record.RentalDate.Year == year
                    && record.RentalDate.Month == month)
                .ToList();
            return new KeyValuePair<string, List<RentalRecord>>(key, records);
        }
    }
}