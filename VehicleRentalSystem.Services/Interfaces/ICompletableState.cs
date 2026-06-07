namespace VehicleRentalSystem.Services.Interfaces
{
    public interface ICompletableState : IRentalState
    {
        IRentalState Complete();
    }
}
