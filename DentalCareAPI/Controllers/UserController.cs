using DentalCareAPI.DTOs;
using DentalCareAPI.Entities;
using DentalCareAPI.Interfaces;
using DentalCareAPI.Mappings;
using Microsoft.AspNetCore.Mvc;

namespace DentalCareAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserCreateDto>>> GetUsers()
        {
            var users = await _userRepository.GetAllUsersAsync();
            var usersDto = users.Select(u => u.ToDto());
            return Ok(usersDto);
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserCreateDto>> GetUser(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user.ToDto());
        }

        // POST: api/User
        [HttpPost]
        public async Task<ActionResult<UserCreateDto>> CreateUser(UserCreateDto userCreateDto)
        {
            var user = userCreateDto.ToEntity();
            user.CreatedAt = DateTime.UtcNow;

            await _userRepository.AddUserAsync(user);

            var userDto = user.ToDto();
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, userDto);
        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserCreateDto userDto)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null) return NotFound();

            user.Apply(userDto);
            await _userRepository.UpdateUserAsync(user);

            return NoContent();
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _userRepository.DeleteUserAsync(id);
            return NoContent();
        }
    }
}
