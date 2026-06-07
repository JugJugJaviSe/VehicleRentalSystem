namespace VehicleRentalSystem.Services.Interfaces
{
    public interface IUndoableCommand
    {
        void Execute();
        void Undo();
    }
}
