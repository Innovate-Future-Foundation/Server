using System.Text;
using Amazon.S3;
using InnovateFuture.Application.Auth.Commands.ConfirmEmail;
using InnovateFuture.Application.Auth.Commands.Login;
using InnovateFuture.Application.Auth.Commands.Password;
using InnovateFuture.Application.Auth.Commands.Register;
using InnovateFuture.Application.Auth.Commands.ResendVerificationEmail;
using InnovateFuture.Application.Auth.Commands.SendTemporaryPassword;
using InnovateFuture.Application.Auth.Commands.SendVerificationEmail;
using InnovateFuture.Application.Auth.Queries.GetMe;
using InnovateFuture.Application.Behaviors;
using InnovateFuture.Application.Organisations.Commands.CreateOrganisation;
using InnovateFuture.Application.Organisations.Commands.UpdateOrganisation;
using InnovateFuture.Application.Organisations.Queries.GetOrganisations;
using InnovateFuture.Application.Profiles.Commands.UpdateProfile;
using InnovateFuture.Application.Profiles.Queries.GetProfile;
using InnovateFuture.Application.Services.Auth.Register;
using InnovateFuture.Application.Services.Security.TokenService;
using InnovateFuture.Application.Services.SendEmail;
using InnovateFuture.Application.Services.UserService;
using InnovateFuture.Application.Users.Commands.CreateUser;
using InnovateFuture.Application.Users.Commands.UpdateUser;
using InnovateFuture.Application.Users.Queries.GetUser;
using InnovateFuture.Application.Users.Queries.GetUsers;
using InnovateFuture.Infrastructure.Activities.Persistence.Interfaces;
using InnovateFuture.Infrastructure.Activities.Persistence.Repositories;
using InnovateFuture.Infrastructure.Common;
using InnovateFuture.Infrastructure.Common.Persistence;
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
using InnovateFuture.Infrastructure.UnitOfWork.Persistence.Interface;
using InnovateFuture.Infrastructure.UnitOfWork.Persistence.Repositiories;
using InnovateFuture.Infrastructure.Users.Persistence.Interfaces;
using InnovateFuture.Infrastructure.Users.Persistence.Repositories;
using MediatR;
using FluentValidation;
using InnovateFuture.Application.Profiles.Queries.GetProfiles;
using InnovateFuture.Application.Services.S3;
using InnovateFuture.Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace InnovateFuture.Application.Common;

public static class ApplicationDependencyInjection
{
    public static IServiceCollection AddApplicationservices(this IServiceCollection services,IConfiguration configuration)
    {
        #region Email Service Configuration
        // bind EmailSettings from appsettings.Development.json
        services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
        #endregion
        
        #region JWT
        services.Configure<JWTConfig>(configuration.GetSection("JWTConfig"));
           
        // Configure JWT Authentication
        var key = Encoding.UTF8.GetBytes(configuration["JWTConfig:SecretKey"]);
        services.AddAuthentication(options =>
            {
                // Explicitly use JWT
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; 
                // Prevents silent failures
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;   
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["JWTConfig:Issuer"],
                    ValidAudience = configuration["JWTConfig:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
                    
                // allow extracting JWT from cookies instead of header
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Cookies["access-token"]; 

                        if (!string.IsNullOrEmpty(accessToken))
                        {
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            });

        services.AddAuthorization();
        #endregion
        
        #region Identity 
        services.AddIdentity<User, IdentityRole<Guid>>()
            .AddEntityFrameworkStores<ApplicationDbContext>() 
            .AddTokenProvider<DataProtectorTokenProvider<User>>("InnovateFuture")
            .AddDefaultTokenProviders();
        #endregion
        
        #region AWS S3
        services.Configure<AWSSettings>(configuration.GetSection("AWS"));
        services.AddAWSService<IAmazonS3>();
        services.AddScoped<S3Service>();
        #endregion
        
        #region Service Instances
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(RegisterOrganisationAdminHandler).Assembly);
                cfg.RegisterServicesFromAssembly(typeof(RegisterNormalUserHandler).Assembly);
                cfg.RegisterServicesFromAssembly(typeof(SendVerificationEmailHandler).Assembly);
                cfg.RegisterServicesFromAssembly(typeof(ResendVerificationEmailHandler).Assembly);
                cfg.RegisterServicesFromAssembly(typeof(ConfirmEmailHandler).Assembly);
                cfg.RegisterServicesFromAssembly(typeof(LoginHandler).Assembly);
                cfg.RegisterServicesFromAssembly(typeof(GetMeQueryHandler).Assembly);
                cfg.RegisterServicesFromAssembly(typeof(ResetPasswordHandler).Assembly);
                cfg.RegisterServicesFromAssembly(typeof(ForgetPasswordHandler).Assembly);
                cfg.RegisterServicesFromAssembly(typeof(SendTemporaryPasswordHandler).Assembly);
                
                cfg.RegisterServicesFromAssembly(typeof(CreateUserHandler).Assembly);
                cfg.RegisterServicesFromAssembly(typeof(UpdateUserHandler).Assembly);
                cfg.RegisterServicesFromAssembly(typeof(GetUsersHandler).Assembly);
                cfg.RegisterServicesFromAssembly(typeof(GetUserHandler).Assembly);
                
                cfg.RegisterServicesFromAssembly(typeof(UpdateProfileHandler).Assembly);
                cfg.RegisterServicesFromAssembly(typeof(GetProfileHandler).Assembly);
                
                cfg.RegisterServicesFromAssembly(typeof(CreateOrganisationHandler).Assembly);
                cfg.RegisterServicesFromAssembly(typeof(UpdateOrganisationHandler).Assembly);
                cfg.RegisterServicesFromAssembly(typeof(GetOrganisationsHandler).Assembly);
            
                cfg.RegisterServicesFromAssembly(typeof(RegisterOrganisationAdminHandler).Assembly);
                cfg.RegisterServicesFromAssembly(typeof(SendVerificationEmailHandler).Assembly);
                cfg.RegisterServicesFromAssembly(typeof(ConfirmEmailHandler).Assembly);
            });
                
