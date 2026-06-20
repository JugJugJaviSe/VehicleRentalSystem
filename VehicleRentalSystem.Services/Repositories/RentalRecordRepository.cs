using System;
using System.Collections.Generic;
using VehicleRentalSystem.Models.Models;
using VehicleRentalSystem.Services.Interfaces;

namespace VehicleRentalSystem.Services.Repositories
{
    public class RentalRecordRepository : IRentalRecordRepository
    {
        private readonly List<RentalRecord> _rentalRecords;

        public RentalRecordRepository()
        {
            _rentalRecords = new List<RentalRecord>();
        }

        public IEnumerable<RentalRecord> GetAll()
        {
            return _rentalRecords;
        }

        public RentalRecord GetById(Guid id)
        {
            return _rentalRecords.Find(rentalRecord => rentalRecord.Id == id);
        }

        public void Add(RentalRecord rentalRecord)
        {
            _rentalRecords.Add(rentalRecord);
        }

        public void AddRange(IEnumerable<RentalRecord> rentalRecords)
        {
            _rentalRecords.AddRange(rentalRecords);
        }

        public void Update(RentalRecord rentalRecord)
        {
            int index = _rentalRecords.FindIndex(existingRecord => existingRecord.Id == rentalRecord.Id);

            if (index >= 0)
            {
                _rentalRecords[index] = rentalRecord;
            }
        }

        public void Delete(Guid id)
        {
            RentalRecord rentalRecord = GetById(id);

            if (rentalRecord != null)
            {
                _rentalRecords.Remove(rentalRecord);
            }
        }
    }
}
