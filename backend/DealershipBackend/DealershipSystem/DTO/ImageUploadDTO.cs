namespace DealershipSystem.DTO;

public class ImageUploadDto
{
    public int CarID { get; set; }
    public IFormFile ImageFile { get; set; } = null!;
}
