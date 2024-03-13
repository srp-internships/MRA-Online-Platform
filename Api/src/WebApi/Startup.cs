using System.Collections.Generic;
using Application;
using Core.Filters;
using Domain.Entities;
using Infrastructure;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.OData;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;

namespace WebApi;

public static class Startup
{
    public static void AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddLogging();
        services.AddApplicationInsightsTelemetry();

        // Add services to the container.
        
        AddSwaggerServices(services);
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddHttpContextAccessor();
       
        services.EnableCorePolicies(configuration);


        //services.AddAzureEmailService();//uncomment this if u wont use email service from Azure from namespace Mra.Shared.Initializer.Azure.EmailService
    }

    public static IEdmModel GetEdmModel()
    {
        ODataConventionModelBuilder builder = new ODataConventionModelBuilder();

        builder.EntitySet<Course>("Course");
        builder.EntitySet<Theme>("Theme");
        builder.EntitySet<Exercise>("Exercise");
        builder.EntitySet<StudentCourse>("StudentCourse");
        builder.EntitySet<StudentCourseExercise>("StudentCourseExercise");
        return builder.GetEdmModel();
    }

    static void AddSwaggerServices(this IServiceCollection services)
    {
        services.AddSwaggerGen(setup =>
        {
            setup.SwaggerDoc("v1", new OpenApiInfo { Title = "C# Online Platform", Version = "v1" });
            setup.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description =
                    "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below. Example: 'Bearer 12345abcdef'",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });
            var securityRequirement = new OpenApiSecurityRequirement();
            var securityScheme = new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "Bearer",
                Name = "Bearer",
                In = ParameterLocation.Header,
            };
            securityRequirement.Add(securityScheme, new List<string>());
            setup.AddSecurityRequirement(securityRequirement);
        });
    }

    private static void EnableCorePolicies(this IServiceCollection services, IConfiguration configuration)
    {
        var coresAllowedHosts = configuration.GetSection(ApplicationConstants.CORS_CONFIG_SECTION_NAME).Get<string[]>();
        services.AddCors(options =>
        {
            options.AddPolicy(ApplicationConstants.ALLOW_ORIGIN_TO_WEB_CLIENT_NAME, policyConfig =>
            {
                policyConfig.WithOrigins(coresAllowedHosts)
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });
    }
}