namespace VehicleRentalSystem.Services.Commands
{
    public interface IUndoableCommand
    {
        void Execute();
        void Undo();
    }
}
