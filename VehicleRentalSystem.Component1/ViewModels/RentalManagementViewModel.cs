using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using VehicleRentalSystem.Component1.Commands;
using VehicleRentalSystem.Component1.Helpers;
using VehicleRentalSystem.Component1.Observers;
using VehicleRentalSystem.Component1.Views;
using VehicleRentalSystem.Models.Enums;
using VehicleRentalSystem.Models.Models;
using VehicleRentalSystem.Services.Commands;
using VehicleRentalSystem.Services.Interfaces;
using VehicleRentalSystem.Component1.Mappers;

namespace VehicleRentalSystem.Component1.ViewModels
{
    public class RentalManagementViewModel : BaseViewModel
    {
        private readonly IRentalRecordRepository _rentalRecordRepository;
        private readonly IPersistenceService _persistenceService;
        private readonly ILoggingService _loggingService;
        private readonly IStateSimulationService _stateSimulationService;
        private readonly CommandHistoryManager _commandHistoryManager;
        private readonly IRentalSubject _rentalStatisticsSubject;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly Action _saveVehicles;
        private readonly Action _refreshVehicles;
        private readonly string _rentalRecordsFilePath;
        private RentalRecord _selectedRentalRecord;
        private string _rentalSearchText;

        public ObservableCollection<RentalRecord> RentalRecords { get; }
        public ICollectionView RentalRecordsView { get; }

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
                RentalRecordsView?.Refresh();
            }
        }

        public RelayCommand AddRentalRecordCommand { get; }
        public RelayCommand EditRentalRecordCommand { get; }
        public RelayCommand DeleteRentalRecordCommand { get; }
        public RelayCommand UndoRentalCommand { get; }
        public RelayCommand RedoRentalCommand { get; }

        public RentalManagementViewModel(
            IRentalRecordRepository rentalRecordRepository,
            IPersistenceService persistenceService,
            ILoggingService loggingService,
            IStateSimulationService stateSimulationService,
            CommandHistoryManager commandHistoryManager,
            IRentalSubject rentalStatisticsSubject,
            IVehicleRepository vehicleRepository,
            Action saveVehicles,
            Action refreshVehicles,
            string rentalRecordsFilePath)
        {
            _rentalRecordRepository = rentalRecordRepository;
            _persistenceService = persistenceService;
            _loggingService = loggingService;
            _stateSimulationService = stateSimulationService;
            _commandHistoryManager = commandHistoryManager;
            _rentalStatisticsSubject = rentalStatisticsSubject;
            _vehicleRepository = vehicleRepository;
            _saveVehicles = saveVehicles;
            _refreshVehicles = refreshVehicles;
            _rentalRecordsFilePath = rentalRecordsFilePath;

            RentalRecords = new ObservableCollection<RentalRecord>();
            RentalRecordsView = CollectionViewSource.GetDefaultView(RentalRecords);
            RentalRecordsView.Filter = FilterRentalRecord;

            LoadRentalRecords();
            SynchronizeVehicleAvailability();

            AddRentalRecordCommand = new RelayCommand(AddRentalRecord);
            EditRentalRecordCommand = new RelayCommand(EditRentalRecord);
            DeleteRentalRecordCommand = new RelayCommand(DeleteRentalRecord);
            UndoRentalCommand = new RelayCommand(UndoRental);
            RedoRentalCommand = new RelayCommand(RedoRental);
        }

        private void LoadRentalRecords()
        {
            List<RentalRecord> rentalRecords =
                _persistenceService.LoadRentalRecords(_rentalRecordsFilePath);

            if (rentalRecords.Count == 0)
            {
                rentalRecords = SampleDataFactory.CreateRentalRecords();
                _persistenceService.SaveRentalRecords(rentalRecords, _rentalRecordsFilePath);
            }

            _rentalRecordRepository.AddRange(rentalRecords);
            RefreshRentalRecords();
        }

        public void SaveRentalRecords()
        {
            _persistenceService.SaveRentalRecords(_rentalRecordRepository.GetAll(), _rentalRecordsFilePath);
        }

        public void RefreshRentalRecords()
        {
            RentalRecords.Clear();

            foreach (RentalRecord rentalRecord in _rentalRecordRepository.GetAll())
            {
                RentalRecords.Add(rentalRecord);
            }

            RentalRecordsView?.Refresh();
        }

        private bool FilterRentalRecord(object item)
        {
            return RentalRecordSearchHelper.Matches(item as RentalRecord, RentalSearchText);
        }

        private void AddRentalRecord()
        {
            List<Vehicle> availableVehicles = _vehicleRepository.GetAll().Where(vehicle => vehicle.IsAvailable).ToList();

            if (!availableVehicles.Any())
            {
                MessageBox.Show(
                    "There are no available vehicles for rental.",
                    "Rental Record",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
                return;
            }

            RentalRecordDialogViewModel viewModel =
                new RentalRecordDialogViewModel(availableVehicles, false);
            RentalRecordDialog dialog = new RentalRecordDialog(viewModel);

            if (dialog.ShowDialog() == true)
            {
                RentalRecord rentalRecord = RentalRecordMapper.CreateFromDialog(viewModel);
                IUndoableCommand command =
                    new AddRentalRecordCommand(_rentalRecordRepository, rentalRecord);
                _commandHistoryManager.ExecuteCommand(command);
                RefreshRentalRecords();
                SaveRentalRecords();
                SynchronizeVehicleAvailability();
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

            RentalRecordDialogViewModel viewModel = new RentalRecordDialogViewModel(_vehicleRepository.GetAll(), true)
            {
                RentalDate = SelectedRentalRecord.RentalDate.Date,
                RentalTimeText = SelectedRentalRecord.RentalDate.ToString("HH:mm"),
                DurationDaysText = SelectedRentalRecord.DurationDays.ToString(),
                TotalCostText = SelectedRentalRecord.TotalCost.ToString(),
                MileageDrivenText = SelectedRentalRecord.MileageDriven.ToString(),
                SelectedState = SelectedRentalRecord.State
            };
            viewModel.SelectedVehicle = _vehicleRepository.GetAll().FirstOrDefault(vehicle => vehicle.Id == SelectedRentalRecord.VehicleId);
            RentalRecordDialog dialog = new RentalRecordDialog(viewModel);

            if (dialog.ShowDialog() == true)
            {
                RentalRecord oldRentalRecord = RentalRecordMapper.Copy(SelectedRentalRecord);
                RentalState oldState = SelectedRentalRecord.State;
                RentalState requestedState = viewModel.SelectedState;
                RentalState nextState = _stateSimulationService.GetNextState(oldState, requestedState);

                if (nextState != requestedState)
                {
                    ShowInvalidTransitionMessage();
                    return;
                }

                RentalRecord newRentalRecord = RentalRecordMapper.CreateUpdatedRentalRecord(SelectedRentalRecord, viewModel, nextState);

                IUndoableCommand command =
                    new UpdateRentalRecordCommand(
                        _rentalRecordRepository,
                        oldRentalRecord,
                        newRentalRecord);
                _commandHistoryManager.ExecuteCommand(command);
                RefreshRentalRecords();
                SaveRentalRecords();
                SynchronizeVehicleAvailability();
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
                new DeleteRentalRecordCommand(_rentalRecordRepository, rentalRecord);
            _commandHistoryManager.ExecuteCommand(command);
            RefreshRentalRecords();
            SaveRentalRecords();
            SynchronizeVehicleAvailability();
            _rentalStatisticsSubject.NotifyObservers();
            _loggingService.Log($"RentalRecord Deleted: Id={rentalRecord.Id}");
        }

        private void ShowInvalidTransitionMessage()
        {
            MessageBox.Show(
                "This rental state transition is not allowed.",
                "Transition Not Allowed",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
        }

        private void UndoRental()
        {
            if (!_commandHistoryManager.CanUndo)
            {
                return;
            }

            _commandHistoryManager.Undo();
            RefreshRentalRecords();
            SaveRentalRecords();
            SynchronizeVehicleAvailability();
            _rentalStatisticsSubject.NotifyObservers();
            _loggingService.Log("Rental Undo Executed");
        }

        private void RedoRental()
        {
            if (!_commandHistoryManager.CanRedo)
            {
                return;
            }

            _commandHistoryManager.Redo();
            RefreshRentalRecords();
            SaveRentalRecords();
            SynchronizeVehicleAvailability();
            _rentalStatisticsSubject.NotifyObservers();
            _loggingService.Log("Rental Redo Executed");
        }

        private void SynchronizeVehicleAvailability()
        {
            foreach (Vehicle vehicle in _vehicleRepository.GetAll())
            {
                vehicle.IsAvailable = !_rentalRecordRepository
                    .GetAll()
                    .Any(rentalRecord =>
                        rentalRecord.VehicleId == vehicle.Id
                        && (rentalRecord.State == RentalState.Active
                            || rentalRecord.State == RentalState.Overdue));
            }

            _refreshVehicles?.Invoke();
            _saveVehicles?.Invoke();
        }
    }
}
