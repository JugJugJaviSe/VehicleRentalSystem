namespace VehicleRentalSystem.Component2.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public VehicleSelectionViewModel VehicleSelection { get; }
        public RentalRecordsViewModel RentalRecords { get; }
        public RentalStatisticsViewModel RentalStatistics { get; }

        public MainViewModel(
            VehicleSelectionViewModel vehicleSelection,
            RentalRecordsViewModel rentalRecords,
            RentalStatisticsViewModel rentalStatistics)
        {
            VehicleSelection = vehicleSelection;
            RentalRecords = rentalRecords;
            RentalStatistics = rentalStatistics;
        }
    }
}
