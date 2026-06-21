using System.Collections.Generic;
using System.Linq;
using VehicleRentalSystem.Component2.Interfaces;
using VehicleRentalSystem.Component2.Models;
using VehicleRentalSystem.Models.Enums;
using VehicleRentalSystem.Models.Models;

namespace VehicleRentalSystem.Component2.Strategies
{
    public class OverdueCountStrategy : IRentalStatisticStrategy
    {
        public string Name => "Overdue Count";

        public StatisticResult Calculate(IEnumerable<RentalRecord> records)
        {
            var count = records.Count(r => r.State == RentalState.Overdue);
            return new StatisticResult { Label = "Overdue Count", Value = count.ToString() };
        }
    }
}
