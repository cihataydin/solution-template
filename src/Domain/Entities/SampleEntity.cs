namespace Domain.Entities;

public class SampleEntity : BaseEntity<Guid> 
{
    public string Name { get; set; } = default!;
}
