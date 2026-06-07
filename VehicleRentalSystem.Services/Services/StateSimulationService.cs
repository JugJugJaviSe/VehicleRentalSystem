using VehicleRentalSystem.Models.Models;
using VehicleRentalSystem.Services.Interfaces;
using VehicleRentalSystem.Services.States;

namespace VehicleRentalSystem.Services.Services
{
    public class StateSimulationService
    {
        public void CompleteRental(RentalRecord rentalRecord)
        {
            if (rentalRecord == null)
            {
                return;
            }

            RentalContext context = new RentalContext(rentalRecord);

            if (context.CurrentState is ICompletableState completable)
            {
                context.SetState(completable.Complete());
            }
        }

        public void MarkAsOverdue(RentalRecord rentalRecord)
        {
            if (rentalRecord == null)
            {
                return;
            }

            RentalContext context = new RentalContext(rentalRecord);

            if (context.CurrentState is IOverduableState overduable)
            {
                context.SetState(overduable.MarkAsOverdue());
            }
        }

        public void CancelRental(RentalRecord rentalRecord)
        {
            if (rentalRecord == null)
            {
                return;
            }

            RentalContext context = new RentalContext(rentalRecord);

            if (context.CurrentState is ICancelableState cancelable)
            {
                context.SetState(cancelable.Cancel());
            }
        }
    }
}
