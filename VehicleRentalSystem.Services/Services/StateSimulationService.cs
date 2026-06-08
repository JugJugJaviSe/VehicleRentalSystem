using System;
using VehicleRentalSystem.Models.Enums;
using VehicleRentalSystem.Services.Interfaces;
using VehicleRentalSystem.Services.States;

namespace VehicleRentalSystem.Services.Services
{
    public class StateSimulationService
    {
        public RentalState GetNextState(
            RentalState currentState,
            RentalState requestedState)
        {
            IRentalState state = CreateState(currentState);
            return state.ChangeTo(requestedState);
        }

        private IRentalState CreateState(RentalState state)
        {
            switch (state)
            {
                case RentalState.Active:
                    return new ActiveState();
                case RentalState.Overdue:
                    return new OverdueState();
                case RentalState.Completed:
                    return new CompletedState();
                case RentalState.Cancelled:
                    return new CancelledState();
                default:
                    throw new ArgumentOutOfRangeException(nameof(state));
            }
        }
    }
}
