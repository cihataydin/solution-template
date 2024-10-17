namespace Extensions
{
    using Middlewares;

    /// <summary>
    /// Represents the request response logging middleware extensions.
    /// </summary>
    public static class RequestResponseLoggingMiddlewareExtensions
    {
        /// <summary>
        /// Uses the request response logging middleware.
        /// </summary>
        /// <param name="builder">The application builder.</param>
        /// <returns>The application builder with used request response logging middleware.</returns>
        public static IApplicationBuilder UseRequestResponseLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestResponseLoggingMiddleware>();
        }
    }
}
