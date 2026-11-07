using DentalCareAPI.DTOs;
using DentalCareAPI.Entities;

namespace DentalCareAPI.Auth;

public interface ITokenService
{
    AuthResponse CreateToken(User user);
}
