using System.Text;
using FluentValidation;
using HealthChecks.UI.Client;
using InnovateFuture.Api.Filters;
using InnovateFuture.Api.Configs;
using InnovateFuture.Api.Middleware;
using InnovateFuture.Application.Auth.ConfirmEmail;
using InnovateFuture.Application.Auth.Login;
using InnovateFuture.Application.Auth.Register;
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
using InnovateFuture.Application.Auth.SendVerificationEmail;
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
using InnovateFuture.Infrastructure.ActivityDays.Persistence.Interfaces;
using InnovateFuture.Infrastructure.ActivityDays.Persistence.Repositories;


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

            #region Email Service Configuration
            // bind EmailSettings from appsettings.Development.json
            builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
            #endregion
            
            #region JWT
            builder.Services.Configure<JWTConfig>(builder.Configuration.GetSection("JWTConfig"));
            #endregion
            
            // Configure JWT Authentication
            var key = Encoding.UTF8.GetBytes(builder.Configuration["JWTConfig:SecretKey"]);
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["JWTConfig:Issuer"],
                        ValidAudience = builder.Configuration["JWTConfig:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(key)
                    };
                    
                    // allow extracting JWT from cookies instead of header
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            // Read Jwt token from cookie
                            var accessToken = context.Request.Query["access_token"];
                            if (!string.IsNullOrEmpty(accessToken))
                            {
                                context.Token = accessToken;
                            }

                            return Task.CompletedTask;
                        }
                    };
                });
            builder.Services.AddAuthorization();
            
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
                configuration.RegisterServicesFromAssembly(typeof(LoginHandler).Assembly);
                
                configuration.RegisterServicesFromAssembly(typeof(CreateUserHandler).Assembly);
                configuration.RegisterServicesFromAssembly(typeof(UpdateUserHandler).Assembly);
                configuration.RegisterServicesFromAssembly(typeof(GetUsersHandler).Assembly);
                configuration.RegisterServicesFromAssembly(typeof(GetUserHandler).Assembly);
                
                configuration.RegisterServicesFromAssembly(typeof(UpdateProfileHandler).Assembly);
                configuration.RegisterServicesFromAssembly(typeof(GetProfileHandler).Assembly);
                
                configuration.RegisterServicesFromAssembly(typeof(CreateOrganisationHandler).Assembly);
                configuration.RegisterServicesFromAssembly(typeof(UpdateOrganisationHandler).Assembly);
                configuration.RegisterServicesFromAssembly(typeof(GetOrganisationsHandler).Assembly);
            
                configuration.RegisterServicesFromAssembly(typeof(RegisterOrganisationAdminHandler).Assembly);
                configuration.RegisterServicesFromAssembly(typeof(SendVerificationEmailHandler).Assembly);
                configuration.RegisterServicesFromAssembly(typeof(ConfirmEmailHandler).Assembly);
            });
                
            // auto mapper instance
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            // customized instances
            builder.Services.AddScoped<ISeedDataService, SeedDataService>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IOrgRepository, OrgRepository>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IProfileRepository, ProfileRepository>();
            builder.Services.AddScoped<IActivityRepository, ActivityRepository>();
            builder.Services.AddScoped<IDayRepository, DayRepository>();
            builder.Services.AddScoped<IActivityDayRepository, ActivityDayRepository>();
            builder.Services.AddScoped<ITourRepository, TourRepository>();
            builder.Services.AddScoped<IStudentTourEnrollmentRepository, StudentTourEnrollmentRepository>();
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<ITokenService, TokenService>();


            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            builder.Services.AddValidatorsFromAssembly(typeof(RegisterOrganisationAdminValidator).Assembly);
            builder.Services.AddValidatorsFromAssembly(typeof(LoginValidator).Assembly);
            
            
            builder.Services.AddValidatorsFromAssembly(typeof(CreateUserCommandValidator).Assembly);
            builder.Services.AddValidatorsFromAssembly(typeof(UpdateUserCommandValidator).Assembly);
            builder.Services.AddValidatorsFromAssembly(typeof(GetUsersQueryValidator).Assembly);
            
            builder.Services.AddValidatorsFromAssembly(typeof(UpdateProfileCommandValidator).Assembly);

            builder.Services.AddValidatorsFromAssembly(typeof(CreateOrganisationCommandValidator).Assembly);
            builder.Services.AddValidatorsFromAssembly(typeof(UpdateOrganisationCommandValidator).Assembly);
            builder.Services.AddValidatorsFromAssembly(typeof(GetOrganisationsQueryValidator).Assembly);

            builder.Services.AddHealthChecks()
                .AddNpgSql(connectionString)
                .AddDbContextCheck<ApplicationDbContext>();
            #endregion
            
            #region DB connection
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
            
            #region Identity 
            builder.Services.AddIdentity<User, IdentityRole<Guid>>()
                .AddEntityFrameworkStores<ApplicationDbContext>() 
                .AddTokenProvider<DataProtectorTokenProvider<User>>("InnovateFuture")
                .AddDefaultTokenProviders();
            #endregion
            
            
            // Disable auto model validation
            builder.Services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);
            

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
            // Auth
            builder.Services.AddValidatorsFromAssemblyContaining<RegisterOrganisationAdminValidator>();
            // Users
            builder.Services.AddValidatorsFromAssemblyContaining<CreateUserCommandValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<UpdateUserCommandValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<GetUsersQueryValidator>();
            // Profiles
            builder.Services.AddValidatorsFromAssemblyContaining<UpdateProfileCommandValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<GetProfilesQueryValidator>();
            // Organisations
            builder.Services.AddValidatorsFromAssemblyContaining<CreateOrganisationCommandValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<UpdateOrganisationCommandValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<GetOrganisationsQueryValidator>();
            #endregion

            #region aws s3
            builder.Services.Configure<AWSSettings>(builder.Configuration.GetSection("AWS"));
            builder.Services.AddAWSService<IAmazonS3>();
            builder.Services.AddScoped<S3Service>();
            #endregion

            #region NLog
            // NLog: Setup NLog for Dependency injection
            builder.Logging.ClearProviders();
            builder.Host.UseNLog();
            #endregion

            builder.Services.AddCors(option =>
            {
                option.AddPolicy(policyName, policy =>
                {
                    policy.WithOrigins("http://localhost:5173")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
            });
            
            var app = builder.Build();
            app.UseCors(policyName);


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
            
            app.UseCors(policyName);
            
            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();
            
            app.Run();
        }
    }
}
