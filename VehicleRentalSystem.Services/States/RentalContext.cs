using VehicleRentalSystem.Models.Enums;
using VehicleRentalSystem.Models.Models;

namespace VehicleRentalSystem.Services.States
{
    public class RentalContext
    {
        public RentalRecord RentalRecord { get; }
        public IRentalState CurrentState { get; private set; }

        public RentalContext(RentalRecord rentalRecord)
        {
            RentalRecord = rentalRecord;
            CurrentState = MapToState(rentalRecord.State);
        }

        public void SetState(IRentalState state)
        {
            if (state == null)
            {
                return;
            }

            CurrentState = state;
            RentalRecord.State = state.StateType;
        }

        private IRentalState MapToState(RentalState rentalState)
        {
            switch (rentalState)
            {
                case RentalState.Completed:
                    return new CompletedState();
                case RentalState.Cancelled:
                    return new CancelledState();
                case RentalState.Overdue:
                    return new OverdueState();
                case RentalState.Active:
                default:
                    return new ActiveState();
            }
        }
    }
}
