using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Microworkers.Infrastructure.Data;

public static class DatabaseConfiguration
{
    public static IServiceCollection AddDatabaseModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? configuration["DATABASE_URL"]
            ?? throw new ArgumentNullException("Connection string não configurada.");

        string databaseType = configuration["DATABASE_TYPE"]?.ToLower()
            ?? throw new ArgumentNullException("Tipo de banco não especificado.");

        services.AddDbContext<MicroworkerContexto>(options =>
        {
            switch (databaseType)
            {
                case "postgresql":
                    options.UseNpgsql(
                        connectionString,
                        npgsqlOptions => npgsqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 3,
                            maxRetryDelay: TimeSpan.FromSeconds(1),
                            errorCodesToAdd: null));
                    break;

                case "mssql":
                    options.UseSqlServer(
                        connectionString,
                        sqlServerOptions => sqlServerOptions.EnableRetryOnFailure(
                            errorNumbersToAdd: null,
                            maxRetryCount: 3,
                            maxRetryDelay: TimeSpan.FromSeconds(1)));
                    break;

                default:
                    throw new NotSupportedException($"Database type '{databaseType}' não é suportado.");
            }

            // Opcional: Log de queries (útil para desenvolvimento)
            options.LogTo(Console.WriteLine, LogLevel.Information);
        });

        return services;
    }

    public static IApplicationBuilder UseApplicationDatabase(this IApplicationBuilder app)
    {
        using (var scope = app.ApplicationServices.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<MicroworkerContexto>();
            context.Database.Migrate();
        }

        return app;
    }
}