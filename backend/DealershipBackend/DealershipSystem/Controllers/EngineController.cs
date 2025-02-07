using System.ComponentModel.DataAnnotations;
using DealershipSystem.DTO;
using DealershipSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace DealershipSystem.Controllers;

[ApiController]
[Route("api/cars/engine")]
public class EngineController : ControllerBase
{
    private readonly EngineSizeService _engineSizeService;

    public EngineController(EngineSizeService engineSizeService)
    {
        _engineSizeService = engineSizeService;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateEngine([FromBody] CreateEngineDTO createEngineDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var createdModel = await _engineSizeService.AddEngineSizeAsync(createEngineDto.ModelID,
                createEngineDto.EngineSize, createEngineDto.FuelType);
            return Ok(createdModel);
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    
    [HttpGet("model/{modelId}")]
    public async Task<ActionResult<IEnumerable<EngineSizeModelDTO>>> GetEnginesByModel(int modelId)
    {
        var engines = await _engineSizeService.GetEngineByModelIdAsync(modelId);
        if (engines == null || !engines.Any())
        {
            return NotFound(new { message = "No engines found for this model." });
        }
        return Ok(engines);
    }
    
    [HttpPut("update")]
    public async Task<ActionResult<EngineSizeModelDTO>> UpdateEngine([FromBody] UpdateEngineDTO updateDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var updatedEngine = await _engineSizeService.UpdateEngineAsync(updateDto.ID, updateDto.NewEngineSize, updateDto.FuelTypeID);
        if (updatedEngine == null)
        {
            return NotFound(new { message = "Engine not found." });
        }

        return Ok(updatedEngine);
    }
    
    [HttpDelete("{engineId}")]
    public async Task<IActionResult> DeleteEngine(int engineId)
    {
        var deleted = await _engineSizeService.DeleteEnginesAsync(engineId);
        if (!deleted)
        {
            return NotFound(new { message = "Engine not found." });
        }

        return Ok();
    }
}