using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DealershipSystem.Helpers;
using DealershipSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace DealershipSystem.Services;

public class JWTService
{
    private readonly RoleService _roleService;

    public JWTService(RoleService roleService)
    {
        _roleService = roleService;
    }
    public async Task<string> GenerateToken(User user)
    {
        var handler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(AuthSettings.PrivateKey);
        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256Signature);
    
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = await GenerateClaims(user),
            Expires = DateTime.UtcNow.AddMinutes(15),
            SigningCredentials = credentials,
        };

        var token = handler.CreateToken(tokenDescriptor);
        return handler.WriteToken(token);
    }
    
    private async Task<ClaimsIdentity> GenerateClaims(User user)
    {
        var userRoles = await _roleService.GetRole(user.Id);
        var claims = new ClaimsIdentity();
        claims.AddClaim(new Claim(ClaimTypes.Email, user.Email));
        claims.AddClaim(new Claim("Id", user.Id.ToString()));
        claims.AddClaim(new Claim(ClaimTypes.Role, userRoles.ElementAt(0)));
        Console.WriteLine(userRoles.ElementAt(0));
        return claims;
    }
}