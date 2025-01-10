using System.ComponentModel.DataAnnotations;
using AutoMapper;
using DealershipSystem.Context;
using DealershipSystem.DTO;
using DealershipSystem.Interfaces;
using DealershipSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
    
    public async Task<(bool IsValid, List<ValidationResult> Errors)> ValidateUserDtoAsync(UserDTO userDto)
    {
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(userDto);

        var isValid = Validator.TryValidateObject(userDto, validationContext, validationResults, true);

        return (isValid, validationResults);
    }
    
    public async Task<User?> GetUserByIdAsync(Guid userId)
    {
        return await _userManager.FindByIdAsync(userId.ToString());
    }
    
    public async Task<string> GenerateJwtTokenAsync(User user)
    {
        return await _jwtService.GenerateToken(user);
    }
    
    public async Task<bool> UpdateUserAsync(Guid id, UserDTO userDto)
    {
        var user = await GetUserByIdAsync(id);
        if (user == null)
        {
            return false;
        }
        
        _mapper.Map(userDto, user);

        try
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true; 
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> DeleteUserAsync(Guid id, UserDTO userDto)
    {
        var userFromDto = await _userManager.FindByEmailAsync(userDto.Email);
        var userFromID = await _userManager.FindByIdAsync(id.ToString());
        
        if (userFromDto == userFromID)
        try
        {
            _context.Users.Remove(userFromID);
            return true;
        }
        catch
        {
            return false;
        }

        return false;
    }

    public async Task<UserDTO[]> GetAllUsersAsync()
    {
        List<User> userList = await _context.Users.ToListAsync();
        UserDTO[] userDtos = _mapper.Map<UserDTO[]>(userList);
        return userDtos;
    }
}