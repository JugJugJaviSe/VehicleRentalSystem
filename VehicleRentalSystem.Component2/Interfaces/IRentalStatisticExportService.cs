using VehicleRentalSystem.Component2.Models;

namespace VehicleRentalSystem.Component2.Interfaces
{
    public interface IRentalStatisticExportService
    {
        void Export(string filePath, StatisticExportData data);
    }
}
