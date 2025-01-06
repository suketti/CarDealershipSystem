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
    private readonly IUserService _authService;
    private readonly UserManager<User> _userManager;
    public UserController(IUserService authService, UserManager<User> userManager)
    {
        _authService = authService;
        _userManager = userManager;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegisterDto registerDto)
    {
        if (await _userManager.FindByEmailAsync(registerDto.Email) != null)
        {
            return BadRequest();
        }

        var result = await _authService.RegisterAsync(registerDto);

        if (result.Succeeded)
        {
            var user = await _userManager.FindByEmailAsync(registerDto.Email);
            var token = await _authService.GenerateJwtTokenAsync(user);
            return Ok(new {token});
        }

        return BadRequest(new { errors = result.Errors });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDTO loginDto)
    {
        var user = await _userManager.FindByEmailAsync(loginDto.Email);

        if (user != null && await _userManager.CheckPasswordAsync(user, loginDto.Password))
        {
            var token = _authService.GenerateJwtTokenAsync(user);
            return Ok(new { Token = token });
        }

        return Unauthorized();
    }
}