using DentalCareAPI.Auth;
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
    private readonly ICurrentUser _currentUser;

    public BrushLogsController(IBrushLogRepository brushLogRepository, ICurrentUser currentUser)
    {
        _brushLogRepository = brushLogRepository;
        _currentUser = currentUser;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResponse<BrushLogResponse>>> GetBrushLogs(int page = 1, int pageSize = 20)
    {
        page = Math.Max(page, 1);
        pageSize = Math.Clamp(pageSize, 1, MaxPageSize);

        var (items, totalCount) = await _brushLogRepository.GetBrushLogsByUserIdAsync(
            _currentUser.Id, page, pageSize);

        return Ok(new PagedResponse<BrushLogResponse>(
            items.Select(b => b.ToResponse()).ToList(), page, pageSize, totalCount));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BrushLogResponse>> GetBrushLogById(int id)
    {
        var log = await _brushLogRepository.GetBrushLogByIdAsync(id, _currentUser.Id);
        return log is null ? NotFound() : Ok(log.ToResponse());
    }

    [HttpPost]
    public async Task<ActionResult<BrushLogResponse>> CreateBrushLog(BrushLogCreateDto dto)
    {
        var log = dto.ToEntity(_currentUser.Id);
        await _brushLogRepository.AddBrushLogAsync(log);

        return CreatedAtAction(nameof(GetBrushLogById), new { id = log.Id }, log.ToResponse());
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBrushLog(int id)
    {
        return await _brushLogRepository.DeleteBrushLogAsync(id, _currentUser.Id) ? NoContent() : NotFound();
    }
}
