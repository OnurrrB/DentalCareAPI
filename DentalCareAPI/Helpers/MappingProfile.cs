using AutoMapper;
using DentalCareAPI.DTOs;
using DentalCareAPI.Entities;

namespace DentalCareAPI.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // UserCreateDto → User
            CreateMap<UserCreateDto, User>();

            // User → UserDto (eğer varsa)
            CreateMap<User, UserCreateDto>();

            // AlarmCreateDto → Alarm
            CreateMap<AlarmCreateDto, Alarm>();

            CreateMap<BrushLogCreateDto, BrushLog>();

            CreateMap<SettingCreateOrUpdateDto, Setting>();



        }
    }
}

