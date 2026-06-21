using System;

namespace VehicleRentalSystem.Component2.Models
{
    public class StatisticExportData
    {
        public string StatisticName { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public DateTime CalculatedAt { get; set; }
        public int RecordsCount { get; set; }
    }
}
