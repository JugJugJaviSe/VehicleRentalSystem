namespace VehicleRentalSystem.Services.States
{
    public interface IOverduableState : IRentalState
    {
        IRentalState MarkAsOverdue();
    }
}
