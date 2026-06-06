using System;
using System.Collections.Generic;
using VehicleRentalSystem.Models.Models;

namespace VehicleRentalSystem.Services.Interfaces
{
    public interface IRentalRecordRepository
    {
        IEnumerable<RentalRecord> GetAll();
        RentalRecord GetById(Guid id);
        void Add(RentalRecord rentalRecord);
        void Update(RentalRecord rentalRecord);
        void Delete(Guid id);
    }
}