            // auto mapper instance
            // services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            // customized instances
            services.AddScoped<ISeedDataService, SeedDataService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IOrgRepository, OrgRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IProfileRepository, ProfileRepository>();
            services.AddScoped<IActivityRepository, ActivityRepository>();
            services.AddScoped<IDayRepository, DayRepository>();
            services.AddScoped<ITourRepository, TourRepository>();
            services.AddScoped<IStudentTourEnrollmentRepository, StudentTourEnrollmentRepository>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ITokenService, TokenService>();
            
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddValidatorsFromAssembly(typeof(RegisterOrganisationAdminValidator).Assembly);
            services.AddValidatorsFromAssembly(typeof(RegisterNormalUserValidator).Assembly);
            services.AddValidatorsFromAssembly(typeof(LoginValidator).Assembly);
            services.AddValidatorsFromAssembly(typeof(CreateUserCommandValidator).Assembly);
            services.AddValidatorsFromAssembly(typeof(UpdateUserCommandValidator).Assembly);
            services.AddValidatorsFromAssembly(typeof(GetUsersQueryValidator).Assembly);
            services.AddValidatorsFromAssembly(typeof(UpdateProfileCommandValidator).Assembly);
            services.AddValidatorsFromAssembly(typeof(CreateOrganisationCommandValidator).Assembly);
            services.AddValidatorsFromAssembly(typeof(UpdateOrganisationCommandValidator).Assembly);
            services.AddValidatorsFromAssembly(typeof(GetOrganisationsQueryValidator).Assembly);
        
            #endregion
            
        #region Fluent Validators
        // Auth
        services.AddValidatorsFromAssemblyContaining<RegisterOrganisationAdminValidator>();
        services.AddValidatorsFromAssemblyContaining<RegisterNormalUserValidator>();
        services.AddValidatorsFromAssemblyContaining<LoginValidator>();
        // Users
        services.AddValidatorsFromAssemblyContaining<CreateUserCommandValidator>();
        services.AddValidatorsFromAssemblyContaining<UpdateUserCommandValidator>();
        services.AddValidatorsFromAssemblyContaining<GetUsersQueryValidator>();
        // Profiles
        services.AddValidatorsFromAssemblyContaining<UpdateProfileCommandValidator>();
        services.AddValidatorsFromAssemblyContaining<GetProfilesQueryValidator>();
        // Organisations
        services.AddValidatorsFromAssemblyContaining<CreateOrganisationCommandValidator>();
        services.AddValidatorsFromAssemblyContaining<UpdateOrganisationCommandValidator>();
        services.AddValidatorsFromAssemblyContaining<GetOrganisationsQueryValidator>();
        #endregion
        
        return services;
    }
}