using VehicleRentalSystem.Models.Enums;

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
