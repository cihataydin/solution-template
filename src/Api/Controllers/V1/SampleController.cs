namespace Api.Controllers.V1
{
    using Asp.Versioning;
    using Domain.Entities;
    using Domain.Interfaces;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Represents the sample controller.
    /// </summary>
    [ApiController]
    [Route("sample")]
    [Tags("sample")]
    [ApiVersion("1.0")]
    public class SampleController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching",
        };

        private readonly IRepository<SampleEntity> repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="SampleController"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public SampleController(IRepository<SampleEntity> repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Gets the weather forecast.
        /// </summary>
        /// <returns>The weather forecast.</returns>
        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            this.repository.Add(new SampleEntity { Name = "Sample-1" });
            var result = await this.repository.ListAsync();
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
