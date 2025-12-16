using Microsoft.EntityFrameworkCore;
using Serilog;
using TaskManager.Infrastructure.Data;
using TaskManager.Infrastructure.Data.Seeders;

namespace TaskManager.WebAPI.Extensions
{
    public static class HostExtensions
    {
        public static async Task ApplyMigrationsAsync(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                await using (var context = services.GetRequiredService<AppDbContext>())
                {
                    await context.Database.MigrateAsync();
                }
                Log.Information("Database migrations applied successfully");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while applying migrations");
                throw;
            }
        }

        public static async Task SeedDatabaseAsync(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                await using (var context = services.GetRequiredService<AppDbContext>())
                {
                    await DatabaseSeeder.SeedAsync(context);
                }
                Log.Information("Database seeded successfully");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while seeding database");
                throw;
            }
        }
    }
}
