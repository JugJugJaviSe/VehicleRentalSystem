using VehicleRentalSystem.Models.Models;
using VehicleRentalSystem.Services.Interfaces;

namespace VehicleRentalSystem.Services.Commands
{
    public class DeleteRentalRecordCommand : IUndoableCommand
    {
        private readonly IRentalRecordRepository _repository;
        private readonly RentalRecord _rentalRecord;

        public DeleteRentalRecordCommand(
            IRentalRecordRepository repository,
            RentalRecord rentalRecord)
        {
            _repository = repository;
            _rentalRecord = rentalRecord;
        }

        public void Execute()
        {
            _repository.Delete(_rentalRecord.Id);
        }

        public void Undo()
        {
            _repository.Add(_rentalRecord);
        }
    }
}
