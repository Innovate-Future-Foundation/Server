using InnovateFuture.Infrastructure.Common.Persistence;

namespace InnovateFuture.Infrastructure.Common;

public interface ISeedDataService
{
    void Initialize(ApplicationDbContext context);
}