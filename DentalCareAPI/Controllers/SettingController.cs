using DentalCareAPI.DTOs;
using DentalCareAPI.Entities;
using DentalCareAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DentalCareAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SettingController : ControllerBase
    {
        private readonly ISettingRepository _settingRepository;

        public SettingController(ISettingRepository settingRepository)
        {
            _settingRepository = settingRepository;
        }

        // POST: api/setting
        [HttpPost]
        public async Task<IActionResult> CreateOrUpdateSetting([FromBody] SettingUpdateDto dto)
        {
            var existingSetting = await _settingRepository.GetSettingByUserIdAsync(dto.UserId);

            if (existingSetting == null)
            {
                var setting = new Setting { UserId = dto.UserId, Language = dto.Language };
                await _settingRepository.AddSettingAsync(setting);
                return CreatedAtAction(nameof(GetSetting), new { userId = setting.UserId }, new SettingResponse(setting.Language));
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

            return Ok(new SettingResponse(setting.Language));
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
