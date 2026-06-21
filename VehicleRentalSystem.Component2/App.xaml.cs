using System.Collections.Generic;
using System.Windows;
using VehicleRentalSystem.Component2.Adapters;
using VehicleRentalSystem.Component2.Interfaces;
using VehicleRentalSystem.Component2.Services;
using VehicleRentalSystem.Component2.Strategies;
using VehicleRentalSystem.Component2.ViewModels;

namespace VehicleRentalSystem.Component2
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var realClient = new VehicleRentalClient();
            var client = new FallbackVehicleRentalClient(realClient);
            var vehicleSelection = new VehicleSelectionViewModel(client);
            var adapter = new RentalRecordDictionaryAdapter();
            var cache = new RentalRecordCache();
            var loadService = new RentalRecordLoadService(client, adapter);
            var groupFactory = new RentalRecordGroupViewModelFactory();
            var strategies = new List<IRentalStatisticStrategy>
            {
                new AverageDurationStrategy(),
                new MaxMileageStrategy(),
                new OverdueCountStrategy()
            };
            var exportService = new CsvRentalStatisticExportService();
            var saveFileDialogService = new SaveFileDialogService();

            var rentalRecords = new RentalRecordsViewModel(loadService, cache, groupFactory, vehicleSelection);
            var rentalStatistics = new RentalStatisticsViewModel(strategies, exportService, saveFileDialogService);

            rentalRecords.Attach(rentalStatistics);

            var mainViewModel = new MainViewModel(vehicleSelection, rentalRecords, rentalStatistics);
            var mainWindow = new MainWindow(mainViewModel);
            mainWindow.Show();
        }
    }
}
