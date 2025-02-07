using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using DealershipSystem.DTO;
using DealershipSystem.Helpers;
using DealershipSystem.Interfaces;
using DealershipSystem.Models;
using DealershipSystem.Services;
using Microsoft.IdentityModel.Tokens;

public class JWTService
{
    private readonly RoleService _roleService;
    private readonly Lazy<IUserService> _userService;
    private readonly IMapper _mapper;

    public JWTService(RoleService roleService, Lazy<IUserService> userService, IMapper mapper)
    {
        _roleService = roleService;
        _userService = userService;
        _mapper = mapper;
    }

    public async Task<(string AccessToken, string RefreshToken)> GenerateTokens(User user)
    {
        var handler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(AuthSettings.PrivateKey);
        var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);
    
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = await GenerateClaims(user),
            Expires = DateTime.UtcNow.AddMinutes(15),
            SigningCredentials = credentials,
        };
        var accessToken = handler.WriteToken(handler.CreateToken(tokenDescriptor));

        // リフレッシュトークンの生成（30日有効）
        var refreshToken = GenerateRefreshToken();
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(30);

        // 修正: Lazy のため .Value を追加
        await _userService.Value.UpdateUserAsync(Guid.Parse(user.Id), _mapper.Map<UserDTO>(user)); 

        return (accessToken, refreshToken);
    }
    
    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public async Task<(bool IsValid, string AccessToken)> RefreshAccessToken(string refreshToken)
    {
        // 修正: Lazy のため .Value を追加
        var user = await _userService.Value.GetUserByRefreshTokenAsync(refreshToken);
        if (user == null || user.RefreshTokenExpiry < DateTime.UtcNow)
        {
            return (false, string.Empty); // 無効なリフレッシュトークン
        }

        var (newAccessToken, newRefreshToken) = await GenerateTokens(user);
        return (true, newAccessToken);
    }

    public async Task<bool> RevokeRefreshToken(string refreshToken)
    {
        // 修正: Lazy のため .Value を追加
        var user = await _userService.Value.GetUserByRefreshTokenAsync(refreshToken);
        if (user == null) return false;

        user.RefreshToken = null;
        user.RefreshTokenExpiry = null;

        // 修正: Lazy のため .Value を追加
        await _userService.Value.UpdateUserAsync(Guid.Parse(user.Id), _mapper.Map<UserDTO>(user));

        return true;
    }
    
    private async Task<ClaimsIdentity> GenerateClaims(User user)
    {
        var userRoles = await _roleService.GetRole(user.Id);
        var claims = new ClaimsIdentity();
        claims.AddClaim(new Claim(ClaimTypes.Email, user.Email));
        claims.AddClaim(new Claim("Id", user.Id.ToString()));

        foreach (var role in userRoles)
        {
            claims.AddClaim(new Claim(ClaimTypes.Role, role));
        }

        return claims;
    }
}
