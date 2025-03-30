using System.Security.Claims;
using AutoMapper;
using DealershipSystem.Context;
using DealershipSystem.DTO;
using DealershipSystem.Interfaces;
using DealershipSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace DealershipSystem.Services;

/// <summary>
/// Service for managing saved cars within the dealership system.
/// </summary>
public class SavedCarService : ISavedCarService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="SavedCarService"/> class.
    /// </summary>
    /// <param name="dbContext">The application database context.</param>
    /// <param name="httpContextAccessor">The HTTP context accessor.</param>
    /// <param name="mapper">The mapper for mapping entities.</param>
    public SavedCarService(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor, IMapper mapper)
    {
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
    }

    /// <summary>
    /// Gets the saved cars for a specific user asynchronously.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>A list of saved car DTOs for the specified user.</returns>
    /// <exception cref="UnauthorizedAccessException">Thrown if the current user is not authorized to access the saved cars.</exception>
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

    /// <summary>
    /// Saves a car for a specific user asynchronously.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="carId">The ID of the car to save.</param>
    /// <exception cref="UnauthorizedAccessException">Thrown if the current user is not authorized to save the car.</exception>
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

    /// <summary>
    /// Removes a saved car for a specific user asynchronously.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="carId">The ID of the car to remove.</param>
    /// <exception cref="UnauthorizedAccessException">Thrown if the current user is not authorized to remove the saved car.</exception>
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

    /// <summary>
    /// Gets the user ID from the JWT.
    /// </summary>
    /// <returns>The user ID as a Guid.</returns>
    /// <exception cref="UnauthorizedAccessException">Thrown if the user ID is not found in the JWT or HttpContext is null.</exception>
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