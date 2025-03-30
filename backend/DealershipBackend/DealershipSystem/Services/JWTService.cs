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

namespace DealershipSystem.Services
{
    /// <summary>
    /// Service class for handling JWT authentication.
    /// </summary>
    public class JWTService
    {
        private readonly RoleService _roleService;
        private readonly Lazy<IUserService> _userService;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="JWTService"/> class.
        /// </summary>
        /// <param name="roleService">The role service.</param>
        /// <param name="userService">The user service.</param>
        /// <param name="mapper">The object mapper.</param>
        public JWTService(RoleService roleService, Lazy<IUserService> userService, IMapper mapper)
        {
            _roleService = roleService;
            _userService = userService;
            _mapper = mapper;
        }

        /// <summary>
        /// Generates access and refresh tokens for a user.
        /// </summary>
        /// <param name="user">The user for whom the tokens are generated.</param>
        /// <returns>A tuple containing the access token and refresh token.</returns>
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

            // Generate refresh token (valid for 30 days)
            var refreshToken = GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(30);

            // Save updated user with refresh token
            await _userService.Value.UpdateUserAsync(Guid.Parse(user.Id), _mapper.Map<UserDTO>(user)); 

            return (accessToken, refreshToken);
        }
        
        /// <summary>
        /// Generates a secure refresh token.
        /// </summary>
        /// <returns>A base64-encoded refresh token.</returns>
        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        /// <summary>
        /// Refreshes the access token using a valid refresh token.
        /// </summary>
        /// <param name="refreshToken">The refresh token.</param>
        /// <returns>A tuple indicating whether the operation was successful and the new access token.</returns>
        public async Task<(bool IsValid, string AccessToken)> RefreshAccessToken(string refreshToken)
        {
            var user = await _userService.Value.GetUserByRefreshTokenAsync(refreshToken);
            if (user == null || user.RefreshTokenExpiry < DateTime.UtcNow)
            {
                return (false, string.Empty); 
            }

            var (newAccessToken, newRefreshToken) = await GenerateTokens(user);
            return (true, newAccessToken);
        }

        /// <summary>
        /// Revokes a refresh token, making it invalid.
        /// </summary>
        /// <param name="refreshToken">The refresh token to revoke.</param>
        /// <returns>A boolean indicating whether the operation was successful.</returns>
        public async Task<bool> RevokeRefreshToken(string refreshToken)
        {
            var user = await _userService.Value.GetUserByRefreshTokenAsync(refreshToken);
            if (user == null) return false;

            user.RefreshToken = null;
            user.RefreshTokenExpiry = null;
            
            await _userService.Value.UpdateUserAsync(Guid.Parse(user.Id), _mapper.Map<UserDTO>(user));

            return true;
        }
        
        /// <summary>
        /// Generates claims for a user.
        /// </summary>
        /// <param name="user">The user whose claims are generated.</param>
        /// <returns>A ClaimsIdentity containing user claims.</returns>
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
}