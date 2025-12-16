using Asp.Versioning;

namespace TaskManager.WebAPI.Extensions
{
    public static class VersioningExtensions
    {
        public static IServiceCollection AddApiVersioningWithHeader(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
                {
                    options.DefaultApiVersion = new ApiVersion(1, 0);
                    options.AssumeDefaultVersionWhenUnspecified = true;
                    options.ReportApiVersions = true;
                    options.ApiVersionReader = new HeaderApiVersionReader("X-API-Version");
                })
                .AddApiExplorer(options =>
                {
                    options.GroupNameFormat = "'v'VVV";
                    options.SubstituteApiVersionInUrl = true;
                });


            return services;
        }
    }
}
