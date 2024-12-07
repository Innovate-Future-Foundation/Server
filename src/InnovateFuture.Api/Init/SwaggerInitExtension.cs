using System.Reflection;
using InnovateFuture.Api.Enums;
using Microsoft.OpenApi.Models;

namespace InnovateFuture.Api.Init;

public static class SwaggerInitExtension
{
    public static void AddSwaggerEXT(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(option =>
            {
                // get version info from version enum
                typeof(APIVersion).GetEnumNames().ToList().ForEach(version =>
                {
                    string versionString = version.ToString();
                    // title displayed in swagger
                    option.SwaggerDoc(versionString, new OpenApiInfo
                    {
                        Title = "My First Web Api Project",
                        Version = versionString,
                        Description = $"This is My First Web Api Project Version: {versionString}",
                        Contact = new OpenApiContact
                        {
                            Name = "Qingyan Yang",
                            Url = new Uri("https://google.com")
                        }
                    });
                });

                // config summery comments into each action in swagger
                var xmlFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //option.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFileName), true);

                // config auth
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Please enter token, format: \"Bearer ***\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                // add requirements for security
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference=new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new List<string>{ }
                    }
                });
            });
        }

        public static void UseSwaggerEXT(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(option =>
            {
                // get version info from version enum
                typeof(APIVersion).GetEnumNames().ToList().ForEach(version =>
                {
                    string versionString = version.ToString();
                    option.SwaggerEndpoint($"/swagger/{versionString}/swagger.json", $"API Version: {versionString}");
                });
            });
        }
}