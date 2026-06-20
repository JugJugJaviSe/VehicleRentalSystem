using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using VehicleRentalSystem.Models.Models;
using VehicleRentalSystem.Services.Interfaces;

namespace VehicleRentalSystem.Services.Services
{
    public class JsonPersistenceService : IPersistenceService
    {
        private readonly Encoding _encoding = new UTF8Encoding(false);

        public void SaveVehicles(IEnumerable<Vehicle> vehicles, string filePath)
        {
            Save(vehicles, filePath);
        }

        public List<Vehicle> LoadVehicles(string filePath)
        {
            return Load<Vehicle>(filePath);
        }

        public void SaveRentalRecords(IEnumerable<RentalRecord> rentalRecords, string filePath)
        {
            Save(rentalRecords, filePath);
        }

        public List<RentalRecord> LoadRentalRecords(string filePath)
        {
            return Load<RentalRecord>(filePath);
        }

        private void Save<T>(IEnumerable<T> items, string filePath)
        {
            try
            {
                string json = JsonConvert.SerializeObject(items, Formatting.Indented);
                File.WriteAllText(filePath, json, _encoding);
            }
            catch (Exception)
            {
                // Persistence failures must not propagate to the UI layer.
            }
        }

        private List<T> Load<T>(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    return new List<T>();
                }

                string json = File.ReadAllText(filePath, _encoding);
                return JsonConvert.DeserializeObject<List<T>>(json)
                    ?? new List<T>();
            }
            catch (Exception)
            {
                return new List<T>();
            }
        }
    }
}