using VehicleRentalSystem.Models.Enums;

namespace VehicleRentalSystem.Services.States
{
    public class CompletedState : IRentalState
    {
        public RentalState StateType => RentalState.Completed;
    }
}
