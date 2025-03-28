using DealershipSystem.Context;
using DealershipSystem.DTO;
using DealershipSystem.Interfaces;
using DealershipSystem.Models;
using Microsoft.EntityFrameworkCore;

public class ReservationService : IReservationService
{
    private readonly ApplicationDbContext _context;

    public ReservationService(ApplicationDbContext context)
    {
        _context = context;
    }

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