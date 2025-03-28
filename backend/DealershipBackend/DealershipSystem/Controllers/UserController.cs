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

    public UserController(IUserService userService, UserManager<User> userManager, IMapper mapper)
    {
        _userService = userService;
        _userManager = userManager;
        _mapper = mapper;
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

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetUserByIdAsync(Guid id)
    {
        var userIdFromToken = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;

        if (userIdFromToken == null || (userIdFromToken != id.ToString() && !User.IsInRole("Admin")))
        {
            return Unauthorized(new { Message = "You are not authorized to access this user." });
        }

        var user = await _userService.GetUserByIdAsync(id);
    
        if (user == null)
        {
            return NotFound(new { Message = "User not found." });
        }

        var userDto = _mapper.Map<UserDTO>(user);
        return Ok(userDto);
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

        var userIdFromToken = User.Claims.FirstOrDefault(c => c.Type == "Id").Value;
        if (userIdFromToken == null)
        {
            return NoContent();
        }

        var didSucceed = await _userService.DeleteUserAsync(Guid.Parse(userIdFromToken), userDto);
        if (!didSucceed)
        {
            return StatusCode(400, new { Message = "Couldn't delete user" });
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
        var requesterId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)); // Get authenticated user's ID

        bool success = await _userService.UpdatePreferredLanguageAsync(requesterId, targetUserId, language);

        if (!success)
        {
            return BadRequest("Invalid request, unauthorized update, or user not found.");
        }

        return Ok("Preferred language updated successfully.");
    }
}