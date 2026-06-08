using VehicleRentalSystem.Models.Enums;
using VehicleRentalSystem.Services.Interfaces;

namespace VehicleRentalSystem.Services.States
{
    public class CompletedState : IRentalState
    {
        public RentalState ChangeTo(RentalState requestedState)
        {
            return RentalState.Completed;
        }
    }
}
