using System.Security.Claims;
using DealershipSystem.Context;
using DealershipSystem.Interfaces;
using DealershipSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace DealershipSystem.Services;

public class SavedCarService : ISavedCarService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SavedCarService(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor)
    {
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<List<int>> GetSavedCarsAsync(Guid userId)
    {
        var currentUserId = GetUserIdFromJwt();
        if (currentUserId != userId)
        {
            throw new UnauthorizedAccessException("Unauthorized access.");
        }

        return await _dbContext.SavedCars
            .Where(sc => sc.UserId == userId)
            .Select(sc => sc.CarId)
            .ToListAsync();
    }

    public async Task SaveCarAsync(Guid userId, int carId)
    {
        var currentUserId = GetUserIdFromJwt();
        if (currentUserId != userId)
        {
            throw new UnauthorizedAccessException("Unauthorized access.");
        }

        var existingSavedCar = await _dbContext.SavedCars
            .FirstOrDefaultAsync(sc => sc.UserId == userId && sc.CarId == carId);

        if (existingSavedCar == null)
        {
            var savedCar = new SavedCar
            {
                UserId = userId,
                CarId = carId
            };

            _dbContext.SavedCars.Add(savedCar);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task RemoveSavedCarAsync(Guid userId, int carId)
    {
        var currentUserId = GetUserIdFromJwt();
        if (currentUserId != userId)
        {
            throw new UnauthorizedAccessException("Unauthorized access.");
        }

        var savedCar = await _dbContext.SavedCars
            .FirstOrDefaultAsync(sc => sc.UserId == userId && sc.CarId == carId);

        if (savedCar != null)
        {
            _dbContext.SavedCars.Remove(savedCar);
            await _dbContext.SaveChangesAsync();
        }
    }

    private Guid GetUserIdFromJwt()
    {
        var userIdClaim = _httpContextAccessor.HttpContext.User.Claims
            .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

        if (userIdClaim == null)
        {
            throw new UnauthorizedAccessException("User ID not found in JWT.");
        }

        return Guid.Parse(userIdClaim.Value);
    }
}
