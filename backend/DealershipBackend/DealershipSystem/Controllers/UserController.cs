using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using AutoMapper;
using DealershipSystem.DTO;
using DealershipSystem.Interfaces;
using DealershipSystem.Models;
using DealershipSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DealershipSystem.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;
    private readonly EmployeeLocationService _employeeLocationService;
    private readonly LocationService _locationService;

    public UserController(IUserService userService, UserManager<User> userManager, IMapper mapper, EmployeeLocationService employeeLocationService, LocationService locationService)
    {
        _userService = userService;
        _userManager = userManager;
        _mapper = mapper;
        _employeeLocationService = employeeLocationService;
        _locationService = locationService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegisterDto registerDto)
    {
        if (await _userManager.FindByEmailAsync(registerDto.Email) != null)
        {
            return BadRequest(new { error = "Email is already in use." });
        }

        var result = await _userService.RegisterAsync(registerDto);

        if (!result.Succeeded)
        {
            return BadRequest(new { errors = result.Errors.Select(e => e.Description) });
        }

        var user = await _userManager.FindByEmailAsync(registerDto.Email);
        var (accessToken, refreshToken) = await _userService.GenerateTokensAsync(user);
        var userDto = _mapper.Map<User, UserDTO>(user);

        return Ok(new { Token = accessToken, RefreshToken = refreshToken, User = userDto });
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDTO loginDto)
    {
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(loginDto);
        if (!Validator.TryValidateObject(loginDto, validationContext, validationResults, true))
        {
            return BadRequest(validationResults.Select(vr => vr.ErrorMessage));
        }

        var user = await _userManager.FindByEmailAsync(loginDto.Email);

        if (user != null && await _userManager.CheckPasswordAsync(user, loginDto.Password))
        {
            var (accessToken, refreshToken) = await _userService.GenerateTokensAsync(user);
            var userDto = _mapper.Map<UserDTO>(user);

            Response.Cookies.Append("RefreshToken", refreshToken, new CookieOptions
            {
                Path = "/",
                HttpOnly = false,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(30)
            });

            Response.Cookies.Append("AccessToken", accessToken, new CookieOptions
            {
                Path = "/",
                HttpOnly = false,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddMinutes(15)
            });
            
            Response.Cookies.Append("userId", user.Id, new CookieOptions
            {
                Path = "/",
                HttpOnly = false,       
                Secure = true,          
                SameSite =  SameSiteMode.None, // CSRF protection
                Expires = DateTime.UtcNow.AddDays(30) // Cookie expiration
            });


            return Ok(new { User = userDto });
        }

        return Unauthorized();
    }
    
    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] Guid userId)
    {
        // Retrieve the current authenticated user's ID
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (currentUserId == null || Guid.Parse(currentUserId) != userId)
        {
            return Unauthorized(new { message = "You are not authorized to log out this user." });
        }

        // Perform the logout
        var result = await _userService.LogoutAsync(userId);

        if (result)
        {
            return Ok(new { message = "Logout successful." });
        }
        else
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Logout failed." });
        }
    }
    
    [HttpPost("dealer-login")]
    public async Task<IActionResult> DealerLogin([FromBody] UserLoginDTO loginDto)
    {
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(loginDto);
        if (!Validator.TryValidateObject(loginDto, validationContext, validationResults, true))
        {
            return BadRequest(validationResults.Select(vr => vr.ErrorMessage));
        }

        var user = await _userManager.FindByEmailAsync(loginDto.Email);

        if (user != null && await _userManager.CheckPasswordAsync(user, loginDto.Password))
        {
            var (accessToken, refreshToken) = await _userService.GenerateTokensAsync(user);
            var userDto = _mapper.Map<UserDTO>(user);

            // Fetch the role and location
            var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault(); // Assuming only one role per user
            var location = await _employeeLocationService.GetEmployeeLocationByEmployeeIdAsync(Guid.Parse(user.Id)); // Assuming you have a method to fetch the location

            // Set cookies
            Response.Cookies.Append("RefreshToken", refreshToken, new CookieOptions
            {
                Path = "/",
                HttpOnly = false,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(30)
            });

            Response.Cookies.Append("AccessToken", accessToken, new CookieOptions
            {
                Path = "/",
                HttpOnly = false,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddMinutes(320)
            });

            Response.Cookies.Append("userId", user.Id, new CookieOptions
            {
                Path = "/",
                HttpOnly = false,
                Secure = true,
                SameSite = SameSiteMode.None, // CSRF protection
                Expires = DateTime.UtcNow.AddDays(30) // Cookie expiration
            });

            var loginResponse = new DealerLoginResponseDTO
            {
                User = userDto,
                Role = role,
                Location = location != null ? _mapper.Map<LocationDto>(location) : null // Map location to LocationDto if found
            };

            return Ok(loginResponse);
        }

        return Unauthorized("Invalid email or password.");
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AdminCreateUser([FromBody] AdminUserCreateDTO dto)
    {
        // Call the service to create the user
        var result = await _userService.AdminCreateUserAsync(dto);

        // Handle the result of user creation
        if (!result.Success)
        {
            return BadRequest(result.ErrorMessage);
        }

        // Return the created user with a 201 Created response
        return Ok(User);
    }



    [HttpPut("update")]
    [Authorize]
    public async Task<IActionResult> UpdateUserAsync([FromBody] UserDTO userDto)
    {

        var (isValid, validationErrors) = await _userService.ValidateUserDtoAsync(userDto);
        if (!isValid)
        {
            return BadRequest(new
            {
                Message = "Validation failed.",
                Errors = validationErrors.Select(v => v.ErrorMessage)
            });
        }

        var userIdFromToken = User.Claims.FirstOrDefault(c => c.Type == "Id").Value;
        var user = await _userService.GetUserByIdAsync(Guid.Parse(userIdFromToken));
        if (userIdFromToken == null || user == null)
        {
            return Unauthorized(new { Message = "You are not authorized to update this user." });
        }

        var isUpdated = await _userService.UpdateUserAsync(Guid.Parse(userIdFromToken), userDto);
        if (!isUpdated)
        {
            return NotFound(new { Message = "User not found or update failed." });
        }

        return Ok(new { Message = "User updated successfully." });
    }
    
    [HttpPut("adminupdate/{userId}")]
    public async Task<IActionResult> UpdateUserAsync(Guid userId, [FromBody] AdminUserUpdateDTO dto)
    {
        if (dto == null)
        {
            return BadRequest("Invalid data.");
        }

        var (success, errorMessage) = await _userService.UpdateAdminUserAsync(userId, dto);

        if (!success)
        {
            return BadRequest(errorMessage);  // Return the error message in case of failure
        }

        return Ok("User updated successfully.");  // Return success message if the update is successful
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetUserByIdAsync(Guid id)
    {
        var userIdFromToken = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;

        // Check if the user is authorized to access the data (self or admin)
        if (userIdFromToken == null || (userIdFromToken != id.ToString() && !User.IsInRole("Admin")))
        {
            return Unauthorized(new { Message = "You are not authorized to access this user." });
        }

        // Get the user from the service
        var user = await _userService.GetUserByIdAsync(id);

        if (user == null)
        {
            return NotFound(new { Message = "User not found." });
        }

        // Check the role of the user and map accordingly
        if (User.IsInRole("Dealer") || User.IsInRole("Admin"))
        {
            // Manually create DealerUserDTO and map basic properties
            var dealerUserDto = new DealerUserDTO
            {
                ID = Guid.Parse(user.Id),
                Name = user.Name,
                NameKanji = user.NameKanji,
                Email = user.Email,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
                PreferredLanguage = user.PreferredLanguage
            };

            // Get the employee location if the user is a Dealer or Admin
            var employeeLocation = await _employeeLocationService.GetEmployeeLocationByEmployeeIdAsync(id);

            if (employeeLocation != null && employeeLocation.LocationId != null)
            {
                // Fetch the full location using the LocationService
                var locationDto = await _locationService.GetLocationByIdAsync(employeeLocation.LocationId);

                // Assign the location data to the DealerUserDTO if it exists
                if (locationDto != null)
                {
                    dealerUserDto.Location = locationDto;
                }
            }

            return Ok(dealerUserDto);
        }

        // If the user is not a Dealer or Admin, return the standard UserDTO
        var userDto = _mapper.Map<UserDTO>(user);
        return Ok(userDto);
    }



    
    [HttpPost("change-password-admin")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ChangePasswordAdmin([FromBody] ChangePasswordAdminDTO changePasswordDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _userService.ChangePasswordAdminAsync(changePasswordDto.UserId, changePasswordDto.NewPassword);

        if (!result.Succeeded)
        {
            return BadRequest(new { errors = result.Errors.Select(e => e.Description) });
        }

        return Ok(new { Message = "Password changed successfully." });
    }
    
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO changePasswordDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _userService.ChangePasswordAsync(changePasswordDto.UserId, changePasswordDto.OldPassword, changePasswordDto.NewPassword);

        if (!result.Succeeded)
        {
            return BadRequest(new { errors = result.Errors.Select(e => e.Description) });
        }

        return Ok(new { Message = "Password changed successfully." });
    }
    
    [HttpDelete("delete")]
    [Authorize]
    public async Task<IActionResult> DeleteUserAsync([FromBody] UserDTO userDto)
    {
        var (isValid, validationErrors) = await _userService.ValidateUserDtoAsync(userDto);
        if (!isValid)
        {
            return BadRequest(new
            {
                Message = "Validation failed.",
                Errors = validationErrors.Select(v => v.ErrorMessage)
            });
        }

        var userIdFromToken = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
        if (userIdFromToken == null)
        {
            return NoContent();
        }

        // Check if the user is deleting their own account or if they are an Admin
        var isAdmin = User.IsInRole("Admin"); // Ensure "Admin" is the role you use for admin users
        if (userIdFromToken != userDto.ID.ToString() && !isAdmin)
        {
            return Unauthorized(new { Message = "You are not authorized to delete this user." });
        }

        var didSucceed = await _userService.DeleteUserAsync(Guid.Parse(userIdFromToken), userDto);
        if (!didSucceed)
        {
            return StatusCode(400, new { Message = "Couldn't delete user" });
        }

        return Ok();
    }


    [HttpDelete("delete-dealer")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteDealerAsync([FromBody] UserDTO userDto)
    {
        var (isValid, validationErrors) = await _userService.ValidateUserDtoAsync(userDto);
        if (!isValid)
        {
            return BadRequest(new
            {
                Message = "Validation failed.",
                Errors = validationErrors.Select(v => v.ErrorMessage)
            });
        }

        var userIdFromToken = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
        if (userIdFromToken == null)
        {
            return NoContent();
        }

        var didSucceed = await _userService.DeleteDealerAsync(Guid.Parse(userIdFromToken), userDto);
        if (!didSucceed)
        {
            return StatusCode(400, new { Message = "Couldn't delete dealer" });
        }

        return Ok();
    }

    
    [HttpGet]
    [Authorize(Roles = "Admin")]

    public async Task<IActionResult> GetAllUsersAsync()
    {
        UserDTO[] users = await _userService.GetAllUsersAsync();
        return Ok(users);

    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
    {
        var (isValid, newAccessToken) =
            await _userService.RefreshAccessTokenAsync(refreshToken); // 🔹 _userService 経由でトークン更新

        if (!isValid)
        {
            return Unauthorized(new { Message = "Invalid or expired refresh token." });
        }

        return Ok(new { AccessToken = newAccessToken });
    }

    [HttpGet("getPrivilege")]
    public async Task<IActionResult> GetUserRank([FromQuery] string userId)
    {
        if (string.IsNullOrEmpty(userId))
        {
            return BadRequest("User ID is required.");
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound("User not found.");
        }

        var roles = await _userManager.GetRolesAsync(user);
        return Ok(roles);
    }
    
    [Authorize] // Requires authentication
    [HttpPut("updatePreferredLanguage/{targetUserId}")]
    public async Task<IActionResult> UpdatePreferredLanguage(Guid targetUserId, [FromBody] string language)
    {
        var requesterId = Guid.Parse(User.FindFirstValue("Id"));

        bool success = await _userService.UpdatePreferredLanguageAsync(requesterId, targetUserId, language);

        if (!success)
        {
            return BadRequest("Invalid request, unauthorized update, or user not found.");
        }

        return Ok("Preferred language updated successfully.");
    }
}