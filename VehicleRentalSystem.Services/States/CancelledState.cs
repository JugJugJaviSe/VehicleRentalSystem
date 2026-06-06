using VehicleRentalSystem.Models.Enums;

namespace VehicleRentalSystem.Services.States
{
    public class CancelledState : IRentalState
    {
        public RentalState StateType => RentalState.Cancelled;
    }
}
