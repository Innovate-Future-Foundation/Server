using HealthChecks.UI.Client;
using InnovateFuture.Api.Configs;
using InnovateFuture.Api.Middleware;
using InnovateFuture.Application.Auth.Consumers;
using InnovateFuture.Application.Common;
using InnovateFuture.Infrastructure.Common;
using InnovateFuture.Infrastructure.Common.Persistence;
using InnovateFuture.Infrastructure.Configs;
using MassTransit;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using NLog;
using NLog.Web;



namespace InnovateFuture.Api
{
    public class Program
    {
        public static void Main(string[] args) 
        {
            // var logger = LogManager.Setup().LoadConfigurationFromFile("nLog.config").GetCurrentClassLogger();
            var policyName = "AllowLocalhost";
            
            var builder = WebApplication.CreateBuilder(args);
            
            builder.Configuration
                .SetBasePath(Directory.GetCurrentDirectory())
                // .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            
            // Configure Logging
            builder.Logging.ClearProviders(); 
            builder.Logging.AddConsole();     
            builder.Logging.AddDebug();     
            builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace); 

            
            var connectionString = builder.Configuration["DBConnection"];
            
            builder.Services.AddAPIServices(builder.Configuration);
            builder.Services.AddApplicationServices(builder.Configuration);
            builder.Services.AddInfrastructureServices(builder.Configuration, connectionString);
            
            
            builder.Services.AddHealthChecks()
                           .AddNpgSql(connectionString)
                           .AddDbContextCheck<ApplicationDbContext>(); 
            
            builder.Logging.ClearProviders();
            builder.Host.UseNLog();
            
            #region MassTransit configuration
            builder.Services.AddMassTransit(x =>
            {
                x.AddConsumer<SendVerificationEventConsumer>();
                x.AddConsumer<SendTemporaryPasswordEventConsumer>();
                x.AddConsumer<ResendUserRegisteredEventConsumer>();
                x.AddConsumer<ForgotPasswordEventConsumer>();
                
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("rabbitmq://localhost");
                    
                    cfg.ReceiveEndpoint("email-queue", e =>
                    {
                        // RabbitMQ sends 10 messages in advance to a consumer
                        e.PrefetchCount = 10; 
                        
                        // one consumer instance can process 5 messages at once
                        e.ConcurrentMessageLimit = 5;
                        
                        e.ConfigureConsumer<SendVerificationEventConsumer>(context);
                        e.ConfigureConsumer<SendTemporaryPasswordEventConsumer>(context);
                        e.ConfigureConsumer<ResendUserRegisteredEventConsumer>(context);
                        e.ConfigureConsumer<ForgotPasswordEventConsumer>(context);
                    });
                });
            });
            #endregion

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
            
            // Enable Logging Middleware
            var logger = app.Services.GetRequiredService<ILogger<Program>>();
            logger.LogInformation("🚀 Application Starting..."); 
            
            app.UseRouting();
            
            app.UseAuthentication();

            app.UseAuthorization();
            
            app.UseCors(policyName);

            app.MapControllers();
            
            app.Run();
        }
    }
}
