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
    
    public async Task<CarModelDTO> CreateCarModelAsync(CreateCarModelDTO carModelDTO)
    {
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(carModelDTO);

        if (!Validator.TryValidateObject(carModelDTO, validationContext, validationResults, true))
        {
            throw new ValidationException(string.Join(", ", validationResults.Select(x => x.ErrorMessage)));
        }

        if (_context.CarMakers.FirstOrDefaultAsync(x => x.ID == carModelDTO.MakerID) == null)
        {
            throw new Exception("Maker doesn't exist");
        }
        
        
        var carModel = new CarModel
        {
            MakerID = carModelDTO.MakerID,
            ModelNameJapanese = carModelDTO.ModelNameJapanese,
            ModelNameEnglish = carModelDTO.ModelNameEnglish,
            ModelCode = carModelDTO.ModelCode,
            ManufacturingStartYear = carModelDTO.ManufacturingStartYear,
            ManufacturingEndYear = carModelDTO.ManufacturingEndYear,
            PassengerCount = carModelDTO.PassengerCount,
            Length = carModelDTO.Length,
            Width = carModelDTO.Width,
            Height = carModelDTO.Height,
            Mass = carModelDTO.Mass
        };

        await _context.CarModels.AddAsync(carModel);
        await _context.SaveChangesAsync();
        return _mapper.Map<CarModelDTO>(carModel);
    }
    
    public async Task<CarModelDTO?> GetCarModelAsync(int id)
    {
        var model = await _context.CarModels.FirstOrDefaultAsync(x => x.ID == id);
        return _mapper.Map<CarModelDTO>(model);
    }

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

    public async Task<List<CarModelDTO>> GetAllCarModelsAsync()
    {
        var models = await _context.CarModels.ToListAsync();
        return _mapper.Map<List<CarModelDTO>>(models); 
    }

    public async Task<CarModelDTO?> UpdateCarModelAsync(int id, UpdateCarModelDTO updatedCarModel)
{
    var carModel = await _context.CarModels.FirstOrDefaultAsync(c => c.ID == id);
    if (carModel == null)
    {
        return null; 
    }

    // Validate updated model
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
    
    carModel.MakerID = updatedCarModel.MakerID;
    carModel.ModelNameJapanese = updatedCarModel.ModelNameJapanese;
    carModel.ModelNameEnglish = updatedCarModel.ModelNameEnglish;
    carModel.ModelCode = updatedCarModel.ModelCode;
    carModel.ManufacturingStartYear = updatedCarModel.ManufacturingStartYear;
    carModel.ManufacturingEndYear = updatedCarModel.ManufacturingEndYear;
    carModel.PassengerCount = updatedCarModel.PassengerCount;
    carModel.Length = updatedCarModel.Length;
    carModel.Width = updatedCarModel.Width;
    carModel.Height = updatedCarModel.Height;
    carModel.Mass = updatedCarModel.Mass;
    
    await _context.SaveChangesAsync();
    
    return _mapper.Map<CarModelDTO>(carModel);
}

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
}