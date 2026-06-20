using System;
using System.IO;
using System.Windows;
using VehicleRentalSystem.Component1.Observers;
using VehicleRentalSystem.Component1.ViewModels;
using VehicleRentalSystem.Services.Commands;
using VehicleRentalSystem.Services.Interfaces;
using VehicleRentalSystem.Services.Repositories;
using VehicleRentalSystem.Services.Services;

namespace VehicleRentalSystem.Component1
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            string dataDirectory = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Data");

            Directory.CreateDirectory(dataDirectory);

            string vehiclesFilePath = Path.Combine(dataDirectory, "vehicles.json");
            string rentalRecordsFilePath = Path.Combine(dataDirectory, "rentalRecords.json");
            string logFilePath = Path.Combine(dataDirectory, "activity.log");

            IPersistenceService persistenceService = new JsonPersistenceService();
            ILoggingService loggingService = new LoggingService(logFilePath);

            IVehicleRepository vehicleRepository = new VehicleRepository();
            IRentalRecordRepository rentalRecordRepository = new RentalRecordRepository();

            IStateSimulationService stateSimulationService = new StateSimulationService();
            IRentalSubject rentalStatisticsSubject = new RentalStatisticsSubject();

            CommandHistoryManager vehicleCommandHistoryManager =
                new CommandHistoryManager();

            CommandHistoryManager rentalCommandHistoryManager =
                new CommandHistoryManager();

            MainViewModel mainViewModel = new MainViewModel(
                vehicleRepository,
                rentalRecordRepository,
                persistenceService,
                loggingService,
                stateSimulationService,
                rentalStatisticsSubject,
                vehicleCommandHistoryManager,
                rentalCommandHistoryManager,
                vehiclesFilePath,
                rentalRecordsFilePath);

            MainWindow mainWindow = new MainWindow(mainViewModel);
            mainWindow.Show();
        }
    }
}