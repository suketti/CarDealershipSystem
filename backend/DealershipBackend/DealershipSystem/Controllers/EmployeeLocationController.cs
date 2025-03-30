using DealershipSystem.Models;
using DealershipSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DealershipSystem.Controllers;

[Route("api/employeeLocations")]
[ApiController]
public class EmployeeLocationController : ControllerBase
{
    private readonly EmployeeLocationService _employeeLocationService;

    public EmployeeLocationController(EmployeeLocationService employeeLocationService)
    {
        _employeeLocationService = employeeLocationService;
    }

    // GET: api/employeeLocations
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<List<EmployeeLocation>>> GetEmployeeLocations()
    {
        var employeeLocations = await _employeeLocationService.GetAllEmployeeLocationsAsync();
        return Ok(employeeLocations);
    }

    // GET: api/employeeLocations/{employeeId}
    [HttpGet("{employeeId}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<EmployeeLocation>> GetEmployeeLocation(Guid employeeId)
    {
        var employeeLocation = await _employeeLocationService.GetEmployeeLocationByEmployeeIdAsync(employeeId);

        if (employeeLocation == null)
            return NotFound(new { Message = "Employee location not found" });

        return Ok(employeeLocation);
    }

    // POST: api/employeeLocations
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddEmployeeLocation(EmployeeLocation employeeLocation)
    {
        var success = await _employeeLocationService.AddEmployeeLocationAsync(employeeLocation);
        if (!success)
            return BadRequest(new { Message = "Failed to add employee location" });

        return CreatedAtAction(nameof(GetEmployeeLocation), new { employeeId = employeeLocation.EmployeeId },
            employeeLocation);
    }
}