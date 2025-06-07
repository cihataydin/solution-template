using Domain.Entities;
using Domain.Enums;

namespace Domain.Models.Request;
public class FilterRequestModel
{
    public int Page { get; set; } = 1;
    public int Limit { get; set; } = 10;
    public string OrderBy { get; set; } = nameof(BaseEntity<Guid>.Id);
    public SortDirection Direction { get; set; } = SortDirection.Asc;
}