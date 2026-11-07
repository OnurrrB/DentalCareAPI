using System.Text.Json.Serialization;

namespace DentalCareAPI.Entities;

[JsonConverter(typeof(JsonStringEnumConverter<Gender>))]
public enum Gender
{
    Unspecified,
    Female,
    Male,
    Other,
}
