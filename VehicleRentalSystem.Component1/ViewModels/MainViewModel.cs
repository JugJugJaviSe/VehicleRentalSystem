using System;
using System.IO;
using VehicleRentalSystem.Component1.Observers;
using VehicleRentalSystem.Services.Commands;
using VehicleRentalSystem.Services.Interfaces;
using VehicleRentalSystem.Services.Repositories;
using VehicleRentalSystem.Services.Services;

namespace VehicleRentalSystem.Component1.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public VehicleManagementViewModel VehicleManagement { get; }
        public RentalManagementViewModel RentalManagement { get; }
        public StatisticsViewModel Statistics { get; }

        public MainViewModel(ILoggingService loggingService)
        {
            if (loggingService == null)
            {
                throw new ArgumentNullException(nameof(loggingService));
            }

            JsonPersistenceService persistenceService = new JsonPersistenceService();
            string dataDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
            Directory.CreateDirectory(dataDirectory);

            VehicleRepository vehicleRepository = new VehicleRepository();
            RentalRecordRepository rentalRecordRepository = new RentalRecordRepository();

            VehicleManagement = new VehicleManagementViewModel(
                vehicleRepository,
                rentalRecordRepository,
                persistenceService,
                loggingService,
                new CommandHistoryManager(),
                Path.Combine(dataDirectory, "vehicles.json"));

            RentalStatisticsSubject rentalStatisticsSubject = new RentalStatisticsSubject();
            RentalManagement = new RentalManagementViewModel(
                rentalRecordRepository,
                persistenceService,
                loggingService,
                new StateSimulationService(),
                new CommandHistoryManager(),
                rentalStatisticsSubject,
                VehicleManagement.Vehicles,
                VehicleManagement.SaveVehicles,
                VehicleManagement.RefreshVehicles,
                Path.Combine(dataDirectory, "rentalRecords.json"));

            Statistics = new StatisticsViewModel(RentalManagement.RentalRecords);
            rentalStatisticsSubject.RegisterObserver(Statistics);

            VehicleManagement.ConfigureRentalCallbacks(
                RentalManagement.RefreshRentalRecords,
                RentalManagement.SaveRentalRecords,
                rentalStatisticsSubject.NotifyObservers);
        }
    }
}
