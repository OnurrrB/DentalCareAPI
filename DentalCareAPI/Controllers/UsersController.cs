using DentalCareAPI.Auth;
using DentalCareAPI.DTOs;
using DentalCareAPI.Interfaces;
using DentalCareAPI.Mappings;
using Microsoft.AspNetCore.Mvc;

namespace DentalCareAPI.Controllers;

[ApiController]
[Route("api/users/me")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUser _currentUser;

    public UsersController(IUserRepository userRepository, ICurrentUser currentUser)
    {
        _userRepository = userRepository;
        _currentUser = currentUser;
    }

    [HttpGet]
    public async Task<ActionResult<UserResponse>> GetProfile()
    {
        var user = await _userRepository.GetUserByIdAsync(_currentUser.Id);
        return user is null ? NotFound() : Ok(user.ToResponse());
    }

    [HttpPut]
    public async Task<IActionResult> UpdateProfile(UserUpdateDto dto)
    {
        var user = await _userRepository.GetUserByIdAsync(_currentUser.Id);
        if (user is null)
            return NotFound();

        user.Apply(dto);
        await _userRepository.UpdateUserAsync(user);

        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteAccount()
    {
        return await _userRepository.DeleteUserAsync(_currentUser.Id) ? NoContent() : NotFound();
    }
}
