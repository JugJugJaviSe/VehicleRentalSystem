using System;
using System.Collections.Generic;
using VehicleRentalSystem.Models.Models;

namespace VehicleRentalSystem.Component2.Interfaces
{
    public interface IRentalRecordLoadService
    {
        KeyValuePair<string, List<RentalRecord>> Load(Guid vehicleId, int year, int month);
    }
}
