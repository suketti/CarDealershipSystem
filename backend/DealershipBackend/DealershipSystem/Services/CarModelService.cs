using System.ComponentModel.DataAnnotations;
using AutoMapper;
using DealershipSystem.Context;
using DealershipSystem.DTO;
using DealershipSystem.Interfaces;
using DealershipSystem.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace DealershipSystem.Services;

public class CarModelService : ICarModelService
{
    private ApplicationDbContext _context;
    private IMapper _mapper;

    public CarModelService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    /// <summary>
    /// Creates a new car model.
    /// </summary>
    /// <param name="carModelDTO">The car model data transfer object.</param>
    /// <returns>The created CarModelDTO.</returns>
    public async Task<CarModelDTO> CreateCarModelAsync(CreateCarModelDTO carModelDTO)
    {
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(carModelDTO);

        if (!Validator.TryValidateObject(carModelDTO, validationContext, validationResults, true))
        {
            throw new ValidationException(string.Join(", ", validationResults.Select(x => x.ErrorMessage)));
        }

        if (!await _context.CarMakers.AnyAsync(x => x.ID == carModelDTO.MakerID))
        {
            throw new Exception("Maker doesn't exist");
        }

        var carModel = _mapper.Map<CarModel>(carModelDTO);
    
        await _context.CarModels.AddAsync(carModel);
        await _context.SaveChangesAsync();
        return _mapper.Map<CarModelDTO>(carModel);
    }

    
    /// <summary>
    /// Retrieves a car model by its ID.
    /// </summary>
    /// <param name="id">The car model ID.</param>
    /// <returns>The CarModelDTO if found, otherwise null.</returns>
    public async Task<CarModelDTO?> GetCarModelAsync(int id)
    {
        var model = await _context.CarModels.FirstOrDefaultAsync(x => x.ID == id);
        return _mapper.Map<CarModelDTO>(model);
    }

    /// <summary>
    /// Retrieves car models filtered by optional parameters.
    /// </summary>
    /// <param name="makerID">The maker ID to filter by.</param>
    /// <param name="startYear">The manufacturing start year to filter by.</param>
    /// <param name="endYear">The manufacturing end year to filter by.</param>
    /// <param name="passengerCount">The passenger count to filter by.</param>
    /// <returns>A list of filtered CarModelDTOs.</returns>
    public async Task<List<CarModelDTO>> GetCarModelsFilteredAsync(int? makerID = null, int? startYear = null, int? endYear = null,
        int? passengerCount = null)
    {
        IQueryable<CarModel> query = _context.CarModels;

        if (makerID.HasValue)
        {
            query = query.Where(c => c.Maker.ID == makerID.Value);
        }
        if (startYear.HasValue)
        {
            query = query.Where(c => c.ManufacturingStartYear >= startYear.Value);
        }
        if (endYear.HasValue)
        {
            query = query.Where(c => c.ManufacturingEndYear <= endYear.Value);
        }
        if (passengerCount.HasValue)
        {
            query = query.Where(c => c.PassengerCount == passengerCount.Value);
        }

        return  _mapper.Map<List<CarModelDTO>>(await query.ToListAsync());
    }

    /// <summary>
    /// Retrieves all car models.
    /// </summary>
    /// <returns>A list of CarModelDTOs.</returns>
    public async Task<List<CarModelDTO>> GetAllCarModelsAsync()
    {
        var models = await _context.CarModels.ToListAsync();
        return _mapper.Map<List<CarModelDTO>>(models); 
    }

    /// <summary>
    /// Updates an existing car model.
    /// </summary>
    /// <param name="id">The ID of the car model to update.</param>
    /// <param name="updatedCarModel">The updated car model data.</param>
    /// <returns>The updated CarModelDTO, or null if not found.</returns>
    public async Task<CarModelDTO?> UpdateCarModelAsync(int id, UpdateCarModelDTO updatedCarModel)
    {
        var carModel = await _context.CarModels.FirstOrDefaultAsync(c => c.ID == id);
        if (carModel == null)
        {
            return null; 
        }

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(updatedCarModel);
        if (!Validator.TryValidateObject(updatedCarModel, validationContext, validationResults, true))
        {
            throw new ValidationException(string.Join(", ", validationResults.Select(x => x.ErrorMessage)));
        }
        
        if (carModel.MakerID != updatedCarModel.MakerID)
        {
            var makerExists = await _context.CarMakers.AnyAsync(x => x.ID == updatedCarModel.MakerID);
            if (!makerExists)
            {
                throw new Exception("Maker doesn't exist");
            }
        }
        
        _mapper.Map(updatedCarModel, carModel);
        await _context.SaveChangesAsync();
        
        return _mapper.Map<CarModelDTO>(carModel);
    }

    /// <summary>
    /// Deletes a car model by ID.
    /// </summary>
    /// <param name="id">The ID of the car model to delete.</param>
    /// <returns>True if deleted, false if not found.</returns>
    public async Task<bool> DeleteCarModelAsync(int id)
    {
        var carModel = await _context.CarModels.FirstOrDefaultAsync(c => c.ID == id);
        if (carModel == null)
        {
            return false; 
        }

        _context.CarModels.Remove(carModel);
        await _context.SaveChangesAsync();
        return true;
    }
    
    /// <summary>
    /// Retrieves all car models for a given maker ID.
    /// </summary>
    /// <param name="makerId">The maker ID.</param>
    /// <returns>A list of CarModelDTOs.</returns>
    public async Task<List<CarModelDTO>> GetCarsByMakerIdAsync(int makerId)
    {
        var cars = await _context.CarModels
            .Where(c => c.MakerID == makerId)
            .ToListAsync();

        return _mapper.Map<List<CarModelDTO>>(cars);
    }
}
