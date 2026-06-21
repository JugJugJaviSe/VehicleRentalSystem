using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using VehicleRentalSystem.Component2.Interfaces;
using VehicleRentalSystem.Models.Enums;
using VehicleRentalSystem.Models.Models;

namespace VehicleRentalSystem.Component2.Services
{
    public class FallbackVehicleRentalClient : IVehicleRentalClient
    {
        private static readonly Guid VehicleAId = new Guid("11111111-1111-1111-1111-111111111111");
        private static readonly Guid VehicleBId = new Guid("22222222-2222-2222-2222-222222222222");
        private static readonly Guid VehicleCId = new Guid("33333333-3333-3333-3333-333333333333");

        private readonly IVehicleRentalClient _inner;

        public bool UsedFallback { get; private set; }

        public FallbackVehicleRentalClient(IVehicleRentalClient inner)
        {
            _inner = inner;
        }

        public List<Vehicle> GetAllVehicles()
        {
            UsedFallback = false;
            try
            {
                return _inner.GetAllVehicles();
            }
            catch (EndpointNotFoundException)
            {
                UsedFallback = true;
                return GetSampleVehicles();
            }
            catch (CommunicationException)
            {
                UsedFallback = true;
                return GetSampleVehicles();
            }
            catch (TimeoutException)
            {
                UsedFallback = true;
                return GetSampleVehicles();
            }
        }

        public List<RentalRecord> GetRentalRecordsByVehicleAndMonth(Guid vehicleId, int year, int month)
        {
            UsedFallback = false;
            try
            {
                return _inner.GetRentalRecordsByVehicleAndMonth(vehicleId, year, month);
            }
            catch (EndpointNotFoundException)
            {
                UsedFallback = true;
                return GetSampleRentalRecords(vehicleId, year, month);
            }
            catch (CommunicationException)
            {
                UsedFallback = true;
                return GetSampleRentalRecords(vehicleId, year, month);
            }
            catch (TimeoutException)
            {
                UsedFallback = true;
                return GetSampleRentalRecords(vehicleId, year, month);
            }
        }

        private List<RentalRecord> GetSampleRentalRecords(Guid vehicleId, int year, int month)
        {
            return GetSampleRentalRecords()
                .Where(record => record.VehicleId == vehicleId
                    && record.RentalDate.Year == year
                    && record.RentalDate.Month == month)
                .ToList();
        }

        private static List<Vehicle> GetSampleVehicles()
        {
            return new List<Vehicle>
            {
                new Vehicle
                {
                    Id = VehicleAId,
                    LicensePlate = "BG-001-AA",
                    Brand = "Toyota",
                    Model = "Corolla",
                    ProductionYear = 2019,
                    FuelType = FuelType.Petrol,
                    IsAvailable = true
                },
                new Vehicle
                {
                    Id = VehicleBId,
                    LicensePlate = "BG-002-BB",
                    Brand = "Volkswagen",
                    Model = "Golf",
                    ProductionYear = 2020,
                    FuelType = FuelType.Diesel,
                    IsAvailable = true
                },
                new Vehicle
                {
                    Id = VehicleCId,
                    LicensePlate = "BG-003-CC",
                    Brand = "Tesla",
                    Model = "Model 3",
                    ProductionYear = 2022,
                    FuelType = FuelType.Electric,
                    IsAvailable = false
                }
            };
        }

        private static List<RentalRecord> GetSampleRentalRecords()
        {
            var thisMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var previousMonth = thisMonth.AddMonths(-1);

            return new List<RentalRecord>
            {
                new RentalRecord
                {
                    Id = Guid.NewGuid(),
                    VehicleId = VehicleAId,
                    RentalDate = thisMonth.AddDays(2).AddHours(9),
                    DurationDays = 3,
                    TotalCost = 12000,
                    MileageDriven = 250,
                    State = RentalState.Completed
                },
                new RentalRecord
                {
                    Id = Guid.NewGuid(),
                    VehicleId = VehicleAId,
                    RentalDate = thisMonth.AddDays(10).AddHours(14),
                    DurationDays = 5,
                    TotalCost = 20000,
                    MileageDriven = 540,
                    State = RentalState.Overdue
                },
                new RentalRecord
                {
                    Id = Guid.NewGuid(),
                    VehicleId = VehicleBId,
                    RentalDate = thisMonth.AddDays(6).AddHours(11),
                    DurationDays = 2,
                    TotalCost = 8000,
                    MileageDriven = 120,
                    State = RentalState.Active
                },
                new RentalRecord
                {
                    Id = Guid.NewGuid(),
                    VehicleId = VehicleCId,
                    RentalDate = previousMonth.AddDays(15).AddHours(16),
                    DurationDays = 4,
                    TotalCost = 16000,
                    MileageDriven = 300,
                    State = RentalState.Completed
                }
            };
        }
    }
}
