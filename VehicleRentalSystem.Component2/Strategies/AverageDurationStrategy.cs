using System.Collections.Generic;
using System.Linq;
using VehicleRentalSystem.Component2.Interfaces;
using VehicleRentalSystem.Component2.Models;
using VehicleRentalSystem.Models.Models;

namespace VehicleRentalSystem.Component2.Strategies
{
    public class AverageDurationStrategy : IRentalStatisticStrategy
    {
        public string Name => "Average Duration";

        public StatisticResult Calculate(IEnumerable<RentalRecord> records)
        {
            var list = records.ToList();
            var avg = list.Count > 0 ? list.Average(r => r.DurationDays) : 0;
            return new StatisticResult { Label = "Average Duration", Value = $"{avg:F2} days" };
        }
    }
}
