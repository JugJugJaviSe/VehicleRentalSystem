using VehicleRentalSystem.Models.Enums;

namespace VehicleRentalSystem.Services.Interfaces
{
    public interface IStateSimulationService
    {
        RentalState GetNextState(
            RentalState currentState,
            RentalState requestedState);
    }
}