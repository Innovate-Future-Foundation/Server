using FluentValidation;
using InnovateFuture.Api.Configs;
using InnovateFuture.Api.Filters;
using InnovateFuture.Api.Middleware;
using InnovateFuture.Application.Behaviors;
using InnovateFuture.Application.Orders.Commands.CreateOrder;
using InnovateFuture.Application.Orders.Queries.GetOrder;
using InnovateFuture.Application.Services.Security;
using InnovateFuture.Infrastructure.Common.Persistence;
using InnovateFuture.Infrastructure.Configs;
using InnovateFuture.Infrastructure.Orders.Persistence.Interfaces;
using InnovateFuture.Infrastructure.Orders.Persistence.Repositories;
using InnovateFuture.Roles.Persistence.Interfaces;
using InnovateFuture.Roles.Persistence.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;

namespace InnovateFuture.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = LogManager
                .Setup()
                .LoadConfigurationFromFile("nLog.config")
                .GetCurrentClassLogger();
            var policyName = "defaultPolicy";

            try
            {
                var builder = WebApplication.CreateBuilder(args);

                #region Logging and NLog setup
                // NLog: Setup NLog for Dependency injection
                builder.Logging.ClearProviders();
                builder.Host.UseNLog();
                #endregion

                #region Controllers and Filters
                builder
                    .Services.AddControllers(options =>
                    {
                        // Register global filters
                        options.Filters.Add<CommonResultFilter>();
                    })
                    .AddJsonOptions(options =>
                    {
                        options.JsonSerializerOptions.ReferenceHandler = System
                            .Text
                            .Json
                            .Serialization
                            .ReferenceHandler
                            .IgnoreCycles;
                    });
                #endregion

                #region MediatR and Application Services
                builder.Services.AddMediatR(configuration =>
                {
                    configuration.RegisterServicesFromAssembly(typeof(CreateOrderHandler).Assembly);
                    configuration.RegisterServicesFromAssembly(typeof(GetOrderHandler).Assembly);
                });

                builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
                builder.Services.AddValidatorsFromAssembly(
                    typeof(CreateOrderCommandValidator).Assembly
                );

                // Register custom services
                builder.Services.AddScoped<IOrderRepository, OrderRepository>();
                builder.Services.AddScoped<IRoleRepository, RoleRepository>(); // Add RoleRepository
                builder.Services.AddTransient(
                    typeof(IPipelineBehavior<,>),
                    typeof(ValidationBehavior<,>)
                );
                builder.Services.AddTransient<CreateTokenService>();
                #endregion

                #region Database Connection
                var connectionString = builder.Configuration.GetConnectionString(
                    "DefaultConnection"
                );

                builder.Services.AddDbContext<ApplicationDbContext>(options =>
                    options
                        .UseNpgsql(
                            connectionString,
                            npgsqlOptions => npgsqlOptions.SetPostgresVersion(new Version(17, 2))
                        )
                        .LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information)
                        .EnableSensitiveDataLogging()
                        .EnableDetailedErrors()
                );
                #endregion

                #region CORS
                builder.Services.AddCors(options =>
                {
                    options.AddPolicy(
                        policyName,
                        policy =>
                        {
                            policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                        }
                    );
                });
                #endregion

                #region JWT Authentication
                builder.Services.Configure<JWTConfig>(
                    builder.Configuration.GetSection(JWTConfig.Section)
                );
                var jwtConfig = builder
                    .Configuration.GetSection(JWTConfig.Section)
                    .Get<JWTConfig>();
                if (jwtConfig == null)
                {
                    throw new InvalidOperationException(
                        "JWT configuration is missing in appsettings."
                    );
                }
                builder.Services.AddJWTEXT(jwtConfig);
                #endregion

                #region Swagger
                builder.Services.AddSwaggerEXT();
                #endregion

                var app = builder.Build();

                #region Configure HTTP Pipeline
                if (app.Environment.IsDevelopment())
                {
                    app.UseSwaggerEXT();
                }
                else
                {
                    builder.Services.AddCors(options =>
                    {
                        options.AddPolicy(
                            policyName,
                            policy =>
                            {
                                policy
                                    .WithOrigins("https://your-frontend-domain.com")
                                    .AllowAnyMethod()
                                    .AllowAnyHeader();
                            }
                        );
                    });
                }

                app.UseMiddleware<GlobalExceptionMiddleware>();

                app.UseHttpsRedirection();
                app.UseCors(policyName);
                app.UseAuthentication();
                app.UseAuthorization();
                app.MapControllers();
                #endregion

                app.Run();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An error occurred during application startup.");
                throw;
            }
            finally
            {
                LogManager.Shutdown();
            }
        }
    }
}
