namespace VehicleRentalSystem.Services.Interfaces
{
    public interface ICancelableState : IRentalState
    {
        IRentalState Cancel();
    }
}
