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

            IVehicleRentalClient realClient = new VehicleRentalClient();
            IVehicleRentalClient client = new FallbackVehicleRentalClient(realClient);

            VehicleSelectionViewModel vehicleSelection = new VehicleSelectionViewModel(client);

            IRentalRecordAdapter adapter = new RentalRecordDictionaryAdapter();
            IRentalRecordCache cache = new RentalRecordCache();
            IRentalRecordLoadService loadService = new RentalRecordLoadService(client, adapter);

            IRentalRecordGroupViewModelFactory groupFactory = new RentalRecordGroupViewModelFactory();

            List<IRentalStatisticStrategy> strategies = new List<IRentalStatisticStrategy>
            {
                new AverageDurationStrategy(),
                new MaxMileageStrategy(),
                new OverdueCountStrategy()
            };

            IRentalStatisticExportService exportService = new CsvRentalStatisticExportService();
            ISaveFileDialogService saveFileDialogService = new SaveFileDialogService();

            RentalRecordsViewModel rentalRecords = new RentalRecordsViewModel(
                loadService,
                cache,
                groupFactory,
                vehicleSelection);

            RentalStatisticsViewModel rentalStatistics = new RentalStatisticsViewModel(
                strategies,
                exportService,
                saveFileDialogService);

            rentalRecords.Attach(rentalStatistics);

            MainViewModel mainViewModel = new MainViewModel(
                vehicleSelection,
                rentalRecords,
                rentalStatistics);

            MainWindow mainWindow = new MainWindow(mainViewModel);
            mainWindow.Show();
        }
    }
}

