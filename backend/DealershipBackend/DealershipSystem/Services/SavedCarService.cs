using System.Security.Claims;
using AutoMapper;
using DealershipSystem.Context;
using DealershipSystem.DTO;
using DealershipSystem.Interfaces;
using DealershipSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace DealershipSystem.Services;

public class SavedCarService : ISavedCarService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;

    public SavedCarService(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor, IMapper mapper)
    {
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
    }

    public async Task<List<CarDTO>> GetSavedCarsAsync(Guid userId)
    {
        var currentUserId = GetUserIdFromJwt();
        if (currentUserId != userId)
        {
            throw new UnauthorizedAccessException("Unauthorized access.");
        }

        var savedCars = await _dbContext.Cars
            .Where(car => _dbContext.SavedCars
                .Where(sc => sc.UserId == userId)
                .Select(sc => sc.CarId)
                .Contains(car.ID))
            .Include(car => car.Brand)
            .Include(car => car.CarModel)
            .Include(car => car.BodyType)
            .Include(car => car.Location)
            .Include(car => car.EngineSize)
            .Include(car => car.FuelType)
            .Include(car => car.DriveTrain)
            .Include(car => car.TransmissionType)
            .Include(car => car.Color)
            .ToListAsync();

        return _mapper.Map<List<CarDTO>>(savedCars);
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
        if (_httpContextAccessor.HttpContext == null)
        {
            throw new UnauthorizedAccessException("HttpContext is null.");
        }

        var userIdClaim = _httpContextAccessor.HttpContext.User.Claims
            .FirstOrDefault(c => c.Type == "Id");

        if (userIdClaim == null)
        {
            throw new UnauthorizedAccessException("User ID not found in JWT.");
        }

        return Guid.Parse(userIdClaim.Value);
    }

}
