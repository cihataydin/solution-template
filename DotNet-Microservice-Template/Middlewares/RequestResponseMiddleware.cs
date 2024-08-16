using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Serilog;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Middlewares;

public class RequestResponseLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public RequestResponseLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        await LogRequest(context);

        var originalBodyStream = context.Response.Body;
        using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        await _next(context);

        await LogResponse(context);

        await responseBody.CopyToAsync(originalBodyStream);
        context.Response.Body = originalBodyStream;
    }

    private async Task LogRequest(HttpContext context)
    {
        var request = context.Request;
        var requestBody = await GetRequestBody(request);

        var logObject = new
        {
            method = request.Method,
            path = request.Path,
            baseUrl = $"{request.Scheme}://{request.Host}",
            ip = context.Connection.RemoteIpAddress?.ToString(),
            body = !string.IsNullOrEmpty(requestBody) ? JsonConvert.DeserializeObject(requestBody) : new {},
            headers = request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()),
            timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
            traceId = context.TraceIdentifier
        };

        Log.Information("[RequestLoggerMiddleware] {Request}", JsonConvert.SerializeObject(logObject));
    }

    private async Task LogResponse(HttpContext context)
    {
        var response = context.Response;
        var responseBody = await GetResponseBody(response);

        var logObject = new
        {
            statusCode = response.StatusCode,
            responseBody = !string.IsNullOrEmpty(responseBody) ? JsonConvert.DeserializeObject(JsonConvert.SerializeObject(responseBody)) : new {},
            method = context.Request.Method,
            ip = context.Connection.RemoteIpAddress?.ToString(),
            path = context.Request.Path,
            baseUrl = $"{context.Request.Scheme}://{context.Request.Host}",
            body = new {},  // Response does not have a body to log
            headers = context.Request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()),
            timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
            traceId = context.TraceIdentifier
        };

        Log.Information("[ResponseLoggerMiddleware] {Response}", JsonConvert.SerializeObject(logObject));
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

public static class RequestResponseLoggingMiddlewareExtensions
{
    public static IApplicationBuilder UseRequestResponseLogging(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RequestResponseLoggingMiddleware>();
    }
}