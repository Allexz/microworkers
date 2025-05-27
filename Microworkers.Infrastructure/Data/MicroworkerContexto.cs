using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microworkers.Domain.Core.Aggregates;

namespace Microworkers.Infrastructure.Data;
internal class MicroworkerContexto : DbContext
{
    private readonly IConfiguration _configuration;
    private readonly bool _isPostgreSql;
    public DbSet<User> Users { get; set; }
    public MicroworkerContexto() { }
    public MicroworkerContexto(DbContextOptions<MicroworkerContexto> options, IConfiguration configuration)
        :base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        ChangeTracker.AutoDetectChangesEnabled = false;
        _configuration = configuration;
        _isPostgreSql = _configuration
            .GetConnectionString("DefaultConnection")
            .Contains("Postgre");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            if (connectionString.Contains("Postgre"))
            {
                optionsBuilder.UseNpgsql(connectionString,
                    npgOptions => npgOptions.MigrationsAssembly("Microworkers.Infrastructure"));
            }
            else
            {
                optionsBuilder.UseSqlServer(connectionString,
                    sqlOptions => sqlOptions.MigrationsAssembly("Microworkers.Infrastructure"));
            }
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MicroworkerContexto).Assembly);
        if (_isPostgreSql)
        {
            ConfigureForPostgreSql(modelBuilder);
        }
        else
        {
            ConfigureForSqlServer(modelBuilder);
        }
        base.OnModelCreating(modelBuilder);
    }

    private void ConfigureForPostgreSql(ModelBuilder modelBuilder)
    {
        // Ativa a extensão UUID para PostgreSQL se não existir
        modelBuilder.HasPostgresExtension("uuid-ossp");

        // Configura o valor padrão para GUIDs
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var properties = entityType.ClrType.GetProperties()
                .Where(p => p.PropertyType == typeof(Guid) && p.Name == "Id");

            foreach (var property in properties)
            {
                modelBuilder.Entity(entityType.Name)
                    .Property(property.Name)
                    .HasDefaultValueSql("gen_random_uuid()");
            }
        }
    }

    private void ConfigureForSqlServer(ModelBuilder modelBuilder)
    {
        // Configura o valor padrão para GUIDs no SQL Server
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var properties = entityType.ClrType.GetProperties()
                .Where(p => p.PropertyType == typeof(Guid) && p.Name == "Id");

            foreach (var property in properties)
            {
                modelBuilder.Entity(entityType.Name)
                    .Property(property.Name)
                    .HasDefaultValueSql("NEWID()");
            }
        }

        // Configuração específica para SQL Server (opcional)
        modelBuilder.HasDefaultSchema("dbo");
    }
}
