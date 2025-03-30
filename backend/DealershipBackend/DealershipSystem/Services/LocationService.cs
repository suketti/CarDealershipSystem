using AutoMapper;
using DealershipSystem.Context;
using DealershipSystem.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DealershipSystem.Services;

/// <summary>
/// Service for managing locations within the dealership system.
/// </summary>
public class LocationService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="LocationService"/> class.
    /// </summary>
    /// <param name="mapper">The mapper for mapping entities.</param>
    /// <param name="context">The application database context.</param>
    public LocationService(IMapper mapper, ApplicationDbContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    /// <summary>
    /// Gets all locations asynchronously.
    /// </summary>
    /// <returns>A list of all locations.</returns>
    public async Task<List<Location>> GetAllLocationsAsync()
    {
        var locations = await _context.Locations.ToListAsync();
        return locations;
    }

    /// <summary>
    /// Gets a location by ID asynchronously.
    /// </summary>
    /// <param name="id">The ID of the location.</param>
    /// <returns>The location DTO if found; otherwise, null.</returns>
    public async Task<LocationDto> GetLocationByIdAsync(int id)
    {
        var location = await _context.Locations.FirstOrDefaultAsync(i => i.ID == id);
        if (location == null)
        {
            return null;
        }

        return _mapper.Map<LocationDto>(location);
    }

    /// <summary>
    /// Creates a new location asynchronously.
    /// </summary>
    /// <param name="location">The location DTO.</param>
    /// <returns>An action result indicating the outcome of the creation.</returns>
    public async Task<IActionResult> CreateLocationAsync(LocationDto location)
    {
        var entity = _mapper.Map<Models.Location>(location);
        var prefecture = await _context.Prefectures.AsNoTracking()
            .FirstOrDefaultAsync(x => x.NameJP == entity.Address.Prefecture.NameJP);

        if (prefecture == null)
        {
            return new StatusCodeResult(422); // Unprocessable Entity
        }

        entity.Address.PrefectureId = prefecture.Id;
        entity.Address.Prefecture = null;

        _context.Locations.Add(entity);
        await _context.SaveChangesAsync();

        return new CreatedAtActionResult("GetById", "Location", new { id = entity.ID }, location);
    }

    /// <summary>
    /// Updates an existing location asynchronously.
    /// </summary>
    /// <param name="locationDto">The location DTO with updated information.</param>
    /// <returns>The updated location if successful; otherwise, null.</returns>
    public async Task<Location> UpdateLocationAsync(LocationDto locationDto)
    {
        var existingLocation = await _context.Locations
            .Include(l => l.Address)
            .ThenInclude(a => a.Prefecture)
            .FirstOrDefaultAsync(l => l.ID == locationDto.Id);

        if (existingLocation == null)
        {
            return null; // Location not found
        }

        // Check if the new prefecture already exists
        var existingPrefecture = await _context.Prefectures
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.NameJP == locationDto.Address.Prefecture.NameJP);

        if (existingPrefecture == null)
        {
            return null; // Prefecture not found, handle this case as needed
        }

        // Update the location details
        existingLocation.LocationName = locationDto.LocationName;
        existingLocation.Address.City = locationDto.Address.City;
        existingLocation.Address.CityRomanized = locationDto.Address.CityRomanized;
        existingLocation.Address.Street = locationDto.Address.Street;
        existingLocation.Address.StreetRomanized = locationDto.Address.StreetRomanized;
        existingLocation.Address.PrefectureId = existingPrefecture.Id;
        existingLocation.PhoneNumber = locationDto.PhoneNumber;
        existingLocation.MaxCapacity = locationDto.MaxCapacity;

        try
        {
            _context.Locations.Update(existingLocation);
            await _context.SaveChangesAsync();

            return existingLocation; 
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Error updating location", ex);
        }
    }

    /// <summary>
    /// Gets all prefectures asynchronously.
    /// </summary>
    /// <returns>A list of all prefectures.</returns>
    public async Task<List<PrefectureDTO>> GetAllPrefectures()
    {
        var prefectures = await _context.Prefectures.ToListAsync();
        return _mapper.Map<List<PrefectureDTO>>(prefectures);
    }

    /// <summary>
    /// Deletes a location asynchronously.
    /// </summary>
    /// <param name="locationId">The ID of the location to delete.</param>
    /// <returns>True if the location was deleted; otherwise, false.</returns>
    public async Task<bool> DeleteLocationAsync(int locationId)
    {
        var location = await _context.Locations.FindAsync(locationId);
        if (location == null)
        {
            return false; 
        }

        try
        {
            _context.Locations.Remove(location);
            await _context.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Error deleting location", ex);
        }
    }

    /// <summary>
    /// Gets the car usage in a location asynchronously.
    /// </summary>
    /// <param name="locationId">The ID of the location.</param>
    /// <returns>A tuple containing the maximum capacity and current usage if found; otherwise, null.</returns>
    public async Task<(int MaxCapacity, int CurrentUsage)?> GetCarUsageInLocationAsync(int locationId)
    {
        var location = await _context.Locations
            .Where(l => l.ID == locationId)
            .Select(l => new
            {
                l.MaxCapacity,
                CurrentUsage = _context.Cars.Count(c => c.LocationID == locationId)
            })
            .FirstOrDefaultAsync();

        if (location == null)
        {
            return null; // Location not found
        }

        return (location.MaxCapacity, location.CurrentUsage);
    }
}