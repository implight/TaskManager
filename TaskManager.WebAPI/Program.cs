using TaskManager.WebAPI.Extensions;

namespace TaskManager.WebAPI
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Hosting.Server;
    using Microsoft.AspNetCore.Hosting.Server.Features;
    using Serilog;

    public class Program
    {
        public static async Task Main(string[] args)
        {
            Environment.SetEnvironmentVariable("BASEDIR", AppDomain.CurrentDomain.BaseDirectory);
            string environmentName = Environment.GetEnvironmentVariable("EnvironmentName");

            Log.Information("EnvironmentName: " + environmentName);

            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: false)
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            Log.Information("Starting host");

            var hostBuilder = CreateHostBuilder(args);

            IHost host = hostBuilder.Build();

            var server = host.Services.GetService<IServer>()!;

            Log.Information("Running host");

            try
            {
                await host.ApplyMigrationsAsync();
                await host.SeedDatabaseAsync();

                await host.StartAsync(default).ConfigureAwait(false);

                var serverAddresses = server.Features.Get<IServerAddressesFeature>()?.Addresses;
                if (serverAddresses != null)
                {
                    foreach (var address in serverAddresses)
                    {
                        Log.Information($"Now listening on: {address}");
                    }
                }

                await host.WaitForShutdownAsync(default).ConfigureAwait(false);

                await host.StopAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                throw;
            }
            finally
            {
                if (host is IAsyncDisposable asyncDisposable)
                {
                    await asyncDisposable.DisposeAsync().ConfigureAwait(false);
                }
                else
                {
                    host.Dispose();
                }

                Log.Information("Host terminated");
                await Log.CloseAndFlushAsync();
            }

            // var builder = WebApplication.CreateBuilder(args);
            //
            // // Add services to the container.
            //
            // builder.Services.AddControllers();
            //
            // var app = builder.Build();
            //
            // // Configure the HTTP request pipeline.
            //
            // app.UseHttpsRedirection();
            //
            // app.UseAuthorization();
            //
            //
            // app.MapControllers();
            //
            // await app.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(
                    (context, builder) =>
                    {
                        var environmentName = context.HostingEnvironment.EnvironmentName;

                        builder.SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                            .AddJsonFile($"appsettings.json", optional: false, reloadOnChange: false)
                            .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: false)
                            .AddJsonFile($"vault.json", optional: true, reloadOnChange: false);
                    })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
