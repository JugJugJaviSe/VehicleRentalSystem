using VehicleRentalSystem.Models.Models;
using VehicleRentalSystem.Services.Interfaces;

namespace VehicleRentalSystem.Services.Commands
{
    public class UpdateRentalRecordCommand : IUndoableCommand
    {
        private readonly IRentalRecordRepository _repository;
        private readonly RentalRecord _oldRentalRecord;
        private readonly RentalRecord _newRentalRecord;

        public UpdateRentalRecordCommand(
            IRentalRecordRepository repository,
            RentalRecord oldRentalRecord,
            RentalRecord newRentalRecord)
        {
            _repository = repository;
            _oldRentalRecord = oldRentalRecord;
            _newRentalRecord = newRentalRecord;
        }

        public void Execute()
        {
            _repository.Update(_newRentalRecord);
        }

        public void Undo()
        {
            _repository.Update(_oldRentalRecord);
        }
    }
}
