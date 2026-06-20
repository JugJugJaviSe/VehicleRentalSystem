using System;
using VehicleRentalSystem.Component1.ViewModels;
using VehicleRentalSystem.Models.Models;

namespace VehicleRentalSystem.Component1.Mappers
{
    public static class VehicleMapper
    {
        public static Vehicle CreateFromDialog(VehicleDialogViewModel viewModel)
        {
            return new Vehicle
            {
                Id = Guid.NewGuid(),
                LicensePlate = viewModel.LicensePlate,
                Brand = viewModel.Brand,
                Model = viewModel.Model,
                ProductionYear = int.Parse(viewModel.ProductionYearText),
                FuelType = viewModel.SelectedFuelType,
                IsAvailable = true
            };
        }

        public static Vehicle CreateUpdatedVehicle(
            Vehicle existingVehicle,
            VehicleDialogViewModel viewModel)
        {
            return new Vehicle
            {
                Id = existingVehicle.Id,
                LicensePlate = viewModel.LicensePlate,
                Brand = viewModel.Brand,
                Model = viewModel.Model,
                ProductionYear = int.Parse(viewModel.ProductionYearText),
                FuelType = viewModel.SelectedFuelType,
                IsAvailable = existingVehicle.IsAvailable
            };
        }

        public static Vehicle Copy(Vehicle vehicle)
        {
            return new Vehicle
            {
                Id = vehicle.Id,
                LicensePlate = vehicle.LicensePlate,
                Brand = vehicle.Brand,
                Model = vehicle.Model,
                ProductionYear = vehicle.ProductionYear,
                FuelType = vehicle.FuelType,
                IsAvailable = vehicle.IsAvailable
            };
        }
    }
}