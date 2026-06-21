using VehicleRentalSystem.Component1.Observers;
using VehicleRentalSystem.Services.Commands;
using VehicleRentalSystem.Services.Interfaces;

namespace VehicleRentalSystem.Component1.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public VehicleManagementViewModel VehicleManagement { get; }
        public RentalManagementViewModel RentalManagement { get; }
        public StatisticsViewModel Statistics { get; }

        public MainViewModel(
            IVehicleRepository vehicleRepository,
            IRentalRecordRepository rentalRecordRepository,
            IPersistenceService persistenceService,
            ILoggingService loggingService,
            IStateSimulationService stateSimulationService,
            IVehicleAvailabilityService vehicleAvailabilityService,
            IRentalSubject rentalStatisticsSubject,
            CommandHistoryManager vehicleCommandHistoryManager,
            CommandHistoryManager rentalCommandHistoryManager,
            string vehiclesFilePath,
            string rentalRecordsFilePath)
        {

            VehicleManagement = new VehicleManagementViewModel(
                vehicleRepository,
                rentalRecordRepository,
                persistenceService,
                loggingService,
                stateSimulationService,
                vehicleCommandHistoryManager,
                vehiclesFilePath);

            RentalManagement = new RentalManagementViewModel(
                rentalRecordRepository,
                persistenceService,
                loggingService,
                stateSimulationService,
                rentalCommandHistoryManager,
                rentalStatisticsSubject,
                vehicleRepository,
                vehicleAvailabilityService,
                VehicleManagement.SaveVehicles,
                rentalRecordsFilePath);

            Statistics = new StatisticsViewModel(RentalManagement.RentalRecords);
            rentalStatisticsSubject.RegisterObserver(Statistics);

            VehicleManagement.ConfigureRentalCallbacks(
                RentalManagement.RefreshRentalRecords,
                RentalManagement.SaveRentalRecords,
                rentalStatisticsSubject.NotifyObservers);
        }
    }
}