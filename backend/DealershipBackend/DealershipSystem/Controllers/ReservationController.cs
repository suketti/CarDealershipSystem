using System.Diagnostics;
using System.Security.Claims;
using DealershipSystem.DTO;
using DealershipSystem.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DealershipSystem.Controllers;

[Route("api/reservations")]
[ApiController]
[Authorize]
public class ReservationController : ControllerBase
{
    private readonly IReservationService _service;

    public ReservationController(IReservationService service)
    {
        _service = service;
    }

    private string? GetUserId() => User.FindFirst("Id")?.Value;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReservationDTO>>> GetAll()
    {
        var userId = GetUserId();
        if (userId! == null) return Unauthorized();
        return Ok(await _service.GetAllAsync(userId));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ReservationDTO>> GetById(int id)
    {
        var userId = GetUserId();
        Debug.WriteLine(userId);
        if (userId == null) return Unauthorized();
        
        var reservation = await _service.GetByIdAsync(id, userId);
        if (reservation == null) return NotFound();
        return Ok(reservation);
    }

    [HttpPost]
    public async Task<ActionResult<ReservationDTO>> Create([FromBody] CreateReservationDTO dto)
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized();
        
        var createdReservation = await _service.CreateAsync(dto, userId);
        if (createdReservation == null) return Forbid();
        
        return CreatedAtAction(nameof(GetById), new { id = createdReservation.Id }, createdReservation);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized();
        
        var success = await _service.DeleteAsync(id, userId);
        if (!success) return NotFound();
        return NoContent();
    }
}