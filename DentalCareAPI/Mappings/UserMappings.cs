using DentalCareAPI.DTOs;
using DentalCareAPI.Entities;

namespace DentalCareAPI.Mappings;

public static class UserMappings
{
    public static User Apply(this User user, UserUpdateDto dto)
    {
        user.Name = dto.Name;
        user.Age = dto.Age;
        user.Gender = dto.Gender;
        user.IsSmoker = dto.IsSmoker;
        user.IsAlcoholUser = dto.IsAlcoholUser;
        user.HasOrthodontic = dto.HasOrthodontic;
        user.IsBruxism = dto.IsBruxism;
        return user;
    }

    public static UserResponse ToResponse(this User user) => new(
        user.Id,
        user.Email,
        user.Name,
        user.Age,
        user.Gender,
        user.IsSmoker,
        user.IsAlcoholUser,
        user.HasOrthodontic,
        user.IsBruxism,
        user.CreatedAt);
}
