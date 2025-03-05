using System.Reflection;
using Microsoft.OpenApi.Models;

namespace InnovateFuture.Api.Configs;

public static class SwaggerInitExtension
{
    public static void AddSwaggerEXT(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(option =>
            {
                // get version info from version enum
                typeof(ApiVersion).GetEnumNames().ToList().ForEach(version =>
                {
                    string versionString = version.ToString();
                    // title displayed in swagger
                    option.SwaggerDoc(versionString, new OpenApiInfo
                    {
                        Title = "Innovate Future API",
                        Version = versionString,
                        Description = $"The Innovate Future API ({versionString}) provides a set of endpoints to manage and interact with resources in overseas tourism. ",
                        Contact = new OpenApiContact
                        {
                            Name = "Contact Us",
                            Url = new Uri("mailto:example@mail.com")
                        }
                    });
                });

                // config summery comments into each action in swagger
                var xmlFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                option.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFileName), true);

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
                typeof(ApiVersion).GetEnumNames().ToList().ForEach(version =>
                {
                    string versionString = version.ToString();
                    option.SwaggerEndpoint($"/swagger/{versionString}/swagger.json", $"API Version: {versionString}");
                });
            });
        }
}