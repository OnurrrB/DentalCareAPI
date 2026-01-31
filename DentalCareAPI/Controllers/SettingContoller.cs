using AutoMapper;
using DentalCareAPI.DTOs;
using DentalCareAPI.Entities;
using DentalCareAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DentalCareAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SettingController : ControllerBase
    {
        private readonly ISettingRepository _settingRepository;
        private readonly IMapper _mapper;

        public SettingController(ISettingRepository settingRepository, IMapper mapper)
        {
            _settingRepository = settingRepository;
            _mapper = mapper;
        }

        // POST: api/setting
        [HttpPost]
        public async Task<IActionResult> CreateOrUpdateSetting([FromBody] SettingCreateOrUpdateDto dto)
        {
            var existingSetting = await _settingRepository.GetSettingByUserIdAsync(dto.UserId);

            if (existingSetting == null)
            {
                var setting = _mapper.Map<Setting>(dto);
                await _settingRepository.AddSettingAsync(setting);
                return CreatedAtAction(nameof(GetSetting), new { userId = setting.UserId }, setting);
            }
            else
            {
                existingSetting.Language = dto.Language;
                await _settingRepository.UpdateSettingAsync(existingSetting);
                return NoContent();
            }
        }

        // GET: api/setting/{userId}
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetSetting(int userId)
        {
            var setting = await _settingRepository.GetSettingByUserIdAsync(userId);
            if (setting == null)
                return NotFound();

            return Ok(setting);
        }

        // DELETE: api/setting/{userId}
        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteSetting(int userId)
        {
            var setting = await _settingRepository.GetSettingByUserIdAsync(userId);
            if (setting == null)
                return NotFound();

            await _settingRepository.DeleteSettingAsync(setting.Id);
            return NoContent();
        }
    }
}
