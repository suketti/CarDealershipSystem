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
    private readonly RoleService _roleService;
    
    public UserService(IMapper mapper, ApplicationDbContext context, UserManager<User> userManager, JWTService jwtService, RoleService roleService)
    {
        _mapper = mapper;
        _context = context;
        _userManager = userManager;
        _jwtService = jwtService;
        _roleService = roleService;
    }

    public async Task<IdentityResult> RegisterAsync(UserRegisterDto registerDto)
    {
        var user = new User
        {
            Name = registerDto.Name,
            NameKanji = registerDto.NameKanji,
            UserName = registerDto.UserName,
            Email = registerDto.Email,
            PreferredLanguage = registerDto.PreferredLanguage,
            PhoneNumber = registerDto.PhoneNumber
        };
        
        var result = await _userManager.CreateAsync(user, registerDto.Password);

        if (result.Succeeded)
        {
            await _roleService.AssignRole(registerDto.Email, "Customer");
        }
        
        return result;
    }
    
    public async Task<string> GenerateJwtTokenAsync(User user)
    {
        return _jwtService.GenerateToken(user);
    }
}