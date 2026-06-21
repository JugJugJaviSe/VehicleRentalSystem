using System.IO;
using System.Text;
using VehicleRentalSystem.Component2.Interfaces;
using VehicleRentalSystem.Component2.Models;

namespace VehicleRentalSystem.Component2.Services
{
    public class CsvRentalStatisticExportService : IRentalStatisticExportService
    {
        public void Export(string filePath, StatisticExportData data)
        {
            var builder = new StringBuilder();
            builder.AppendLine("StatisticName,Value,CalculatedAt,RecordsCount");
            builder.AppendLine(string.Join(",",
                Escape(data.StatisticName),
                Escape(data.Value),
                Escape(data.CalculatedAt.ToString("yyyy-MM-dd HH:mm:ss")),
                Escape(data.RecordsCount.ToString())));

            File.WriteAllText(filePath, builder.ToString(), Encoding.UTF8);
        }

        private static string Escape(string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            if (value.Contains(",") || value.Contains("\"") || value.Contains("\n") || value.Contains("\r"))
                return "\"" + value.Replace("\"", "\"\"") + "\"";

            return value;
        }
    }
}
