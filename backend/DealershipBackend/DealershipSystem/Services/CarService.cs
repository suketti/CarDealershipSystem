using AutoMapper;
using DealershipSystem.Context;
using DealershipSystem.DTO;
using DealershipSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace DealershipSystem.Services;

public class CarService
{
    private readonly IMapper _mapper;
    private readonly ApplicationDbContext _context;
    
    public CarService(IMapper mapper, ApplicationDbContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<List<CarDTO>> GetAllCarsAsync()
    {
        var cars = await _context.Cars.ToListAsync();

        return _mapper.Map<List<CarDTO>>(cars);
    }

    public async Task<CarDTO?> GetCarByIdAsync(int id)
    {
        var car = await _context.Cars.FirstOrDefaultAsync(c => c.ID == id);

        return car != null ? _mapper.Map<CarDTO>(car) : null;
    }
    
    //TODO: Add verification
    public async Task AddCarAsync(CreateCarDTO carDto)
    {
        var car = _mapper.Map<Car>(carDto);
        _context.Cars.Add(car);
        await _context.SaveChangesAsync();
    }
}