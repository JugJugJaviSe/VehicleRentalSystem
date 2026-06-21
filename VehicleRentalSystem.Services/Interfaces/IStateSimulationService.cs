using VehicleRentalSystem.Models.Enums;

namespace VehicleRentalSystem.Services.Interfaces
{
    public interface IStateSimulationService
    {
        bool CanChangeTo(
            RentalState currentState,
            RentalState requestedState);

        bool BlocksVehicleAvailability(RentalState state);
    }
}