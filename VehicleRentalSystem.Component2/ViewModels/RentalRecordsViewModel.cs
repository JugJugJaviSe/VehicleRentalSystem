using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ServiceModel;
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
        private int _year = DateTime.Now.Year;
        private int _month = DateTime.Now.Month;
        private string _statusMessage;

        public ObservableCollection<RentalRecordGroupViewModel> DisplayedGroups { get; }
            = new ObservableCollection<RentalRecordGroupViewModel>();

        public int Year
        {
            get => _year;
            set { _year = value; OnPropertyChanged(); }
        }

        public int Month
        {
            get => _month;
            set { _month = value; OnPropertyChanged(); }
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set { _statusMessage = value; OnPropertyChanged(); }
        }

        public ICommand LoadRentalRecordsCommand { get; }

        public ICommand ClearRentalRecordsCommand { get; }

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
            LoadRentalRecordsCommand = new RelayCommand(_ => LoadRentalRecords());
            ClearRentalRecordsCommand = new RelayCommand(_ => ClearRentalRecords());
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

            try
            {
                var vehicleId = _vehicleSelection.SelectedVehicle.Id;

                var entry = _loadService.Load(vehicleId, Year, Month);

                if (entry.Value.Count == 0)
                {
                    _cache.Remove(vehicleId, Year, Month);
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
            catch (CommunicationException ex)
            {
                StatusMessage = $"Service unavailable: {ex.Message}";
            }
            catch (TimeoutException)
            {
                StatusMessage = "Connection timed out.";
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

        private void RefreshDisplayedGroups()
        {
            DisplayedGroups.Clear();
            foreach (var group in _cache.GetGroups())
            {
                DisplayedGroups.Add(_groupFactory.Create(group));
            }
        }
    }
}
