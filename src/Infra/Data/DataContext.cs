using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog;


namespace Infra.Data;
public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> opts) : base(opts) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.LogTo(message => Log.Information(message));
        base.OnConfiguring(optionsBuilder);
    }

    public virtual DbSet<SampleEntity> Samples => Set<SampleEntity>();
}