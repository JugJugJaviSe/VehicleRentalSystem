using System;
using VehicleRentalSystem.Models.Models;

namespace VehicleRentalSystem.Component1.Helpers
{
    public static class VehicleSearchHelper
    {
        public static bool Matches(Vehicle vehicle, string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                return true;
            }

            if (vehicle == null)
            {
                return false;
            }

            string trimmedSearchText = searchText.Trim();
            return Contains(vehicle.Id.ToString(), trimmedSearchText)
                || Contains(vehicle.LicensePlate, trimmedSearchText)
                || Contains(vehicle.Brand, trimmedSearchText)
                || Contains(vehicle.Model, trimmedSearchText)
                || Contains(vehicle.ProductionYear.ToString(), trimmedSearchText)
                || Contains(vehicle.FuelType.ToString(), trimmedSearchText)
                || Contains(vehicle.IsAvailable.ToString(), trimmedSearchText);
        }

        private static bool Contains(string value, string searchText)
        {
            return value != null
                && value.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }
}
