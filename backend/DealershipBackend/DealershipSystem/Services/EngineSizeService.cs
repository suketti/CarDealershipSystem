using AutoMapper;
using DealershipSystem.Context;
using DealershipSystem.DTO;
using DealershipSystem.Interfaces;
using DealershipSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace DealershipSystem.Services
{
    /// <summary>
    /// Service class for managing engine sizes.
    /// </summary>
    public class EngineSizeService : IEngineSizeService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="EngineSizeService"/> class.
        /// </summary>
        /// <param name="context">The application database context.</param>
        /// <param name="mapper">The AutoMapper instance for DTO mapping.</param>
        public EngineSizeService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        /// <summary>
        /// Adds a new engine size entry to the database.
        /// </summary>
        /// <param name="modelId">The ID of the car model.</param>
        /// <param name="engineSize">The engine size in cubic centimeters.</param>
        /// <param name="fuelTypeId">The ID of the fuel type.</param>
        /// <returns>The added engine size as a DTO.</returns>
        /// <exception cref="KeyNotFoundException">Thrown if the car model is not found.</exception>
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

        /// <summary>
        /// Retrieves all engine sizes from the database.
        /// </summary>
        /// <returns>A list of engine size DTOs.</returns>
        public async Task<List<EngineSizeModelDTO>> GetEnginesAsync()
        {
            var engines = await _context.EngineSizeModels.ToListAsync();
            return _mapper.Map<List<EngineSizeModelDTO>>(engines);
        }

        /// <summary>
        /// Retrieves engine sizes by car model ID.
        /// </summary>
        /// <param name="modelId">The car model ID.</param>
        /// <returns>A collection of engine size DTOs, or null if none are found.</returns>
        public async Task<IEnumerable<EngineSizeModelDTO>?> GetEngineByModelIdAsync(int modelId)
        {
            var engines =  await _context.EngineSizeModels.Where(x => x.ModelID == modelId).ToListAsync();
            return _mapper.Map<List<EngineSizeModelDTO>>(engines);
        }

        /// <summary>
        /// Updates an existing engine size entry.
        /// </summary>
        /// <param name="engineId">The ID of the engine size entry.</param>
        /// <param name="newEngineSize">The updated engine size.</param>
        /// <param name="fuelTypeId">The new fuel type ID.</param>
        /// <returns>The updated engine size DTO if successful, otherwise null.</returns>
        public async Task<EngineSizeModelDTO?> UpdateEngineAsync(int engineId, int newEngineSize, int fuelTypeId)
        {
            var engine = await _context.EngineSizeModels.FirstOrDefaultAsync(e => e.ID == engineId);
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

        /// <summary>
        /// Deletes an engine size entry from the database.
        /// </summary>
        /// <param name="engineId">The ID of the engine size entry to delete.</param>
        /// <returns>True if deletion was successful, otherwise false.</returns>
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
}
