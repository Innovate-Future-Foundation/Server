using HealthChecks.UI.Client;
using InnovateFuture.Api.Filters;
using InnovateFuture.Api.Configs;
using InnovateFuture.Api.Middleware;
using InnovateFuture.Application.Common;
using InnovateFuture.Domain.Enums;
using InnovateFuture.Infrastructure.Common;
using InnovateFuture.Infrastructure.Common.Persistence;
using InnovateFuture.Infrastructure.Configs;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using NLog;
using NLog.Web;



namespace InnovateFuture.Api
{
    public class Program
    {
        public static void Main(string[] args) 
        {
            var logger = LogManager.Setup().LoadConfigurationFromFile("nLog.config").GetCurrentClassLogger();
            var policyName = "AllowLocalhost";
            
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration["DBConnection"];
            
            builder.Services.AddAPIServices(builder.Configuration);
            builder.Services.AddApplicationServices(builder.Configuration);
            builder.Services.AddInfrastructureServices(builder.Configuration, connectionString);
            
            
            builder.Services.AddHealthChecks()
                           .AddNpgSql(connectionString)
                           .AddDbContextCheck<ApplicationDbContext>(); 
            
            builder.Logging.ClearProviders();
            builder.Host.UseNLog();

            var app = builder.Build();
            
            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerEXT();
                app.Services.SeedDataEXT();
            }

            app.UseMiddleware<GlobalExceptionMiddleware>();
            
            app.MapHealthChecks("health",new HealthCheckOptions
            {
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
            
            app.UseRouting();
            
            app.UseAuthentication();

            app.UseAuthorization();
            
            app.UseCors(policyName);

            app.MapControllers();
            
            app.Run();
        }
    }
}
