using VehicleRentalSystem.Models.Enums;
using VehicleRentalSystem.Services.Interfaces;

namespace VehicleRentalSystem.Services.States
{
    public class CancelledState : IRentalState
    {
        public bool BlocksVehicleAvailability => false;

        public bool CanChangeTo(RentalState requestedState)
        {
            return requestedState == RentalState.Cancelled;
        }
    }
}