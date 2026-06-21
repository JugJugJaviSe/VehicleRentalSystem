using VehicleRentalSystem.Models.Enums;

namespace VehicleRentalSystem.Services.Interfaces
{
    public interface IRentalState
    {
        bool CanChangeTo(RentalState requestedState);

        bool BlocksVehicleAvailability { get; }
    }
}