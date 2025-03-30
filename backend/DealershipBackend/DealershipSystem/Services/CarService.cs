using AutoMapper;
using DealershipSystem.Context;
using DealershipSystem.DTO;
using DealershipSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace DealershipSystem.Services
{
    /// <summary>
    /// Service class for managing car-related operations.
    /// </summary>
    public class CarService
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;
        
        public CarService(IMapper mapper, ApplicationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        /// <summary>
        /// Retrieves all cars from the database.
        /// </summary>
        /// <returns>A list of CarDTOs.</returns>
        public async Task<List<CarDTO>> GetAllCarsAsync()
        {
            var cars = await _context.Cars.ToListAsync();
            return _mapper.Map<List<CarDTO>>(cars);
        }

        /// <summary>
        /// Retrieves a specific car by ID.
        /// </summary>
        /// <param name="id">The car ID.</param>
        /// <returns>The CarDTO if found, otherwise null.</returns>
        public async Task<CarDTO?> GetCarByIdAsync(int id)
        {
            var car = await _context.Cars.FirstOrDefaultAsync(c => c.ID == id);
            return car != null ? _mapper.Map<CarDTO>(car) : null;
        }
        
        /// <summary>
        /// Adds a new car to the database.
        /// </summary>
        /// <param name="createCarDto">The car data to be added.</param>
        /// <returns>The created CarDTO.</returns>
        public async Task<CarDTO> AddCarAsync(CreateCarDTO createCarDto)
        {
            var car = _mapper.Map<Car>(createCarDto);

            // Assign related entities using their IDs
            car.Brand = await _context.CarMakers.FindAsync(createCarDto.Brand);
            car.CarModel = await _context.CarModels.FindAsync(createCarDto.Model);
            car.BodyType = await _context.BodyTypes.FindAsync(createCarDto.BodyType);
            car.Location = await _context.Locations.FindAsync(createCarDto.Location);
            car.EngineSize = await _context.EngineSizeModels.FindAsync(createCarDto.EngineSize);
            car.FuelType = await _context.FuelTypes.FindAsync(createCarDto.FuelType);
            car.DriveTrain = await _context.DrivetrainTypes.FindAsync(createCarDto.DriveTrain);
            car.TransmissionType = await _context.TransmissionTypes.FindAsync(createCarDto.TransmissionType);
            car.Color = await _context.Colors.FindAsync(createCarDto.Color);

            // Assign extras if provided
            if (createCarDto.Extras != null && createCarDto.Extras.Any())
            {
                car.CarExtras = await _context.CarExtras
                    .Where(extra => createCarDto.Extras.Contains(extra.ExtraID))
                    .ToListAsync();
            }

            // Ensure MOTExpiry is stored in UTC
            if (createCarDto.MOTExpiry.HasValue)
            {
                car.MOTExpiry = createCarDto.MOTExpiry.Value.Kind == DateTimeKind.Unspecified
                    ? DateTime.SpecifyKind(createCarDto.MOTExpiry.Value, DateTimeKind.Utc)
                    : createCarDto.MOTExpiry.Value.ToUniversalTime();
            }

            await _context.Cars.AddAsync(car);
            await _context.SaveChangesAsync();

            return _mapper.Map<Car, CarDTO>(car);
        }

        /// <summary>
        /// Deletes a car from the database by ID.
        /// </summary>
        /// <param name="id">The ID of the car to delete.</param>
        /// <returns>True if deleted, false if not found.</returns>
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

        /// <summary>
        /// Updates an existing car record.
        /// </summary>
        /// <param name="id">The ID of the car to update.</param>
        /// <param name="editCarDto">The updated car data.</param>
        /// <returns>The updated CarDTO, or null if not found.</returns>
        public async Task<CarDTO?> EditCarAsync(int id, CreateCarDTO editCarDto)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return null;
            }

            _mapper.Map(editCarDto, car);

            // Assign related entities
            car.Brand = await _context.CarMakers.FindAsync(editCarDto.Brand);
            car.CarModel = await _context.CarModels.FindAsync(editCarDto.Model);
            car.BodyType = await _context.BodyTypes.FindAsync(editCarDto.BodyType);
            car.Location = await _context.Locations.FindAsync(editCarDto.Location);
            car.EngineSize = await _context.EngineSizeModels.FindAsync(editCarDto.EngineSize);
            car.FuelType = await _context.FuelTypes.FindAsync(editCarDto.FuelType);
            car.DriveTrain = await _context.DrivetrainTypes.FindAsync(editCarDto.DriveTrain);
            car.TransmissionType = await _context.TransmissionTypes.FindAsync(editCarDto.TransmissionType);
            car.Color = await _context.Colors.FindAsync(editCarDto.Color);

            // Assign extras if provided
            if (editCarDto.Extras != null && editCarDto.Extras.Any())
            {
                car.CarExtras = await _context.CarExtras
                    .Where(extra => editCarDto.Extras.Contains(extra.ExtraID))
                    .ToListAsync();
            }
            
            // Ensure MOTExpiry is stored in UTC
            if (car.MOTExpiry != null)
            {
                car.MOTExpiry = car.MOTExpiry.Value.Kind == DateTimeKind.Unspecified 
                    ? (DateTime?)DateTime.SpecifyKind(car.MOTExpiry.Value, DateTimeKind.Utc) 
                    : car.MOTExpiry.Value.ToUniversalTime();
            }

            _context.Cars.Update(car);
            await _context.SaveChangesAsync();

            return _mapper.Map<CarDTO>(car);
        }
    }
}