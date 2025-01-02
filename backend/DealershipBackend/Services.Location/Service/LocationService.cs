using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Location.Context;
using Services.Location.DTO;
using Services.Location.Entities;

namespace Services.Location.Services
{
    public class LocationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public LocationService(IMapper mapper, ApplicationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<List<LocationDto>> GetAllLocationsAsync()
        {
            var locations = await _context.Locations.ToListAsync();
            var locationDtos = _mapper.Map<List<LocationDto>>(locations);

            return locationDtos;
        }

        public async Task<LocationDto> GetLocationByIdAsync(int id)
        {
            var location = await _context.Locations.FirstOrDefaultAsync(i => i.ID == id);
            if (location == null)
            {
                return null;
            }

            return _mapper.Map<LocationDto>(location);
        }

        public async Task<IActionResult> CreateLocationAsync(LocationDto location)
        {
            var entity = _mapper.Map<Entities.Location>(location);
            var prefecture = await _context.Prefectures.AsNoTracking()
                .FirstOrDefaultAsync(x => x.NameJP == entity.Address.Prefecture.NameJP);
            
            if (prefecture == null)
            {
                return new StatusCodeResult(422); // Unprocessable Entity
            }

            entity.Address.PrefectureId = prefecture.Id;
            entity.Address.Prefecture = null;

            _context.Locations.Add(entity);
            await _context.SaveChangesAsync();

            return new CreatedAtActionResult("GetById", "Location", new { id = entity.ID }, location);
        }
    }
}