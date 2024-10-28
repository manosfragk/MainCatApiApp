namespace CatApiApp.Dtos {
    /// <summary>
    /// Data Transfer Object for user login information.
    /// </summary>
    public class LoginDto {
        /// <summary>
        /// Gets or sets the username of the user attempting to log in.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the password of the user attempting to log in.
        /// </summary>
        public string Password { get; set; }
    }
}
