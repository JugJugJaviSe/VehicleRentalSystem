using System;
using System.Globalization;
using VehicleRentalSystem.Models.Enums;
using VehicleRentalSystem.Models.Models;

namespace VehicleRentalSystem.Component1.Validators
{
    public static class RentalRecordValidator
    {
        public static ValidationResult ValidateInput(
            object selectedVehicle,
            DateTime? rentalDate,
            string rentalTimeText,
            string durationDaysText,
            string totalCostText,
            string mileageDrivenText,
            object selectedState,
            bool validatePastDateTime,
            out DateTime rentalDateTime)
        {
            rentalDateTime = default;

            if (!(selectedVehicle is Vehicle vehicle))
            {
                return ValidationResult.Failure("Vehicle must be selected.");
            }

            if (!rentalDate.HasValue)
            {
                return ValidationResult.Failure("Rental date is required.");
            }

            if (!TimeSpan.TryParseExact(
                rentalTimeText,
                @"hh\:mm",
                CultureInfo.InvariantCulture,
                out TimeSpan rentalTime))
            {
                return ValidationResult.Failure("Rental time must be in HH:mm format.");
            }

            rentalDateTime = rentalDate.Value.Date + rentalTime;

            if (validatePastDateTime && rentalDateTime < DateTime.Now)
            {
                return ValidationResult.Failure(
                    "Rental start date and time cannot be in the past.");
            }

            if (!TryParseFiniteDouble(durationDaysText, out double durationDays))
            {
                return ValidationResult.Failure("Duration must be a valid number.");
            }

            if (!TryParseFiniteDouble(totalCostText, out double totalCost))
            {
                return ValidationResult.Failure("Total cost must be a valid number.");
            }

            if (!TryParseFiniteDouble(mileageDrivenText, out double mileageDriven))
            {
                return ValidationResult.Failure("Mileage driven must be a valid number.");
            }

            if (!(selectedState is RentalState state))
            {
                return ValidationResult.Failure("Rental state must be selected.");
            }

            return Validate(
                vehicle.Id,
                rentalDateTime,
                durationDays,
                totalCost,
                mileageDriven,
                state);
        }

        public static ValidationResult Validate(
            Guid vehicleId,
            DateTime rentalDate,
            double durationDays,
            double totalCost,
            double mileageDriven,
            RentalState state)
        {
            if (vehicleId == Guid.Empty)
            {
                return ValidationResult.Failure("Vehicle must be selected.");
            }

            if (rentalDate == DateTime.MinValue)
            {
                return ValidationResult.Failure("Rental date is required.");
            }

            if (durationDays <= 0)
            {
                return ValidationResult.Failure("Duration must be greater than 0.");
            }

            if (totalCost < 0)
            {
                return ValidationResult.Failure("Total cost cannot be negative.");
            }

            if (mileageDriven < 0)
            {
                return ValidationResult.Failure("Mileage driven cannot be negative.");
            }

            if (!Enum.IsDefined(typeof(RentalState), state))
            {
                return ValidationResult.Failure("Rental state is invalid.");
            }

            return ValidationResult.Success();
        }

        private static bool TryParseFiniteDouble(string text, out double value)
        {
            return double.TryParse(text, out value)
                && !double.IsNaN(value)
                && !double.IsInfinity(value);
        }
    }
}
