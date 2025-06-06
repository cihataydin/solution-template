using Domain.Models.Request.Sample;
using Domain.Models.Response.Sample;

namespace Domain.Interfaces;

public interface ISampleService
{
    Task<CreateSampleResponseModel> CreateSampleAsync(CreateSampleRequestModel request, CancellationToken cancellationToken = default);
    Task<GetSamplesResponseModel> GetSamplesAsync(GetSamplesRequestModel request, CancellationToken cancellationToken = default);
    Task<UpdateSampleResponseModel> UpdateSampleAsync(UpdateSampleRequestModel request, CancellationToken cancellationToken = default);
    Task<GetSampleResponseModel> GetSampleAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> DeleteSampleAsync(Guid id, CancellationToken cancellationToken = default);
}
