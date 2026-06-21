using VehicleRentalSystem.Models.Enums;
using VehicleRentalSystem.Services.Interfaces;

namespace VehicleRentalSystem.Services.States
{
    public class ActiveState : IRentalState
    {
        public bool BlocksVehicleAvailability => true;

        public bool CanChangeTo(RentalState requestedState)
        {
            return requestedState == RentalState.Active
                || requestedState == RentalState.Completed
                || requestedState == RentalState.Cancelled
                || requestedState == RentalState.Overdue;
        }
    }
}