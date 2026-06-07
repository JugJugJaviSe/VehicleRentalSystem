using VehicleRentalSystem.Models.Enums;

namespace VehicleRentalSystem.Services.Interfaces
{
    public interface IRentalState
    {
        RentalState StateType { get; }
    }
}
