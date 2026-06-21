using System.Collections.Generic;
using VehicleRentalSystem.Models.Models;

namespace VehicleRentalSystem.Component2.Interfaces
{
    public interface IRentalRecordAdapter
    {
        Dictionary<string, List<RentalRecord>> AdaptToDictionary(IEnumerable<RentalRecord> rentalRecords);
    }
}
