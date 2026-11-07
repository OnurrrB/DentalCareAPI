using DentalCareAPI.Entities;

namespace DentalCareAPI.DTOs;

public record UserResponse(
    int Id,
    string Email,
    string Name,
    int Age,
    Gender Gender,
    bool IsSmoker,
    bool IsAlcoholUser,
    bool HasOrthodontic,
    bool IsBruxism,
    DateTime CreatedAt);
