using AutoMapper;
using DentalCareAPI.DTOs;
using DentalCareAPI.Entities;
using DentalCareAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DentalCareAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserCreateDto>>> GetUsers()
        {
            var users = await _userRepository.GetAllUsersAsync();
            var usersDto = _mapper.Map<IEnumerable<UserCreateDto>>(users);
            return Ok(usersDto);
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserCreateDto>> GetUser(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(_mapper.Map<UserCreateDto>(user));
        }

        // POST: api/User
        [HttpPost]
        public async Task<ActionResult<UserCreateDto>> CreateUser(UserCreateDto userCreateDto)
        {
            var user = _mapper.Map<User>(userCreateDto);
            user.CreatedAt = DateTime.UtcNow;

            await _userRepository.AddUserAsync(user);

            var userDto = _mapper.Map<UserCreateDto>(user);
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, userDto);
        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserCreateDto userDto)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null) return NotFound();

            _mapper.Map(userDto, user);
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
