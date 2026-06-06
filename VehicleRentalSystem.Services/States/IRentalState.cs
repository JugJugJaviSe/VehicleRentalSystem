using VehicleRentalSystem.Models.Enums;

namespace VehicleRentalSystem.Services.States
{
    public interface IRentalState
    {
        RentalState StateType { get; }
    }
}
