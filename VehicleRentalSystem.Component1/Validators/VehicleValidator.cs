using System;
using VehicleRentalSystem.Models.Enums;

namespace VehicleRentalSystem.Component1.Validators
{
    public static class VehicleValidator
    {
        public static ValidationResult ValidateInput(
            string licensePlate,
            string brand,
            string model,
            string productionYearText,
            object selectedFuelType)
        {
            if (string.IsNullOrWhiteSpace(licensePlate))
            {
                return ValidationResult.Failure("License plate is required.");
            }

            if (licensePlate.Trim().Length < 3)
            {
                return ValidationResult.Failure(
                    "License plate must contain at least 3 characters.");
            }

            if (string.IsNullOrWhiteSpace(brand))
            {
                return ValidationResult.Failure("Brand is required.");
            }

            if (brand.Trim().Length < 2)
            {
                return ValidationResult.Failure("Brand must contain at least 2 characters.");
            }

            if (string.IsNullOrWhiteSpace(model))
            {
                return ValidationResult.Failure("Model is required.");
            }

            if (!int.TryParse(productionYearText, out int productionYear))
            {
                return ValidationResult.Failure("Production year must be a valid number.");
            }

            if (!(selectedFuelType is FuelType fuelType))
            {
                return ValidationResult.Failure("Fuel type must be selected.");
            }

            return Validate(licensePlate, brand, model, productionYear, fuelType);
        }

        public static ValidationResult Validate(
            string licensePlate,
            string brand,
            string model,
            int productionYear,
            FuelType fuelType)
        {
            if (string.IsNullOrWhiteSpace(licensePlate))
            {
                return ValidationResult.Failure("License plate is required.");
            }

            if (licensePlate.Trim().Length < 3)
            {
                return ValidationResult.Failure(
                    "License plate must contain at least 3 characters.");
            }

            if (string.IsNullOrWhiteSpace(brand))
            {
                return ValidationResult.Failure("Brand is required.");
            }

            if (brand.Trim().Length < 2)
            {
                return ValidationResult.Failure("Brand must contain at least 2 characters.");
            }

            if (string.IsNullOrWhiteSpace(model))
            {
                return ValidationResult.Failure("Model is required.");
            }

            int maximumProductionYear = DateTime.Now.Year + 1;

            if (productionYear < 1980 || productionYear > maximumProductionYear)
            {
                return ValidationResult.Failure(
                    $"Production year must be between 1980 and {maximumProductionYear}.");
            }

            if (!Enum.IsDefined(typeof(FuelType), fuelType))
            {
                return ValidationResult.Failure("Fuel type is invalid.");
            }

            return ValidationResult.Success();
        }
    }
}
