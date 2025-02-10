
namespace InnovateFuture.Infrastructure.Common;

public interface ISeedDataService
{
    Task InitializeAsync();
    Task<bool> CanSeedAsync();
}