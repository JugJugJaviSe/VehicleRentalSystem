using System;
using System.Collections.Generic;
using System.Linq;
using VehicleRentalSystem.Component2.Helpers;
using VehicleRentalSystem.Component2.Interfaces;
using VehicleRentalSystem.Models.Models;

namespace VehicleRentalSystem.Component2.Services
{
    public class RentalRecordCache : IRentalRecordCache
    {
        private readonly Dictionary<string, List<RentalRecord>> _records
            = new Dictionary<string, List<RentalRecord>>();

        public void AddOrUpdate(string key, IEnumerable<RentalRecord> records)
        {
            var materialized = records.ToList();
            if (materialized.Count == 0)
            {
                _records.Remove(key);
                return;
            }

            _records[key] = materialized;
        }

        public void Remove(Guid vehicleId, int year, int month)
        {
            var key = RentalRecordCacheKeyBuilder.Build(vehicleId, year, month);
            _records.Remove(key);
        }

        public void Clear()
        {
            _records.Clear();
        }

        public IReadOnlyList<IReadOnlyList<RentalRecord>> GetGroups()
        {
            return _records.Values
                .Where(group => group.Count > 0)
                .Select(group => (IReadOnlyList<RentalRecord>)group)
                .ToList();
        }

        public IReadOnlyList<RentalRecord> GetAllRecords()
        {
            return _records.Values
                .Where(group => group.Count > 0)
                .SelectMany(group => group)
                .ToList();
        }
    }
}
