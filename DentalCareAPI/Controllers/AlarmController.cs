using DentalCareAPI.DTOs;
using DentalCareAPI.Entities;
using DentalCareAPI.Interfaces;
using DentalCareAPI.Mappings;
using Microsoft.AspNetCore.Mvc;

namespace DentalCareAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AlarmController : ControllerBase
{
    private readonly IAlarmRepository _alarmRepository;

    public AlarmController(IAlarmRepository alarmRepository)
    {
        _alarmRepository = alarmRepository;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAlarm([FromBody] AlarmCreateDto alarmCreateDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var alarmEntity = alarmCreateDto.ToEntity();

        await _alarmRepository.AddAlarmAsync(alarmEntity);


        return CreatedAtAction(nameof(GetAlarmById), new { id = alarmEntity.Id }, alarmEntity);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAlarmById(int id)
    {
        var alarm = await _alarmRepository.GetAlarmByIdAsync(id);
        if (alarm == null)
            return NotFound();

        return Ok(alarm);
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetAlarmsByUserId(int userId)
    {
        var alarms = await _alarmRepository.GetAlarmsByUserIdAsync(userId);
        return Ok(alarms);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAlarm(int id, [FromBody] AlarmCreateDto alarmUpdateDto)
    {
        if (id != alarmUpdateDto.UserId)
            return BadRequest("Id mismatch");

        var alarmEntity = alarmUpdateDto.ToEntity();

        await _alarmRepository.UpdateAlarmAsync(alarmEntity);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAlarm(int id)
    {
        await _alarmRepository.DeleteAlarmAsync(id);
        return NoContent();
    }
}
