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

    public CarMetadataService(IMapper mapper, ApplicationDbContext context)
    {
        _mapper = mapper;
        _context = context;
    }

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

    public async Task<List<BodyTypeDTO>> GetBodyTypesAsync()
    {
        var bodyTypes = await _context.BodyTypes.AsNoTracking().ToListAsync();
        var bodyTypesDTOs = _mapper.Map<List<BodyTypeDTO>>(bodyTypes);

        return bodyTypesDTOs;   
    }

    public async Task<BodyTypeDTO?> GetBodyTypeByIdAsync(int id)
    {
        var bodyType = await _context.BodyTypes.FirstOrDefaultAsync(bt => bt.ID == id);
        return bodyType != null ? _mapper.Map<BodyTypeDTO>(bodyType) : null;
    }
    
    public async Task<List<TransmissionTypeDTO>> GetTransmissionTypesAsync()
    {
        var transmissionTypes = await _context.TransmissionTypes.AsNoTracking().ToListAsync();
        var transmissionTypeDTOs = _mapper.Map<List<TransmissionTypeDTO>>(transmissionTypes);

        return transmissionTypeDTOs;
    }

    public async Task<BodyTypeDTO?> GetTransmissionTypeByIdAsync(int id)
    {
        var transmissionType = await _context.TransmissionTypes.FirstOrDefaultAsync(t => t.ID == id);
        return transmissionType != null ? _mapper.Map<BodyTypeDTO>(transmissionType) : null;
    }

    public async Task<List<FuelTypeDTO>> GetFuelTypesAsync()
    {
        var fuelTypes = await _context.FuelTypes.AsNoTracking().ToListAsync();
        return _mapper.Map<List<FuelTypeDTO>>(fuelTypes);
    }

    public async Task<FuelTypeDTO?> GetFuelTypeByIdAsync(int id)
    {
        var fuelType = await _context.FuelTypes.FirstOrDefaultAsync(f => f.ID == id);
        return fuelType != null ? _mapper.Map<FuelTypeDTO>(fuelType) : null;
    }

    public async Task<List<DrivetrainTypeDTO>> GetDrivetrainTypesAsync()
    {
        var driveTrainTypes = await _context.DrivetrainTypes.AsNoTracking().ToListAsync();
        return _mapper.Map<List<DrivetrainTypeDTO>>(driveTrainTypes);
    }
    
    public async Task<DrivetrainTypeDTO?> GetDrivetrainTypeByIdAsync(int id)
    {
        var driveTrainType = await _context.DrivetrainTypes.FirstOrDefaultAsync(dt => dt.ID == id);
        return driveTrainType != null ? _mapper.Map<DrivetrainTypeDTO>(driveTrainType) : null;
    }

    public async Task<List<ColorDTO>> GetColorTypesAsync()
    {
        var colorTypes = await _context.Colors.AsNoTracking().ToListAsync();
        return _mapper.Map<List<ColorDTO>>(colorTypes);
    }

    public async Task<ColorDTO?> GetColorByIdAsync(int id)
    {
        var colorType = await _context.Colors.FirstOrDefaultAsync(c => c.ID == id);
        return colorType != null ? _mapper.Map<ColorDTO>(colorType) : null;
    }
}