using System;
using System.Collections.Generic;
using VehicleRentalSystem.Models.Enums;
using VehicleRentalSystem.Models.Models;

namespace VehicleRentalSystem.Component1.Helpers
{
    public static class SampleDataFactory
    {
        public static List<Vehicle> CreateVehicles()
        {
            return new List<Vehicle>
            {
                new Vehicle
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    LicensePlate = "NS-123-AA",
                    Brand = "Toyota",
                    Model = "Corolla",
                    ProductionYear = 2020,
                    FuelType = FuelType.Hybrid,
                    IsAvailable = false
                },
                new Vehicle
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    LicensePlate = "BG-456-BB",
                    Brand = "Volkswagen",
                    Model = "Golf",
                    ProductionYear = 2019,
                    FuelType = FuelType.Diesel,
                    IsAvailable = true
                },
                new Vehicle
                {
                    Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    LicensePlate = "NI-789-CC",
                    Brand = "Tesla",
                    Model = "Model 3",
                    ProductionYear = 2022,
                    FuelType = FuelType.Electric,
                    IsAvailable = false
                }
            };
        }

        public static List<RentalRecord> CreateRentalRecords()
        {
            return new List<RentalRecord>
            {
                new RentalRecord
                {
                    Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                    VehicleId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    RentalDate = DateTime.Today.AddDays(1),
                    DurationDays = 5,
                    TotalCost = 25000,
                    MileageDriven = 420,
                    State = RentalState.Active
                },
                new RentalRecord
                {
                    Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                    VehicleId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    RentalDate = DateTime.Today.AddDays(3),
                    DurationDays = 2,
                    TotalCost = 9800,
                    MileageDriven = 185,
                    State = RentalState.Completed
                },
                new RentalRecord
                {
                    Id = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                    VehicleId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    RentalDate = DateTime.Today.AddDays(5),
                    DurationDays = 7,
                    TotalCost = 42000,
                    MileageDriven = 760,
                    State = RentalState.Overdue
                }
            };
        }
    }
}