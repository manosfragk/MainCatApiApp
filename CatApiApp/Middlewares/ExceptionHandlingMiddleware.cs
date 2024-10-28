namespace CatApiApp.Middlewares {
    /// <summary>
    /// Middleware for handling unhandled exceptions in the request pipeline.
    /// </summary>
    /// <remarks>
    /// This middleware catches any unhandled exceptions that occur during request processing,
    /// logs the error details, and returns a generic error response to the client.
    /// </remarks>
    /// <param name="next">The next middleware delegate in the pipeline.</param>
    /// <param name="logger">The logger instance used to log exception details.</param>
    public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger) {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger = logger;

        /// <summary>
        /// Invokes the middleware to handle incoming HTTP requests and catch unhandled exceptions.
        /// </summary>
        /// <param name="context">The current HTTP context.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task InvokeAsync(HttpContext context) {
            try {
                // Pass the request to the next middleware component
                await _next(context);
            }
            catch (Exception ex) {
                // Log the exception details
                _logger.LogError(ex, "An unhandled exception occurred while processing the request.");

                // Set the response status code to 500 and return a generic error message
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync("An unexpected error occurred. Please try again later.");
            }
        }
    }
}
