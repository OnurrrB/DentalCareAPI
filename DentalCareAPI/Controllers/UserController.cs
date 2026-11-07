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
        public async Task<ActionResult<IEnumerable<UserResponse>>> GetUsers()
        {
            var users = await _userRepository.GetAllUsersAsync();
            var usersDto = users.Select(u => u.ToResponse());
            return Ok(usersDto);
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponse>> GetUser(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user.ToResponse());
        }

        // POST: api/User
        [HttpPost]
        public async Task<ActionResult<UserResponse>> CreateUser(UserCreateDto userCreateDto)
        {
            var user = new User().Apply(userCreateDto);
            user.CreatedAt = DateTime.UtcNow;

            await _userRepository.AddUserAsync(user);

            var userDto = user.ToResponse();
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, userDto);
        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserUpdateDto userDto)
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
            return await _userRepository.DeleteUserAsync(id) ? NoContent() : NotFound();
        }
    }
}
