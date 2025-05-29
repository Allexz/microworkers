using Microsoft.EntityFrameworkCore;
using Microworkers.Infrastructure.Data.Models;

namespace Microworkers.Infrastructure.Data;
// Infrastructure/Data/EventStoreContext.cs
public class EventStoreContext : DbContext
{
    public DbSet<StoredEvent> Events { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<StoredEvent>(b =>
        {
            b.Property(e => e.Id).ValueGeneratedNever();
            b.Property(e => e.EventType).IsRequired();
            b.Property(e => e.Payload).IsRequired().HasColumnType("jsonb");
        });
    }
}
