using InnovateFuture.Domain.Entities;
using InnovateFuture.Domain.Enums;
using InnovateFuture.Infrastructure.Common.Persistence;

namespace InnovateFuture.Infrastructure.Common;

public class SeedDataService:ISeedDataService
{
    // Primary IDs for roles, organisations, and users
    private static readonly Guid _role01Id = Guid.Parse("e114c66a-07b2-4768-b0cf-c111895ce0c4");
    private static readonly Guid _role02Id = Guid.Parse("d3788298-39b4-4a40-9985-bfa6a830acd9");
    private static readonly Guid _role03Id = Guid.Parse("3b69fda3-555a-4658-a6ab-31e1f327ef79");
    private static readonly Guid _role04Id = Guid.Parse("32ef6536-3cb1-4846-bd32-cd34b489fd43");
    private static readonly Guid _role05Id = Guid.Parse("28c99a2a-e593-4353-8dc2-cb83fc1ebfea");
    
    private static readonly Guid _org01Id = Guid.Parse("d96e643e-a7aa-42b0-a8cd-1cdd8610e857");
    private static readonly Guid _org02Id = Guid.Parse("0aecbf37-ead3-470c-ad8b-790d7eea3b0a");
    
    private static readonly Guid _user01Id = Guid.Parse("725f77b0-258a-4a92-827a-f5c4adfcba49");
    private static readonly Guid _profile01Id = Guid.Parse("4d69456b-9b86-43b9-b8f7-09a88062eb6b");
    
    private static readonly Guid _cognitoUuid = Guid.Parse("e95e0498-b0c1-700b-bb76-f571c5ec3f7c");
    

    // Seed roles
    private static Role[] GetRoles() =>
    [
        new Role("Platform Admin", RoleEnum.PlatformAdmin, _role01Id, "Responsible for managing the entire platform, including..."),
        new Role("Organisation Admin", RoleEnum.OrgAdmin, _role02Id, "Oversees organisational-level operations, including..."),
        new Role("Organisation Teacher", RoleEnum.OrgTeacher, _role03Id, "Handles teaching-related responsibilities within the organisation, such as..."),
        new Role("Parent", RoleEnum.Parent, _role04Id, "Allows monitoring of a child’s progress..."),
        new Role("Student", RoleEnum.Student, _role05Id, "Access to tour details...")
    ];

    // Seed organisations
    private static Organisation[] GetOrganisations() =>
    [
        new Organisation("org_name_01_test", _org01Id),
        new Organisation("org_name_02_test", _org02Id)
    ];

    // Seed users
    private static User[] GetUsers() =>
    [
        new User("yangqingyan0@gmail.com", _user01Id, _cognitoUuid)
    ];

    // Seed profiles
    private static Profile[] GetProfiles() =>
    [
        new Profile(
            _user01Id,
            _role01Id,
            _org01Id,
        null,
        null,
            _profile01Id)
    ];
    public void Initialize(ApplicationDbContext context)
    {
        context.Roles.AddRange(GetRoles());
        context.Organisations.AddRange(GetOrganisations());
        var users = GetUsers();
        context.Users.AddRange(users);
        var profiles = GetProfiles();
        context.Profiles.AddRange(profiles);
        context.SaveChanges();
        
        // update default profile of each user
        foreach (var user in users)
        {
            var defaultProfileId =profiles.FirstOrDefault(p=>p.UserId==user.UserId)?.ProfileId;
            if (defaultProfileId.HasValue)
            {
                user.UpdateDefaultProfile(defaultProfileId.Value);
            }
        }
        context.Users.UpdateRange(users);
        context.SaveChanges();
    }
}