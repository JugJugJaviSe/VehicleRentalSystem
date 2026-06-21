using System.Collections.Generic;
using VehicleRentalSystem.Component2.Models;
using VehicleRentalSystem.Models.Models;

namespace VehicleRentalSystem.Component2.Interfaces
{
    public interface IRentalStatisticStrategy
    {
        string Name { get; }
        StatisticResult Calculate(IEnumerable<RentalRecord> records);
    }
}
