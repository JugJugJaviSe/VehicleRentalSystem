namespace VehicleRentalSystem.Services.States
{
    public interface ICancelableState : IRentalState
    {
        IRentalState Cancel();
    }
}
