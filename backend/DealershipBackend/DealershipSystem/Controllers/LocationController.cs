using System.ComponentModel.DataAnnotations;
using DealershipSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DealershipSystem.Controllers;

[ApiController]
[Route("api/locations")]
public class LocationController : ControllerBase
{
    private readonly LocationService _locationService;

    public LocationController(LocationService locationService)
    {
        _locationService = locationService;
    }
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var locations = await _locationService.GetAllLocationsAsync();
        if (locations.Count == 0)
        {
            return StatusCode(204); // No Content
        }

        return Ok(locations);
    }
    
    [HttpGet("prefectures")]
    public async Task<IActionResult> GetPrefectures()
    {
        var prefectures = await _locationService.GetAllPrefectures();
        return Ok(prefectures);
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var locationDto = await _locationService.GetLocationByIdAsync(id);
        if (locationDto == null)
        {
            return NotFound();
        }

        return Ok(locationDto);
    }

    [HttpPost]
    //[Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateLocation([FromBody] LocationDto location)
    {
        var result = await _locationService.CreateLocationAsync(location);
        return result;
    }
    [Authorize(Roles = "Admin")]
    [HttpPut("update")]
    public async Task<IActionResult> UpdateLocationAsync([FromBody] LocationDto locationDto)
    {
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(locationDto, serviceProvider: null, items: null);
        if (!Validator.TryValidateObject(locationDto, validationContext, validationResults, true))
        {
            return BadRequest(validationResults.Select(vr => vr.ErrorMessage));
        }

        try
        {
            
            var updatedLocation = await _locationService.UpdateLocationAsync(locationDto);
            
            if (updatedLocation == null)
            {
                return NotFound("Location not found.");
            }
            
            return Ok(updatedLocation);
        }
        catch (InvalidOperationException ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
    [HttpDelete("{locationId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteLocationAsync(int locationId)
    {
        try
        {
            var isDeleted = await _locationService.DeleteLocationAsync(locationId);
            
            if (!isDeleted)
            {
                return NoContent(); 
            }
            
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}