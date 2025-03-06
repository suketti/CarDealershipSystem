using DealershipSystem.Context;
using DealershipSystem.DTO;
using DealershipSystem.Interfaces;
using DealershipSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace DealershipSystem.Services;

public class ReservationService : IReservationService
{
    private readonly ApplicationDbContext _context;

    public ReservationService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ReservationDTO>> GetAllAsync()
    {
        return await _context.Reservations.Select(r => new ReservationDTO()
        {
            Id = r.Id,
            UserId = r.UserId,
            CarId = r.CarId,
            Date = r.Date
        }).ToListAsync();
    }

    public async Task<ReservationDTO> GetByIdAsync(int id)
    {
        var reservation = await _context.Reservations.FindAsync(id);
        if (reservation == null) return null;
        return new ReservationDTO() { Id = reservation.Id, UserId = reservation.UserId, CarId = reservation.CarId, Date = reservation.Date };
    }

    public async Task<ReservationDTO> CreateAsync(CreateReservationDTO dto)
    {
        var reservation = new Reservation { UserId = dto.UserId, CarId = dto.CarId, Date = dto.Date };
        _context.Reservations.Add(reservation);
        await _context.SaveChangesAsync();
        return new ReservationDTO() { Id = reservation.Id, UserId = reservation.UserId, CarId = reservation.CarId, Date = reservation.Date };
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var reservation = await _context.Reservations.FindAsync(id);
        if (reservation == null) return false;
        _context.Reservations.Remove(reservation);
        await _context.SaveChangesAsync();
        return true;
    }
}

