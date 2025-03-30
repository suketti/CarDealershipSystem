using DealershipSystem.Models;
using Microsoft.EntityFrameworkCore;
using DealershipSystem.Context;

namespace DealershipSystem.Services
{
    /// <summary>
    /// Service class for handling image uploads and retrieval.
    /// </summary>
    public class ImageService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageService"/> class.
        /// </summary>
        /// <param name="environment">The web hosting environment.</param>
        /// <param name="context">The application database context.</param>
        public ImageService(IWebHostEnvironment environment, ApplicationDbContext context)
        {
            _environment = environment;
            _context = context;
        }

        /// <summary>
        /// Uploads an image for a specific car.
        /// </summary>
        /// <param name="imageFile">The image file to upload.</param>
        /// <param name="carId">The ID of the car associated with the image.</param>
        /// <returns>The URL of the uploaded image.</returns>
        /// <exception cref="ArgumentException">Thrown when the provided file is invalid.</exception>
        public async Task<string> UploadImageAsync(IFormFile imageFile, int carId)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                throw new ArgumentException("Invalid image file.");
            }

            // Define the folder path as wwwroot/uploads/{carId}
            string carFolder = Path.Combine(_environment.WebRootPath, "uploads", carId.ToString());
            
            if (!Directory.Exists(carFolder))
            {
                Directory.CreateDirectory(carFolder);
            }

            // Get the original filename
            string fileName = Path.GetFileName(imageFile.FileName);
            string filePath = Path.Combine(carFolder, fileName);

            // Check if the image already exists
            if (System.IO.File.Exists(filePath))
            {
                return $"/uploads/{carId}/{fileName}"; // Return existing URL if already uploaded
            }

            // Save the file to disk
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            // Generate the URL in the format /uploads/{carId}/{filename}
            string imageUrl = $"/uploads/{carId}/{fileName}";

            // Save image record in the database
            var image = new Image
            {
                CarID = carId,
                URL = imageUrl
            };

            _context.Images.Add(image);
            await _context.SaveChangesAsync();

            return imageUrl;
        }

        /// <summary>
        /// Retrieves all image URLs associated with a specific car.
        /// </summary>
        /// <param name="carId">The ID of the car.</param>
        /// <returns>A list of image URLs.</returns>
        public async Task<List<string>> GetImagesForCarAsync(int carId)
        {
            return await _context.Images
                .Where(img => img.CarID == carId)
                .Select(img => img.URL)
                .ToListAsync();
        }
    }
}
