using DealershipSystem.DTO;
using DealershipSystem.Interfaces;
using DealershipSystem.Models;
using DealershipSystem.Services;
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
        var result = await _authService.RegisterAsync(registerDto);

        if (result.Succeeded)
        {
            var user = await _userManager.FindByEmailAsync(registerDto.Email);
            var token = await _authService.GenerateJwtTokenAsync(user);
            return Ok(new {token});
        }

        return BadRequest(new { errors = result.Errors });
    }
}