using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Mra.Shared.Initializer.Azure.Insight;
using Mra.Shared.Initializer.Azure.KeyVault;

namespace WebApi;

public class Program
{
    public static async Task Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();
        
        await host.RunAsync();
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
                webBuilder.UseStartup<Startup>())
        .ConfigureLogging((context, builder) =>
        {
            if (context.HostingEnvironment.IsProduction())
            {                 
                builder.AddApiApplicationInsights(context.Configuration);
                context.Configuration.Get<ConfigurationManager>().ConfigureAzureKeyVault("MRAAcademy");
            }
        });
}