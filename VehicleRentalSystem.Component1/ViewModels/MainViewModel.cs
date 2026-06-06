using System;
using System.Collections.ObjectModel;
using VehicleRentalSystem.Component1.Commands;
using VehicleRentalSystem.Component1.Views;
using VehicleRentalSystem.Models.Enums;
using VehicleRentalSystem.Models.Models;

namespace VehicleRentalSystem.Component1.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private Vehicle _selectedVehicle;
        private RentalRecord _selectedRentalRecord;

        public ObservableCollection<Vehicle> Vehicles { get; }

        public ObservableCollection<RentalRecord> RentalRecords { get; }

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

        public RelayCommand AddVehicleCommand { get; }
        public RelayCommand EditVehicleCommand { get; }
        public RelayCommand DeleteVehicleCommand { get; }

        public RelayCommand AddRentalRecordCommand { get; }
        public RelayCommand EditRentalRecordCommand { get; }
        public RelayCommand DeleteRentalRecordCommand { get; }

        public RelayCommand UndoCommand { get; }
        public RelayCommand RedoCommand { get; }

        public MainViewModel()
        {
            Vehicles = new ObservableCollection<Vehicle>();
            RentalRecords = new ObservableCollection<RentalRecord>();

            LoadSampleData();

            AddVehicleCommand = new RelayCommand(AddVehicle);
            EditVehicleCommand = new RelayCommand(EditVehicle);
            DeleteVehicleCommand = new RelayCommand(DeleteVehicle);

            AddRentalRecordCommand = new RelayCommand(AddRentalRecord);
            EditRentalRecordCommand = new RelayCommand(EditRentalRecord);
            DeleteRentalRecordCommand = new RelayCommand(DeleteRentalRecord);

            UndoCommand = new RelayCommand(Undo);
            RedoCommand = new RelayCommand(Redo);
        }

        private void LoadSampleData()
        {
            Vehicle bmw = new Vehicle
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                LicensePlate = "NS-101-AA",
                Brand = "BMW",
                Model = "X5",
                ProductionYear = 2022,
                FuelType = FuelType.Diesel
            };

            Vehicle audi = new Vehicle
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                LicensePlate = "BG-202-BB",
                Brand = "Audi",
                Model = "A6",
                ProductionYear = 2021,
                FuelType = FuelType.Hybrid
            };

            Vehicle tesla = new Vehicle
            {
                Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                LicensePlate = "SU-303-CC",
                Brand = "Tesla",
                Model = "Model 3",
                ProductionYear = 2023,
                FuelType = FuelType.Electric
            };

            Vehicles.Add(bmw);
            Vehicles.Add(audi);
            Vehicles.Add(tesla);

            RentalRecords.Add(new RentalRecord
            {
                Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                VehicleId = bmw.Id,
                RentalDate = new DateTime(2026, 6, 1),
                DurationDays = 7,
                TotalCost = 700,
                MileageDriven = 320,
                State = RentalState.Active
            });

            RentalRecords.Add(new RentalRecord
            {
                Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                VehicleId = audi.Id,
                RentalDate = new DateTime(2026, 5, 20),
                DurationDays = 5,
                TotalCost = 450,
                MileageDriven = 275,
                State = RentalState.Completed
            });

            RentalRecords.Add(new RentalRecord
            {
                Id = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                VehicleId = tesla.Id,
                RentalDate = new DateTime(2026, 5, 28),
                DurationDays = 3,
                TotalCost = 360,
                MileageDriven = 190,
                State = RentalState.Overdue
            });
        }

        private void AddVehicle()
        {
            VehicleDialog dialog = new VehicleDialog();

            if (dialog.ShowDialog() == true)
            {
                Vehicles.Add(new Vehicle
                {
                    Id = Guid.NewGuid(),
                    LicensePlate = dialog.LicensePlate,
                    Brand = dialog.Brand,
                    Model = dialog.Model,
                    ProductionYear = dialog.ProductionYear,
                    FuelType = dialog.FuelType
                });
            }
        }

        private void EditVehicle()
        {
            if (SelectedVehicle == null)
            {
                return;
            }

            VehicleDialog dialog = new VehicleDialog
            {
                LicensePlate = SelectedVehicle.LicensePlate,
                Brand = SelectedVehicle.Brand,
                Model = SelectedVehicle.Model,
                ProductionYear = SelectedVehicle.ProductionYear,
                FuelType = SelectedVehicle.FuelType
            };

            if (dialog.ShowDialog() == true)
            {
                SelectedVehicle.LicensePlate = dialog.LicensePlate;
                SelectedVehicle.Brand = dialog.Brand;
                SelectedVehicle.Model = dialog.Model;
                SelectedVehicle.ProductionYear = dialog.ProductionYear;
                SelectedVehicle.FuelType = dialog.FuelType;

                System.Windows.Data.CollectionViewSource.GetDefaultView(Vehicles).Refresh();
            }
        }

        private void DeleteVehicle()
        {
            if (SelectedVehicle == null)
            {
                return;
            }

            Vehicles.Remove(SelectedVehicle);
        }

        private void AddRentalRecord()
        {
            RentalRecordDialog dialog = new RentalRecordDialog
            {
                Vehicles = Vehicles
            };

            if (dialog.ShowDialog() == true)
            {
                RentalRecords.Add(new RentalRecord
                {
                    Id = Guid.NewGuid(),
                    VehicleId = dialog.SelectedVehicleId,
                    RentalDate = dialog.RentalDate,
                    DurationDays = dialog.DurationDays,
                    TotalCost = dialog.TotalCost,
                    MileageDriven = dialog.MileageDriven,
                    State = dialog.SelectedState
                });
            }
        }

        private void EditRentalRecord()
        {
            if (SelectedRentalRecord == null)
            {
                return;
            }

            RentalRecordDialog dialog = new RentalRecordDialog
            {
                Vehicles = Vehicles,
                SelectedVehicleId = SelectedRentalRecord.VehicleId,
                RentalDate = SelectedRentalRecord.RentalDate,
                DurationDays = SelectedRentalRecord.DurationDays,
                TotalCost = SelectedRentalRecord.TotalCost,
                MileageDriven = SelectedRentalRecord.MileageDriven,
                SelectedState = SelectedRentalRecord.State
            };

            if (dialog.ShowDialog() == true)
            {
                SelectedRentalRecord.VehicleId = dialog.SelectedVehicleId;
                SelectedRentalRecord.RentalDate = dialog.RentalDate;
                SelectedRentalRecord.DurationDays = dialog.DurationDays;
                SelectedRentalRecord.TotalCost = dialog.TotalCost;
                SelectedRentalRecord.MileageDriven = dialog.MileageDriven;
                SelectedRentalRecord.State = dialog.SelectedState;

                System.Windows.Data.CollectionViewSource.GetDefaultView(RentalRecords).Refresh();
            }
        }

        private void DeleteRentalRecord()
        {
            if (SelectedRentalRecord == null)
            {
                return;
            }

            RentalRecords.Remove(SelectedRentalRecord);
        }

        private void Undo()
        {
        }

        private void Redo()
        {
        }
    }
}
