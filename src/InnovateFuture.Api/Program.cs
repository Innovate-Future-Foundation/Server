using FluentValidation;
using HealthChecks.UI.Client;
using InnovateFuture.Api.Filters;
using InnovateFuture.Api.Configs;
using InnovateFuture.Api.Middleware;
using InnovateFuture.Application.Behaviors;
using InnovateFuture.Application.Profiles.Commands.UpdateProfile;
using InnovateFuture.Application.Profiles.Queries.GetProfile;
using InnovateFuture.Application.Roles.Queries.GetRole;
using InnovateFuture.Application.Roles.Queries.GetRoles;
using InnovateFuture.Application.Services.Security;
using InnovateFuture.Application.Users.Commands.CreateUser;
using InnovateFuture.Application.Users.Commands.UpdateUser;
using InnovateFuture.Application.Users.Queries.GetUser;
using InnovateFuture.Application.Users.Queries.GetUsers;
using InnovateFuture.Infrastructure.Common.Persistence;
using InnovateFuture.Infrastructure.Configs;
using InnovateFuture.Infrastructure.Organisations.Persistence.Interfaces;
using InnovateFuture.Infrastructure.Organisations.Persistence.Repositories;
using InnovateFuture.Infrastructure.Profiles.Persistence.Interfaces;
using InnovateFuture.Infrastructure.Profiles.Persistence.Repositories;
using InnovateFuture.Infrastructure.Roles.Persistence.Interfaces;
using InnovateFuture.Infrastructure.Roles.Persistence.Repositories;
using InnovateFuture.Infrastructure.Users.Persistence.Interfaces;
using InnovateFuture.Infrastructure.Users.Persistence.Repositories;
using MediatR;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
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
            
            var connectionString = builder.Configuration["DBConnection"];
            
            #region filter
            builder.Services.AddControllers(option =>
            {
                //global filter register, working for all actions
                option.Filters.Add<CommonResultFilter>();
                option.Filters.Add<ModelValidationFilter>();
                // option.Filters.Add<ExceptionFilter>();
            }).AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
            });
            #endregion
            
            #region service instances
            builder.Services.AddMediatR(configuration =>
            {
                configuration.RegisterServicesFromAssembly(typeof(CreateUserHandler).Assembly);
                configuration.RegisterServicesFromAssembly(typeof(UpdateUserHandler).Assembly);
                configuration.RegisterServicesFromAssembly(typeof(GetUsersHandler).Assembly);
                configuration.RegisterServicesFromAssembly(typeof(GetUserHandler).Assembly);
                
                configuration.RegisterServicesFromAssembly(typeof(UpdateProfileHandler).Assembly);
                configuration.RegisterServicesFromAssembly(typeof(GetProfileHandler).Assembly);
                
                configuration.RegisterServicesFromAssembly(typeof(GetRoleHandler).Assembly);
                configuration.RegisterServicesFromAssembly(typeof(GetRolesHandler).Assembly);
            });
            // auto mapper instance
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            // customized instances
            builder.Services.AddScoped<IOrgRepository, OrgRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IProfileRepository, ProfileRepository>();
            builder.Services.AddScoped<IRoleRepository, RoleRepository>();

            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            builder.Services.AddValidatorsFromAssembly(typeof(CreateUserCommandValidator).Assembly);
            builder.Services.AddValidatorsFromAssembly(typeof(UpdateUserCommandValidator).Assembly);
            builder.Services.AddValidatorsFromAssembly(typeof(GetUsersQueryValidator).Assembly);
            
            builder.Services.AddValidatorsFromAssembly(typeof(GetRolesQueryValidator).Assembly);
            
            builder.Services.AddValidatorsFromAssembly(typeof(UpdateProfileCommandValidator).Assembly);

            builder.Services.AddHealthChecks()
                .AddNpgSql(connectionString)
                .AddDbContextCheck<ApplicationDbContext>();
            #endregion
            
            #region DB connection
            builder.Services.Configure<DBConnectionConfig>(builder.Configuration);

            Console.WriteLine($"Connection string: {connectionString}");
            
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
            Console.WriteLine($"JWT Config: {jwtConfig.SecretKey}");
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
            builder.Services.AddValidatorsFromAssemblyContaining<CreateUserCommandValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<UpdateUserCommandValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<GetUsersQueryValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<GetRolesQueryValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<UpdateProfileCommandValidator>();
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
            
            app.MapHealthChecks("health",new HealthCheckOptions
            {
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
            
            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();
            
            app.Run();
        }
    }
}
