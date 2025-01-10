using System.ComponentModel.DataAnnotations;
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
[Route("/Users")]
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
            return BadRequest();
        }

        var result = await _userService.RegisterAsync(registerDto);

        if (result.Succeeded)
        {
            var user = await _userManager.FindByEmailAsync(registerDto.Email);
            var token = await _userService.GenerateJwtTokenAsync(user);
            return Ok(new { token });
        }

        return BadRequest(new { errors = result.Errors });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDTO loginDto)
    {
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(loginDto, serviceProvider: null, items: null);
        if (!Validator.TryValidateObject(loginDto, validationContext, validationResults, true))
        {
            return BadRequest(validationResults.Select(vr => vr.ErrorMessage));
        }
        
        var user = await _userManager.FindByEmailAsync(loginDto.Email);
        
        if (user != null && await _userManager.CheckPasswordAsync(user, loginDto.Password))
        {
            var token = await _userService.GenerateJwtTokenAsync(user);
            var userDto = _mapper.Map<User, UserDTO>(user);
            return Ok(new { Token = token, User = userDto});
        }

        return Unauthorized();
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
}