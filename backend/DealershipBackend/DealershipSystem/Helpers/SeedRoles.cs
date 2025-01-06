using Microsoft.AspNetCore.Identity;

namespace DealershipSystem.Helpers;

public class SeedRoles
{

    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        var roleMan = serviceProvider.GetRequiredService <RoleManager<IdentityRole>>();
        
        var roles = new[] { "Admin", "Manager", "Dealer", "Customer" };

        foreach (var role in roles)
        {
            if (!await roleMan.RoleExistsAsync(role))
            {
                await roleMan.CreateAsync(new IdentityRole(role));
            }
        }
    }
}