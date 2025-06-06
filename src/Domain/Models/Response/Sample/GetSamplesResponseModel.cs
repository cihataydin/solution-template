namespace Domain.Models.Response.Sample;

public class GetSamplesResponseModel
{
    public IList<GetSampleResponseModel> Samples { get; set; } = new List<GetSampleResponseModel>();
}
