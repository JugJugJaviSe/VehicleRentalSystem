using System;
using VehicleRentalSystem.Component1.ViewModels;
using VehicleRentalSystem.Models.Enums;
using VehicleRentalSystem.Models.Models;

namespace VehicleRentalSystem.Component1.Mappers
{
    public static class RentalRecordMapper
    {
        public static RentalRecord CreateFromDialog(RentalRecordDialogViewModel viewModel)
        {
            return new RentalRecord
            {
                Id = Guid.NewGuid(),
                VehicleId = viewModel.SelectedVehicleId,
                RentalDate = viewModel.RentalDateTime,
                DurationDays = viewModel.DurationDays,
                TotalCost = viewModel.TotalCost,
                MileageDriven = viewModel.MileageDriven,
                State = RentalState.Active
            };
        }

        public static RentalRecord CreateUpdatedRentalRecord(
            RentalRecord existingRentalRecord,
            RentalRecordDialogViewModel viewModel,
            RentalState nextState)
        {
            return new RentalRecord
            {
                Id = existingRentalRecord.Id,
                VehicleId = existingRentalRecord.VehicleId,
                RentalDate = viewModel.RentalDateTime,
                DurationDays = viewModel.DurationDays,
                TotalCost = viewModel.TotalCost,
                MileageDriven = viewModel.MileageDriven,
                State = nextState
            };
        }

        public static RentalRecord Copy(RentalRecord rentalRecord)
        {
            return new RentalRecord
            {
                Id = rentalRecord.Id,
                VehicleId = rentalRecord.VehicleId,
                RentalDate = rentalRecord.RentalDate,
                DurationDays = rentalRecord.DurationDays,
                TotalCost = rentalRecord.TotalCost,
                MileageDriven = rentalRecord.MileageDriven,
                State = rentalRecord.State
            };
        }
    }
}