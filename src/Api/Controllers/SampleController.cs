namespace Api.Controllers
{
    using Asp.Versioning;
    using Domain.Interfaces;
    using Domain.Models.Request.Sample;
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
        private readonly ISampleService sampleService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SampleController"/> class.
        /// </summary>
        /// <param name="sampleService">The sample service.</param>
        public SampleController(ISampleService sampleService)
        {
            this.sampleService = sampleService ?? throw new ArgumentNullException(nameof(sampleService));
        }

        /// <summary>
        /// Gets a samples.
        /// </summary>
        /// <returns>The sample response model.</returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await this.sampleService.GetSamplesAsync(new GetSamplesRequestModel(), CancellationToken.None);
            return this.Ok(result);
        }

        /// <summary>
        /// Gets a sample by ID.
        /// </summary>
        /// <param name="id">The sample ID.</param>
        /// <returns>The sample response model.</returns>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await this.sampleService.GetSampleAsync(id, CancellationToken.None);
            if (result == null)
            {
                return this.NotFound();
            }

            return this.Ok(result);
        }

        /// <summary>
        /// Creates a new sample.
        /// </summary>
        /// <param name="request">The create sample request model.</param>
        /// <returns>The created sample response model.</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSampleRequestModel request)
        {
            if (request == null)
            {
                return this.BadRequest("Request cannot be null.");
            }

            var result = await this.sampleService.CreateSampleAsync(request, CancellationToken.None);
            return this.CreatedAtAction(nameof(this.Get), new { id = result.Id }, result);
        }

        /// <summary>
        /// Updates an existing sample.
        /// </summary>
        /// <param name="request">The update sample request model.</param>
        /// <returns>The updated sample response model.</returns>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateSampleRequestModel request)
        {
            if (request == null || request.Id == Guid.Empty)
            {
                return this.BadRequest("Request cannot be null and ID must be provided.");
            }

            var result = await this.sampleService.UpdateSampleAsync(request, CancellationToken.None);
            if (result == null)
            {
                return this.NotFound();
            }

            return this.Ok(result);
        }

        /// <summary>
        /// Deletes a sample by ID.
        /// </summary>
        /// <param name="id">The sample ID.</param>
        /// <returns>A boolean indicating success or failure.</returns>
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                return this.BadRequest("ID must be provided.");
            }

            var result = await this.sampleService.DeleteSampleAsync(id, CancellationToken.None);
            if (!result)
            {
                return this.NotFound();
            }

            return this.NoContent();
        }
    }
}