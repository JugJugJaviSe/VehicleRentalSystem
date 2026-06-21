using System.Collections.Generic;
using VehicleRentalSystem.Models.Models;

namespace VehicleRentalSystem.Component2.Interfaces
{
    public interface IRentalRecordsObserver
    {
        void OnRentalRecordsLoaded(IReadOnlyList<RentalRecord> records);
    }
}
