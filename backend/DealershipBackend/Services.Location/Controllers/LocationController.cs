using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Location.Context;
using Services.Location.DTO;

namespace Services.Location.Controllers;

[ApiController]
[Route("[controller]")]

public class LocationController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    
    public LocationController(IMapper mapper, ApplicationDbContext context)
    {
        _mapper = mapper;
        _context = context;
    }
    
    [HttpGet(Name = "GetLocations")]
    public async Task<IActionResult> Get()
    {
        var locations = await _context.Locations.ToListAsync();
        if (locations.Count == 0)
        {
            return StatusCode(204);
        }
        
        return Ok(locations);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var location =  await _context.Locations.FirstOrDefaultAsync(i => i.ID == id);

        if (location == null)
        {
            return NotFound();
        }

        var locationDto = _mapper.Map<LocationDto>(location);
        
        return Ok(locationDto);
    }

    [HttpPost]
    public async Task<IActionResult> CreateLocation([FromBody] LocationDto location)
    { 
        var entity = _mapper.Map<Entities.Location>(location);
        var prefecture = await _context.Prefectures.AsNoTracking().FirstOrDefaultAsync(x => x.NameJP == entity.Address.Prefecture.NameJP);
        if (prefecture == null)
        {
            return StatusCode(422);
        }
        
        entity.Address.PrefectureId = prefecture.Id;

        entity.Address.Prefecture = null;
        _context.Locations.Add(entity);
        
        _context.SaveChanges();
        
        return CreatedAtAction(nameof(GetById), new { id = entity.ID }, location);
    }
}