using DentalCareAPI.Auth;
using DentalCareAPI.DTOs;
using DentalCareAPI.Interfaces;
using DentalCareAPI.Mappings;
using Microsoft.AspNetCore.Mvc;

namespace DentalCareAPI.Controllers;

[ApiController]
[Route("api/alarms")]
public class AlarmsController : ControllerBase
{
    private readonly IAlarmRepository _alarmRepository;
    private readonly ICurrentUser _currentUser;

    public AlarmsController(IAlarmRepository alarmRepository, ICurrentUser currentUser)
    {
        _alarmRepository = alarmRepository;
        _currentUser = currentUser;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AlarmResponse>>> GetAlarms()
    {
        var alarms = await _alarmRepository.GetAlarmsByUserIdAsync(_currentUser.Id);
        return Ok(alarms.Select(a => a.ToResponse()));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AlarmResponse>> GetAlarmById(int id)
    {
        var alarm = await _alarmRepository.GetAlarmByIdAsync(id, _currentUser.Id);
        return alarm is null ? NotFound() : Ok(alarm.ToResponse());
    }

    [HttpPost]
    public async Task<ActionResult<AlarmResponse>> CreateAlarm(AlarmCreateDto dto)
    {
        var alarm = dto.ToEntity(_currentUser.Id);
        await _alarmRepository.AddAlarmAsync(alarm);

        return CreatedAtAction(nameof(GetAlarmById), new { id = alarm.Id }, alarm.ToResponse());
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAlarm(int id, AlarmUpdateDto dto)
    {
        var alarm = await _alarmRepository.GetAlarmByIdAsync(id, _currentUser.Id);
        if (alarm is null)
            return NotFound();

        alarm.Apply(dto);
        await _alarmRepository.UpdateAlarmAsync(alarm);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAlarm(int id)
    {
        return await _alarmRepository.DeleteAlarmAsync(id, _currentUser.Id) ? NoContent() : NotFound();
    }
}
