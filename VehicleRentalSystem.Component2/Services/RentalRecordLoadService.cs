using System;
using System.Collections.Generic;
using VehicleRentalSystem.Component2.Interfaces;
using VehicleRentalSystem.Models.Models;

namespace VehicleRentalSystem.Component2.Services
{
    public class RentalRecordLoadService : IRentalRecordLoadService
    {
        private readonly IVehicleRentalClient _client;
        private readonly IRentalRecordAdapter _adapter;

        public RentalRecordLoadService(IVehicleRentalClient client, IRentalRecordAdapter adapter)
        {
            _client = client;
            _adapter = adapter;
        }

        public KeyValuePair<string, List<RentalRecord>> Load(Guid vehicleId, int year, int month)
        {
            var fetchedRecords = _client.GetRentalRecordsByVehicleAndMonth(vehicleId, year, month);
            return _adapter.AdaptToDictionaryEntry(vehicleId, year, month, fetchedRecords);
        }
    }
}
