using FluentValidation;
using InnovateFuture.Api.Filters;
using InnovateFuture.Api.Configs;
using InnovateFuture.Api.Middleware;
using InnovateFuture.Application.Behaviors;
using InnovateFuture.Application.Orders.Commands.CreateOrder;
using InnovateFuture.Application.Orders.Queries.GetOrder;
using InnovateFuture.Application.Organisations.Commands.CreateOrganisation;
using InnovateFuture.Application.Organisations.Queries.GetOrganisation;
using InnovateFuture.Application.Services.Security;
using InnovateFuture.Infrastructure.Common.Persistence;
using InnovateFuture.Infrastructure.Configs;
using InnovateFuture.Infrastructure.Orders.Persistence.Interfaces;
using InnovateFuture.Infrastructure.Orders.Persistence.Repositories;
using InnovateFuture.Infrastructure.Organisations.Persistence.Interfaces;
using InnovateFuture.Infrastructure.Organisations.Persistence.Repositories;
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
            var logger = LogManager.Setup().LoadConfigurationFromFile("nLog.config").GetCurrentClassLogger();
            var policyName = "defalutPolicy";

            var builder = WebApplication.CreateBuilder(args);
            
            #region filter
            builder.Services.AddControllers(option =>
            {
                //global filter register, working for all actions
                option.Filters.Add<CommonResultFilter>();
                // option.Filters.Add<ExceptionFilter>();
            }).AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
            });
            #endregion
            
            #region service instances
            builder.Services.AddMediatR(configuration =>
            {
                configuration.RegisterServicesFromAssembly(typeof(CreateOrderHandler).Assembly);
                configuration.RegisterServicesFromAssembly(typeof(GetOrderHandler).Assembly);
                configuration.RegisterServicesFromAssembly(typeof(CreateOrganisationHandler).Assembly);
                configuration.RegisterServicesFromAssembly(typeof(GetOrganisationHandler).Assembly);
            });
            // auto mapper instance
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            // customized instances
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<IOrganisationRepository, OrganisationRepository>();
            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            builder.Services.AddValidatorsFromAssembly(typeof(CreateOrderCommandValidator).Assembly);
            builder.Services.AddValidatorsFromAssembly(typeof(CreateOrganisationValidator).Assembly);
            #endregion
            
            #region DB connection
            builder.Services.Configure<DBConnectionConfig>(builder.Configuration);

            var connectionString = builder.Configuration["DBConnection"];
            
            builder.Services.AddDbContext<ApplicationDbContext>(
                dbContextOptions => dbContextOptions
                    .UseNpgsql(connectionString,
                        npgsqlOptions => npgsqlOptions.SetPostgresVersion(new Version(17, 2)))
                    // The following three options help with debugging, but should
                    // be changed or removed for production.
                    .LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information)
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors()
            );
            #endregion
            
            // Disable auto model validation
            builder.Services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);
            
            #region JWT
            // get jwt config values from appsettings and create an obj using JWTConfig model class, IOC will handle dependency injection
            builder.Services.Configure<JWTConfig>(builder.Configuration.GetSection(JWTConfig.Section));
            // directly get jwt config value from appsettings and construct into an obj
            var jwtConfig = builder.Configuration.GetSection(JWTConfig.Section).Get<JWTConfig>();
            if (jwtConfig == null)
            {
                throw new InvalidOperationException("JWT configuration is missing in appsettings.");
            }
            builder.Services.AddJWTEXT(jwtConfig);

            builder.Services.AddTransient<CreateTokenService>();

            #endregion
            
            #region cors
            // cors
            builder.Services.AddCors(option =>
            {
                option.AddPolicy(policyName, policy =>
                {

                    policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });
            #endregion
            
            // swagger config => see more details in swagger config extension
            builder.Services.AddSwaggerEXT();

            #region fluent validators
            builder.Services.AddValidatorsFromAssemblyContaining<CreateOrderCommandValidator>();
            #endregion

            #region NLog
            // NLog: Setup NLog for Dependency injection
            builder.Logging.ClearProviders();
            builder.Host.UseNLog();
            #endregion

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerEXT();
            }else
            {
                builder.Services.AddCors(option =>
                {
                    option.AddPolicy(policyName, policy =>
                    {
                        policy.WithOrigins("https://your-frontend-domain.com")
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                    });
                });
            }
            
            app.UseMiddleware<GlobalExceptionMiddleware>();
            
            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();
            
            app.Run();
        }
    }
}
