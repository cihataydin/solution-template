using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Models.Request.Sample;
using Domain.Models.Response.Sample;

namespace Domain.Services;

public class SampleService : ISampleService
{
    private readonly IRepository<SampleEntity> _repository;
    private readonly IMapper _mapper;

    public SampleService(IRepository<SampleEntity> repository,
    IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<GetSampleResponseModel> GetSampleAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, cancellationToken);
        return _mapper.Map<GetSampleResponseModel>(entity);
    }

    public async Task<GetSamplesResponseModel> GetSamplesAsync(GetSamplesRequestModel request, CancellationToken cancellationToken = default)
    {
        Expression<Func<SampleEntity, bool>> predicate = e => true;
        if (!string.IsNullOrEmpty(request.Name))
        {
            predicate = e => e.Name == request.Name;

        }
        var samples = await _repository.ListAsync(predicate, true, cancellationToken);
        var response = new GetSamplesResponseModel
        {
            Samples = _mapper.Map<List<GetSampleResponseModel>>(samples)
        };
        return response;
    }

    public async Task<CreateSampleResponseModel> CreateSampleAsync(CreateSampleRequestModel request, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<SampleEntity>(request);
        var createdEntity = await _repository.AddAsync(entity, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
        return _mapper.Map<CreateSampleResponseModel>(createdEntity);
    }

    public async Task<UpdateSampleResponseModel> UpdateSampleAsync(UpdateSampleRequestModel request, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<SampleEntity>(request);
        var updatedEntity = _repository.Update(entity);
        await _repository.SaveChangesAsync(cancellationToken);
        return _mapper.Map<UpdateSampleResponseModel>(updatedEntity);
    }

    public async Task<bool> DeleteSampleAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, cancellationToken);
        if (entity == null) return false;

        _repository.Remove(entity);
        return true && await _repository.SaveChangesAsync(cancellationToken) > 0;
    }
}