using System;
using System.Collections.Generic;
using System.Linq;
using VehicleRentalSystem.Component1.Commands;
using VehicleRentalSystem.Component1.Validators;
using VehicleRentalSystem.Models.Enums;

namespace VehicleRentalSystem.Component1.ViewModels
{
    public class VehicleDialogViewModel : BaseViewModel
    {
        private string _licensePlate;
        private string _brand;
        private string _model;
        private string _productionYearText;
        private FuelType _selectedFuelType;
        private string _validationMessage;
        private bool _wasSaved;

        public event Action<bool?> RequestClose;

        public string LicensePlate
        {
            get => _licensePlate;
            set
            {
                if (_licensePlate == value)
                {
                    return;
                }

                _licensePlate = value;
                OnPropertyChanged();
            }
        }

        public string Brand
        {
            get => _brand;
            set
            {
                if (_brand == value)
                {
                    return;
                }

                _brand = value;
                OnPropertyChanged();
            }
        }

        public string Model
        {
            get => _model;
            set
            {
                if (_model == value)
                {
                    return;
                }

                _model = value;
                OnPropertyChanged();
            }
        }

        public string ProductionYearText
        {
            get => _productionYearText;
            set
            {
                if (_productionYearText == value)
                {
                    return;
                }

                _productionYearText = value;
                OnPropertyChanged();
            }
        }

        public FuelType SelectedFuelType
        {
            get => _selectedFuelType;
            set
            {
                if (_selectedFuelType == value)
                {
                    return;
                }

                _selectedFuelType = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<FuelType> FuelTypes { get; }

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

        public VehicleDialogViewModel()
        {
            FuelTypes = Enum.GetValues(typeof(FuelType)).Cast<FuelType>();
            SelectedFuelType = FuelTypes.First();
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
        }

        private void Save()
        {
            ValidationResult validationResult = VehicleValidator.ValidateInput(
                LicensePlate,
                Brand,
                Model,
                ProductionYearText,
                SelectedFuelType);

            if (!validationResult.IsValid)
            {
                ValidationMessage = validationResult.ErrorMessage;
                return;
            }

            ValidationMessage = string.Empty;
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
