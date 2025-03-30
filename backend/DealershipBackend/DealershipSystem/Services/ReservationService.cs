using DealershipSystem.Context;
using DealershipSystem.DTO;
using DealershipSystem.Interfaces;
using DealershipSystem.Models;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Service for managing reservations within the dealership system.
/// </summary>
public class ReservationService : IReservationService
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="ReservationService"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    public ReservationService(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Gets all reservations for a specific user asynchronously.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>A list of reservation DTOs for the specified user.</returns>
    public async Task<IEnumerable<ReservationDTO>> GetAllAsync(string userId)
    {
        return await _context.Reservations
            .Where(r => r.UserId == Guid.Parse(userId))
            .Select(r => new ReservationDTO()
            {
                Id = r.Id,
                UserId = r.UserId,
                CarId = r.CarId,
                Date = r.Date
            }).ToListAsync();
    }

    /// <summary>
    /// Gets a reservation by ID for a specific user asynchronously.
    /// </summary>
    /// <param name="id">The ID of the reservation.</param>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>The reservation DTO if found; otherwise, null.</returns>
    public async Task<ReservationDTO?> GetByIdAsync(int id, string userId)
    {
        var reservation = await _context.Reservations
            .FirstOrDefaultAsync(r => r.Id == id && r.UserId == Guid.Parse(userId));
        
        return reservation == null ? null : new ReservationDTO()
        {
            Id = reservation.Id,
            UserId = reservation.UserId,
            CarId = reservation.CarId,
            Date = reservation.Date
        };
    }

    /// <summary>
    /// Creates a new reservation asynchronously.
    /// </summary>
    /// <param name="dto">The DTO containing the reservation details.</param>
    /// <param name="userId">The ID of the user creating the reservation.</param>
    /// <returns>The created reservation DTO if successful; otherwise, null.</returns>
    public async Task<ReservationDTO?> CreateAsync(CreateReservationDTO dto, string userId)
    {
        if (dto.UserId != Guid.Parse(userId)) return null;
        
        var reservation = new Reservation
        {
            UserId = dto.UserId,
            CarId = dto.CarId,
            Date = dto.Date
        };
        
        _context.Reservations.Add(reservation);
        await _context.SaveChangesAsync();
        
        return new ReservationDTO()
        {
            Id = reservation.Id,
            UserId = reservation.UserId,
            CarId = reservation.CarId,
            Date = reservation.Date
        };
    }

    /// <summary>
    /// Updates an existing reservation asynchronously.
    /// </summary>
    /// <param name="id">The ID of the reservation to update.</param>
    /// <param name="dto">The DTO containing the updated reservation details.</param>
    /// <param name="userId">The ID of the user updating the reservation.</param>
    /// <returns>The updated reservation DTO if successful; otherwise, null.</returns>
    public async Task<ReservationDTO?> UpdateAsync(int id, UpdateReservationDTO dto, string userId)
    {
        var reservation = await _context.Reservations
            .FirstOrDefaultAsync(r => r.Id == id && r.UserId == Guid.Parse(userId));
        
        if (reservation == null) return null;
        
        reservation.CarId = dto.CarId;
        reservation.Date = dto.Date;
        
        await _context.SaveChangesAsync();
        
        return new ReservationDTO()
        {
            Id = reservation.Id,
            UserId = reservation.UserId,
            CarId = reservation.CarId,
            Date = reservation.Date
        };
    }

    /// <summary>
    /// Deletes a reservation asynchronously.
    /// </summary>
    /// <param name="id">The ID of the reservation to delete.</param>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="isAdmin">Indicates whether the user is an admin.</param>
    /// <returns>True if the reservation was deleted; otherwise, false.</returns>
    public async Task<bool> DeleteAsync(int id, string userId, bool isAdmin)
    {
        var reservation = await _context.Reservations
            .FirstOrDefaultAsync(r => r.Id == id && (r.UserId == Guid.Parse(userId) || isAdmin));
        
        if (reservation == null) return false;
        
        _context.Reservations.Remove(reservation);
        await _context.SaveChangesAsync();
        return true;
    }
}