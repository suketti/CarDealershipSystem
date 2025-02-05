using System.ComponentModel.DataAnnotations;
using DealershipSystem.DTO;
using DealershipSystem.Models;
using DealershipSystem.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace DealershipSystem.Controllers;

[ApiController]
[Route("api/cars/models")]
public class CarModelController : ControllerBase
{
    private readonly CarModelService _carModelService;

    public CarModelController(CarModelService carModelService)
    {
        _carModelService = carModelService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllCars()
    {
        var models = await _carModelService.GetAllCarModelsAsync();
        return Ok(models);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateCarModel([FromBody] CreateCarModelDTO carModelDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var createdModel = await _carModelService.CreateCarModelAsync(carModelDto);
            return CreatedAtAction(nameof(CreateCarModel), new { id = createdModel.ID }, createdModel);
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    
    [HttpGet("search")]
    public async Task<ActionResult<List<CarModelDTO>>> SearchCarModels(
        [FromQuery] int? makerID,
        [FromQuery] int? startYear,
        [FromQuery] int? endYear,
        [FromQuery] int? passengerCount)
    {
        var carModels = await _carModelService.GetCarModelsFilteredAsync(makerID, startYear, endYear, passengerCount);
    
        return Ok(carModels);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCarModel(int id, [FromBody] UpdateCarModelDTO updatedCarModel)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var updatedModel = await _carModelService.UpdateCarModelAsync(id, updatedCarModel);
            if (updatedModel == null)
            {
                return NotFound(new { message = "Car model not found" });
            }
            return Ok(updatedModel);
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCarModel(int id)
    {
        var success = await _carModelService.DeleteCarModelAsync(id);
        if (!success)
        {
            return NotFound(new { message = "Car model not found" });
        }
        return NoContent();
    }
}