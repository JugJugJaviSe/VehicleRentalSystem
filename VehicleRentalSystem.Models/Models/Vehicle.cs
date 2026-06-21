using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using VehicleRentalSystem.Models.Enums;

namespace VehicleRentalSystem.Models.Models
{
    [DataContract]
    public class Vehicle : INotifyPropertyChanged
    {
        private Guid _id;
        private string _licensePlate;
        private string _brand;
        private string _model;
        private int _productionYear;
        private FuelType _fuelType;
        private bool _isAvailable = true;

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        [DataMember]
        public Guid Id
        {
            get => _id;
            set
            {
                if (_id == value)
                {
                    return;
                }

                _id = value;
                OnPropertyChanged();
            }
        }

        [DataMember]
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

        [DataMember]
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

        [DataMember]
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

        [DataMember]
        public int ProductionYear
        {
            get => _productionYear;
            set
            {
                if (_productionYear == value)
                {
                    return;
                }

                _productionYear = value;
                OnPropertyChanged();
            }
        }

        [DataMember]
        public FuelType FuelType
        {
            get => _fuelType;
            set
            {
                if (_fuelType == value)
                {
                    return;
                }

                _fuelType = value;
                OnPropertyChanged();
            }
        }

        [DataMember]
        public bool IsAvailable
        {
            get => _isAvailable;
            set
            {
                if (_isAvailable == value)
                {
                    return;
                }

                _isAvailable = value;
                OnPropertyChanged();
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(propertyName));
        }
    }
}