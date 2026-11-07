using DentalCareAPI.DTOs;
using DentalCareAPI.Interfaces;
using DentalCareAPI.Mappings;
using Microsoft.AspNetCore.Mvc;

namespace DentalCareAPI.Controllers;

[ApiController]
[Route("api/brush-logs")]
public class BrushLogsController : ControllerBase
{
    private const int MaxPageSize = 100;

    private readonly IBrushLogRepository _brushLogRepository;

    public BrushLogsController(IBrushLogRepository brushLogRepository)
    {
        _brushLogRepository = brushLogRepository;
    }

    [HttpGet("user/{userId}")]
    public async Task<ActionResult<PagedResponse<BrushLogResponse>>> GetBrushLogs(
        int userId, int page = 1, int pageSize = 20)
    {
        page = Math.Max(page, 1);
        pageSize = Math.Clamp(pageSize, 1, MaxPageSize);

        var (items, totalCount) = await _brushLogRepository.GetBrushLogsByUserIdAsync(userId, page, pageSize);

        return Ok(new PagedResponse<BrushLogResponse>(
            items.Select(b => b.ToResponse()).ToList(), page, pageSize, totalCount));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BrushLogResponse>> GetBrushLogById(int id)
    {
        var log = await _brushLogRepository.GetBrushLogByIdAsync(id);
        return log is null ? NotFound() : Ok(log.ToResponse());
    }

    [HttpPost]
    public async Task<ActionResult<BrushLogResponse>> CreateBrushLog(BrushLogCreateDto dto)
    {
        var log = dto.ToEntity(dto.UserId);
        await _brushLogRepository.AddBrushLogAsync(log);

        return CreatedAtAction(nameof(GetBrushLogById), new { id = log.Id }, log.ToResponse());
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBrushLog(int id)
    {
        return await _brushLogRepository.DeleteBrushLogAsync(id) ? NoContent() : NotFound();
    }
}
