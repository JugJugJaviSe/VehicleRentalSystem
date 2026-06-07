using VehicleRentalSystem.Models.Enums;
using VehicleRentalSystem.Services.Interfaces;

namespace VehicleRentalSystem.Services.States
{
    public class OverdueState : ICompletableState, ICancelableState
    {
        public RentalState StateType => RentalState.Overdue;

        public IRentalState Complete()
        {
            return new CompletedState();
        }

        public IRentalState Cancel()
        {
            return new CancelledState();
        }
    }
}
