using Microsoft.Extensions.Hosting;
using Application;
using Core.Filters;
using Infrastructure;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MRA.Configurations.Initializer.Azure.AppConfig;
using MRA.Configurations.Initializer.Azure.Insight;
using MRA.Configurations.Initializer.Azure.KeyVault;
using WebApi;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsProduction())
{
    builder.Configuration.ConfigureAzureKeyVault("MRAAcademy");
    string appConfigConnectionString = builder.Configuration["AppConfigConnectionString"];
    builder.Configuration.AddAzureAppConfig(appConfigConnectionString);
    builder.Logging.AddApiApplicationInsights(builder.Configuration);
}


builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApiServices(builder.Configuration);

builder.Services.AddControllers(options => options.Filters.Add<ApiExceptionFilterAttribute>())
    .AddOData(op =>
        op.Select().Filter().Expand().Count().SetMaxTop(null).AddRouteComponents("api/odata", Startup.GetEdmModel()));


var app = builder.Build();
app.UseRouting();
app.UseCors(ApplicationConstants.ALLOW_ORIGIN_TO_WEB_CLIENT_NAME);
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// Initialize and seed database
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    if (dbContext.Database.IsRelational())
    {
        await dbContext.Database.MigrateAsync();
    }
}

app.MapControllerRoute("default", "{controller}/{action}/{id}");
await app.RunAsync();

public partial class Program;