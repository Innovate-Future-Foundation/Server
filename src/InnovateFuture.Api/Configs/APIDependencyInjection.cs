using InnovateFuture.Api.Filters;
using Microsoft.AspNetCore.Mvc;

namespace InnovateFuture.Api.Configs
{
    public static class APIDependencyInjection
    {
        public static IServiceCollection AddAPIServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpContextAccessor();

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