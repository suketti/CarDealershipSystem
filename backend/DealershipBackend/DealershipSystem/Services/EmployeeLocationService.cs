using DealershipSystem.Context;
using DealershipSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace DealershipSystem.Services;

public class EmployeeLocationService
{
    private readonly ApplicationDbContext _context;

    public EmployeeLocationService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<EmployeeLocation>> GetAllEmployeeLocationsAsync()
    {
        return await _context.EmployeeLocations.ToListAsync();
    }

    public async Task<EmployeeLocation> GetEmployeeLocationByEmployeeIdAsync(int employeeId)
    {
        return await _context.EmployeeLocations
            .FirstOrDefaultAsync(el => el.EmployeeId == employeeId);
    }

    public async Task<bool> AddEmployeeLocationAsync(EmployeeLocation employeeLocation)
    {
        _context.EmployeeLocations.Add(employeeLocation);
        return await _context.SaveChangesAsync() > 0;
    }
}