using InnovateFuture.Domain.Entities;
using InnovateFuture.Domain.Enums;
using InnovateFuture.Infrastructure.Common.Persistence;

namespace InnovateFuture.Infrastructure.Common;

public class SeedDataService:ISeedDataService
{
    private readonly ApplicationDbContext _dbContext;

    public SeedDataService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    private static readonly Guid _org01Id = Guid.Parse("d96e643e-a7aa-42b0-a8cd-1cdd8610e857");
    
    private static readonly Guid _user01Id = Guid.Parse("725f77b0-258a-4a92-827a-f5c4adfcba49");

    private static readonly Guid _profile01Id = Guid.Parse("4d69456b-9b86-43b9-b8f7-09a88062eb6b");
    
    private static readonly Guid _cognitoUuid01 = Guid.Parse("e95e0498-b0c1-700b-bb76-f571c5ec3f7c");
    
    // Seed organisations
    private static Organisation[] GetOrganisations() =>
    [
        new Organisation("org_name_01_test", _org01Id,null,null,null,"org_01_test@test.com",SubscriptionEnum.basic),
    ];

    // Seed users
    private static User[] GetUsers() =>
    [
        new User("example0@gmail.com", _user01Id, _cognitoUuid01,null,null,"example0")
    ];

    // Seed profiles
    private static Profile[] GetProfiles() =>
    [
        new Profile(
            _user01Id,
            RoleEnum.PlatformAdmin,
            null,
        null,
        null,
            _profile01Id),
    ];
    public void Initialize()
    {
        _dbContext.Organisations.AddRange(GetOrganisations());
        var users = GetUsers();
        _dbContext.Users.AddRange(users);
        var profiles = GetProfiles();
        _dbContext.Profiles.AddRange(profiles);
        _dbContext.SaveChanges();
        
        // update default profile of each user
        users.ToList().ForEach(u =>
        {
            var defaultProfileId =profiles.FirstOrDefault(p=>p.UserId==u.UserId)!.ProfileId;
            u.UpdateDefaultProfile(defaultProfileId);
        });
        
        _dbContext.Users.UpdateRange(users);
        _dbContext.SaveChanges();
    }
    public bool CanSeed()
    {
        return (!_dbContext.Users.Any() && !_dbContext.Profiles.Any() &&
                !_dbContext.Organisations.Any());
    }
}