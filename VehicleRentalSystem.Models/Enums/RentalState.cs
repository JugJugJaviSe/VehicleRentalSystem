using System.Runtime.Serialization;

namespace VehicleRentalSystem.Models.Enums
{
    [DataContract]
    public enum RentalState
    {
        [EnumMember]
        Active,
        [EnumMember]
        Completed,
        [EnumMember]
        Cancelled,
        [EnumMember]
        Overdue
    }
}
