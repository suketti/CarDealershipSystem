using DealershipSystem.DTO;
using DealershipSystem.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DealershipSystem.Controllers;

[Route("api/reservations")]
[ApiController]
public class ReservationController : ControllerBase
{
    private readonly IReservationService _service;

    public ReservationController(IReservationService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReservationDTO>>> GetAll()
    {
        return Ok(await _service.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ReservationDTO>> GetById(int id)
    {
        var reservation = await _service.GetByIdAsync(id);
        if (reservation == null) return NotFound();
        return Ok(reservation);
    }

    [HttpPost]
    public async Task<ActionResult<ReservationDTO>> Create([FromBody] CreateReservationDTO dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var createdReservation = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = createdReservation.Id }, createdReservation);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _service.DeleteAsync(id);
        if (!success) return NotFound();
        return NoContent();
    }
}