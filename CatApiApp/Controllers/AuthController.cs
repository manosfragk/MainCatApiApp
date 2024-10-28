using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CatApiApp.Interfaces;
using CatApiApp.Dtos;

namespace CatApiApp.Controllers {
    /// <summary>
    /// Controller for handling authentication-related actions such as user registration and login.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="AuthController"/> class.
    /// </remarks>
    /// <param name="userManager">The user manager for managing identity users.</param>
    /// <param name="authService">The authentication service for generating JWT tokens.</param>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(UserManager<IdentityUser> userManager, IAuthService authService) : ControllerBase {
        private readonly UserManager<IdentityUser> _userManager = userManager;
        private readonly IAuthService _authService = authService;

        /// <summary>
        /// Registers a new user with the provided information.
        /// </summary>
        /// <param name="model">The user registration data.</param>
        /// <returns>
        /// An <see cref="IActionResult"/> indicating the result of the registration.
        /// Returns <c>200 OK</c> if successful; otherwise, returns <c>400 Bad Request</c> with validation errors.
        /// </returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model) {
            var user = new IdentityUser { UserName = model.Username, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok("User registered successfully.");
        }

        /// <summary>
        /// Authenticates a user and provides a JWT token for authorized access.
        /// </summary>
        /// <param name="model">The user login data.</param>
        /// <returns>
        /// An <see cref="IActionResult"/> with a JWT token if successful;
        /// otherwise, returns <c>401 Unauthorized</c> if authentication fails.
        /// </returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model) {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password)) {
                var token = await _authService.GenerateJwtTokenAsync(user);
                return Ok(new { token });
            }
            return Unauthorized("Invalid login attempt.");
        }
    }
}
