using AutoMapper;
using DentalCareAPI.DTOs;
using DentalCareAPI.Entities;
using DentalCareAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class AlarmController : ControllerBase
{
    private readonly IAlarmRepository _alarmRepository;
    private readonly IMapper _mapper;

    public AlarmController(IAlarmRepository alarmRepository, IMapper mapper)
    {
        _alarmRepository = alarmRepository;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAlarm([FromBody] AlarmCreateDto alarmCreateDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var alarmEntity = _mapper.Map<Alarm>(alarmCreateDto);

        await _alarmRepository.AddAlarmAsync(alarmEntity);

        // Eğer AddAlarmAsync ID'yi set etmiyorsa, ekleme sonrası ID almayı garanti et
        // Örnek: alarmEntity.Id = ...

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

        var alarmEntity = _mapper.Map<Alarm>(alarmUpdateDto);

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
