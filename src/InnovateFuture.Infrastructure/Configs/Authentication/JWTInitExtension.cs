using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace InnovateFuture.Infrastructure.Configs.Authentication;

public static class JWTInitExtension
{
    public static void AddJWTEXT(this IServiceCollection services, JWTConfig jWTConfig)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).
            AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jWTConfig.Issuer,
                    ValidateIssuer = true,
                    ValidAudience = jWTConfig.Audience,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jWTConfig.SecrectKey))
                };
            });
    }
}