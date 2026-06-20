using System.Runtime.Serialization;

namespace VehicleRentalSystem.Models.Enums
{
    [DataContract]
    public enum FuelType
    {
        [EnumMember]
        Petrol,
        [EnumMember]
        Diesel,
        [EnumMember]
        Hybrid,
        [EnumMember]
        Electric
    }
}
