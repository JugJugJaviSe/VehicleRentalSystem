using System;
using System.Collections.Generic;
using System.ServiceModel;
using VehicleRentalSystem.Models.Models;

namespace VehicleRentalSystem.WcfService.Contracts
{
    [ServiceContract]
    public interface IVehicleRentalService
    {
        [OperationContract]
        List<Vehicle> GetAllVehicles();

        [OperationContract]
        List<RentalRecord> GetRentalRecordsByVehicleAndMonth(Guid vehicleId, int year, int month);
    }
}
