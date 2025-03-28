using System.Diagnostics;
using System.Security.Claims;
using DealershipSystem.DTO;
using DealershipSystem.Interfaces;
using DealershipSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DealershipSystem.Controllers;

[Route("api/reservations")]
[ApiController]
[Authorize]
public class ReservationController : ControllerBase
{
    private readonly IReservationService _service;
    private readonly IMessageService _messageService;
    private readonly IUserService _userService;

    public ReservationController(IReservationService service, IMessageService messageService, IUserService userService)
    {
        _service = service;
        _messageService = messageService;
        _userService = userService;
    }

    private string? GetUserId() => User.FindFirst("Id")?.Value;
    private bool IsAdmin() => User.IsInRole("Admin");

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReservationDTO>>> GetAll()
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized();
        return Ok(await _service.GetAllAsync(userId));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ReservationDTO>> GetById(int id)
    {
        var userId = GetUserId();
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
        
        var preferredLanguage = await _userService.GetPreferredLanguageAsync(Guid.Parse(userId));
        var messageContent = preferredLanguage switch
        {
            "jp" => "予約が作成されました。",
            "hu" => "Foglalás létrehozva.",
            _ => "Reservation created."
        };
        await _messageService.CreateMessageAsync(messageContent, Guid.Parse(userId));
        
        return CreatedAtAction(nameof(GetById), new { id = createdReservation.Id }, createdReservation);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateReservationDTO dto)
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized();
        
        var updatedReservation = await _service.UpdateAsync(id, dto, userId);
        if (updatedReservation == null) return NotFound();
        
        var preferredLanguage = await _userService.GetPreferredLanguageAsync(Guid.Parse(userId));
        var messageContent = preferredLanguage switch
        {
            "jp" => "予約が更新されました。",
            "hu" => "Foglalás frissítve.",
            _ => "Reservation updated."
        };
        await _messageService.CreateMessageAsync(messageContent, Guid.Parse(userId));
        
        return Ok(updatedReservation);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized();
        
        var success = await _service.DeleteAsync(id, userId, IsAdmin());
        if (!success) return NotFound();
        
        var preferredLanguage = await _userService.GetPreferredLanguageAsync(Guid.Parse(userId));
        var messageContent = preferredLanguage switch
        {
            "jp" => "予約が削除されました。",
            "hu" => "Foglalás törölve.",
            _ => "Reservation deleted."
        };
        await _messageService.CreateMessageAsync(messageContent, Guid.Parse(userId));
        
        return NoContent();
    }
}