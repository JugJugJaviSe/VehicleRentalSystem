using System.Windows;
using VehicleRentalSystem.Component2.Services;
using VehicleRentalSystem.Component2.ViewModels;

namespace VehicleRentalSystem.Component2
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var client = new VehicleRentalClient();
            var vehicleSelection = new VehicleSelectionViewModel(client);
            var mainViewModel = new MainViewModel(vehicleSelection);
            var mainWindow = new MainWindow(mainViewModel);
            mainWindow.Show();
        }
    }
}
