using VehicleRentalSystem.Models.Models;
using VehicleRentalSystem.Services.Interfaces;

namespace VehicleRentalSystem.Services.Commands
{
    public class AddRentalRecordCommand : IUndoableCommand
    {
        private readonly IRentalRecordRepository _repository;
        private readonly RentalRecord _rentalRecord;

        public AddRentalRecordCommand(
            IRentalRecordRepository repository,
            RentalRecord rentalRecord)
        {
            _repository = repository;
            _rentalRecord = rentalRecord;
        }

        public void Execute()
        {
            _repository.Add(_rentalRecord);
        }

        public void Undo()
        {
            _repository.Delete(_rentalRecord.Id);
        }
    }
}
