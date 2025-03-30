using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DealershipSystem.DTO;
using DealershipSystem.Services;

[Route("api/images")]
[ApiController]
public class ImageController : ControllerBase
{
    private readonly ImageService _imageService;

    public ImageController(ImageService imageService)
    {
        _imageService = imageService;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadImage([FromForm] ImageUploadDto dto)
    {
        try
        {
            string imageUrl = await _imageService.UploadImageAsync(dto.ImageFile, dto.CarID);
            return Ok(new { Message = "Image uploaded successfully", ImageUrl = imageUrl });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }

    [HttpGet("car/{carId}")]
    public async Task<IActionResult> GetImagesForCar(int carId)
    {
        var images = await _imageService.GetImagesForCarAsync(carId);
        return Ok(images);
    }
}