using VehicleRentalSystem.Models.Enums;

namespace VehicleRentalSystem.Services.States
{
    public class ActiveState : ICompletableState, ICancelableState, IOverduableState
    {
        public RentalState StateType => RentalState.Active;

        public IRentalState Complete()
        {
            return new CompletedState();
        }

        public IRentalState Cancel()
        {
            return new CancelledState();
        }

        public IRentalState MarkAsOverdue()
        {
            return new OverdueState();
        }
    }
}
