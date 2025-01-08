
using InnovateFuture.Infrastructure.Common;
using InnovateFuture.Infrastructure.Common.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace InnovateFuture.Infrastructure.Configs;

public static class SeedDataExtension
{
    public static void SeedDataEXT(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var seedDataService = scope.ServiceProvider.GetRequiredService<ISeedDataService>();
            
            // Check if data already exists
            if (!dbContext.Users.Any() && !dbContext.Profiles.Any() && !dbContext.Roles.Any() && !dbContext.Organisations.Any())
            {
                seedDataService.Initialize(dbContext);
            }
        }
    }
}