using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.Windows.Input;
using VehicleRentalSystem.Component2.Commands;
using VehicleRentalSystem.Component2.Interfaces;
using VehicleRentalSystem.Component2.Models;
using VehicleRentalSystem.Component2.Services;

namespace VehicleRentalSystem.Component2.ViewModels
{
    public class RentalRecordsViewModel : BaseViewModel
    {
        private readonly IVehicleRentalClient _client;
        private readonly IRentalRecordAdapter _adapter;
        private readonly VehicleSelectionViewModel _vehicleSelection;
        private int _year = DateTime.Now.Year;
        private int _month = DateTime.Now.Month;
        private string _statusMessage;

        public ObservableCollection<AdaptedRentalRecordGroup> AdaptedGroups { get; }
            = new ObservableCollection<AdaptedRentalRecordGroup>();

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
                    _vehicleSelection.SelectedVehicle.Id, Year, Month);

                var dict = _adapter.AdaptToDictionary(records);

                AdaptedGroups.Clear();
                foreach (var kv in dict)
                {
                    var first = kv.Value[0];
                    var recordsText = string.Join("; ", kv.Value.Select(
                        r => $"[duration: {r.DurationDays} days, cost: {r.TotalCost:F2} RSD, mileage: {r.MileageDriven:F2} km, state: {r.State}]"));

                    AdaptedGroups.Add(new AdaptedRentalRecordGroup
                    {
                        Key = kv.Key,
                        VehicleId = first.VehicleId,
                        RentalDateText = first.RentalDate.ToString("yyyy-MM-dd HH:mm"),
                        RecordsText = recordsText,
                        Count = kv.Value.Count
                    });
                }

                StatusMessage = $"Loaded {AdaptedGroups.Count} group(s).";
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
