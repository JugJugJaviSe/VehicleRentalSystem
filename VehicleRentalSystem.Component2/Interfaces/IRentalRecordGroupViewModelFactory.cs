using System.Collections.Generic;
using VehicleRentalSystem.Component2.ViewModels;
using VehicleRentalSystem.Models.Models;

namespace VehicleRentalSystem.Component2.Interfaces
{
    public interface IRentalRecordGroupViewModelFactory
    {
        RentalRecordGroupViewModel Create(IReadOnlyList<RentalRecord> records);
    }
}
