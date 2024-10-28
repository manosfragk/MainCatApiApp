using Microsoft.AspNetCore.Identity;

namespace CatApiApp.Interfaces {
    /// <summary>
    /// Service interface for handling authentication tasks, including generating JWT tokens.
    /// </summary>
    public interface IAuthService {
        /// <summary>
        /// Generates a JWT token for the specified user.
        /// </summary>
        /// <param name="user">The user for whom to generate the token.</param>
        /// <returns>A JWT token as a <see cref="string"/>.</returns>
        Task<string> GenerateJwtTokenAsync(IdentityUser user);
    }
}
