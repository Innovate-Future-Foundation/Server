
using InnovateFuture.Infrastructure.Common;
using Microsoft.Extensions.DependencyInjection;

namespace InnovateFuture.Infrastructure.Configs;

public static class SeedDataExtension
{
    public static void SeedDataEXT(this IServiceProvider serviceProvider)
    {
        using (var scope = serviceProvider.CreateScope())
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