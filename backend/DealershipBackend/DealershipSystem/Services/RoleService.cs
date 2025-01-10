using DealershipSystem.Models;
using Microsoft.AspNetCore.Identity;

namespace DealershipSystem.Services;

public class RoleService
{
    private readonly UserManager<User> _userManager;

    public RoleService(UserManager<User> userManager)
    {
        _userManager = userManager;
    }
    
    public async Task AssignRole(string email, string role)
    {

        var user = await _userManager.FindByEmailAsync(email);
        if (user != null && await _userManager.IsInRoleAsync(user, role) == false)
        {
            await _userManager.AddToRoleAsync(user, role);
        }
    }

    public async Task<IList<string>> GetRole(string UserId)
    {
        var user = await _userManager.FindByIdAsync(UserId);
        var role = await _userManager.GetRolesAsync(user);
        return role;
    }
}