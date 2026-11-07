using DentalCareAPI.DTOs;
using DentalCareAPI.Entities;

namespace DentalCareAPI.Mappings;

public static class UserMappings
{
    public static User ToEntity(this UserCreateDto dto) => new User().Apply(dto);

    public static User Apply(this User user, UserCreateDto dto)
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

    public static UserCreateDto ToDto(this User user) => new()
    {
        Name = user.Name,
        Age = user.Age,
        Gender = user.Gender,
        IsSmoker = user.IsSmoker,
        IsAlcoholUser = user.IsAlcoholUser,
        HasOrthodontic = user.HasOrthodontic,
        IsBruxism = user.IsBruxism,
    };
}
