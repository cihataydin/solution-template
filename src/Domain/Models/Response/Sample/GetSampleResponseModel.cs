namespace Domain.Models.Response.Sample;

public class GetSampleResponseModel
{
    public Guid? Id { get; set; }

    public string? Name { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? CreatedAt { get; set; }
}