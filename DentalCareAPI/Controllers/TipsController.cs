using DentalCareAPI.DTOs;
using DentalCareAPI.Interfaces;
using DentalCareAPI.Mappings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DentalCareAPI.Controllers;

[AllowAnonymous]
[ApiController]
[Route("api/tips")]
public class TipsController : ControllerBase
{
    private readonly ITipRepository _tipRepository;

    public TipsController(ITipRepository tipRepository)
    {
        _tipRepository = tipRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TipResponse>>> GetTips()
    {
        var tips = await _tipRepository.GetAllTipsAsync();
        return Ok(tips.Select(t => t.ToResponse()));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TipResponse>> GetTipById(int id)
    {
        var tip = await _tipRepository.GetTipByIdAsync(id);
        return tip is null ? NotFound() : Ok(tip.ToResponse());
    }
}
