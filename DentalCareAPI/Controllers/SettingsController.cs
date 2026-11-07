using DentalCareAPI.Auth;
using DentalCareAPI.DTOs;
using DentalCareAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DentalCareAPI.Controllers;

[ApiController]
[Route("api/settings")]
public class SettingsController : ControllerBase
{
    private readonly ISettingRepository _settingRepository;
    private readonly ICurrentUser _currentUser;

    public SettingsController(ISettingRepository settingRepository, ICurrentUser currentUser)
    {
        _settingRepository = settingRepository;
        _currentUser = currentUser;
    }

    [HttpGet]
    public async Task<ActionResult<SettingResponse>> GetSettings()
    {
        var setting = await _settingRepository.GetSettingByUserIdAsync(_currentUser.Id);
        return setting is null ? NotFound() : Ok(new SettingResponse(setting.Language));
    }

    [HttpPut]
    public async Task<IActionResult> UpdateSettings(SettingUpdateDto dto)
    {
        var setting = await _settingRepository.GetSettingByUserIdAsync(_currentUser.Id);
        if (setting is null)
            return NotFound();

        setting.Language = dto.Language;
        await _settingRepository.UpdateSettingAsync(setting);

        return NoContent();
    }
}
