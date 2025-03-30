using DealershipSystem.Models;
using Microsoft.AspNetCore.Identity;

namespace DealershipSystem.Services;

/// <summary>
/// Service for managing user roles within the dealership system.
/// </summary>
public class RoleService
{
    private readonly UserManager<User> _userManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="RoleService"/> class.
    /// </summary>
    /// <param name="userManager">The user manager for managing user roles.</param>
    public RoleService(UserManager<User> userManager)
    {
        _userManager = userManager;
    }
    
    /// <summary>
    /// Assigns a role to a user by email.
    /// </summary>
    /// <param name="email">The email of the user.</param>
    /// <param name="role">The role to assign.</param>
    public async Task AssignRole(string email, string role)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user != null && await _userManager.IsInRoleAsync(user, role) == false)
        {
            await _userManager.AddToRoleAsync(user, role);
        }
    }

    /// <summary>
    /// Gets the roles of a user by user ID.
    /// </summary>
    /// <param name="UserId">The ID of the user.</param>
    /// <returns>A list of roles assigned to the user.</returns>
    public async Task<IList<string>> GetRole(string UserId)
    {
        var user = await _userManager.FindByIdAsync(UserId);
        var role = await _userManager.GetRolesAsync(user);
        return role;
    }
    
    /// <summary>
    /// Removes a role from a user asynchronously.
    /// </summary>
    /// <param name="user">The user from whom to remove the role.</param>
    /// <param name="role">The role to remove.</param>
    /// <returns>True if the role was successfully removed; otherwise, false.</returns>
    public async Task<bool> RemoveRoleFromUserAsync(User user, string role)
    {
        var result = await _userManager.RemoveFromRoleAsync(user, role);
        return result.Succeeded;
    }
}