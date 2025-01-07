using InnovateFuture.Domain.Entities;
using InnovateFuture.Domain.Enums;

namespace InnovateFuture.Infrastructure.Common;

public static class DataSeed
{
    // Primary IDs for roles, organisations, and users
    private static readonly Guid[] PrimaryIds =
    {
        Guid.Parse("e114c66a-07b2-4768-b0cf-c111895ce0c4"),
        Guid.Parse("d3788298-39b4-4a40-9985-bfa6a830acd9"),
        Guid.Parse("3b69fda3-555a-4658-a6ab-31e1f327ef79"),
        Guid.Parse("32ef6536-3cb1-4846-bd32-cd34b489fd43"),
        Guid.Parse("28c99a2a-e593-4353-8dc2-cb83fc1ebfea"),
        Guid.Parse("d96e643e-a7aa-42b0-a8cd-1cdd8610e857"),
        Guid.Parse("0aecbf37-ead3-470c-ad8b-790d7eea3b0a"),
        Guid.Parse("725f77b0-258a-4a92-827a-f5c4adfcba49"),
        Guid.Parse("4d69456b-9b86-43b9-b8f7-09a88062eb6b"),
    };

    private static readonly Guid[] CognitoUuids =
    {
        Guid.Parse("e95e0498-b0c1-700b-bb76-f571c5ec3f7c"),
    };

    // Seed roles
    public static Role[] GetRoles() =>
    [
        new Role("Platform Admin", RoleEnum.PlatformAdmin, PrimaryIds[0], "Responsible for managing the entire platform, including..."),
            new Role("Organisation Admin", RoleEnum.OrgAdmin, PrimaryIds[1], "Oversees organisational-level operations, including..."),
            new Role("Organisation Teacher", RoleEnum.OrgTeacher, PrimaryIds[2], "Handles teaching-related responsibilities within the organisation, such as..."),
            new Role("Parent", RoleEnum.Parent, PrimaryIds[3], "Allows monitoring of a child’s progress..."),
            new Role("Student", RoleEnum.Student, PrimaryIds[4], "Access to tour details...")
    ];

    // Seed organisations
    public static Organisation[] GetOrganisations() =>
    [
        new Organisation("org_name_01_test", PrimaryIds[5]),
        new Organisation("org_name_02_test", PrimaryIds[6])
    ];

    // Seed users
    public static User[] GetUsers() =>
    [
        new User("yangqingyan0@gmail.com", PrimaryIds[7], CognitoUuids[0])
    ];

    // Seed profiles
    public static Profile[] GetProfiles() =>
    [
        new Profile(
                PrimaryIds[7],                  // UserId
                PrimaryIds[0],                  // RoleId
                PrimaryIds[5],                  // OrgId
                null,                           // InvitedBy
                null,                           // SupervisedBy
                PrimaryIds[8])                  // ProfileId
    ];

    // Helper method to set default profile ID after creation
    public static void UpdateDefaultProfile(User[] users)
    {
        users[0].UpdateDefaultProfile(PrimaryIds[8]);
    }
}