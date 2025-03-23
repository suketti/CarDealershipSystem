using AutoMapper;
using DealershipSystem.Context;
using DealershipSystem.DTO;
using DealershipSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace DealershipSystem.Services
{
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
        
        public async Task<CarDTO> AddCarAsync(CreateCarDTO createCarDto)
        {
            var car = _mapper.Map<Car>(createCarDto); 
            
            car.Brand = await _context.CarMakers.FindAsync(createCarDto.Brand);
            car.CarModel = await _context.CarModels.FindAsync(createCarDto.Model);
            car.BodyType = await _context.BodyTypes.FindAsync(createCarDto.BodyType);
            car.Location = await _context.Locations.FindAsync(createCarDto.Location);
            car.EngineSize = await _context.EngineSizeModels.FindAsync(createCarDto.EngineSize);
            car.FuelType = await _context.FuelTypes.FindAsync(createCarDto.FuelType);
            car.DriveTrain = await _context.DrivetrainTypes.FindAsync(createCarDto.DriveTrain);
            car.TransmissionType = await _context.TransmissionTypes.FindAsync(createCarDto.TransmissionType);
            car.Color = await _context.Colors.FindAsync(createCarDto.Color);
            
            if (createCarDto.Extras != null && createCarDto.Extras.Any())
            {
                car.CarExtras = await _context.CarExtras
                    .Where(extra => createCarDto.Extras.Contains(extra.ExtraID))
                    .ToListAsync();
            }

            await _context.Cars.AddAsync(car);
            await _context.SaveChangesAsync();

            return _mapper.Map<Car, CarDTO>(car);
        }

        public async Task<bool> DeleteCarAsync(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return false;
            }

            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<CarDTO?> EditCarAsync(int id, CreateCarDTO editCarDto)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return null;
            }

            _mapper.Map(editCarDto, car);

            car.Brand = await _context.CarMakers.FindAsync(editCarDto.Brand);
            car.CarModel = await _context.CarModels.FindAsync(editCarDto.Model);
            car.BodyType = await _context.BodyTypes.FindAsync(editCarDto.BodyType);
            car.Location = await _context.Locations.FindAsync(editCarDto.Location);
            car.EngineSize = await _context.EngineSizeModels.FindAsync(editCarDto.EngineSize);
            car.FuelType = await _context.FuelTypes.FindAsync(editCarDto.FuelType);
            car.DriveTrain = await _context.DrivetrainTypes.FindAsync(editCarDto.DriveTrain);
            car.TransmissionType = await _context.TransmissionTypes.FindAsync(editCarDto.TransmissionType);
            car.Color = await _context.Colors.FindAsync(editCarDto.Color);
            
            if (editCarDto.Extras != null && editCarDto.Extras.Any())
            {
                car.CarExtras = await _context.CarExtras
                    .Where(extra => editCarDto.Extras.Contains(extra.ExtraID))
                    .ToListAsync();
            }

            _context.Cars.Update(car);
            await _context.SaveChangesAsync();

            return _mapper.Map<CarDTO>(car);
        }
    }
}