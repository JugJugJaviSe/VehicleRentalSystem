using VehicleRentalSystem.Models.Enums;
using VehicleRentalSystem.Services.Interfaces;

namespace VehicleRentalSystem.Services.States
{
    public class OverdueState : IRentalState
    {
        public bool BlocksVehicleAvailability => true;

        public bool CanChangeTo(RentalState requestedState)
        {
            return requestedState == RentalState.Overdue
                || requestedState == RentalState.Completed;
        }
    }
}