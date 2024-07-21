using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using @true.code.testtask.Domain.Models;

namespace @true.code.testtask.Infrastructure;

public class TodoDbContext(IOptionsMonitor<AppOptions> options) : DbContext
{
    public DbSet<Priority> Priorities { get; set; }

    public DbSet<TodoItem> TodoItems { get; set; }

    public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = options.CurrentValue.ConnectionString;
        optionsBuilder.UseNpgsql(connectionString);
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Priority>().HasIndex(x => x.Level).IsUnique();
        base.OnModelCreating(builder);
    }
}