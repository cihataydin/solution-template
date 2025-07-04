#pragma warning disable SA1200 // Using directive should appear within a namespace declaration
using Api.Configurations;
using Api.MappingProfiles;
using Api.Middlewares;
using Asp.Versioning;
using Asp.Versioning.Builder;
using Domain.Interfaces;
using Domain.Services;
using Infra.Cache;
using Infra.Data;
using Infra.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Prometheus;
using Scalar.AspNetCore;
using Serilog;
#pragma warning restore SA1200 // Using directive should appear within a namespace declaration

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<DataContext>(opts => opts.UseInMemoryDatabase("DemoDb"))

    // .AddDbContext<DataContext>(opts => opts.UseNpgsql(builder.Configuration.GetConnectionString("PgpoolDb")))
    .AddScoped(typeof(IRepository<>), typeof(Repository<>))
    .AddScoped(typeof(ISampleService), typeof(SampleService));

builder.Services.AddAutoMapper(typeof(SampleProfile).Assembly);

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

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
   {
       c.SwaggerDoc("v1", new OpenApiInfo { Title = "Solution Template - V1", Version = "v1.0", Description = "This is the first version of the solution template.", Contact = new OpenApiContact { Name = "samarootan", Url = new Uri("https://samarootan.com") } });
       c.SwaggerDoc("v2", new OpenApiInfo { Title = "Solution Template - V2", Version = "v2.0", Description = "This is the second version of the solution template.", Contact = new OpenApiContact { Name = "samarootan", Url = new Uri("https://samarootan.com") } });
   });

builder.Host.UseSerilog();

builder.Services.AddMemoryCache();
builder.Services.AddStackExchangeRedisCache(opts =>
{
    opts.Configuration = builder.Configuration.GetConnectionString("Redis");
    opts.InstanceName = "SolutionTemplate:";
});
builder.Services.Configure<CacheManagerOptions>(builder.Configuration.GetSection("CacheOptions"));
builder.Services.AddSingleton<ICacheManager, CacheManager>();

var app = builder.Build();

app.UseHttpMetrics();

app.UseWhen(
    ctx => !ctx.Request.Path.StartsWithSegments("/metrics"),
    branch => branch.UseMiddleware<RequestResponseLoggingMiddleware>());
LogConfiguration.Initialize(builder.Configuration);
app.UseSwagger(c =>
{
    c.RouteTemplate = "openapi/{documentName}.json";
});

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/openapi/v1.json", "Solution Template - V1");
    options.SwaggerEndpoint("/openapi/v2.json", "Solution Template - V2");
});
app.MapScalarApiReference();
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
.HasApiVersion(new ApiVersion(2, 0));

app.UseGlobalExceptionHandler();

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
