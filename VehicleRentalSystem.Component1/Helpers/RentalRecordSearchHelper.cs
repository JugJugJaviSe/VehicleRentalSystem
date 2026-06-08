using System;
using VehicleRentalSystem.Models.Models;

namespace VehicleRentalSystem.Component1.Helpers
{
    public static class RentalRecordSearchHelper
    {
        public static bool Matches(RentalRecord rentalRecord, string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                return true;
            }

            if (rentalRecord == null)
            {
                return false;
            }

            string trimmedSearchText = searchText.Trim();
            return Contains(rentalRecord.Id.ToString(), trimmedSearchText)
                || Contains(rentalRecord.VehicleId.ToString(), trimmedSearchText)
                || Contains(rentalRecord.RentalDate.ToString(), trimmedSearchText)
                || Contains(rentalRecord.DurationDays.ToString(), trimmedSearchText)
                || Contains(rentalRecord.TotalCost.ToString(), trimmedSearchText)
                || Contains(rentalRecord.MileageDriven.ToString(), trimmedSearchText)
                || Contains(rentalRecord.State.ToString(), trimmedSearchText);
        }

        private static bool Contains(string value, string searchText)
        {
            return value != null
                && value.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }
}
