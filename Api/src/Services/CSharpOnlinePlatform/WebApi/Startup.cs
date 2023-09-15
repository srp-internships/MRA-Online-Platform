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

using Mra.Shared.Initializer.Azure.KeyVault;

namespace WebApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging();
            services.AddApplicationInsightsTelemetry();

            // Add services to the container.
            services.AddApplication();
            services.AddInfrastructure(Configuration);
            AddSwaggerServices(services);
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddHttpContextAccessor();
            services.AddControllers(options => options.Filters.Add<ApiExceptionFilterAttribute>())
                 .AddOData(op => op.Select().Filter().Expand().Count().SetMaxTop(null).AddRouteComponents("api/odata", GetEdmModel()));
            EnableCorePolicies(services);

            
            
            //services.AddAzureEmailService();//uncomment this if u wont use email service from Azure from namespace Mra.Shared.Initializer.Azure.EmailService
        }

        IEdmModel GetEdmModel()
        {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            
            builder.EntitySet<Course>("Course");
            builder.EntitySet<Theme>("Theme");
            builder.EntitySet<Exercise>("Exercise");
            builder.EntitySet<StudentCourse>("StudentCourse");
            builder.EntitySet<StudentCourseExercise>("StudentCourseExercise");
            return builder.GetEdmModel();
        }

        void AddSwaggerServices(IServiceCollection services)
        {
            
            services.AddSwaggerGen(setup =>
            {
                setup.SwaggerDoc("v1", new OpenApiInfo { Title = "C# Online Platform", Version = "v1" });
                setup.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below. Example: 'Bearer 12345abcdef'",
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

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment environment, IMigration migration)
        {
            app.UseRouting();
            app.UseCors(ApplicationConstants.ALLOW_ORIGIN_TO_WEB_CLIENT_NAME);
            if (environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(routebuilder =>
            {
                routebuilder.MapControllers();
            });
            // Migrate and seed database
            migration.Migrate();
            // initializer.InitializeAsync().Wait();
        }

        void EnableCorePolicies(IServiceCollection services)
        {
            var coresAllowedHosts = Configuration.GetSection(ApplicationConstants.CORS_CONFIG_SECTION_NAME).Get<string[]>();
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
}
