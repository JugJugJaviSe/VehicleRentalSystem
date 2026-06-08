using System;
using System.Collections.Generic;
using System.Linq;
using VehicleRentalSystem.Component1.Commands;
using VehicleRentalSystem.Component1.Validators;
using VehicleRentalSystem.Models.Enums;
using VehicleRentalSystem.Models.Models;

namespace VehicleRentalSystem.Component1.ViewModels
{
    public class RentalRecordDialogViewModel : BaseViewModel
    {
        private Vehicle _selectedVehicle;
        private DateTime? _rentalDate;
        private string _rentalTimeText;
        private string _durationDaysText;
        private string _totalCostText;
        private string _mileageDrivenText;
        private RentalState _selectedState;
        private string _validationMessage;
        private bool _wasSaved;
        private readonly bool _isEditMode;

        public bool IsEditMode => _isEditMode;
        public bool IsAddMode => !_isEditMode;
        public bool CanChangeVehicle => !IsEditMode;

        public event Action<bool?> RequestClose;

        public IEnumerable<Vehicle> Vehicles { get; }

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
                OnPropertyChanged(nameof(SelectedVehicleId));
            }
        }

        public Guid SelectedVehicleId =>
            SelectedVehicle != null ? SelectedVehicle.Id : Guid.Empty;

        public DateTime? RentalDate
        {
            get => _rentalDate;
            set
            {
                if (_rentalDate == value)
                {
                    return;
                }

                _rentalDate = value;
                OnPropertyChanged();
            }
        }

        public string RentalTimeText
        {
            get => _rentalTimeText;
            set
            {
                if (_rentalTimeText == value)
                {
                    return;
                }

                _rentalTimeText = value;
                OnPropertyChanged();
            }
        }

        public DateTime RentalDateTime { get; private set; }

        public string DurationDaysText
        {
            get => _durationDaysText;
            set
            {
                if (_durationDaysText == value)
                {
                    return;
                }

                _durationDaysText = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DurationDays));
            }
        }

        public string TotalCostText
        {
            get => _totalCostText;
            set
            {
                if (_totalCostText == value)
                {
                    return;
                }

                _totalCostText = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TotalCost));
            }
        }

        public string MileageDrivenText
        {
            get => _mileageDrivenText;
            set
            {
                if (_mileageDrivenText == value)
                {
                    return;
                }

                _mileageDrivenText = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(MileageDriven));
            }
        }

        public double DurationDays => double.Parse(DurationDaysText);
        public double TotalCost => double.Parse(TotalCostText);
        public double MileageDriven => double.Parse(MileageDrivenText);

        public RentalState SelectedState
        {
            get => _selectedState;
            set
            {
                if (_selectedState == value)
                {
                    return;
                }

                _selectedState = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<RentalState> RentalStates { get; }

        public string ValidationMessage
        {
            get => _validationMessage;
            private set
            {
                if (_validationMessage == value)
                {
                    return;
                }

                _validationMessage = value;
                OnPropertyChanged();
            }
        }

        public bool WasSaved
        {
            get => _wasSaved;
            private set
            {
                if (_wasSaved == value)
                {
                    return;
                }

                _wasSaved = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand SaveCommand { get; }
        public RelayCommand CancelCommand { get; }

        public RentalRecordDialogViewModel(IEnumerable<Vehicle> vehicles, bool isEditMode = false)
        {
            _isEditMode = isEditMode;
            Vehicles = vehicles ?? Enumerable.Empty<Vehicle>();
            SelectedVehicle = Vehicles.FirstOrDefault();
            RentalStates = Enum.GetValues(typeof(RentalState)).Cast<RentalState>();
            SelectedState = RentalState.Active;
            RentalTimeText = DateTime.Now.AddMinutes(1).ToString("HH:mm");
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
        }

        private void Save()
        {
            ValidationResult validationResult = RentalRecordValidator.ValidateInput(
                SelectedVehicle,
                RentalDate,
                RentalTimeText,
                DurationDaysText,
                TotalCostText,
                MileageDrivenText,
                SelectedState,
                validatePastDateTime: IsAddMode,
                out DateTime rentalDateTime);

            if (!validationResult.IsValid)
            {
                ValidationMessage = validationResult.ErrorMessage;
                return;
            }

            ValidationMessage = string.Empty;
            RentalDateTime = rentalDateTime;
            WasSaved = true;
            RequestClose?.Invoke(true);
        }

        private void Cancel()
        {
            WasSaved = false;
            RequestClose?.Invoke(false);
        }
    }
}
