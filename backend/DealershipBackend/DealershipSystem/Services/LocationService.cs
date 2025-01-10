using AutoMapper;
using DealershipSystem.Context;
using DealershipSystem.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DealershipSystem.Services;

public class LocationService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public LocationService(IMapper mapper, ApplicationDbContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<List<LocationDto>> GetAllLocationsAsync()
    {
        var locations = await _context.Locations.ToListAsync();
        var locationDtos = _mapper.Map<List<LocationDto>>(locations);

        return locationDtos;
    }

    public async Task<LocationDto> GetLocationByIdAsync(int id)
    {
        var location = await _context.Locations.FirstOrDefaultAsync(i => i.ID == id);
        if (location == null)
        {
            return null;
        }

        return _mapper.Map<LocationDto>(location);
    }

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
    
    public async Task<Location> UpdateLocationAsync(LocationDto locationDto)
    {
        var existingLocation = await _context.Locations.FindAsync(locationDto.Id);
        if (existingLocation == null)
        {
            return null; // Location not found
        }
        existingLocation.LocationName = locationDto.LocationName;
        existingLocation.Address.City = locationDto.Address.City;
        existingLocation.Address.CityRomanized = locationDto.Address.CityRomanized;
        existingLocation.Address.Street = locationDto.Address.Street;
        existingLocation.Address.StreetRomanized = locationDto.Address.StreetRomanized;
        existingLocation.Address.Prefecture.Name = locationDto.Address.Prefecture.Name;
        existingLocation.Address.Prefecture.NameJP = locationDto.Address.Prefecture.NameJP;
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
}