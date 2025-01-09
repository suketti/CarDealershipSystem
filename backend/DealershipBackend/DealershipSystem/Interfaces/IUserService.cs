using System.ComponentModel.DataAnnotations;
using DealershipSystem.DTO;
using DealershipSystem.Models;
using Microsoft.AspNetCore.Identity;

namespace DealershipSystem.Interfaces;

public interface IUserService
{
    Task<IdentityResult> RegisterAsync(UserRegisterDto registerDto);
    Task<string> GenerateJwtTokenAsync(User user);
    Task<(bool IsValid, List<ValidationResult> Errors)> ValidateUserDtoAsync(UserDTO userDto);
    Task<User?> GetUserByIdAsync(Guid userId);
    Task<bool> UpdateUserAsync(Guid id, UserDTO userDto);
    
}
