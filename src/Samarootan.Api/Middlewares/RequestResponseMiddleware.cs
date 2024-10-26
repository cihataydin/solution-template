namespace Samarootan.Api.Middlewares
{
    using System.Text;
    using Newtonsoft.Json;
    using Samarootan.Api.Constants;
    using Serilog;

    /// <summary>
    /// Represents the request response logging middleware.
    /// </summary>
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IConfiguration configuration;

        private bool useJsonFormat;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestResponseLoggingMiddleware"/> class.
        /// </summary>
        /// <param name="next">The request delegate.</param>
        /// <param name="configuration">The configuration.</param>
        public RequestResponseLoggingMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            this.next = next;
            this.configuration = configuration;
        }

        /// <summary>
        /// Invokes the request response logging middleware.
        /// </summary>
        /// <param name="context">The HTTP context.</param>
        /// <returns>The task.</returns>
        public async Task Invoke(HttpContext context)
        {
            this.useJsonFormat = this.configuration.GetValue<bool>(ConfigurationConstant.UseJsonFormat);
            await this.LogRequest(context);

            var originalBodyStream = context.Response.Body;
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            await this.next(context);

            await this.LogResponse(context);

            await responseBody.CopyToAsync(originalBodyStream);
            context.Response.Body = originalBodyStream;
        }

        private async Task LogRequest(HttpContext context)
        {
            var request = context.Request;
            var requestBody = await this.GetRequestBody(request);

            var logObject = new
            {
                method = request.Method,
                path = request.Path,
                baseUrl = $"{request.Scheme}://{request.Host}",
                ip = context.Connection.RemoteIpAddress?.ToString(),
                body = !string.IsNullOrEmpty(requestBody) ? JsonConvert.DeserializeObject(requestBody) : new { },
                headers = request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()),
                timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                traceId = context.TraceIdentifier,
            };

            Log.Information("{Context} {Request}", this.useJsonFormat ? string.Empty : "[RequestLoggerMiddleware]", JsonConvert.SerializeObject(logObject));
        }

        private async Task LogResponse(HttpContext context)
        {
            var response = context.Response;
            var responseBody = await this.GetResponseBody(response);

            var logObject = new
            {
                statusCode = response.StatusCode,
                responseBody = !string.IsNullOrEmpty(responseBody) ? JsonConvert.DeserializeObject(JsonConvert.SerializeObject(responseBody)) : new { },
                method = context.Request.Method,
                ip = context.Connection.RemoteIpAddress?.ToString(),
                path = context.Request.Path,
                baseUrl = $"{context.Request.Scheme}://{context.Request.Host}",
                body = new { },  // Response does not have a body to log
                headers = context.Request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()),
                timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                traceId = context.TraceIdentifier,
            };

            Log.Information("{Context} {Response}", this.useJsonFormat ? string.Empty : "[ResponseLoggerMiddleware]", JsonConvert.SerializeObject(logObject));
        }

        private async Task<string> GetRequestBody(HttpRequest request)
        {
            request.EnableBuffering();
            using var reader = new StreamReader(request.Body, Encoding.UTF8, true, 1024, true);
            var body = await reader.ReadToEndAsync();
            request.Body.Position = 0;
            return body;
        }

        private async Task<string> GetResponseBody(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            var body = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);
            return body;
        }
    }
}
