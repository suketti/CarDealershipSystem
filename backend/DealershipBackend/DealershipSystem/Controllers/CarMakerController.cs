using DealershipSystem.DTO;
using DealershipSystem.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace DealershipSystem.Controllers;

[ApiController]
[Route("api/cars/makers")]
public class CarMakerController : ControllerBase
{
    private readonly CarMakerService _carMakerService;
    
    public CarMakerController(CarMakerService carMakerService)
    {
        _carMakerService = carMakerService;
    }

    [HttpGet]
    public async Task<IActionResult> GetMakers()
    {
        var makers = await _carMakerService.GetMakersAsync();
        return Ok(makers);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetMakerById(int id)
    {
        var maker = await _carMakerService.GetMakerByIdAsync(id);
        if (maker == null)
        {
            return BadRequest();
        }

        return Ok(maker);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateMaker([FromBody] CreateCarMakerDTO createCarMakerDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var cm = await _carMakerService.CreateNewMakerAsync(createCarMakerDto);
        if (cm == null)
        {
            return StatusCode(403);
        }
        return Ok(cm);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateMaker([FromBody] UpdateCarMakerDTO updateCarMakerDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        
        var res = await _carMakerService.UpdateMakerByIdAsync(updateCarMakerDto);
        if (res == null)
        {
            return BadRequest();
        }

        return Ok(res);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMaker(int id)
    {
        var res = await _carMakerService.DeleteMakerByIdAsync(id);
        if (res == false)
        {
            return StatusCode(404);
        }
        return Ok();
    }
    
}

