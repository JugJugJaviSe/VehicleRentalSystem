using System;
using System.Collections.Generic;
using System.ServiceModel;
using VehicleRentalSystem.Component2.Interfaces;
using VehicleRentalSystem.Models.Models;
using VehicleRentalSystem.WcfService.Contracts;

namespace VehicleRentalSystem.Component2.Services
{
    public class VehicleRentalClient : IVehicleRentalClient
    {
        public bool UsedFallback => false;

        public List<Vehicle> GetAllVehicles()
        {
            return CallService(channel => channel.GetAllVehicles());
        }

        public List<RentalRecord> GetRentalRecordsByVehicleAndMonth(Guid vehicleId, int year, int month)
        {
            return CallService(channel => channel.GetRentalRecordsByVehicleAndMonth(vehicleId, year, month));
        }

        private T CallService<T>(Func<IVehicleRentalService, T> operation)
        {
            var factory = new ChannelFactory<IVehicleRentalService>("VehicleRentalServiceEndpoint");
            IVehicleRentalService channel = factory.CreateChannel();
            try
            {
                T result = operation(channel);
                ((IClientChannel)channel).Close();
                factory.Close();
                return result;
            }
            catch
            {
                ((IClientChannel)channel).Abort();
                factory.Abort();
                throw;
            }
        }
    }
}