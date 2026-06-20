namespace VehicleRentalSystem.Component2.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public VehicleSelectionViewModel VehicleSelection { get; }

        public MainViewModel(VehicleSelectionViewModel vehicleSelection)
        {
            VehicleSelection = vehicleSelection;
        }
    }
}
