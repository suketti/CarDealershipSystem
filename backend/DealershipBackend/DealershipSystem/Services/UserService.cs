using AutoMapper;
using DealershipSystem.Context;
using DealershipSystem.DTO;
using DealershipSystem.Interfaces;
using DealershipSystem.Models;
using Microsoft.AspNetCore.Identity;

namespace DealershipSystem.Services;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;
    private readonly JWTService _jwtService;

    public UserService(IMapper mapper, ApplicationDbContext context, UserManager<User> userManager, JWTService jwtService)
    {
        _mapper = mapper;
        _context = context;
        _userManager = userManager;
        _jwtService = jwtService;
    }

    public async Task<IdentityResult> RegisterAsync(UserRegisterDto registerDto)
    {
        var user = new User
        {
            Name = registerDto.Name,
            NameKanji = registerDto.NameKanji,
            UserName = registerDto.UserName,
            Email = registerDto.Email,
            PreferredLanguage = registerDto.PreferredLanguage
        };
        var result = await _userManager.CreateAsync(user, registerDto.Password);

        return result;
    }
    
    public async Task<string> GenerateJwtTokenAsync(User user)
    {
        return _jwtService.GenerateToken(user);
    }
}