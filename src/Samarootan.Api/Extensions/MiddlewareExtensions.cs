namespace Samarootan.Api.Extensions
{
    using Samarootan.Api.Middlewares;

    /// <summary>
    /// Represents the middleware extensions.
    /// </summary>
    public static class MiddlewareExtensions
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
