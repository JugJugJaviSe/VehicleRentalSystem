namespace VehicleRentalSystem.Component2.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public VehicleSelectionViewModel VehicleSelection { get; }
        public RentalRecordsViewModel RentalRecords { get; }

        public MainViewModel(VehicleSelectionViewModel vehicleSelection, RentalRecordsViewModel rentalRecords)
        {
            VehicleSelection = vehicleSelection;
            RentalRecords = rentalRecords;
        }
    }
}
