#pragma warning disable SA1200 // Using directive should appear within a namespace declaration
using Asp.Versioning;
using Asp.Versioning.Builder;
using Microsoft.OpenApi.Models;
using Prometheus;
using Samarootan.Api.Configurations;
using Samarootan.Api.Extensions;
using Serilog;
#pragma warning restore SA1200 // Using directive should appear within a namespace declaration

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddApiVersioning(options =>
   {
       options.DefaultApiVersion = new ApiVersion(1, 0);
       options.AssumeDefaultVersionWhenUnspecified = true;
       options.ReportApiVersions = true;
       options.ApiVersionReader = ApiVersionReader.Combine(
           new HeaderApiVersionReader("x-api-version"));
   }).AddApiExplorer(options =>
   {
       options.GroupNameFormat = "'v'VVV";
       options.SubstituteApiVersionInUrl = true;
   });

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
   {
       c.SwaggerDoc("v1", new OpenApiInfo { Title = "Solution Template - V1", Version = "v1.0", Description = "This is the first version of the solution template.", Contact = new OpenApiContact { Name = "samarootan", Url = new Uri("https://samarootan.com") } });
       c.SwaggerDoc("v2", new OpenApiInfo { Title = "Solution Template - V2", Version = "v2.0", Description = "This is the second version of the solution template.", Contact = new OpenApiContact { Name = "samarootan", Url = new Uri("https://samarootan.com") } });
   });

builder.Host.UseSerilog();

var app = builder.Build();

app.UseHttpMetrics();

app.UseRequestResponseLogging();
LogConfiguration.Initialize(builder.Configuration);

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Solution Template - V1");
    options.SwaggerEndpoint("/swagger/v2/swagger.json", "Solution Template - V2");
});

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching",
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast(
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi()
.WithApiVersionSet(new ApiVersionSet(new ApiVersionSetBuilder(string.Empty), "weatherForecast"))
.HasApiVersion(new ApiVersion(1, 0));

app.MapControllers();
app.MapMetrics();

app.Run();

/// <summary>
/// Represents the weather forecast.
/// </summary>
/// <param name="date"></param>
/// <param name="temperatureC"></param>
/// <param name="summary"></param>
public record WeatherForecast(DateOnly date, int temperatureC, string summary)
{
    /// <summary>
    /// Gets the temperature in Fahrenheit.
    /// </summary>
    public int TemperatureF => 32 + (int)(this.temperatureC / 0.5556);
}
