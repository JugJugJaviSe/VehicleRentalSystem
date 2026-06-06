namespace VehicleRentalSystem.Services.States
{
    public interface ICompletableState : IRentalState
    {
        IRentalState Complete();
    }
}
