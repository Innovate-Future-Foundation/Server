
using InnovateFuture.Domain.Enums;
using InnovateFuture.Infrastructure.Common.Persistence;
using InnovateFuture.Infrastructure.Configs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
namespace InnovateFuture.Infrastructure.Common;

public static class InfrastructureDependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,IConfiguration configuration,string connectionString)
    {
        
        #region DB Connection
            
        services.Configure<DBConnectionConfig>(options =>
            configuration.GetSection("DBConnection").Bind(options));
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
        // register enums
        dataSourceBuilder.MapEnum<RoleEnum>();
        dataSourceBuilder.MapEnum<SubscriptionEnum>();
        dataSourceBuilder.MapEnum<OrgStatusEnum>();
        dataSourceBuilder.MapEnum<TourStatusEnum>();
        dataSourceBuilder.MapEnum<EnrollmentStatusEnum>();
            
        var dataSource = dataSourceBuilder.Build();
            
       services.AddDbContext<ApplicationDbContext>(
            dbContextOptions => dbContextOptions
                .UseNpgsql(dataSource,
                    npgsqlOptions => npgsqlOptions.SetPostgresVersion(new Version(17, 2)))
                // The following three options help with debugging, but should
                // be changed or removed for production.
                .LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors()
        );
        #endregion
        return services;
    }
}