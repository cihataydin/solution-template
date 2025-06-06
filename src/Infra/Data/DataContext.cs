using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data;
public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> opts) : base(opts) { }

    public virtual DbSet<SampleEntity> Samples => Set<SampleEntity>();
}