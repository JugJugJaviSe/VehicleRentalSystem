using VehicleRentalSystem.Models.Enums;
using VehicleRentalSystem.Services.Interfaces;

namespace VehicleRentalSystem.Services.States
{
    public class OverdueState : IRentalState
    {
        public RentalState ChangeTo(RentalState requestedState)
        {
            if (requestedState == RentalState.Overdue
                || requestedState == RentalState.Completed)
            {
                return requestedState;
            }

            return RentalState.Overdue;
        }
    }
}
