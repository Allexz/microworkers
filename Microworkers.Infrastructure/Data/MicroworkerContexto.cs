using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microworkers.Infrastructure.Data;
internal class MicroworkerContexto : DbContext
{
    public MicroworkerContexto() { }
    public MicroworkerContexto(DbContextOptions<MicroworkerContexto> options)
        :base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        ChangeTracker.AutoDetectChangesEnabled = false;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MicroworkerContexto).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
