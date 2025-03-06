using DealershipSystem.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DealershipSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SavedCarsController : ControllerBase
    {
        private readonly ISavedCarService _savedCarService;

        public SavedCarsController(ISavedCarService savedCarService)
        {
            _savedCarService = savedCarService;
        }

        // Get saved cars for a specific user
        [HttpGet("{userId}")]
        public async Task<ActionResult<List<int>>> GetSavedCars(string userId)
        {
            try
            {
                var savedCars = await _savedCarService.GetSavedCarsAsync(Guid.Parse(userId));
                return Ok(savedCars);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // Save a car for the user
        [HttpPost("{userId}/save")]
        public async Task<ActionResult> SaveCar(Guid userId, [FromBody] int carId)
        {
            try
            {
                await _savedCarService.SaveCarAsync(userId, carId);
                return NoContent(); // 204 No Content
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // Remove a saved car for the user
        [HttpDelete("{userId}/remove/{carId}")]
        public async Task<ActionResult> RemoveSavedCar(Guid userId, int carId)
        {
            try
            {
                await _savedCarService.RemoveSavedCarAsync(userId, carId);
                return NoContent(); // 204 No Content
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
