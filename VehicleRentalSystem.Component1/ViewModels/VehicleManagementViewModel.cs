using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using VehicleRentalSystem.Component1.Commands;
using VehicleRentalSystem.Component1.Helpers;
using VehicleRentalSystem.Component1.Views;
using VehicleRentalSystem.Models.Enums;
using VehicleRentalSystem.Models.Models;
using VehicleRentalSystem.Services.Commands;
using VehicleRentalSystem.Services.Interfaces;
using VehicleRentalSystem.Services.Services;

namespace VehicleRentalSystem.Component1.ViewModels
{
    public class VehicleManagementViewModel : BaseViewModel
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IRentalRecordRepository _rentalRecordRepository;
        private readonly JsonPersistenceService _persistenceService;
        private readonly ILoggingService _loggingService;
        private readonly CommandHistoryManager _commandHistoryManager;
        private readonly string _vehiclesFilePath;
        private Action _refreshRentalRecords;
        private Action _saveRentalRecords;
        private Action _notifyRentalStatistics;
        private Vehicle _selectedVehicle;
        private string _vehicleSearchText;

        public ObservableCollection<Vehicle> Vehicles { get; }
        public ICollectionView VehiclesView { get; }

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
                VehiclesView.Refresh();
            }
        }

        public RelayCommand AddVehicleCommand { get; }
        public RelayCommand EditVehicleCommand { get; }
        public RelayCommand DeleteVehicleCommand { get; }
        public RelayCommand UndoVehicleCommand { get; }
        public RelayCommand RedoVehicleCommand { get; }

        public VehicleManagementViewModel(
            IVehicleRepository vehicleRepository,
            IRentalRecordRepository rentalRecordRepository,
            JsonPersistenceService persistenceService,
            ILoggingService loggingService,
            CommandHistoryManager commandHistoryManager,
            string vehiclesFilePath)
        {
            _vehicleRepository = vehicleRepository;
            _rentalRecordRepository = rentalRecordRepository;
            _persistenceService = persistenceService;
            _loggingService = loggingService;
            _commandHistoryManager = commandHistoryManager;
            _vehiclesFilePath = vehiclesFilePath;

            Vehicles = new ObservableCollection<Vehicle>();
            LoadVehicles();
            VehiclesView = CollectionViewSource.GetDefaultView(Vehicles);
            VehiclesView.Filter = FilterVehicle;

            AddVehicleCommand = new RelayCommand(AddVehicle);
            EditVehicleCommand = new RelayCommand(EditVehicle);
            DeleteVehicleCommand = new RelayCommand(DeleteVehicle);
            UndoVehicleCommand = new RelayCommand(UndoVehicle);
            RedoVehicleCommand = new RelayCommand(RedoVehicle);
        }

        public void ConfigureRentalCallbacks(
            Action refreshRentalRecords,
            Action saveRentalRecords,
            Action notifyRentalStatistics)
        {
            _refreshRentalRecords = refreshRentalRecords;
            _saveRentalRecords = saveRentalRecords;
            _notifyRentalStatistics = notifyRentalStatistics;
        }

        private void LoadVehicles()
        {
            List<Vehicle> vehicles = _persistenceService.LoadVehicles(_vehiclesFilePath);

            if (vehicles.Count == 0)
            {
                vehicles = SampleDataFactory.CreateVehicles();
                _persistenceService.SaveVehicles(vehicles, _vehiclesFilePath);
            }

            foreach (Vehicle vehicle in vehicles)
            {
                _vehicleRepository.Add(vehicle);
                Vehicles.Add(vehicle);
            }
        }

        public void SaveVehicles()
        {
            _persistenceService.SaveVehicles(Vehicles, _vehiclesFilePath);
        }

        public void RefreshVehicles()
        {
            Vehicles.Clear();

            foreach (Vehicle vehicle in _vehicleRepository.GetAll())
            {
                Vehicles.Add(vehicle);
            }

            VehiclesView.Refresh();
        }

        private bool FilterVehicle(object item)
        {
            return VehicleSearchHelper.Matches(item as Vehicle, VehicleSearchText);
        }

        private void AddVehicle()
        {
            VehicleDialogViewModel viewModel = new VehicleDialogViewModel();
            VehicleDialog dialog = new VehicleDialog(viewModel);

            if (dialog.ShowDialog() == true)
            {
                Vehicle vehicle = new Vehicle
                {
                    Id = Guid.NewGuid(),
                    LicensePlate = viewModel.LicensePlate,
                    Brand = viewModel.Brand,
                    Model = viewModel.Model,
                    ProductionYear = int.Parse(viewModel.ProductionYearText),
                    FuelType = viewModel.SelectedFuelType,
                    IsAvailable = true
                };

                IUndoableCommand command = new AddVehicleCommand(_vehicleRepository, vehicle);
                _commandHistoryManager.ExecuteCommand(command);
                RefreshVehicles();
                SaveVehicles();
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

            VehicleDialogViewModel viewModel = new VehicleDialogViewModel
            {
                LicensePlate = SelectedVehicle.LicensePlate,
                Brand = SelectedVehicle.Brand,
                Model = SelectedVehicle.Model,
                ProductionYearText = SelectedVehicle.ProductionYear.ToString(),
                SelectedFuelType = SelectedVehicle.FuelType
            };
            VehicleDialog dialog = new VehicleDialog(viewModel);

            if (dialog.ShowDialog() == true)
            {
                Vehicle oldVehicle = CopyVehicle(SelectedVehicle);
                Vehicle newVehicle = new Vehicle
                {
                    Id = SelectedVehicle.Id,
                    LicensePlate = viewModel.LicensePlate,
                    Brand = viewModel.Brand,
                    Model = viewModel.Model,
                    ProductionYear = int.Parse(viewModel.ProductionYearText),
                    FuelType = viewModel.SelectedFuelType,
                    IsAvailable = SelectedVehicle.IsAvailable
                };

                IUndoableCommand command =
                    new UpdateVehicleCommand(_vehicleRepository, oldVehicle, newVehicle);
                _commandHistoryManager.ExecuteCommand(command);
                RefreshVehicles();
                SaveVehicles();
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
            List<RentalRecord> relatedRentalRecords = _rentalRecordRepository
                .GetAll()
                .Where(rentalRecord => rentalRecord.VehicleId == vehicle.Id)
                .ToList();

            bool hasUnfinishedRental = relatedRentalRecords.Any(rentalRecord =>
                rentalRecord.State == RentalState.Active
                || rentalRecord.State == RentalState.Overdue);

            if (hasUnfinishedRental)
            {
                MessageBox.Show(
                    "Vehicle cannot be deleted because it has an active or overdue rental.",
                    "Delete Vehicle",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            IUndoableCommand command = new DeleteVehicleCommand(
                _vehicleRepository,
                _rentalRecordRepository,
                vehicle,
                relatedRentalRecords);
            _commandHistoryManager.ExecuteCommand(command);
            RefreshVehicles();
            SaveVehicles();
            RefreshSaveAndNotifyRentals();
            _loggingService.Log(
                $"Vehicle Deleted: {vehicle.Brand} {vehicle.Model} ({vehicle.LicensePlate})");
        }

        private void UndoVehicle()
        {
            if (!_commandHistoryManager.CanUndo)
            {
                return;
            }

            _commandHistoryManager.Undo();
            RefreshVehicles();
            SaveVehicles();
            RefreshSaveAndNotifyRentals();
            _loggingService.Log("Vehicle Undo Executed");
        }

        private void RedoVehicle()
        {
            if (!_commandHistoryManager.CanRedo)
            {
                return;
            }

            _commandHistoryManager.Redo();
            RefreshVehicles();
            SaveVehicles();
            RefreshSaveAndNotifyRentals();
            _loggingService.Log("Vehicle Redo Executed");
        }

        private void RefreshSaveAndNotifyRentals()
        {
            _refreshRentalRecords?.Invoke();
            _saveRentalRecords?.Invoke();
            _notifyRentalStatistics?.Invoke();
        }

        private Vehicle CopyVehicle(Vehicle vehicle)
        {
            return new Vehicle
            {
                Id = vehicle.Id,
                LicensePlate = vehicle.LicensePlate,
                Brand = vehicle.Brand,
                Model = vehicle.Model,
                ProductionYear = vehicle.ProductionYear,
                FuelType = vehicle.FuelType,
                IsAvailable = vehicle.IsAvailable
            };
        }

    }
}
