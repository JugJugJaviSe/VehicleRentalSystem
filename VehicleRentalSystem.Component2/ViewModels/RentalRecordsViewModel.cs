using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using VehicleRentalSystem.Component2.Commands;
using VehicleRentalSystem.Component2.Interfaces;
using VehicleRentalSystem.Models.Models;

namespace VehicleRentalSystem.Component2.ViewModels
{
    public class RentalRecordsViewModel : BaseViewModel, IRentalRecordsSubject
    {
        private readonly IRentalRecordLoadService _loadService;
        private readonly IRentalRecordCache _cache;
        private readonly IRentalRecordGroupViewModelFactory _groupFactory;
        private readonly VehicleSelectionViewModel _vehicleSelection;
        private readonly List<IRentalRecordsObserver> _observers = new List<IRentalRecordsObserver>();
        private readonly RelayCommand _loadRentalRecordsCommand;
        private readonly RelayCommand _clearRentalRecordsCommand;
        private string _yearInputText = DateTime.Now.Year.ToString();
        private string _monthInputText = DateTime.Now.Month.ToString();
        private string _statusMessage;

        public ObservableCollection<RentalRecordGroupViewModel> DisplayedGroups { get; }
            = new ObservableCollection<RentalRecordGroupViewModel>();

        public string YearInputText
        {
            get => _yearInputText;
            set { _yearInputText = value; OnPropertyChanged(); OnInputChanged(); }
        }

        public string MonthInputText
        {
            get => _monthInputText;
            set { _monthInputText = value; OnPropertyChanged(); OnInputChanged(); }
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set { _statusMessage = value; OnPropertyChanged(); }
        }

        public ICommand LoadRentalRecordsCommand => _loadRentalRecordsCommand;

        public ICommand ClearRentalRecordsCommand => _clearRentalRecordsCommand;

        public RentalRecordsViewModel(
            IRentalRecordLoadService loadService,
            IRentalRecordCache cache,
            IRentalRecordGroupViewModelFactory groupFactory,
            VehicleSelectionViewModel vehicleSelection)
        {
            _loadService = loadService;
            _cache = cache;
            _groupFactory = groupFactory;
            _vehicleSelection = vehicleSelection;
            _loadRentalRecordsCommand = new RelayCommand(_ => LoadRentalRecords(), _ => CanLoadRentalRecords());
            _clearRentalRecordsCommand = new RelayCommand(_ => ClearRentalRecords(), _ => CanClearRentalRecords());
            _vehicleSelection.PropertyChanged += OnVehicleSelectionChanged;
        }

        private void OnVehicleSelectionChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(VehicleSelectionViewModel.SelectedVehicle))
                _loadRentalRecordsCommand.RaiseCanExecuteChanged();
        }

        public void Attach(IRentalRecordsObserver observer)
        {
            _observers.Add(observer);
        }

        public void Detach(IRentalRecordsObserver observer)
        {
            _observers.Remove(observer);
        }

        public void NotifyObservers()
        {
            var allRecords = _cache.GetAllRecords();
            foreach (var observer in _observers)
                observer.OnRentalRecordsLoaded(allRecords);
        }

        private void LoadRentalRecords()
        {
            if (_vehicleSelection.SelectedVehicle == null)
            {
                StatusMessage = "No vehicle selected.";
                return;
            }

            if (!TryParseInputs(out var year, out var month, out var error))
            {
                StatusMessage = error;
                return;
            }

            try
            {
                var vehicleId = _vehicleSelection.SelectedVehicle.Id;

                var entry = _loadService.Load(vehicleId, year, month);

                if (entry.Value.Count == 0)
                {
                    _cache.Remove(vehicleId, year, month);
                    RefreshDisplayedGroups();
                    NotifyObservers();
                    StatusMessage = "No rental records found for selected vehicle and month.";
                    return;
                }

                _cache.AddOrUpdate(entry.Key, entry.Value);
                RefreshDisplayedGroups();
                NotifyObservers();

                StatusMessage = $"Loaded {entry.Value.Count} record(s).";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error: {ex.Message}";
            }
        }

        private void ClearRentalRecords()
        {
            _cache.Clear();
            RefreshDisplayedGroups();
            NotifyObservers();
            StatusMessage = "Displayed rental records cleared.";
        }

        private bool CanLoadRentalRecords()
        {
            return _vehicleSelection.SelectedVehicle != null
                && TryParseInputs(out _, out _, out _);
        }

        private bool CanClearRentalRecords()
        {
            return DisplayedGroups.Count > 0;
        }

        private void OnInputChanged()
        {
            StatusMessage = TryParseInputs(out _, out _, out var error) ? string.Empty : error;
            _loadRentalRecordsCommand.RaiseCanExecuteChanged();
        }

        private bool TryParseInputs(out int year, out int month, out string error)
        {
            year = 0;
            month = 0;
            error = null;

            if (string.IsNullOrWhiteSpace(YearInputText) || string.IsNullOrWhiteSpace(MonthInputText))
            {
                error = "Year and month are required.";
                return false;
            }

            if (!int.TryParse(MonthInputText, out month))
            {
                error = "Month must be a number.";
                return false;
            }

            if (!int.TryParse(YearInputText, out year))
            {
                error = "Year must be a number.";
                return false;
            }

            if (month < 1 || month > 12)
            {
                error = "Month must be between 1 and 12.";
                return false;
            }

            if (year < 2000 || year > DateTime.Now.Year + 1)
            {
                error = "Year must be between 2000 and next year.";
                return false;
            }

            return true;
        }

        private void RefreshDisplayedGroups()
        {
            DisplayedGroups.Clear();
            foreach (var group in _cache.GetGroups())
            {
                DisplayedGroups.Add(_groupFactory.Create(group));
            }

            _clearRentalRecordsCommand.RaiseCanExecuteChanged();
        }
    }
}
