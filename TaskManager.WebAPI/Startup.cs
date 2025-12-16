using Microsoft.Extensions.Hosting;
using TaskManager.Application.DI;
using TaskManager.Infrastructure.DI;
using TaskManager.WebAPI.Extensions;
using TaskManager.WebAPI.Filters;

namespace TaskManager.WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<ValidateModelAttribute>();
            services.AddScoped<AntiInjectionFilter>();

            services.AddApplicationLayer();
            services.AddInfrastructureLayer(Configuration);

            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddApiVersioningWithHeader();
            services.AddCustomSwagger();
            services.AddCustomAuthentication(Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment environment, IServiceProvider serviceProvider)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "TaskManager WebAPI v1");
                options.RoutePrefix = "swagger";
            });

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.AddMiddleware();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
