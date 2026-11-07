using DentalCareAPI.Entities;

namespace DentalCareAPI.DTOs;

public class UserCreateDto
{
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public Gender Gender { get; set; }
    public bool IsSmoker { get; set; }
    public bool IsAlcoholUser { get; set; }
    public bool HasOrthodontic { get; set; }
    public bool IsBruxism { get; set; }
}
