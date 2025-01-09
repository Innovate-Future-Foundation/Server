
using InnovateFuture.Infrastructure.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace InnovateFuture.Infrastructure.Configs;

public static class SeedDataExtension
{
    public static void SeedDataEXT(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var seedDataService = scope.ServiceProvider.GetRequiredService<ISeedDataService>();
            
            // Check if data already exists
            if (seedDataService.CanSeed())
            {
                seedDataService.Initialize();
            }
        }
    }
}