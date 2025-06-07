using Domain.Models.Request.Sample;
using Domain.Models.Response.Sample;

namespace Domain.Interfaces;

public interface ISampleService
{
    GetSamplesResponseModel GetSamples(GetSamplesRequestModel request);
    Task<GetSampleResponseModel> GetSampleAsync(Guid id, CancellationToken cancellationToken = default);
    Task<CreateSampleResponseModel> CreateSampleAsync(CreateSampleRequestModel request, CancellationToken cancellationToken = default);
    Task<UpdateSampleResponseModel> UpdateSampleAsync(UpdateSampleRequestModel request, CancellationToken cancellationToken = default);
    Task<bool> DeleteSampleAsync(Guid id, CancellationToken cancellationToken = default);
}
