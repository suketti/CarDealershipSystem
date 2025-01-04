using DealershipSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace DealershipSystem.Controllers;

[ApiController]
[Route("locations")]
public class LocationController : ControllerBase
{
    private readonly LocationService _locationService;

    public LocationController(LocationService locationService)
    {
        _locationService = locationService;
    }

    [HttpGet(Name = "GetLocations")]
    public async Task<IActionResult> Get()
    {
        var locations = await _locationService.GetAllLocationsAsync();
        if (locations.Count == 0)
        {
            return StatusCode(204); // No Content
        }

        return Ok(locations);
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
    public async Task<IActionResult> CreateLocation([FromBody] LocationDto location)
    {
        var result = await _locationService.CreateLocationAsync(location);
        return result;
    }
}