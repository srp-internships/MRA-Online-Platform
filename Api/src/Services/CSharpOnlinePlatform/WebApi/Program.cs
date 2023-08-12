using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.ApplicationInsights;
using MRA.Jobs.Web.AzureKeyVault;
using Microsoft.AspNetCore.Builder;

namespace WebApi;

public class Program
{
    public async static Task Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();

        var builder = WebApplication.CreateBuilder(args);

        builder.ConfigureAzureKeyVault();

        await host.RunAsync();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
                webBuilder.UseStartup<Startup>())
        .ConfigureLogging((context, builder) =>
        {
            if (!context.HostingEnvironment.IsDevelopment())
            {                 
                builder.AddApplicationInsights(
              configureTelemetryConfiguration: (config) =>
              {
                  config.ConnectionString = context.Configuration["Logging:ApplicationInsights:ConnectionString"];
              },
              configureApplicationInsightsLoggerOptions: (options) => { });
            }
        });
}