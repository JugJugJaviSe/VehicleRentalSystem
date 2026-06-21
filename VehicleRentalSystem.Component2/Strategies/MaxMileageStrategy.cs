using System.Collections.Generic;
using System.Linq;
using VehicleRentalSystem.Component2.Interfaces;
using VehicleRentalSystem.Component2.Models;
using VehicleRentalSystem.Models.Models;

namespace VehicleRentalSystem.Component2.Strategies
{
    public class MaxMileageStrategy : IRentalStatisticStrategy
    {
        public string Name => "Max Mileage";

        public StatisticResult Calculate(IEnumerable<RentalRecord> records)
        {
            var list = records.ToList();
            var max = list.Count > 0 ? list.Max(r => r.MileageDriven) : 0;
            return new StatisticResult { Label = "Max Mileage", Value = $"{max:F2} km" };
        }
    }
}
