using AutoMapper;
using DealershipSystem.Context;
using DealershipSystem.DTO;
using DealershipSystem.Interfaces;
using DealershipSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace DealershipSystem.Services;

public class CarMakerService : ICarMakerService
{
    private IMapper _mapper;
    private ApplicationDbContext _context;

    // Constructor for initializing the service with AutoMapper and DbContext.
    public CarMakerService(IMapper mapper, ApplicationDbContext applicationDbContext)
    {
        _mapper = mapper;
        _context = applicationDbContext;
    }
    
    /// <summary>
    /// Creates a new car maker in the database if it doesn't already exist.
    /// If the maker already exists, returns null.
    /// </summary>
    /// <param name="createCarMakerDto">The DTO containing car maker data to create.</param>
    /// <returns>A CarMakerDTO if creation was successful, or null if the maker already exists.</returns>
    public async Task<CarMakerDTO?> CreateNewMakerAsync(CreateCarMakerDTO createCarMakerDto)
    {
        var makerExists = await _context.CarMakers.FirstOrDefaultAsync(x =>
            x.BrandEnglish == createCarMakerDto.BrandEnglish || x.BrandJapanese == createCarMakerDto.BrandJapanese);

        if (makerExists != null)
        {
            return null; // Return null if the car maker already exists.
        }

        // Create a new car maker entity.
        CarMaker cm = new CarMaker
        {
            BrandJapanese = createCarMakerDto.BrandJapanese,
            BrandEnglish = createCarMakerDto.BrandEnglish
        };

        // Add the new maker to the database and save changes.
        _context.CarMakers.Add(cm);
        await _context.SaveChangesAsync();
        
        // Retrieve the newly created car maker from the database.
        var cmFromDb = await _context.CarMakers.FirstOrDefaultAsync(x => x.BrandEnglish == cm.BrandEnglish);
        
        return _mapper.Map<CarMakerDTO>(cmFromDb); // Map the CarMaker to CarMakerDTO and return it.
    }

    /// <summary>
    /// Retrieves all car makers from the database.
    /// </summary>
    /// <returns>A list of CarMakerDTOs representing all car makers.</returns>
    public async Task<List<CarMakerDTO>> GetMakersAsync()
    {
        var makerList = await _context.CarMakers.ToListAsync();
        return _mapper.Map<List<CarMakerDTO>>(makerList); // Map the list of CarMakers to DTOs and return it.
    }

    /// <summary>
    /// Retrieves a specific car maker by its ID.
    /// </summary>
    /// <param name="id">The ID of the car maker to retrieve.</param>
    /// <returns>A CarMakerDTO representing the car maker, or null if not found.</returns>
    public async Task<CarMakerDTO?> GetMakerByIdAsync(int id)
    {
        var maker = await _context.CarMakers.FirstOrDefaultAsync(x => x.ID == id);
        if (maker == null)
        {
            return null; // Return null if the maker was not found.
        }

        return _mapper.Map<CarMakerDTO>(maker); // Map the found CarMaker to a DTO and return it.
    }

    /// <summary>
    /// Updates an existing car maker's data in the database by its ID.
    /// </summary>
    /// <param name="updateCarMakerDto">The DTO containing the updated car maker data.</param>
    /// <returns>The updated CarMakerDTO, or null if the maker was not found.</returns>
    public async Task<CarMakerDTO?> UpdateMakerByIdAsync(UpdateCarMakerDTO updateCarMakerDto)
    {
        var maker = await _context.CarMakers.FirstOrDefaultAsync(x => x.ID == updateCarMakerDto.ID);
        if (maker == null)
        {
            return null; // Return null if the car maker was not found.
        }

        // Update the car maker's properties.
        maker.BrandEnglish = updateCarMakerDto.BrandEnglish;
        maker.BrandJapanese = updateCarMakerDto.BrandJapanese;

        // Save the changes to the database.
        await _context.SaveChangesAsync();
        
        return _mapper.Map<CarMakerDTO>(maker); // Map the updated CarMaker to DTO and return it.
    }

    /// <summary>
    /// Deletes a car maker by its ID from the database.
    /// </summary>
    /// <param name="id">The ID of the car maker to delete.</param>
    /// <returns>True if deletion was successful, false if the maker was not found.</returns>
    public async Task<bool> DeleteMakerByIdAsync(int id)
    {
        try
        {
            var maker = await _context.CarMakers.FirstOrDefaultAsync(x => x.ID == id);
            if (maker == null)
            {
                return false; // Return false if the maker was not found.
            }

            // Remove the car maker from the database.
            _context.CarMakers.Remove(maker);
            await _context.SaveChangesAsync();
            return true; // Return true if deletion was successful.
        }
        catch
        {
            return false; // Return false if an error occurred during deletion.
        }
    }
}
