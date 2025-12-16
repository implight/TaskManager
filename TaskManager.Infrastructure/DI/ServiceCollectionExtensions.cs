using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskManager.Application.Data;
using TaskManager.Application.Data.Repositories;
using TaskManager.Infrastructure.Data;
using TaskManager.Infrastructure.Data.Repositories;
using TaskManager.Infrastructure.Options;

namespace TaskManager.Infrastructure.DI
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
        {
            var dbOptions = configuration.GetSection("Database").Get<DatabaseOptions>()!;
            services.AddSingleton(Microsoft.Extensions.Options.Options.Create(dbOptions));

            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(
                    dbOptions.GetConnectionString(),
                    npgsqlOptions =>
                    {
                        npgsqlOptions.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName);
                    }).UseSnakeCaseNamingConvention());

            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<ITaskTypeRepository, TaskTypeRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
