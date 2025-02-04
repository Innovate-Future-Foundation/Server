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
    
    // Primary IDs for roles, organisations, and users
    private static readonly Guid _role01Id = Guid.Parse("e114c66a-07b2-4768-b0cf-c111895ce0c4");
    private static readonly Guid _role02Id = Guid.Parse("d3788298-39b4-4a40-9985-bfa6a830acd9");
    private static readonly Guid _role03Id = Guid.Parse("3b69fda3-555a-4658-a6ab-31e1f327ef79");
    private static readonly Guid _role04Id = Guid.Parse("32ef6536-3cb1-4846-bd32-cd34b489fd43");
    private static readonly Guid _role05Id = Guid.Parse("28c99a2a-e593-4353-8dc2-cb83fc1ebfea");
    private static readonly Guid _role06Id = Guid.Parse("64fe5f03-b1c4-4b44-9894-a89f5772a751");
    
    private static readonly Guid _org01Id = Guid.Parse("d96e643e-a7aa-42b0-a8cd-1cdd8610e857");
    private static readonly Guid _org02Id = Guid.Parse("0aecbf37-ead3-470c-ad8b-790d7eea3b0a");
    private static readonly Guid _org03Id = Guid.Parse("b8fff326-d13b-4ade-9822-e9ee8da23129");
    private static readonly Guid _org04Id = Guid.Parse("806b2ff3-76b4-4139-a417-f63d6b8c04b2");
    private static readonly Guid _org05Id = Guid.Parse("53700e74-4fd1-4aa2-9a6c-83eefd806efb");
    private static readonly Guid _org06Id = Guid.Parse("6a4f721b-f71c-4cc4-9b14-8777ffbe0e55");
    private static readonly Guid _org07Id = Guid.Parse("e2041d02-eeb0-4fa7-b5b4-f313af5d495b");
    private static readonly Guid _org08Id = Guid.Parse("a8b91013-2f9a-4c2c-9806-6e4a8a2e7875");
    private static readonly Guid _org09Id = Guid.Parse("5c12a964-b4d2-46a7-bc50-480bd85fde40");
    private static readonly Guid _org10Id = Guid.Parse("c3ff657e-2770-4963-b49f-cf4d2bf393b0");
    
    
    private static readonly Guid _user01Id = Guid.Parse("725f77b0-258a-4a92-827a-f5c4adfcba49");
    private static readonly Guid _user02Id = Guid.Parse("529b4745-1d71-492e-901b-4227120155ff");
    private static readonly Guid _user03Id = Guid.Parse("922e21f3-ae20-4deb-9af7-26ff459eedef");
    private static readonly Guid _user04Id = Guid.Parse("1f269532-300a-4793-a151-d717b3cb9086");
    private static readonly Guid _user05Id = Guid.Parse("28ed4bfe-0fcc-4109-bcd0-013415a780a3");
    private static readonly Guid _user06Id = Guid.Parse("b6eaaead-edbf-4981-b1b8-9bbbc9c85145");
    private static readonly Guid _user07Id = Guid.Parse("2e8a59f0-5b5f-450d-a80d-222933e36dbf");
    private static readonly Guid _user08Id = Guid.Parse("134a16ab-07a7-41be-9471-0779b862ba34");
    private static readonly Guid _user09Id = Guid.Parse("09918989-ff19-4549-9a72-586059b88747");
    private static readonly Guid _user10Id = Guid.Parse("6f953d78-0eaf-4cee-a2aa-b135162e0a64");

    private static readonly Guid _profile01Id = Guid.Parse("4d69456b-9b86-43b9-b8f7-09a88062eb6b");
    private static readonly Guid _profile02Id = Guid.Parse("e09de84e-4799-40e9-9164-83826e9b6432");
    private static readonly Guid _profile03Id = Guid.Parse("7986eff2-8fa2-444e-8db2-294d9050137b");
    private static readonly Guid _profile04Id = Guid.Parse("e5a98a0c-0f68-4092-9769-68b2d9653351");
    private static readonly Guid _profile05Id = Guid.Parse("a7722a22-5c2e-46ff-949d-8c32835334ae");
    private static readonly Guid _profile06Id = Guid.Parse("7c050778-5945-47d5-880f-a38980b79c8a");
    private static readonly Guid _profile07Id = Guid.Parse("53ff570f-4427-4e16-8358-8fc33de8a511");
    private static readonly Guid _profile08Id = Guid.Parse("37742c25-4c58-4e61-9b23-4092ed51cc10");
    private static readonly Guid _profile09Id = Guid.Parse("e9f3e6d8-a92a-49ec-9b0d-2646880f664a");
    private static readonly Guid _profile10Id = Guid.Parse("31ed1e3e-20f3-4ff0-a4fe-013886a94552");

    
    private static readonly Guid _cognitoUuid01 = Guid.Parse("e95e0498-b0c1-700b-bb76-f571c5ec3f7c");
    private static readonly Guid _cognitoUuid02 = Guid.Parse("27d2c05b-4cde-43f7-879e-1333994d325d");
    private static readonly Guid _cognitoUuid03 = Guid.Parse("37686b84-ff3f-4f4c-84e7-4a08bf64f05d");
    private static readonly Guid _cognitoUuid04 = Guid.Parse("c804bc94-76f3-4de5-9ad4-3724e1d498b0");
    private static readonly Guid _cognitoUuid05 = Guid.Parse("d9d96d0a-57f3-49fc-b390-24147b69b1c2");
    private static readonly Guid _cognitoUuid06 = Guid.Parse("e9762744-b2c2-49b9-9b41-237b60939af1");
    private static readonly Guid _cognitoUuid07 = Guid.Parse("eaf10250-42f4-4dd4-9639-bb67245f449c");
    private static readonly Guid _cognitoUuid08 = Guid.Parse("85cd92de-03bb-479f-97ab-cb6db54fc001");
    private static readonly Guid _cognitoUuid09 = Guid.Parse("33f540c0-06a6-43bc-b6ba-fc2aef45706e");
    private static readonly Guid _cognitoUuid10 = Guid.Parse("db19ac57-e96e-41f6-9416-329762fae260");

    
    // Seed roles
    private static Role[] GetRoles() =>
    [
        new Role("Platform Admin", RoleEnum.PlatformAdmin, _role01Id, "Responsible for managing the entire platform, including..."),
        new Role("Organisation Admin", RoleEnum.OrgAdmin, _role02Id, "Oversees organisational-level operations, including inviting managers..."),
        new Role("Organisation Manager", RoleEnum.OrgManager, _role03Id, "Oversees organisational-level operations, including..."),
        new Role("Organisation Teacher", RoleEnum.OrgTeacher, _role04Id, "Handles teaching-related responsibilities within the organisation, such as..."),
        new Role("Parent", RoleEnum.Parent, _role05Id, "Allows monitoring of a child’s progress..."),
        new Role("Student", RoleEnum.Student, _role06Id, "Access to tour details...")
    ];

    // Seed organisations
    private static Organisation[] GetOrganisations() =>
    [
        new Organisation("org_name_01_test", _org01Id,null,null,null,"org_01_test@test.com",SubscriptionEnum.basic),
        new Organisation("org_name_02_test", _org02Id,null,null,null,"org_02_test@test.com",SubscriptionEnum.premium),
        new Organisation("org_name_03_test", _org03Id,null,null,null,"org_03_test@test.com",SubscriptionEnum.free),
        new Organisation("org_name_04_test", _org04Id,null,null,null,"org_04_test@test.com",SubscriptionEnum.basic),
        new Organisation("org_name_05_test", _org05Id,null,null,null,"org_05_test@test.com",SubscriptionEnum.basic),
        new Organisation("org_f_name_06_test", _org06Id,null,null,null,"org_q_06_test@test.com",SubscriptionEnum.premium),
        new Organisation("org_f_name_07_test", _org07Id,null,null,null,"org_q_07_test@test.com",SubscriptionEnum.basic),
        new Organisation("org_f_name_08_test", _org08Id,null,null,null,"org_q_08_test@test.com",SubscriptionEnum.free),
        new Organisation("org_f_name_09_test", _org09Id,null,null,null,"org_q_09_test@test.com",SubscriptionEnum.basic),
        new Organisation("org_f_name_10_test", _org10Id,null,null,null,"org_q_10_test@test.com",SubscriptionEnum.free),
    ];

    // Seed users
    private static User[] GetUsers() =>
    [
        new User("example0@gmail.com", _user01Id, _cognitoUuid01,null,"example0"),
        new User("example1@gmail.com", _user02Id, _cognitoUuid02,null,"example1"),
        new User("example2@gmail.com", _user03Id, _cognitoUuid03,null,"example2"),
        new User("example3@gmail.com", _user04Id, _cognitoUuid04,null,"example3"),
        new User("example4@gmail.com", _user05Id, _cognitoUuid05,null,"example4"),
        new User("example5@gmail.com", _user06Id, _cognitoUuid06,null,"example5"),
        new User("example6@gmail.com", _user07Id, _cognitoUuid07,null,"example6"),
        new User("example7@gmail.com", _user08Id, _cognitoUuid08,null,"example7"),
        new User("example8@gmail.com", _user09Id, _cognitoUuid09,null,"example8"),
        new User("example9@gmail.com", _user10Id, _cognitoUuid10,null,"example9"),
    ];

    // Seed profiles
    private static Profile[] GetProfiles() =>
    [
        new Profile(
            _user01Id,
            _role01Id,
            null,
        null,
        null,
            _profile01Id),
        new Profile(
            _user02Id,
            _role02Id,
            _org01Id,
            null,
            null,
            _profile02Id),
        new Profile(
            _user03Id,
            _role03Id,
            _org01Id,
            _profile02Id,
            null,
            _profile03Id),
        new Profile(
            _user04Id,
            _role03Id,
            _org01Id,
            _profile02Id,
            null,
            _profile04Id),
        new Profile(
            _user05Id,
            _role04Id,
            _org01Id,
            _profile04Id,
            null,
            _profile05Id),
        new Profile(
            _user06Id,
            _role05Id,
            _org01Id,
            _profile05Id,
            null,
            _profile06Id),
        new Profile(
            _user07Id,
            _role06Id,
            _org01Id,
            _profile05Id,
            _profile06Id,
            _profile07Id),
        //
        new Profile(
            _user08Id,
            _role03Id,
            _org01Id,
            _profile02Id,
            null,
            _profile08Id),
        new Profile(
            _user09Id,
            _role03Id,
            _org01Id,
            _profile02Id,
            null,
            _profile09Id),
        new Profile(
            _user10Id,
            _role04Id,
            _org01Id,
            _profile04Id,
            null,
            _profile10Id)
    ];
    public void Initialize()
    {
        _dbContext.Roles.AddRange(GetRoles());
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
        return (!_dbContext.Users.Any() && !_dbContext.Profiles.Any() && !_dbContext.Roles.Any() &&
                !_dbContext.Organisations.Any());
    }
}