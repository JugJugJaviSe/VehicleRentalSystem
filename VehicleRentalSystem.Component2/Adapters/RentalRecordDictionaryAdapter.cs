using System.Collections.Generic;
using VehicleRentalSystem.Component2.Interfaces;
using VehicleRentalSystem.Models.Models;

namespace VehicleRentalSystem.Component2.Adapters
{
    public class RentalRecordDictionaryAdapter : IRentalRecordAdapter
    {
        public Dictionary<string, List<RentalRecord>> Adapt(IEnumerable<RentalRecord> rentalRecords)
        {
            var result = new Dictionary<string, List<RentalRecord>>();
            foreach (var record in rentalRecords)
            {
                var key = $"{record.VehicleId}-{record.RentalDate:yyyy-MM-dd HH:mm}";
                if (!result.ContainsKey(key))
                    result[key] = new List<RentalRecord>();
                result[key].Add(record);
            }
            return result;
        }
    }
}
