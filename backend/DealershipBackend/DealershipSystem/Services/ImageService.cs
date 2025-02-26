
using DealershipSystem.Models;

using Microsoft.EntityFrameworkCore;
using DealershipSystem.Context;

public class ImageService
{
    private readonly IWebHostEnvironment _environment;
    private readonly ApplicationDbContext _context;

    public ImageService(IWebHostEnvironment environment, ApplicationDbContext context)
    {
        _environment = environment;
        _context = context;
    }

    public async Task<string> UploadImageAsync(IFormFile imageFile, int carId)
    {
        if (imageFile == null || imageFile.Length == 0)
        {
            throw new ArgumentException("Invalid image file.");
        }

        string uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }

        string uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(imageFile.FileName)}";
        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await imageFile.CopyToAsync(fileStream);
        }

        string imageUrl = $"/uploads/{uniqueFileName}";
        
        var image = new Image
        {
            CarID = carId,
            URL = imageUrl
        };

        _context.Images.Add(image);
        await _context.SaveChangesAsync();

        return imageUrl;
    }

    public async Task<List<string>> GetImagesForCarAsync(int carId)
    {
        return await _context.Images
            .Where(img => img.CarID == carId)
            .Select(img => img.URL)
            .ToListAsync();
    }
}