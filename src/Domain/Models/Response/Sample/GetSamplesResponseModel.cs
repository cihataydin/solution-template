namespace Domain.Models.Response.Sample;

public class GetSamplesResponseModel
{
    public IReadOnlyList<GetSampleResponseModel> Items { get; }
    public int TotalCount { get; }
    public int TotalPages { get; }

    public GetSamplesResponseModel(
        IReadOnlyList<GetSampleResponseModel> items,
        int totalCount,
        int totalPages)
    {
        Items      = items;
        TotalCount = totalCount;
        TotalPages = totalPages;
    }
}
