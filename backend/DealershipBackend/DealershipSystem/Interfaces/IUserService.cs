using System.ComponentModel.DataAnnotations;
using DealershipSystem.DTO;
using DealershipSystem.Models;
using Microsoft.AspNetCore.Identity;

namespace DealershipSystem.Interfaces;

public interface IUserService
{
    Task<IdentityResult> RegisterAsync(UserRegisterDto registerDto);
    Task<User?> GetUserByIdAsync(Guid userId);
    Task<(bool IsValid, List<ValidationResult> Errors)> ValidateUserDtoAsync(UserDTO userDto);
    Task<User?> GetUserByRefreshTokenAsync(string refreshToken);
    Task<bool> UpdateUserAsync(Guid id, UserDTO userDto);
    Task<bool> DeleteUserAsync(Guid id, UserDTO userDto);
    Task<UserDTO[]> GetAllUsersAsync();
    Task<(string AccessToken, string RefreshToken)> GenerateTokensAsync(User user);
    Task<(bool IsValid, string AccessToken)> RefreshAccessTokenAsync(string refreshToken);

    Task<IdentityResult> ChangePasswordAsync(Guid userId, string oldPassword ,string newPassword);
    Task<IdentityResult> ChangePasswordAdminAsync(Guid userId, string newPassword);
    Task<string> GetPreferredLanguageAsync(Guid userId);
    Task<bool> UpdatePreferredLanguageAsync(Guid requesterId, Guid targetUserId, string language);
}
