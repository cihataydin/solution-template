namespace Api.Middlewares
{
    using System;
    using System.Net;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Domain.Exceptions;
    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// Extension methods for configuring the exception handling middleware.
    /// </summary>
    public static class ExceptionHandlingExtensions
    {
        /// <summary>
        /// Adds the global exception handler middleware to the application pipeline.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <returns>The application builder with the middleware added.</returns>
        public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
            => app.UseMiddleware<ExceptionHandlingMiddleware>();
    }

    /// <summary>
    /// Middleware to handle exceptions globally in the application.
    /// </summary>
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate next;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionHandlingMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next middleware in the pipeline.</param>
        public ExceptionHandlingMiddleware(RequestDelegate next)
            => this.next = next;

        /// <summary>
        /// Invokes the middleware to handle exceptions.
        /// </summary>
        /// <param name="ctx">The HTTP context.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task InvokeAsync(HttpContext ctx)
        {
            try
            {
                await this.next(ctx);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(ctx, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext ctx, Exception exception)
        {
            HttpStatusCode status;
            string errorCode;
            string message = exception.Message;

            if (exception is DomainException de)
            {
                status = de.StatusCode;
                errorCode = de.ErrorCode;
            }
            else
            {
                status = HttpStatusCode.InternalServerError;
                errorCode = "UNHANDLED_ERROR";
            }

            ctx.Response.ContentType = "application/json";
            ctx.Response.StatusCode = (int)status;

            var payload = JsonSerializer.Serialize(new
            {
                error = message,
                code = errorCode,
            });

            return ctx.Response.WriteAsync(payload);
        }
    }
}
