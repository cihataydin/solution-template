namespace Domain.Entities;

public abstract class BaseEntity<TKey>
{
    public TKey Id { get; set; } = default!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
