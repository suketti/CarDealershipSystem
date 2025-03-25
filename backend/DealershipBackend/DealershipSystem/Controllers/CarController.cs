using System.ComponentModel.DataAnnotations;
using DealershipSystem.DTO;
using DealershipSystem.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace DealershipSystem.Controllers;

[ApiController]
[Route("api/cars")]
public class CarController : ControllerBase
{
    private readonly CarService _carService;

    public CarController(CarService carService)
    {
        _carService = carService;
    }

    [HttpGet]
    [EnableCors]
    public async Task<IActionResult> GetAll()
    {
        var cars = await _carService.GetAllCarsAsync();
        return Ok(cars);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var car = await _carService.GetCarByIdAsync(id);
        return car != null ? Ok(car) : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> CreateCar([FromBody] CreateCarDTO carDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var createdModel = await _carService.AddCarAsync(carDto);
            return CreatedAtAction(nameof(GetById), new { id = createdModel.ID }, createdModel);
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> EditCar(int id, [FromBody] CreateCarDTO carDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var updatedCar = await _carService.EditCarAsync(id, carDto);
            return updatedCar != null ? Ok(updatedCar) : NotFound();
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCar(int id)
    {
        var result = await _carService.DeleteCarAsync(id);
        return result ? NoContent() : NotFound();
    }
    
   
}