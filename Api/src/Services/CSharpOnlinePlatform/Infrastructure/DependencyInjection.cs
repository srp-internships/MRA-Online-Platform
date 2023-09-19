using Application;
using Application.Account.Services;
using Application.CodeAnalyzer.Services;
using Application.Common.Interfaces;
using Application.Services;
using Infrastructure.Account.Services;
using Infrastructure.CodeAnalyzer.Services;
using Infrastructure.Persistence;
using Infrastructure.Persistence.SeedData;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Principal;
using Infrastructure.Identity;
using Mra.Shared.Common.Constants;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        
        //services.AddAzureEmailService();//uncomment this if u wont use email service from Azure from namespace Mra.Shared.Initializer.Azure.EmailService

        if (configuration.GetValue<bool>(ApplicationConstants.USE_MEMORY_DB))
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase(ApplicationConstants.ONLINE_PLATFORM_DB));
        }
        else
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString(ApplicationConstants.DEFAULT_CONNECTION),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
        }

        services.AddIdentityServices();
        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        services.AddScoped<ILoadSeedData, LoadSeedDataFromJson>();
        services.AddScoped<IMigration, DbMigration>();

        services.AddHttpContextAccessor();
        services.AddTransient<IPrincipal>(provider => provider.GetService<IHttpContextAccessor>().HttpContext.User);
        services.AddScoped<IUserHttpContextAccessor, UserHttpContextAccessor>();

        services.AddHttpClient(ApplicationConstants.COMPILER_CLIENT, c =>
        {
            c.BaseAddress = new Uri(configuration[ApplicationConstants.COMPILER_API_HOST]);
            c.DefaultRequestHeaders.Add("API_KEY", configuration[ApplicationConstants.COMPILER_API_KEY]);
        });
        services.AddScoped<ICodeAnalyzerService, CodeAnalyzerService>();

        services.AddScoped<IGoogleDriveService, GoogleDriveService>();

        return services;
    }

    static void AddIdentityServices(this IServiceCollection services)
    {
        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer();

        services.AddAuthorization(a =>
        {
            a.AddPolicy(ApplicationPolicies.Administrator, op => op
                .RequireRole(ApplicationClaimValues.Administrator)
                .RequireClaim(ClaimTypes.Application, ApplicationClaimValues.ApplicationName));

            a.AddPolicy(ApplicationPolicies.Teacher, op => op
                .RequireRole(ApplicationClaimValues.Teacher, ApplicationClaimValues.Administrator)
                .RequireClaim(ClaimTypes.Application, ApplicationClaimValues.ApplicationName)
                .RequireClaim(ClaimTypes.Id));

            a.AddPolicy(ApplicationPolicies.Student, op => op
                .RequireRole(ApplicationClaimValues.Student, ApplicationClaimValues.Teacher,
                    ApplicationClaimValues.Administrator)
                .RequireClaim(ClaimTypes.Application, ApplicationClaimValues.ApplicationName)
                .RequireClaim(ClaimTypes.Id));
        });
        
    }
}