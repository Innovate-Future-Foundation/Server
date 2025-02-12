
// using InnovateFuture.Infrastructure.Common;

using InnovateFuture.Infrastructure.Common;
using Microsoft.Extensions.DependencyInjection;

namespace InnovateFuture.Infrastructure.Configs;

public static class SeedDataExtension
{
    public static async Task SeedDataEXT(this IServiceProvider serviceProvider)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var seedDataService = scope.ServiceProvider.GetRequiredService<ISeedDataService>();

        // ✅ Prevent unnecessary seeding if data already exists
        if (!await seedDataService.CanSeedAsync()) return;

        await seedDataService.InitializeAsync();
    }
}