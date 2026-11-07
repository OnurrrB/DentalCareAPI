using DentalCareAPI.DTOs;
using DentalCareAPI.Entities;
using DentalCareAPI.Interfaces;
using DentalCareAPI.Mappings;
using Microsoft.AspNetCore.Mvc;

namespace DentalCareAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BrushLogController : ControllerBase
    {
        private readonly IBrushLogRepository _brushLogRepository;

        public BrushLogController(IBrushLogRepository brushLogRepository)
        {
            _brushLogRepository = brushLogRepository;
        }

        // POST: api/brushlog
        [HttpPost]
        public async Task<IActionResult> CreateBrushLog([FromBody] BrushLogCreateDto dto)
        {
            var brushLog = dto.ToEntity();
            await _brushLogRepository.AddBrushLogAsync(brushLog);

            return CreatedAtAction(nameof(GetBrushLogById), new { id = brushLog.Id }, brushLog);
        }

        // GET: api/brushlog/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBrushLogById(int id)
        {
            var log = await _brushLogRepository.GetBrushLogByIdAsync(id);
            if (log == null)
                return NotFound();

            return Ok(log);
        }

        // GET: api/brushlog/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetBrushLogsByUserId(int userId)
        {
            var logs = await _brushLogRepository.GetBrushLogsByUserIdAsync(userId);
            return Ok(logs);
        }

        // DELETE: api/brushlog/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBrushLog(int id)
        {
            await _brushLogRepository.DeleteBrushLogAsync(id);
            return NoContent();
        }
    }
}
