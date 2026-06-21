using System;

namespace VehicleRentalSystem.Component2.Helpers
{
    public static class RentalRecordCacheKeyBuilder
    {
        public static string Build(Guid vehicleId, int year, int month)
        {
            return $"{vehicleId}-{year:D4}-{month:D2}";
        }
    }
}
