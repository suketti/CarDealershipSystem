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
    private ICarMakerService _carMakerServiceImplementation;

    public CarMakerService(IMapper mapper, ApplicationDbContext applicationDbContext)
    {
        _mapper = mapper;
        _context = applicationDbContext;
    }
    
    public async Task<CarMakerDTO?> CreateNewMakerAsync(CreateCarMakerDTO createCarMakerDto)
    {
        var makerExists = await _context.CarMakers.FirstOrDefaultAsync(x =>
            x.BrandEnglish == createCarMakerDto.BrandEnglish || x.BrandJapanese == createCarMakerDto.BrandJapanese);

        if (makerExists != null)
        {
            return null;
        }
        CarMaker cm = new CarMaker
        {
            BrandJapanese = createCarMakerDto.BrandJapanese,
            BrandEnglish = createCarMakerDto.BrandEnglish
        };

        _context.CarMakers.Add(cm);
        await _context.SaveChangesAsync();
        var cmFromDb = await _context.CarMakers.FirstOrDefaultAsync(x => x.BrandEnglish == cm.BrandEnglish);
        return _mapper.Map<CarMakerDTO>(cmFromDb);
    }

    public async Task<List<CarMakerDTO>> GetMakersAsync()
    {
        var makerList = await _context.CarMakers.ToListAsync();
        return _mapper.Map<List<CarMakerDTO>>(makerList);
    }

    public async Task<CarMakerDTO?> GetMakerByIdAsync(int id)
    {
        var maker = _context.CarMakers.FirstOrDefaultAsync(x => x.ID == id);
        if (maker == null)
        {
            return null;
        }

        return _mapper.Map<CarMakerDTO>(maker);
    }

    public async Task<CarMakerDTO?> UpdateMakerByIdAsync(UpdateCarMakerDTO updateCarMakerDto)
    {
        var maker = await _context.CarMakers.FirstOrDefaultAsync(x => x.ID == updateCarMakerDto.ID);
        if (maker == null)
        {
            return null;
        }

        maker.BrandEnglish = updateCarMakerDto.BrandEnglish;
        maker.BrandJapanese = updateCarMakerDto.BrandJapanese;
        await _context.SaveChangesAsync();
        return _mapper.Map<CarMakerDTO>(maker);
    }

    public async Task<bool> DeleteMakerByIdAsync(int id)
    {
        try
        {
            var maker = await _context.CarMakers.FirstOrDefaultAsync(x => x.ID == id);
            if (maker == null)
            {
                return false;
            }

            _context.CarMakers.Remove(maker);
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }
}