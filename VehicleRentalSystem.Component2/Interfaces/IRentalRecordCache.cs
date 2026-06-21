using System;
using System.Collections.Generic;
using VehicleRentalSystem.Models.Models;

namespace VehicleRentalSystem.Component2.Interfaces
{
    public interface IRentalRecordCache
    {
        void AddOrUpdate(string key, IEnumerable<RentalRecord> records);

        void Remove(Guid vehicleId, int year, int month);

        void Clear();

        IReadOnlyList<IReadOnlyList<RentalRecord>> GetGroups();

        IReadOnlyList<RentalRecord> GetAllRecords();
    }
}
