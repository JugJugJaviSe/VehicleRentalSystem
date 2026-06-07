namespace VehicleRentalSystem.Services.Interfaces
{
    public interface IOverduableState : IRentalState
    {
        IRentalState MarkAsOverdue();
    }
}
