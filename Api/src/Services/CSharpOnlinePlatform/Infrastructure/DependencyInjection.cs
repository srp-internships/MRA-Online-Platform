using Application;
using Application.Account.Services;
using Application.CodeAnalyzer.Services;
using Application.Common.Interfaces;
using Application.JWTToken;
using Application.Services;
using Infrastructure.Account.Services;
using Infrastructure.CodeAnalyzer.Services;
using Infrastructure.JWTToken;
using Infrastructure.Persistence;
using Infrastructure.Persistence.SeedData;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Security.Principal;
using System.Text;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
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
            services.AddIdentityServices(configuration);
            services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
            services.AddScoped<ILoadSeedData, LoadSeedDataFromJson>();
            services.AddScoped<IApplicationDbContextInitializer, ApplicationDbContextInitializer>();
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
            services.AddSingleton<IEmailSenderService, EmailSenderService>();
            services.AddIdentityCore<User>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddScoped<IGoogleDriveService, GoogleDriveService>();

            return services;
        }

        static void AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<User, IdentityRole<Guid>>(options =>
            {
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
            }).AddEntityFrameworkStores<ApplicationDbContext>()
              .AddTokenProvider(configuration[ApplicationConstants.JWT_TOKEN_PROVIDER], typeof(DataProtectorTokenProvider<User>));

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.SaveToken = true;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration[ApplicationConstants.JWT_VALID_ISSUER],
                    ValidAudience = configuration[ApplicationConstants.JWT_VALID_AUDIENCE],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration[ApplicationConstants.JWT_SECRET])),
                    ClockSkew = TimeSpan.Zero
                };
                o.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add(ApplicationConstants.TOKEN_EXPIRED, ApplicationConstants.IS_TOKEN_EXPIRED);
                        }
                        return Task.CompletedTask;
                    }
                };
            });
            services.AddScoped<IJWTManagerRepository, JWTManagerRepository>();
        }
    }
}
