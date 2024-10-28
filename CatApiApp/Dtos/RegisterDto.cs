namespace CatApiApp.Dtos {
    /// <summary>
    /// Data Transfer Object for user registration information.
    /// </summary>
    public class RegisterDto {
        /// <summary>
        /// Gets or sets the username for the new user.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the email address for the new user.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the password for the new user.
        /// </summary>
        public string Password { get; set; }
    }
}
