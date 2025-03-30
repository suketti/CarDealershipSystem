using DealershipSystem.Context;
using DealershipSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace DealershipSystem.Services
{
    /// <summary>
    /// Service class for managing employee locations.
    /// </summary>
    public class EmployeeLocationService
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeeLocationService"/> class.
        /// </summary>
        /// <param name="context">The application database context.</param>
        public EmployeeLocationService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all employee locations from the database.
        /// </summary>
        /// <returns>A list of employee locations.</returns>
        public async Task<List<EmployeeLocation>> GetAllEmployeeLocationsAsync()
        {
            return await _context.EmployeeLocations.ToListAsync();
        }

        /// <summary>
        /// Retrieves an employee location by employee ID.
        /// </summary>
        /// <param name="employeeId">The employee ID.</param>
        /// <returns>The employee location if found, otherwise null.</returns>
        public async Task<EmployeeLocation?> GetEmployeeLocationByEmployeeIdAsync(Guid employeeId)
        {
            return await _context.EmployeeLocations
                .FirstOrDefaultAsync(el => el.EmployeeId == employeeId);
        }

        /// <summary>
        /// Adds a new employee location to the database.
        /// </summary>
        /// <param name="employeeLocation">The employee location data to be added.</param>
        /// <returns>True if the operation was successful, otherwise false.</returns>
        public async Task<bool> AddEmployeeLocationAsync(EmployeeLocation employeeLocation)
        {
            var location = new EmployeeLocation
            {
                EmployeeId = employeeLocation.EmployeeId,
                LocationId = employeeLocation.LocationId
            };
    
            _context.EmployeeLocations.Add(location);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
