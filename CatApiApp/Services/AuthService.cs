using CatApiApp.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CatApiApp.Services {
    /// <summary>
    /// Service responsible for authentication tasks, including generating JWT tokens for users.
    /// </summary>
    /// <param name="userManager">The user manager for managing identity users and their roles.</param>
    /// <param name="configuration">The configuration settings, including JWT options.</param>
    public class AuthService(UserManager<IdentityUser> userManager, IConfiguration configuration) : IAuthService {
        private readonly UserManager<IdentityUser> _userManager = userManager;
        private readonly IConfiguration _configuration = configuration;

        /// <summary>
        /// Generates a JSON Web Token (JWT) for the specified user, including claims and roles.
        /// </summary>
        /// <param name="user">The <see cref="IdentityUser"/> for whom to generate the token.</param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation, with a JWT token string as a result.
        /// </returns>
        public async Task<string> GenerateJwtTokenAsync(IdentityUser user) {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            // Add user roles as claims
            var roles = await _userManager.GetRolesAsync(user);
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            // Create security key and signing credentials
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Create the token with issuer, audience, claims, expiration, and signing credentials
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            // Return the serialized JWT token as a string
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
