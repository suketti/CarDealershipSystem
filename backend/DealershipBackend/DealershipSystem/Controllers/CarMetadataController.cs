using DealershipSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace DealershipSystem.Controllers;

[ApiController]
[Route("api/cars/metadata")]
public class CarMetadataController : ControllerBase
{
    private readonly CarMetadataService _carMetadataService;


    public CarMetadataController(CarMetadataService carMetadataService)
    {
        _carMetadataService = carMetadataService;
    }

    [HttpGet("bodytypes")]
    public async Task<IActionResult> GetAllBodyTypes()
    {
        var bodyTypes = await _carMetadataService.GetBodyTypesAsync();
        return Ok(bodyTypes);
    }

    [HttpGet("bodytypes/{id:int}")]
    public async Task<IActionResult> GetBodyTypeById(int id)
    {
        var bodyType = await _carMetadataService.GetBodyTypeByIDAsync(id);
        return bodyType != null ? Ok(bodyType) : NotFound();
    }
    
    [HttpGet("tranmissionTypes")]
    public async Task<IActionResult> GetAllTransmissionTypesAsync()
    {
        var transmissionTypes = await _carMetadataService.GetTransmissionTypesAsync();
        return Ok(transmissionTypes);
    }

    [HttpGet("transmissionTypes/{id:int}")]
    public async Task<IActionResult> GetTransmissionTypeByIdAsync(int id)
    {
        var transmissionType = await _carMetadataService.GetTransmissionTypeByIdAsync(id);
        return transmissionType != null ? Ok(transmissionType) : NotFound();
    }
}

