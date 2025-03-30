using System.ComponentModel.DataAnnotations;
using AutoMapper;
using DealershipSystem.Context;
using DealershipSystem.DTO;
using DealershipSystem.Interfaces;
using DealershipSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DealershipSystem.Services;

/// <summary>
/// Service for managing users within the dealership system.
/// </summary>
public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;
    private readonly JWTService _jwtService;
    private readonly RoleService _roleService;
    private readonly EmployeeLocationService _employeeLocationService;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="UserService"/> class.
    /// </summary>
    /// <param name="mapper">The mapper for mapping entities.</param>
    /// <param name="context">The application database context.</param>
    /// <param name="userManager">The user manager for managing users.</param>
    /// <param name="jwtService">The JWT service for generating tokens.</param>
    /// <param name="roleService">The role service for managing user roles.</param>
    /// <param name="employeeLocationService">The employee location service for managing employee locations.</param>
    public UserService(IMapper mapper, ApplicationDbContext context, UserManager<User> userManager, JWTService jwtService, RoleService roleService, EmployeeLocationService employeeLocationService)
    {
        _mapper = mapper;
        _context = context;
        _userManager = userManager;
        _jwtService = jwtService;
        _roleService = roleService;
        _employeeLocationService = employeeLocationService;
    }

    /// <summary>
    /// Registers a new user asynchronously.
    /// </summary>
    /// <param name="registerDto">The registration DTO containing user details.</param>
    /// <returns>The result of the identity operation.</returns>
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

    /// <summary>
    /// Admin creates a new user asynchronously.
    /// </summary>
    /// <param name="dto">The DTO containing user details.</param>
    /// <returns>A tuple indicating the success, error message, and user DTO.</returns>
    public async Task<(bool Success, string ErrorMessage, UserDTO User)> AdminCreateUserAsync(AdminUserCreateDTO dto)
    {
        // Check if the email already exists
        var existingUser = await _userManager.FindByEmailAsync(dto.Email);
        if (existingUser != null)
        {
            return (false, "Email is already in use.", null);
        }

        // Create the new user using Identity Framework
        var user = new User()
        {
            UserName = dto.Email, // Use email as the username
            Email = dto.Email,
            Name = dto.Name,
            NameKanji = dto.NameKanji,
            PhoneNumber = dto.PhoneNumber,
            PreferredLanguage = dto.PreferredLanguage
        };

        // Create the user with the provided password
        var result = await _userManager.CreateAsync(user, dto.Password);

        if (!result.Succeeded)
        {
            // Return error message if user creation fails
            return (false, result.Errors.FirstOrDefault()?.Description, null);
        }

        // If the user is a Dealer, assign a location
        if (dto.Role == "Dealer")
        {
            if (dto.Location == null || dto.Location.Id == 0)
            {
                return (false, "Location is required for a dealer.", null);
            }

            // Create the EmployeeLocation record
            var employeeLocation = new EmployeeLocation
            {
                EmployeeId = Guid.Parse(user.Id),
                LocationId = dto.Location.Id
            };

            _context.EmployeeLocations.Add(employeeLocation);
            await _context.SaveChangesAsync();
        }

        // Assign role to the user (Admin or Dealer)
        if (dto.Role == "Dealer" || dto.Role == "Admin")
        {
            await _userManager.AddToRoleAsync(user, dto.Role);
        }

        // Return the appropriate DTO
        var userDTO = dto.Role == "Dealer"
            ? new DealerUserDTO
            {
                ID = Guid.Parse(user.Id),
                Name = user.Name,
                NameKanji = user.NameKanji,
                Email = user.Email,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
                PreferredLanguage = user.PreferredLanguage,
                Location = dto.Location
            }
            : new UserDTO
            {
                ID = Guid.Parse(user.Id),
                Name = user.Name,
                NameKanji = user.NameKanji,
                Email = user.Email,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
                PreferredLanguage = user.PreferredLanguage,
            };

        return (true, null, userDTO);
    }

    /// <summary>
    /// Gets a user DTO by ID asynchronously.
    /// </summary>
    /// <param name="id">The ID of the user.</param>
    /// <returns>The user DTO if found; otherwise, null.</returns>
    public async Task<UserDTO> GetUserDTOByIdAsync(string id)
    {
        var user = await _context.Users
            .Where(u => u.Id == id)
            .FirstOrDefaultAsync();

        if (user == null) return null;

        // Fetch the location ID for the user from the EmployeeLocations table
        var employeeLocation = await _context.EmployeeLocations.Where(el => el.EmployeeId.ToString() == user.Id).FirstOrDefaultAsync();

        if (employeeLocation != null)
        {
            // Fetch the location details from the Location table using the LocationId
            var location = await _context.Locations
                .Where(l => l.ID == employeeLocation.LocationId)
                .FirstOrDefaultAsync();

            return new DealerUserDTO
            {
                ID = Guid.Parse(user.Id),
                Name = user.Name,
                NameKanji = user.NameKanji,
                Email = user.Email,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
                PreferredLanguage = user.PreferredLanguage,
                Location = _mapper.Map<LocationDto>(location) // Get the location name, or null if no location exists
            };
        }
        return new UserDTO
        {
            ID = Guid.Parse(user.Id),
            Name = user.Name,
            NameKanji = user.NameKanji,
            Email = user.Email,
            UserName = user.UserName,
            PhoneNumber = user.PhoneNumber,
            PreferredLanguage = user.PreferredLanguage,
        };
    }

    /// <summary>
    /// Validates a user DTO asynchronously.
    /// </summary>
    /// <param name="userDto">The user DTO to validate.</param>
    /// <returns>A tuple indicating whether the DTO is valid and a list of validation errors.</returns>
    public async Task<(bool IsValid, List<ValidationResult> Errors)> ValidateUserDtoAsync(UserDTO userDto)
    {
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(userDto);

        var isValid = Validator.TryValidateObject(userDto, validationContext, validationResults, true);

        return (isValid, validationResults);
    }

    /// <summary>
    /// Gets a user by ID asynchronously.
    /// </summary>
    /// <param name="id">The ID of the user.</param>
    /// <returns>The user if found; otherwise, null.</returns>
    public async Task<User> GetUserByIdAsync(Guid id)
    {
        var user = await _context.Users.FindAsync(id.ToString());
        if (user == null) return null;

        // If needed, you can return the user object directly here
        return user;
    }

    /// <summary>
    /// Generates tokens for a user asynchronously.
    /// </summary>
    /// <param name="user">The user for whom to generate tokens.</param>
    /// <returns>A tuple containing the access token and refresh token.</returns>
    public async Task<(string AccessToken, string RefreshToken)> GenerateTokensAsync(User user)
    {
        return await _jwtService.GenerateTokens(user);
    }

    /// <summary>
    /// Refreshes the access token asynchronously.
    /// </summary>
    /// <param name="refreshToken">The refresh token.</param>
    /// <returns>A tuple indicating whether the refresh was successful and the new access token.</returns>
    public async Task<(bool IsValid, string AccessToken)> RefreshAccessTokenAsync(string refreshToken)
    {
        return await _jwtService.RefreshAccessToken(refreshToken);
    }

    /// <summary>
    /// Gets a user by refresh token asynchronously.
    /// </summary>
    /// <param name="refreshToken">The refresh token.</param>
    /// <returns>The user if found; otherwise, null.</returns>
    public async Task<User?> GetUserByRefreshTokenAsync(string refreshToken)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
    }

    /// <summary>
    /// Updates a user asynchronously.
    /// </summary>
    /// <param name="id">The ID of the user to update.</param>
    /// <param name="userDto">The user DTO containing updated information.</param>
    /// <returns>True if the update was successful; otherwise, false.</returns>
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

    /// <summary>
    /// Updates an admin user asynchronously.
    /// </summary>
    /// <param name="id">The ID of the user to update.</param>
    /// <param name="dto">The DTO containing updated user information.</param>
    /// <returns>A tuple indicating the success and error message.</returns>
    public async Task<(bool Success, string ErrorMessage)> UpdateAdminUserAsync(Guid id, AdminUserUpdateDTO dto)
    {
        // Retrieve the user by ID
        var user = await GetUserByIdAsync(id);
        if (user == null)
        {
            return (false, "User not found.");
        }

        // Map the updated fields from the DTO to the user entity
        user.Email = dto.Email;
        user.Name = dto.Name;
        user.NameKanji = dto.NameKanji;
        user.PhoneNumber = dto.PhoneNumber;
        user.PreferredLanguage = dto.PreferredLanguage;

        // Update user in Identity Framework
        var updateResult = await _userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
        {
            return (false, updateResult.Errors.FirstOrDefault()?.Description);
        }

        // If the user is a Dealer, update the location
        if (dto.Role == "Dealer")
        {
            if (dto.Location == null || dto.Location.Id == 0)
            {
                return (false, "Location is required for a dealer.");
            }

            var employeeLocation = await _context.EmployeeLocations
                .FirstOrDefaultAsync(el => el.EmployeeId == id);
            if (employeeLocation != null)
            {
                employeeLocation.LocationId = dto.Location.Id;
                _context.EmployeeLocations.Update(employeeLocation);
            }
            else
            {
                var newEmployeeLocation = new EmployeeLocation
                {
                    EmployeeId = id,
                    LocationId = dto.Location.Id
                };
                _context.EmployeeLocations.Add(newEmployeeLocation);
            }
        }

        // Save changes to the database
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }

        return (true, null);
    }

    /// <summary>
    /// Deletes a user asynchronously.
    /// </summary>
    /// <param name="deletingUserId">The ID of the user performing the deletion.</param>
    /// <param name="userDto">The DTO containing the user to be deleted.</param>
    /// <returns>True if the deletion was successful; otherwise, false.</returns>
    public async Task<bool> DeleteUserAsync(Guid deletingUserId, UserDTO userDto)
    {
        // Get the user who is requesting the delete action (the one doing the deletion)
        var deletingUser = await _userManager.FindByIdAsync(deletingUserId.ToString());

        // Get the user that is supposed to be deleted (using the email from the DTO)
        var userToDelete = await _userManager.FindByEmailAsync(userDto.Email);

        if (deletingUser == null || userToDelete == null)
        {
            return false; // One or both users not found
        }

        // Check if the deleting user is an admin
        var deletingUserRoles = await _roleService.GetRole(deletingUser.Id.ToString());
        bool isAdmin = deletingUserRoles != null && deletingUserRoles.Contains("Admin");

        // If the deleting user is deleting themselves, they can only do so if they are not an admin
        if (deletingUser == userToDelete)
        {
            if (isAdmin)
            {
                return false; // Admin cannot delete themselves
            }

            // Proceed to delete themselves (if not an admin)
            return await DeleteSelfAsync(deletingUser, userToDelete);
        }

        // If deleting user is an admin, proceed to delete the target user
        if (isAdmin)
        {
            return await DeleteOtherUserAsync(deletingUser, userToDelete);
        }

        // If the deleting user is not an admin and not deleting themselves, deny the action
        return false;
    }

    /// <summary>
    /// Deletes the current user asynchronously.
    /// </summary>
    /// <param name="deletingUser">The user performing the deletion.</param>
    /// <param name="userToDelete">The user to be deleted.</param>
    /// <returns>True if the deletion was successful; otherwise, false.</returns>
    public async Task<bool> DeleteSelfAsync(User deletingUser, User userToDelete)
    {
        try
        {
            // Remove roles associated with the user
            var roles = await _userManager.GetRolesAsync(userToDelete);
            if (roles != null && roles.Any())
            {
                foreach (var role in roles)
                {
                    await _userManager.RemoveFromRoleAsync(userToDelete, role);
                }
            }

            // Remove user from the Users table
            _context.Users.Remove(userToDelete);
            await _context.SaveChangesAsync(); // Commit the changes to the database

            return true; // Successfully deleted themselves
        }
        catch
        {
            return false; // Failed to delete the user
        }
    }

    /// <summary>
    /// Deletes another user asynchronously.
    /// </summary>
    /// <param name="deletingUser">The user performing the deletion.</param>
    /// <param name="userToDelete">The user to be deleted.</param>
    /// <returns>True if the deletion was successful; otherwise, false.</returns>
    public async Task<bool> DeleteOtherUserAsync(User deletingUser, User userToDelete)
    {
        try
        {
            // Remove roles associated with the user to be deleted
            var roles = await _userManager.GetRolesAsync(userToDelete);
            if (roles != null && roles.Any())
            {
                foreach (var role in roles)
                {
                    await _userManager.RemoveFromRoleAsync(userToDelete, role);
                }
            }

            // Remove user from the Users table
            _context.Users.Remove(userToDelete);
            await _context.SaveChangesAsync(); // Commit the changes to the database

            return true; // Successfully deleted the other user
        }
        catch
        {
            return false; // Failed to delete the user
        }
    }

    /// <summary>
    /// Deletes a dealer asynchronously.
    /// </summary>
    /// <param name="adminId">The ID of the admin performing the deletion.</param>
    /// <param name="userDto">The DTO containing the user to be deleted.</param>
    /// <returns>True if the deletion was successful; otherwise, false.</returns>
    public async Task<bool> DeleteDealerAsync(Guid adminId, UserDTO userDto)
    {
        // Find the requesting admin user using the ID passed
        var adminUser = await _userManager.FindByIdAsync(adminId.ToString());
    
        // Find the target user (the one to be deleted) using the provided userDto
        var targetUser = await _userManager.FindByEmailAsync(userDto.Email); // You can use email or another unique identifier here
    
        // Check if either the admin or target user does not exist
        if (adminUser == null || targetUser == null)
        {
            return false; // User not found
        }

        // Check if the requesting user (admin) has the 'Admin' role
        var adminRoles = await _roleService.GetRole(adminUser.Id.ToString());
        if (adminRoles == null || !adminRoles.Contains("Admin"))
        {
            return false; // Admin role not found, the user is not authorized to delete
        }

        // Proceed with the deletion of the target user
        try
        {
            // Remove roles from the target user
            var roles = await _roleService.GetRole(targetUser.Id.ToString());
            if (roles != null)
            {
                foreach (var role in roles)
                {
                    await _roleService.RemoveRoleFromUserAsync(targetUser, role);
                }
            }

            // Remove the target user from the database
            _context.Users.Remove(targetUser);
            await _context.SaveChangesAsync(); // Commit the changes

            return true; // Deletion successful
        }
        catch (Exception)
        {
            return false; // Deletion failed
        }
    }

    /// <summary>
    /// Gets all users asynchronously.
    /// </summary>
    /// <returns>An array of user DTOs.</returns>
    public async Task<UserDTO[]> GetAllUsersAsync()
    {
        List<User> userList = await _context.Users.ToListAsync();
        UserDTO[] userDtos = _mapper.Map<UserDTO[]>(userList);
        return userDtos;
    }

    /// <summary>
    /// Changes the password of a user asynchronously.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="oldPassword">The old password.</param>
    /// <param name="newPassword">The new password.</param>
    /// <returns>The result of the identity operation.</returns>
    public async Task<IdentityResult> ChangePasswordAsync(Guid userId, string oldPassword, string newPassword)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
        {
            return IdentityResult.Failed(new IdentityError { Description = "User not found." });
        }

        return await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
    }

    /// <summary>
    /// Changes the password of a user as an admin asynchronously.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="newPassword">The new password.</param>
    /// <returns>The result of the identity operation.</returns>
    public async Task<IdentityResult> ChangePasswordAdminAsync(Guid userId, string newPassword)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
        {
            return IdentityResult.Failed(new IdentityError { Description = "User not found." });
        }
        
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        return await _userManager.ResetPasswordAsync(user, token, newPassword);
    }

    /// <summary>
    /// Gets the preferred language of a user asynchronously.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>The preferred language of the user.</returns>
    public async Task<string> GetPreferredLanguageAsync(Guid userId)
    {
        var user = await _context.Users.FindAsync(userId.ToString());
        return user?.PreferredLanguage ?? "en";
    }

    /// <summary>
    /// Updates the preferred language of a user asynchronously.
    /// </summary>
    /// <param name="requesterId">The ID of the user making the request.</param>
    /// <param name="targetUserId">The ID of the user whose language is being updated.</param>
    /// <param name="language">The new preferred language.</param>
    /// <returns>True if the update was successful; otherwise, false.</returns>
    public async Task<bool> UpdatePreferredLanguageAsync(Guid requesterId, Guid targetUserId, string language)
    {
        var allowedLanguages = new[] { "jp", "en", "hu" };

        if (!allowedLanguages.Contains(language.ToLower()))
        {
            return false; // Invalid language
        }

        if (requesterId != targetUserId)
        {
            return false; // Unauthorized update attempt
        }

        var user = await _context.Users.FindAsync(targetUserId.ToString());
        if (user == null)
        {
            return false; // User not found
        }

        user.PreferredLanguage = language;
        await _context.SaveChangesAsync();
        return true;
    }
}