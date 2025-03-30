using AutoMapper;
using DealershipSystem.Context;
using DealershipSystem.DTO;
using DealershipSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace DealershipSystem.Services;

public class CarMetadataService
{
    private readonly IMapper _mapper;
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="CarMetadataService"/> class.
    /// </summary>
    /// <param name="mapper">The AutoMapper instance.</param>
    /// <param name="context">The database context.</param>
    public CarMetadataService(IMapper mapper, ApplicationDbContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    /// <summary>
    /// Creates a new car body type if it does not already exist.
    /// </summary>
    /// <param name="bodyTypeDto">The DTO containing body type details.</param>
    /// <returns>A tuple containing the body type entity and a boolean indicating if it was newly created.</returns>
    public async Task<(BodyType bodyType, bool isNew)> CreateCarBodyTypeAsync(CreateBodyTypeDTO bodyTypeDto)
    {
        var bodyType = _mapper.Map<BodyType>(bodyTypeDto);
        var existingBodyType = await _context.BodyTypes.FirstOrDefaultAsync(bt =>
            bt.NameEnglish == bodyType.NameEnglish || bt.NameJapanese == bodyType.NameJapanese);

        if (existingBodyType != null)
        {
            return (existingBodyType, false);
        }
        
        _context.BodyTypes.Add(bodyType);
        await _context.SaveChangesAsync();

        return (bodyType, true);
    }

    /// <summary>
    /// Retrieves a list of all body types.
    /// </summary>
    /// <returns>A list of body type DTOs.</returns>
    public async Task<List<BodyTypeDTO>> GetBodyTypesAsync()
    {
        var bodyTypes = await _context.BodyTypes.AsNoTracking().ToListAsync();
        return _mapper.Map<List<BodyTypeDTO>>(bodyTypes);
    }

    /// <summary>
    /// Retrieves a body type by its ID.
    /// </summary>
    /// <param name="id">The ID of the body type.</param>
    /// <returns>The body type DTO if found, otherwise null.</returns>
    public async Task<BodyTypeDTO?> GetBodyTypeByIdAsync(int id)
    {
        var bodyType = await _context.BodyTypes.FirstOrDefaultAsync(bt => bt.ID == id);
        return bodyType != null ? _mapper.Map<BodyTypeDTO>(bodyType) : null;
    }
    
    /// <summary>
    /// Retrieves a list of all transmission types.
    /// </summary>
    /// <returns>A list of transmission type DTOs.</returns>
    public async Task<List<TransmissionTypeDTO>> GetTransmissionTypesAsync()
    {
        var transmissionTypes = await _context.TransmissionTypes.AsNoTracking().ToListAsync();
        return _mapper.Map<List<TransmissionTypeDTO>>(transmissionTypes);
    }

    /// <summary>
    /// Retrieves a transmission type by its ID.
    /// </summary>
    /// <param name="id">The ID of the transmission type.</param>
    /// <returns>The transmission type DTO if found, otherwise null.</returns>
    public async Task<BodyTypeDTO?> GetTransmissionTypeByIdAsync(int id)
    {
        var transmissionType = await _context.TransmissionTypes.FirstOrDefaultAsync(t => t.ID == id);
        return transmissionType != null ? _mapper.Map<BodyTypeDTO>(transmissionType) : null;
    }

    /// <summary>
    /// Retrieves a list of all fuel types.
    /// </summary>
    /// <returns>A list of fuel type DTOs.</returns>
    public async Task<List<FuelTypeDTO>> GetFuelTypesAsync()
    {
        var fuelTypes = await _context.FuelTypes.AsNoTracking().ToListAsync();
        return _mapper.Map<List<FuelTypeDTO>>(fuelTypes);
    }

    /// <summary>
    /// Retrieves a fuel type by its ID.
    /// </summary>
    /// <param name="id">The ID of the fuel type.</param>
    /// <returns>The fuel type DTO if found, otherwise null.</returns>
    public async Task<FuelTypeDTO?> GetFuelTypeByIdAsync(int id)
    {
        var fuelType = await _context.FuelTypes.FirstOrDefaultAsync(f => f.ID == id);
        return fuelType != null ? _mapper.Map<FuelTypeDTO>(fuelType) : null;
    }

    /// <summary>
    /// Retrieves a list of all drivetrain types.
    /// </summary>
    /// <returns>A list of drivetrain type DTOs.</returns>
    public async Task<List<DrivetrainTypeDTO>> GetDrivetrainTypesAsync()
    {
        var driveTrainTypes = await _context.DrivetrainTypes.AsNoTracking().ToListAsync();
        return _mapper.Map<List<DrivetrainTypeDTO>>(driveTrainTypes);
    }
    
    /// <summary>
    /// Retrieves a drivetrain type by its ID.
    /// </summary>
    /// <param name="id">The ID of the drivetrain type.</param>
    /// <returns>The drivetrain type DTO if found, otherwise null.</returns>
    public async Task<DrivetrainTypeDTO?> GetDrivetrainTypeByIdAsync(int id)
    {
        var driveTrainType = await _context.DrivetrainTypes.FirstOrDefaultAsync(dt => dt.ID == id);
        return driveTrainType != null ? _mapper.Map<DrivetrainTypeDTO>(driveTrainType) : null;
    }

    /// <summary>
    /// Retrieves a list of all color types.
    /// </summary>
    /// <returns>A list of color type DTOs.</returns>
    public async Task<List<ColorDTO>> GetColorTypesAsync()
    {
        var colorTypes = await _context.Colors.AsNoTracking().ToListAsync();
        return _mapper.Map<List<ColorDTO>>(colorTypes);
    }

    /// <summary>
    /// Retrieves a color by its ID.
    /// </summary>
    /// <param name="id">The ID of the color.</param>
    /// <returns>The color DTO if found, otherwise null.</returns>
    public async Task<ColorDTO?> GetColorByIdAsync(int id)
    {
        var colorType = await _context.Colors.FirstOrDefaultAsync(c => c.ID == id);
        return colorType != null ? _mapper.Map<ColorDTO>(colorType) : null;
    }
}
