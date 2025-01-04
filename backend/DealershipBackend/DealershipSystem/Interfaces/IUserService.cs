using DealershipSystem.DTO;
using DealershipSystem.Models;
using Microsoft.AspNetCore.Identity;

namespace DealershipSystem.Interfaces;

public interface IUserService
{
    Task<IdentityResult> RegisterAsync(UserRegisterDto registerDto);
    Task<string> GenerateJwtTokenAsync(User user);
}
