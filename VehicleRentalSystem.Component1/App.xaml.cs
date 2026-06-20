using System;
using System.IO;
using System.ServiceModel;
using System.Windows;
using VehicleRentalSystem.Component1.Observers;
using VehicleRentalSystem.Component1.Services;
using VehicleRentalSystem.Component1.ViewModels;
using VehicleRentalSystem.Services.Commands;
using VehicleRentalSystem.Services.Interfaces;
using VehicleRentalSystem.Services.Repositories;
using VehicleRentalSystem.Services.Services;
using VehicleRentalSystem.WcfService.Contracts;

namespace VehicleRentalSystem.Component1
{
    public partial class App : Application
    {
        private ServiceHost _serviceHost;

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

            var binding = new NetTcpBinding(SecurityMode.None)
            {
                OpenTimeout = TimeSpan.FromSeconds(10),
                CloseTimeout = TimeSpan.FromSeconds(10),
                SendTimeout = TimeSpan.FromSeconds(30),
                ReceiveTimeout = TimeSpan.FromMinutes(10),
                MaxReceivedMessageSize = 1024 * 1024
            };

            var wcfService = new VehicleRentalService(vehicleRepository, rentalRecordRepository);
            _serviceHost = new ServiceHost(wcfService, new Uri("net.tcp://localhost:8734/VehicleRentalService/"));
            _serviceHost.AddServiceEndpoint(typeof(IVehicleRentalService), binding, "");
            _serviceHost.Open();

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

        protected override void OnExit(ExitEventArgs e)
        {
            _serviceHost?.Close();
            base.OnExit(e);
        }
    }
}
