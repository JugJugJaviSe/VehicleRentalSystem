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
            var factory = new ChannelFactory<IVehicleRentalService>("VehicleRentalServiceEndpoint");
            IVehicleRentalService channel = factory.CreateChannel();
            try
            {
                List<Vehicle> result = channel.GetAllVehicles();
                ((IClientChannel)channel).Close();
                factory.Close();
                return result;
            }
            catch (CommunicationException)
            {
                ((IClientChannel)channel).Abort();
                factory.Abort();
                throw;
            }
            catch (TimeoutException)
            {
                ((IClientChannel)channel).Abort();
                factory.Abort();
                throw;
            }
            catch (Exception)
            {
                ((IClientChannel)channel).Abort();
                factory.Abort();
                throw;
            }
        }

        public List<RentalRecord> GetRentalRecordsByVehicleAndMonth(Guid vehicleId, int year, int month)
        {
            var factory = new ChannelFactory<IVehicleRentalService>("VehicleRentalServiceEndpoint");
            IVehicleRentalService channel = factory.CreateChannel();
            try
            {
                List<RentalRecord> result = channel.GetRentalRecordsByVehicleAndMonth(vehicleId, year, month);
                ((IClientChannel)channel).Close();
                factory.Close();
                return result;
            }
            catch (CommunicationException)
            {
                ((IClientChannel)channel).Abort();
                factory.Abort();
                throw;
            }
            catch (TimeoutException)
            {
                ((IClientChannel)channel).Abort();
                factory.Abort();
                throw;
            }
            catch (Exception)
            {
                ((IClientChannel)channel).Abort();
                factory.Abort();
                throw;
            }
        }
    }
}
