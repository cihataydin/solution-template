namespace Samarootan.Api.Controllers.V2
{
    using Asp.Versioning;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Represents the sample controller.
    /// </summary>
    [ApiController]
    [Route("sample")]
    [Tags("sample")]
    [ApiVersion("2.0")]
    public class SampleController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching",
        };

        /// <summary>
        /// Gets the weather forecast.
        /// </summary>
        /// <returns>The weather forecast.</returns>
        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast(
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    Summaries[Random.Shared.Next(Summaries.Length)]))
                .ToArray();
            return forecast;
        }
    }
}
