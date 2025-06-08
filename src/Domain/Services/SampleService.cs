using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Models.Request.Sample;
using Domain.Models.Response.Sample;

namespace Domain.Services;

public class SampleService : ISampleService
{
    private readonly IRepository<SampleEntity> _repository;
    private readonly IMapper _mapper;

    private readonly ICacheManager _cacheManager;

    public SampleService(IRepository<SampleEntity> repository,
    IMapper mapper,
    ICacheManager cacheManager)
    {
        _repository = repository;
        _mapper = mapper;
        _cacheManager = cacheManager;
    }

    public async Task<GetSampleResponseModel> GetSampleAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var cachedSample = await _cacheManager.GetAsync<SampleEntity>($"Sample_{id}");

        if (cachedSample is not null)
        {
            return _mapper.Map<GetSampleResponseModel>(cachedSample);
        }
        
        var entity = await _repository.GetByIdAsync(id, cancellationToken) ?? throw new DomainException($"Sample with ID {id} not found.");

        return _mapper.Map<GetSampleResponseModel>(entity);
    }

    public GetSamplesResponseModel GetSamples(GetSamplesRequestModel request)
    {
        var query = _repository.Query();

        if (!string.IsNullOrWhiteSpace(request.Name))
            query = query.Where(u => u.Name == request.Name);

        if (!string.IsNullOrWhiteSpace(request.OrderBy))
        {
            query = request.Direction == SortDirection.Desc
                ? query.OrderByDescending(u => _repository.Property(u, request.OrderBy))
                : query.OrderBy(u => _repository.Property(u, request.OrderBy));
        }
        else
        {
            query = query.OrderByDescending(u => _repository.Property(u, nameof(SampleEntity.CreatedAt)));
        }

        var totalCount = query.Count();

        var skip = (request.Page - 1) * request.Limit;
        var samples = query
                          .Skip(skip)
                          .Take(request.Limit)
                          .ToList();

        var items = samples
            .Select(u => _mapper.Map<GetSampleResponseModel>(u))
            .ToList();

        var totalPages = (int)Math.Ceiling(totalCount / (double)request.Limit);

        return new GetSamplesResponseModel(items, totalCount, totalPages);
    }

    public async Task<CreateSampleResponseModel> CreateSampleAsync(CreateSampleRequestModel request, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<SampleEntity>(request);
        var createdEntity = await _repository.AddAsync(entity, cancellationToken);

        await _repository.SaveChangesAsync(cancellationToken);

        await _cacheManager.SetAsync($"Sample_{createdEntity.Id}", createdEntity);

        return _mapper.Map<CreateSampleResponseModel>(createdEntity);
    }

    public async Task<UpdateSampleResponseModel> UpdateSampleAsync(UpdateSampleRequestModel request, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<SampleEntity>(request);

        var updatedEntity = _repository.Update(entity);

        await _repository.SaveChangesAsync(cancellationToken);

        await _cacheManager.SetAsync($"Sample_{updatedEntity.Id}", updatedEntity);

        return _mapper.Map<UpdateSampleResponseModel>(updatedEntity);
    }

    public async Task<bool> DeleteSampleAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, cancellationToken);
        if (entity == null) return false;

        _repository.Remove(entity);

        var result = await _repository.SaveChangesAsync(cancellationToken);

        await _cacheManager.RemoveAsync($"Sample_{id}");

        return result > 0;
    }
}