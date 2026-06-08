using VehicleRentalSystem.Models.Enums;
using VehicleRentalSystem.Services.Interfaces;

namespace VehicleRentalSystem.Services.States
{
    public class ActiveState : IRentalState
    {
        public RentalState ChangeTo(RentalState requestedState)
        {
            if (requestedState == RentalState.Active
                || requestedState == RentalState.Completed
                || requestedState == RentalState.Cancelled
                || requestedState == RentalState.Overdue)
            {
                return requestedState;
            }

            return RentalState.Active;
        }
    }
}
