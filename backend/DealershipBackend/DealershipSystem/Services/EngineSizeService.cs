using AutoMapper;
using DealershipSystem.Context;
using DealershipSystem.DTO;
using DealershipSystem.Interfaces;
using DealershipSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace DealershipSystem.Services;

public class EngineSizeService : IEngineSizeService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public EngineSizeService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<EngineSizeModelDTO> AddEngineSizeAsync(int modelId, int engineSize, int fuelTypeId)
    {
        var carModel = await _context.CarModels.FindAsync(modelId);
        if (carModel == null)
            throw new KeyNotFoundException($"CarModel with ID {modelId} not found.");

        var fuelType = await _context.FuelTypes.FindAsync(fuelTypeId);
        
        var engineSizeModel = new EngineSizeModel
        {
            ModelID = modelId,
            EngineSize = engineSize,
            FuelType = fuelType
        };

        _context.EngineSizeModels.Add(engineSizeModel);
        await _context.SaveChangesAsync();
        return _mapper.Map<EngineSizeModelDTO>(engineSizeModel);
    }

    public async Task<List<EngineSizeModelDTO>> GetEnginesAsync()
    {
        var engines = await _context.EngineSizeModels.ToListAsync();
        return _mapper.Map<List<EngineSizeModelDTO>>(engines);
    }

    public async Task<IEnumerable<EngineSizeModelDTO>?> GetEngineByModelIdAsync(int modelId)
    {
        var engines =  await _context.EngineSizeModels.Where(x => x.ModelID == modelId).ToListAsync();
        return _mapper.Map<List<EngineSizeModelDTO>>(engines);
    }

    public async Task<EngineSizeModelDTO?> UpdateEngineAsync(int engineId, int newEngineSize, int fuelTypeId)
    {
        var engine = await _context.EngineSizeModels.FirstOrDefaultAsync(e => e.ModelID == engineId);
        if (engine == null)
        {
            return null;
        }

        engine.EngineSize = newEngineSize;
        var ft = await _context.FuelTypes.FirstOrDefaultAsync(x => x.ID == fuelTypeId);
        if (ft == null)
        {
            return null;
        }
        engine.FuelType = ft;

        await _context.SaveChangesAsync();
        return _mapper.Map<EngineSizeModelDTO>(engine);
    }

    public async Task<bool> DeleteEnginesAsync(int engineId)
    {
        var engine = await _context.EngineSizeModels.FirstOrDefaultAsync(x => x.ID == engineId);
        if (engine == null)
        {
            return false;
        }
        _context.EngineSizeModels.Remove(engine);
        await _context.SaveChangesAsync();
        return true;
    }
}