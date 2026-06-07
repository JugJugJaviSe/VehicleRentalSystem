using VehicleRentalSystem.Models.Enums;
using VehicleRentalSystem.Services.Interfaces;

namespace VehicleRentalSystem.Services.States
{
    public class CancelledState : IRentalState
    {
        public RentalState StateType => RentalState.Cancelled;
    }
}
