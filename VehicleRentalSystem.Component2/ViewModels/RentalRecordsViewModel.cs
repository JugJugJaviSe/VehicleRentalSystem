using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.Windows.Input;
using VehicleRentalSystem.Component2.Commands;
using VehicleRentalSystem.Component2.Interfaces;
using VehicleRentalSystem.Component2.Services;
using VehicleRentalSystem.Models.Models;

namespace VehicleRentalSystem.Component2.ViewModels
{
    public class RentalRecordsViewModel : BaseViewModel, IRentalRecordsSubject
    {
        private readonly IVehicleRentalClient _client;
        private readonly IRentalRecordAdapter _adapter;
        private readonly VehicleSelectionViewModel _vehicleSelection;
        private readonly List<IRentalRecordsObserver> _observers = new List<IRentalRecordsObserver>();
        private IReadOnlyList<RentalRecord> _lastLoadedRecords = new List<RentalRecord>();
        private int _year = DateTime.Now.Year;
        private int _month = DateTime.Now.Month;
        private string _statusMessage;
        private string _resultVehicleId;
        private string _resultRentalPeriod;
        private int _resultRecordCount;

        public ObservableCollection<RentalRecord> DisplayedRecords { get; }
            = new ObservableCollection<RentalRecord>();

        public IReadOnlyList<RentalRecord> LastLoadedRecords => _lastLoadedRecords;

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

        public string ResultVehicleId
        {
            get => _resultVehicleId;
            set { _resultVehicleId = value; OnPropertyChanged(); }
        }

        public string ResultRentalPeriod
        {
            get => _resultRentalPeriod;
            set { _resultRentalPeriod = value; OnPropertyChanged(); }
        }

        public int ResultRecordCount
        {
            get => _resultRecordCount;
            set { _resultRecordCount = value; OnPropertyChanged(); }
        }

        public ICommand LoadRentalRecordsCommand { get; }

        public RentalRecordsViewModel(
            IVehicleRentalClient client,
            IRentalRecordAdapter adapter,
            VehicleSelectionViewModel vehicleSelection)
        {
            _client = client;
            _adapter = adapter;
            _vehicleSelection = vehicleSelection;
            LoadRentalRecordsCommand = new RelayCommand(_ => LoadRentalRecords());
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
            foreach (var observer in _observers)
                observer.OnRentalRecordsLoaded(_lastLoadedRecords);
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
                var records = _client.GetRentalRecordsByVehicleAndMonth(
                    _vehicleSelection.SelectedVehicle.Id, Year, Month).ToList();

                _lastLoadedRecords = records;

                var dict = _adapter.AdaptToDictionary(records);

                ResultVehicleId = _vehicleSelection.SelectedVehicle.Id.ToString();
                ResultRentalPeriod = $"{Year:D4}-{Month:D2}";

                DisplayedRecords.Clear();

                var entry = dict.Values.FirstOrDefault();
                if (entry != null)
                {
                    foreach (var record in entry)
                    {
                        DisplayedRecords.Add(record);
                    }
                }

                ResultRecordCount = DisplayedRecords.Count;
                StatusMessage = $"Loaded {ResultRecordCount} record(s).";
                NotifyObservers();
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
    }
}
