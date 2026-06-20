using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.Windows.Input;
using VehicleRentalSystem.Component2.Commands;
using VehicleRentalSystem.Component2.Services;
using VehicleRentalSystem.Models.Models;

namespace VehicleRentalSystem.Component2.ViewModels
{
    public class VehicleSelectionViewModel : BaseViewModel
    {
        private readonly IVehicleRentalClient _client;
        private Vehicle _selectedVehicle;
        private string _statusMessage;

        public ObservableCollection<Vehicle> Vehicles { get; } = new ObservableCollection<Vehicle>();

        public Vehicle SelectedVehicle
        {
            get => _selectedVehicle;
            set { _selectedVehicle = value; OnPropertyChanged(); }
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set { _statusMessage = value; OnPropertyChanged(); }
        }

        public ICommand LoadVehiclesCommand { get; }

        public VehicleSelectionViewModel(IVehicleRentalClient client)
        {
            _client = client;
            LoadVehiclesCommand = new RelayCommand(_ => LoadVehicles());
        }

        private void LoadVehicles()
        {
            try
            {
                var vehicles = _client.GetAllVehicles();
                Vehicles.Clear();
                foreach (var v in vehicles)
                    Vehicles.Add(v);
                StatusMessage = $"Loaded {vehicles.Count} vehicle(s).";
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
