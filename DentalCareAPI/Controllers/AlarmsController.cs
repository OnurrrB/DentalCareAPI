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

    public AlarmsController(IAlarmRepository alarmRepository)
    {
        _alarmRepository = alarmRepository;
    }

    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<AlarmResponse>>> GetAlarms(int userId)
    {
        var alarms = await _alarmRepository.GetAlarmsByUserIdAsync(userId);
        return Ok(alarms.Select(a => a.ToResponse()));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AlarmResponse>> GetAlarmById(int id)
    {
        var alarm = await _alarmRepository.GetAlarmByIdAsync(id);
        return alarm is null ? NotFound() : Ok(alarm.ToResponse());
    }

    [HttpPost]
    public async Task<ActionResult<AlarmResponse>> CreateAlarm(AlarmCreateDto dto)
    {
        var alarm = dto.ToEntity(dto.UserId);
        await _alarmRepository.AddAlarmAsync(alarm);

        return CreatedAtAction(nameof(GetAlarmById), new { id = alarm.Id }, alarm.ToResponse());
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAlarm(int id, AlarmUpdateDto dto)
    {
        var alarm = await _alarmRepository.GetAlarmByIdAsync(id);
        if (alarm is null)
            return NotFound();

        alarm.Apply(dto);
        await _alarmRepository.UpdateAlarmAsync(alarm);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAlarm(int id)
    {
        return await _alarmRepository.DeleteAlarmAsync(id) ? NoContent() : NotFound();
    }
}
