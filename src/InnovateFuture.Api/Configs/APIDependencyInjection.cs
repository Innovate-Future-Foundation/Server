using System.Text;
using InnovateFuture.Api.Authorization;
using InnovateFuture.Api.Filters;
using InnovateFuture.Application.Services.Security.TokenService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;


namespace InnovateFuture.Api.Configs
{
    public static class APIDependencyInjection
    {
        public static IServiceCollection AddAPIServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpContextAccessor();
            
            #region JWT
            services.Configure<JWTConfig>(configuration.GetSection("JWTConfig"));
           
            var key = Encoding.UTF8.GetBytes(configuration["JWTConfig:SecretKey"]);
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; 
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

            services.AddAuthorization(options =>
            {
                options.AddPolicy("NotParentOrStudent", policy => policy.Requirements.Add( new NotParentOrStudentRequirement()));
            });
            #endregion

            #region Controllers and Filters
            services.AddControllers(option =>
                {
                    option.Filters.Add<CommonResultFilter>();
                    option.Filters.Add<ModelValidationFilter>();
                })
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
                });
            #endregion
            
            // Disable automatic model validation
            services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);

            services.AddSwaggerEXT();
            
            #region Cors
            services.AddCors(options =>
            {
                options.AddPolicy("AllowLocalhost", policy =>
                {
                    policy.WithOrigins(configuration["FrontEndBaseUrl"])
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
            });
            #endregion
            
            return services;
        }
    }
}