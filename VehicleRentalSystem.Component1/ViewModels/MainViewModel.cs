using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Data;
using VehicleRentalSystem.Component1.Commands;
using VehicleRentalSystem.Component1.Observers;
using VehicleRentalSystem.Component1.Views;
using VehicleRentalSystem.Models.Enums;
using VehicleRentalSystem.Models.Models;
using VehicleRentalSystem.Services.Commands;
using VehicleRentalSystem.Services.Interfaces;
using VehicleRentalSystem.Services.Repositories;
using VehicleRentalSystem.Services.Services;

namespace VehicleRentalSystem.Component1.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private Vehicle _selectedVehicle;
        private RentalRecord _selectedRentalRecord;
        private string _vehicleSearchText;
        private string _rentalSearchText;
        private readonly JsonPersistenceService _persistenceService;
        private readonly VehicleRepository _vehicleRepository;
        private readonly RentalRecordRepository _rentalRecordRepository;
        private readonly CommandHistoryManager _vehicleCommandHistoryManager;
        private readonly CommandHistoryManager _rentalCommandHistoryManager;
        private readonly StateSimulationService _stateSimulationService;
        private readonly ILoggingService _loggingService;
        private readonly RentalStatisticsSubject _rentalStatisticsSubject;
        private readonly string _vehiclesFilePath;
        private readonly string _rentalRecordsFilePath;

        public ObservableCollection<Vehicle> Vehicles { get; }

        public ObservableCollection<RentalRecord> RentalRecords { get; }
        public StatisticsViewModel Statistics { get; }

        public string VehicleSearchText
        {
            get => _vehicleSearchText;
            set
            {
                if (_vehicleSearchText == value)
                {
                    return;
                }

                _vehicleSearchText = value;
                OnPropertyChanged();
                System.Windows.Data.CollectionViewSource.GetDefaultView(Vehicles).Refresh();
            }
        }

        public string RentalSearchText
        {
            get => _rentalSearchText;
            set
            {
                if (_rentalSearchText == value)
                {
                    return;
                }

                _rentalSearchText = value;
                OnPropertyChanged();
                System.Windows.Data.CollectionViewSource.GetDefaultView(RentalRecords).Refresh();
            }
        }

        public Vehicle SelectedVehicle
        {
            get => _selectedVehicle;
            set
            {
                if (_selectedVehicle == value)
                {
                    return;
                }

                _selectedVehicle = value;
                OnPropertyChanged();
            }
        }

        public RentalRecord SelectedRentalRecord
        {
            get => _selectedRentalRecord;
            set
            {
                if (_selectedRentalRecord == value)
                {
                    return;
                }

                _selectedRentalRecord = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand AddVehicleCommand { get; }
        public RelayCommand EditVehicleCommand { get; }
        public RelayCommand DeleteVehicleCommand { get; }

        public RelayCommand AddRentalRecordCommand { get; }
        public RelayCommand EditRentalRecordCommand { get; }
        public RelayCommand DeleteRentalRecordCommand { get; }
        public RelayCommand CompleteRentalCommand { get; }
        public RelayCommand CancelRentalCommand { get; }
        public RelayCommand MarkOverdueCommand { get; }

        public RelayCommand VehicleUndoCommand { get; }
        public RelayCommand VehicleRedoCommand { get; }
        public RelayCommand RentalUndoCommand { get; }
        public RelayCommand RentalRedoCommand { get; }

        public MainViewModel(VehicleRentalSystem.Services.Interfaces.ILoggingService loggingService)
        {
            _loggingService = loggingService ?? throw new ArgumentNullException(nameof(loggingService));
            Vehicles = new ObservableCollection<Vehicle>();
            RentalRecords = new ObservableCollection<RentalRecord>();

            _persistenceService = new JsonPersistenceService();
            _vehicleRepository = new VehicleRepository();
            _rentalRecordRepository = new RentalRecordRepository();
            _vehicleCommandHistoryManager = new CommandHistoryManager();
            _rentalCommandHistoryManager = new CommandHistoryManager();
            _stateSimulationService = new StateSimulationService();
            string dataDirectory = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
            Directory.CreateDirectory(dataDirectory);
            _vehiclesFilePath = System.IO.Path.Combine(dataDirectory, "vehicles.json");
            _rentalRecordsFilePath = System.IO.Path.Combine(dataDirectory, "rentalRecords.json");

            LoadData();
            CollectionViewSource.GetDefaultView(Vehicles).Filter = FilterVehicle;
            CollectionViewSource.GetDefaultView(RentalRecords).Filter = FilterRentalRecord;
            _rentalStatisticsSubject = new RentalStatisticsSubject();
            Statistics = new StatisticsViewModel(RentalRecords);
            _rentalStatisticsSubject.RegisterObserver(Statistics);

            AddVehicleCommand = new RelayCommand(AddVehicle);
            EditVehicleCommand = new RelayCommand(EditVehicle);
            DeleteVehicleCommand = new RelayCommand(DeleteVehicle);

            AddRentalRecordCommand = new RelayCommand(AddRentalRecord);
            EditRentalRecordCommand = new RelayCommand(EditRentalRecord);
            DeleteRentalRecordCommand = new RelayCommand(DeleteRentalRecord);
            CompleteRentalCommand = new RelayCommand(CompleteRental);
            CancelRentalCommand = new RelayCommand(CancelRental);
            MarkOverdueCommand = new RelayCommand(MarkOverdue);

            VehicleUndoCommand = new RelayCommand(UndoVehicle);
            VehicleRedoCommand = new RelayCommand(RedoVehicle);
            RentalUndoCommand = new RelayCommand(UndoRental);
            RentalRedoCommand = new RelayCommand(RedoRental);
        }

        private bool FilterVehicle(object item)
        {
            if (string.IsNullOrWhiteSpace(VehicleSearchText))
            {
                return true;
            }

            Vehicle vehicle = item as Vehicle;

            if (vehicle == null)
            {
                return false;
            }

            string searchText = VehicleSearchText.Trim();
            return Contains(vehicle.LicensePlate, searchText)
                || Contains(vehicle.Brand, searchText)
                || Contains(vehicle.Model, searchText)
                || Contains(vehicle.ProductionYear.ToString(), searchText)
                || Contains(vehicle.FuelType.ToString(), searchText);
        }

        private bool FilterRentalRecord(object item)
        {
            if (string.IsNullOrWhiteSpace(RentalSearchText))
            {
                return true;
            }

            RentalRecord rentalRecord = item as RentalRecord;

            if (rentalRecord == null)
            {
                return false;
            }

            string searchText = RentalSearchText.Trim();
            return Contains(rentalRecord.VehicleId.ToString(), searchText)
                || Contains(rentalRecord.RentalDate.ToString(), searchText)
                || Contains(rentalRecord.DurationDays.ToString(), searchText)
                || Contains(rentalRecord.TotalCost.ToString(), searchText)
                || Contains(rentalRecord.MileageDriven.ToString(), searchText)
                || Contains(rentalRecord.State.ToString(), searchText);
        }

        private bool Contains(string value, string searchText)
        {
            return value != null
                && value.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private void LoadData()
        {
            var vehicles = _persistenceService.LoadVehicles(_vehiclesFilePath);
            var rentalRecords = _persistenceService.LoadRentalRecords(_rentalRecordsFilePath);

            if (vehicles.Count > 0)
            {
                foreach (Vehicle vehicle in vehicles)
                {
                    _vehicleRepository.Add(vehicle);
                    Vehicles.Add(vehicle);
                }
            }

            if (rentalRecords.Count > 0)
            {
                foreach (RentalRecord rentalRecord in rentalRecords)
                {
                    _rentalRecordRepository.Add(rentalRecord);
                    RentalRecords.Add(rentalRecord);
                }
            }
        }

        private void SaveData()
        {
            _persistenceService.SaveVehicles(Vehicles, _vehiclesFilePath);
            _persistenceService.SaveRentalRecords(RentalRecords, _rentalRecordsFilePath);
        }

        private void RefreshVehicles()
        {
            Vehicles.Clear();

            foreach (Vehicle vehicle in _vehicleRepository.GetAll())
            {
                Vehicles.Add(vehicle);
            }
        }

        private void RefreshRentalRecords()
        {
            RentalRecords.Clear();

            foreach (RentalRecord rentalRecord in _rentalRecordRepository.GetAll())
            {
                RentalRecords.Add(rentalRecord);
            }
        }

        private void AddVehicle()
        {
            VehicleDialog dialog = new VehicleDialog();

            if (dialog.ShowDialog() == true)
            {
                Vehicle vehicle = new Vehicle
                {
                    Id = Guid.NewGuid(),
                    LicensePlate = dialog.LicensePlate,
                    Brand = dialog.Brand,
                    Model = dialog.Model,
                    ProductionYear = dialog.ProductionYear,
                    FuelType = dialog.FuelType
                };

                IUndoableCommand command =
                    new AddVehicleCommand(_vehicleRepository, vehicle);
                _vehicleCommandHistoryManager.ExecuteCommand(command);
                RefreshVehicles();
                SaveData();
                _loggingService.Log(
                    $"Vehicle Added: {vehicle.Brand} {vehicle.Model} ({vehicle.LicensePlate})");
            }
        }

        private void EditVehicle()
        {
            if (SelectedVehicle == null)
            {
                return;
            }

            VehicleDialog dialog = new VehicleDialog
            {
                LicensePlate = SelectedVehicle.LicensePlate,
                Brand = SelectedVehicle.Brand,
                Model = SelectedVehicle.Model,
                ProductionYear = SelectedVehicle.ProductionYear,
                FuelType = SelectedVehicle.FuelType
            };

            if (dialog.ShowDialog() == true)
            {
                Vehicle oldVehicle = new Vehicle
                {
                    Id = SelectedVehicle.Id,
                    LicensePlate = SelectedVehicle.LicensePlate,
                    Brand = SelectedVehicle.Brand,
                    Model = SelectedVehicle.Model,
                    ProductionYear = SelectedVehicle.ProductionYear,
                    FuelType = SelectedVehicle.FuelType
                };

                Vehicle newVehicle = new Vehicle
                {
                    Id = SelectedVehicle.Id,
                    LicensePlate = dialog.LicensePlate,
                    Brand = dialog.Brand,
                    Model = dialog.Model,
                    ProductionYear = dialog.ProductionYear,
                    FuelType = dialog.FuelType
                };

                IUndoableCommand command =
                    new UpdateVehicleCommand(
                        _vehicleRepository,
                        oldVehicle,
                        newVehicle);
                _vehicleCommandHistoryManager.ExecuteCommand(command);
                RefreshVehicles();
                SaveData();
                _loggingService.Log(
                    $"Vehicle Edited: {newVehicle.Brand} {newVehicle.Model} ({newVehicle.LicensePlate})");
            }
        }

        private void DeleteVehicle()
        {
            if (SelectedVehicle == null)
            {
                return;
            }

            Vehicle vehicle = SelectedVehicle;
            IUndoableCommand command =
                new DeleteVehicleCommand(
                    _vehicleRepository,
                    vehicle);
            _vehicleCommandHistoryManager.ExecuteCommand(command);
            RefreshVehicles();
            SaveData();
            _loggingService.Log(
                $"Vehicle Deleted: {vehicle.Brand} {vehicle.Model} ({vehicle.LicensePlate})");
        }

        private void AddRentalRecord()
        {
            RentalRecordDialog dialog = new RentalRecordDialog
            {
                Vehicles = Vehicles
            };

            if (dialog.ShowDialog() == true)
            {
                RentalRecord rentalRecord = new RentalRecord
                {
                    Id = Guid.NewGuid(),
                    VehicleId = dialog.SelectedVehicleId,
                    RentalDate = dialog.RentalDate,
                    DurationDays = dialog.DurationDays,
                    TotalCost = dialog.TotalCost,
                    MileageDriven = dialog.MileageDriven,
                    State = dialog.SelectedState
                };

                IUndoableCommand command =
                    new AddRentalRecordCommand(
                        _rentalRecordRepository,
                        rentalRecord);
                _rentalCommandHistoryManager.ExecuteCommand(command);
                RefreshRentalRecords();
                SaveData();
                _rentalStatisticsSubject.NotifyObservers();
                _loggingService.Log($"RentalRecord Added: Id={rentalRecord.Id}");
            }
        }

        private void EditRentalRecord()
        {
            if (SelectedRentalRecord == null)
            {
                return;
            }

            RentalRecordDialog dialog = new RentalRecordDialog
            {
                Vehicles = Vehicles,
                SelectedVehicleId = SelectedRentalRecord.VehicleId,
                RentalDate = SelectedRentalRecord.RentalDate,
                DurationDays = SelectedRentalRecord.DurationDays,
                TotalCost = SelectedRentalRecord.TotalCost,
                MileageDriven = SelectedRentalRecord.MileageDriven,
                SelectedState = SelectedRentalRecord.State
            };

            if (dialog.ShowDialog() == true)
            {
                RentalRecord oldRentalRecord = new RentalRecord
                {
                    Id = SelectedRentalRecord.Id,
                    VehicleId = SelectedRentalRecord.VehicleId,
                    RentalDate = SelectedRentalRecord.RentalDate,
                    DurationDays = SelectedRentalRecord.DurationDays,
                    TotalCost = SelectedRentalRecord.TotalCost,
                    MileageDriven = SelectedRentalRecord.MileageDriven,
                    State = SelectedRentalRecord.State
                };

                RentalRecord newRentalRecord = new RentalRecord
                {
                    Id = SelectedRentalRecord.Id,
                    VehicleId = dialog.SelectedVehicleId,
                    RentalDate = dialog.RentalDate,
                    DurationDays = dialog.DurationDays,
                    TotalCost = dialog.TotalCost,
                    MileageDriven = dialog.MileageDriven,
                    State = dialog.SelectedState
                };

                IUndoableCommand command =
                    new UpdateRentalRecordCommand(
                        _rentalRecordRepository,
                        oldRentalRecord,
                        newRentalRecord);
                _rentalCommandHistoryManager.ExecuteCommand(command);
                RefreshRentalRecords();
                SaveData();
                _rentalStatisticsSubject.NotifyObservers();
                _loggingService.Log($"RentalRecord Edited: Id={newRentalRecord.Id}");
            }
        }

        private void DeleteRentalRecord()
        {
            if (SelectedRentalRecord == null)
            {
                return;
            }

            RentalRecord rentalRecord = SelectedRentalRecord;
            IUndoableCommand command =
                new DeleteRentalRecordCommand(
                    _rentalRecordRepository,
                    rentalRecord);
            _rentalCommandHistoryManager.ExecuteCommand(command);
            RefreshRentalRecords();
            SaveData();
            _rentalStatisticsSubject.NotifyObservers();
            _loggingService.Log($"RentalRecord Deleted: Id={rentalRecord.Id}");
        }

        private void CompleteRental()
        {
            ApplyRentalTransition(
                rentalRecord => _stateSimulationService.CompleteRental(rentalRecord),
                "Completing this rental is not allowed in its current state.",
                "Rental Completed");
        }

        private void CancelRental()
        {
            ApplyRentalTransition(
                rentalRecord => _stateSimulationService.CancelRental(rentalRecord),
                "Cancelling this rental is not allowed in its current state.",
                "Rental Cancelled");
        }

        private void MarkOverdue()
        {
            ApplyRentalTransition(
                rentalRecord => _stateSimulationService.MarkAsOverdue(rentalRecord),
                "Marking this rental as overdue is not allowed in its current state.",
                "Rental Marked Overdue");
        }

        private void ApplyRentalTransition(
            Action<RentalRecord> transition,
            string invalidTransitionMessage,
            string actionDescription)
        {
            if (SelectedRentalRecord == null)
            {
                System.Windows.MessageBox.Show(
                    "Select a rental record first.",
                    "Rental State",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Information);
                return;
            }

            RentalState previousState = SelectedRentalRecord.State;
            transition(SelectedRentalRecord);

            if (SelectedRentalRecord.State == previousState)
            {
                System.Windows.MessageBox.Show(
                    invalidTransitionMessage,
                    "Transition Not Allowed",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Warning);
                return;
            }

            System.Windows.Data.CollectionViewSource.GetDefaultView(RentalRecords).Refresh();
            SaveData();
            _rentalStatisticsSubject.NotifyObservers();
            _loggingService.Log($"{actionDescription}: Id={SelectedRentalRecord.Id}");
        }

        private void UndoVehicle()
        {
            if (!_vehicleCommandHistoryManager.CanUndo)
            {
                return;
            }

            _vehicleCommandHistoryManager.Undo();
            RefreshVehicles();
            SaveData();
            _loggingService.Log("Vehicle Undo Executed");
        }

        private void RedoVehicle()
        {
            if (!_vehicleCommandHistoryManager.CanRedo)
            {
                return;
            }

            _vehicleCommandHistoryManager.Redo();
            RefreshVehicles();
            SaveData();
            _loggingService.Log("Vehicle Redo Executed");
        }

        private void UndoRental()
        {
            if (!_rentalCommandHistoryManager.CanUndo)
            {
                return;
            }

            _rentalCommandHistoryManager.Undo();
            RefreshRentalRecords();
            SaveData();
            _rentalStatisticsSubject.NotifyObservers();
            _loggingService.Log("Rental Undo Executed");
        }

        private void RedoRental()
        {
            if (!_rentalCommandHistoryManager.CanRedo)
            {
                return;
            }

            _rentalCommandHistoryManager.Redo();
            RefreshRentalRecords();
            SaveData();
            _rentalStatisticsSubject.NotifyObservers();
            _loggingService.Log("Rental Redo Executed");
        }
    }
}
