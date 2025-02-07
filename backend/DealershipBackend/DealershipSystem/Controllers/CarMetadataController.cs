using DealershipSystem.DTO;
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
        var bodyType = await _carMetadataService.GetBodyTypeByIdAsync(id);
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

    [HttpGet("fuelTypes")]
    public async Task<IActionResult> GetFuelTypesAsync()
    {
        var fuelTypes = await _carMetadataService.GetFuelTypesAsync();
        return Ok(fuelTypes);
    }

    [HttpGet("fuelTypes/{id}")]
    public async Task<IActionResult> GetFuelTypeByIdAsync(int id)
    {
        var fuelType = await _carMetadataService.GetFuelTypeByIdAsync(id);
        return fuelType != null ? Ok(fuelType) : NotFound();
    }
    
    [HttpGet("drivetrainTypes")]
    public async Task<IActionResult> GetAllDrivetrainTypes()
    {
        var drivetrainTypes = await _carMetadataService.GetDrivetrainTypesAsync();
        return Ok(drivetrainTypes);
    }
    
    [HttpGet("drivetrainTypes/{id}")]
    public async Task<IActionResult> GetDrivetrainTypeById(int id)
    {
        var drivetrainType = await _carMetadataService.GetDrivetrainTypeByIdAsync(id);

        return drivetrainType != null ? Ok(drivetrainType) : NotFound();
    }

    [HttpGet("colors")]
    public async Task<IActionResult> GetColors()
    {
        var colorTypes = await _carMetadataService.GetColorTypesAsync();
        return Ok(colorTypes);
    }

    [HttpGet("colors/{id}")]
    public async Task<IActionResult> GetColorById(int id)
    {
        var colorType = await _carMetadataService.GetColorByIdAsync(id);
        return colorType != null ? Ok(colorType) : NotFound();
    }
}

