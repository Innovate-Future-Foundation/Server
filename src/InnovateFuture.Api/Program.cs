using System.Text;
using FluentValidation;
using HealthChecks.UI.Client;
using InnovateFuture.Api.Filters;
using InnovateFuture.Api.Configs;
using InnovateFuture.Api.Middleware; 
using InnovateFuture.Application.Behaviors;
using InnovateFuture.Application.Profiles.Commands.UpdateProfile;
using InnovateFuture.Application.Profiles.Queries.GetProfile;
using InnovateFuture.Application.Users.Commands.CreateUser;
using InnovateFuture.Application.Users.Commands.UpdateUser;
using InnovateFuture.Application.Users.Queries.GetUser;
using InnovateFuture.Application.Users.Queries.GetUsers;
using InnovateFuture.Infrastructure.Common;
using InnovateFuture.Application.Organisations.Commands.CreateOrganisation;
using InnovateFuture.Application.Organisations.Commands.UpdateOrganisation;
using InnovateFuture.Application.Organisations.Queries.GetOrganisations;
using InnovateFuture.Application.Profiles.Queries.GetProfiles;
using InnovateFuture.Domain.Enums;
using InnovateFuture.Application.Services.Security.TokenService;
using InnovateFuture.Application.Services.SendEmail;
using InnovateFuture.Application.Services.UserService;
using InnovateFuture.Domain.Entities;
using InnovateFuture.Infrastructure.Activities.Persistence.Interfaces;
using InnovateFuture.Infrastructure.Activities.Persistence.Repositories;
using InnovateFuture.Infrastructure.Common.Persistence;
using InnovateFuture.Infrastructure.Configs;
using InnovateFuture.Infrastructure.Days.Persistence.Interfaces;
using InnovateFuture.Infrastructure.Days.Persistence.Repositories;
using InnovateFuture.Infrastructure.Organisations.Persistence.Interfaces;
using InnovateFuture.Infrastructure.Organisations.Persistence.Repositories;
using InnovateFuture.Infrastructure.Profiles.Persistence.Interfaces;
using InnovateFuture.Infrastructure.Profiles.Persistence.Repositories;
using InnovateFuture.Infrastructure.StudentTourEnrollments.Persistence.Interfaces;
using InnovateFuture.Infrastructure.StudentTourEnrollments.Persistence.Repositories;
using InnovateFuture.Infrastructure.Tours.Persistence.Interfaces;
using InnovateFuture.Infrastructure.Tours.Persistence.Repositories;
using InnovateFuture.Infrastructure.Users.Persistence.Interfaces;
using InnovateFuture.Infrastructure.Users.Persistence.Repositories;
using MediatR;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;
using InnovateFuture.Infrastructure.UnitOfWork.Persistence.Interface;
using InnovateFuture.Infrastructure.UnitOfWork.Persistence.Repositiories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using InnovateFuture.Application.Services.S3;
using Amazon.S3;
using InnovateFuture.Application.Auth.Commands.ConfirmEmail;
using InnovateFuture.Application.Auth.Commands.Login;
using InnovateFuture.Application.Auth.Commands.Password;
using InnovateFuture.Application.Auth.Commands.Register;
using InnovateFuture.Application.Auth.Commands.ResendVerificationEmail;
using InnovateFuture.Application.Auth.Commands.SendTemporaryPassword;
using InnovateFuture.Application.Auth.Commands.SendVerificationEmail;
using InnovateFuture.Application.Auth.Queries.GetMe;
using InnovateFuture.Application.Common;
using InnovateFuture.Application.Services.Auth.Register;


namespace InnovateFuture.Api
{
    public class Program
    {
        public static void Main(string[] args) 
        {
            var logger = LogManager.Setup().LoadConfigurationFromFile("nLog.config").GetCurrentClassLogger();
            var policyName = "AllowLocalhost";
            
            var builder = WebApplication.CreateBuilder(args);
            
            builder.Services.AddApplicationservices(builder.Configuration);
            // builder.Services.AddInfrastructureLayer(builder.Configuration);
            // builder.Services.AddPresentationLayer();
            
            builder.Services.AddHttpContextAccessor();
            
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
            
            
            var connectionString = builder.Configuration["DBConnection"];
            #region DB connection
            builder.Services.AddHealthChecks()
                .AddNpgSql(connectionString)
                .AddDbContextCheck<ApplicationDbContext>();
            
            builder.Services.Configure<DBConnectionConfig>(builder.Configuration);
            var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
            // register enums
            dataSourceBuilder.MapEnum<RoleEnum>();
            dataSourceBuilder.MapEnum<SubscriptionEnum>();
            dataSourceBuilder.MapEnum<OrgStatusEnum>();
            dataSourceBuilder.MapEnum<TourStatusEnum>();
            dataSourceBuilder.MapEnum<EnrollmentStatusEnum>();
            
            var dataSource = dataSourceBuilder.Build();
            
            builder.Services.AddDbContext<ApplicationDbContext>(
                dbContextOptions => dbContextOptions
                    .UseNpgsql(dataSource,
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
            
            // swagger config => see more details in swagger config extension
            builder.Services.AddSwaggerEXT();
            

            #region NLog
            // NLog: Setup NLog for Dependency injection
            builder.Logging.ClearProviders();
            builder.Host.UseNLog();
            #endregion

            builder.Services.AddCors(option =>
            {
                option.AddPolicy(policyName, policy =>
                {
                    policy.WithOrigins($"{builder.Configuration["FrontEndBaseUrl"]}")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        // access-token in cookies
                        .AllowCredentials();
                });
            });

            var app = builder.Build();
            
            // Configure the HTTP request pipeline.
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
