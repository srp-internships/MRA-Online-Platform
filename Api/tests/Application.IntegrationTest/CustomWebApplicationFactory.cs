using Application.CodeAnalyzer.Services;
using Application.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Application.IntegrationTest
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration(configurationBuilder =>
            {
                var integrationConfig = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.Development.Test.json")
                    .AddEnvironmentVariables()
                    .Build();

                configurationBuilder.AddConfiguration(integrationConfig);
            });

            builder.ConfigureTestServices(services =>
            {
                var serviceProvider = services.BuildServiceProvider();
                var descriptor =
                    new ServiceDescriptor(
                        typeof(ICodeAnalyzerService),
                        typeof(TestCodeAnalyzerService),
                        ServiceLifetime.Transient);
                services.Replace(descriptor);

                descriptor =
                   new ServiceDescriptor(
                       typeof(IGoogleDriveService),
                       typeof(TestGoogleDriveService),
                       ServiceLifetime.Transient);
                services.Replace(descriptor);
            });
        }
    }
}
