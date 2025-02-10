using Bogus;
using InnovateFuture.Domain.Entities;
using InnovateFuture.Domain.Enums;
using InnovateFuture.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace InnovateFuture.Infrastructure.Common;

public class SeedDataService:ISeedDataService
{
    private readonly ApplicationDbContext _dbContext;
    public SeedDataService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    // Seed organisations
    private static List<Organisation> GetOrganisations()
    {
        var fakeOrganisations = new Faker<Organisation>("en")
            .RuleFor(o => o.Id, f => Guid.NewGuid())
            .RuleFor(o => o.OrgName, f => f.Company.CompanyName())
            .RuleFor(o => o.Email, (f, o) => f.Internet.Email(o.OrgName))
            .RuleFor(o => o.LogoUrl, f => f.Internet.Avatar())
            .RuleFor(o => o.WebsiteUrl, f => f.Internet.UrlWithPath())
            .RuleFor(o => o.Status, f => f.PickRandom<StatusEnum>())
            .RuleFor(o => o.Subscription, f => f.PickRandom<SubscriptionEnum>())
            .RuleFor(o => o.CreatedAt, f => DateTime.UtcNow )
            .RuleFor(o => o.UpdatedAt, f => DateTime.UtcNow);
        
        return fakeOrganisations.Generate(10);
    }
    
    // Seed users
    private static List<User> GetUsers()
    {
        var fakeUsers = new Faker<User>("en")
            .CustomInstantiator(f => new User()) 
            .RuleFor(u => u.Id, f => Guid.NewGuid())
            .RuleFor(u => u.UserName, f => f.Name.FullName())
            .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.UserName))
            .RuleFor(u => u.PasswordHash, f => f.Internet.Password())
            .RuleFor(u => u.EmailConfirmed, f => f.Random.Bool())
            .RuleFor(u => u.CreatedAt, f => DateTime.UtcNow)
            .RuleFor(u => u.UpdatedAt, f => DateTime.UtcNow);
        
        return fakeUsers.Generate(50);
    }

    // Seed profiles
    private static List<Profile> GetProfiles(List<User> users, List<Organisation> organisations)
    {
        var fakeProfileGenerator = new Faker<Profile>("en")
            .RuleFor(p => p.Id, f => Guid.NewGuid())
            .RuleFor(p => p.Name, f => f.Name.FullName())
            .RuleFor(p => p.Email, f => f.Internet.Email())
            .RuleFor(p => p.Phone, f => f.Phone.PhoneNumber())
            .RuleFor(p => p.Role, f => f.PickRandom<RoleEnum>())
            .RuleFor(p => p.CreatedAt, f => DateTime.UtcNow)
            .RuleFor(p => p.UpdatedAt, f => DateTime.UtcNow)
            .RuleFor(p=>p.AvatarUrl, f => f.Internet.Avatar())
            .RuleFor(p=>p.IsActive, f => f.Random.Bool());
        
        var profiles = new List<Profile>();

        
        // ✅ Generate Platform Admins (No specific organisation)
        int countPlatformAdmins = 0;
        var uniqueUserIdForPlatformAdmins = UniqueUserIdGenerator(5, users);
            var platformAdmins = fakeProfileGenerator.Clone()
                .RuleFor(p => p.Role, _ => RoleEnum.PlatformAdmin)
                .RuleFor(p => p.UserId, (f) => uniqueUserIdForPlatformAdmins[countPlatformAdmins++]) 
                .Generate(5);
        
        profiles.AddRange(platformAdmins);
        
        foreach (var organisation in organisations)
        {
            var orgProfiles = new List<Profile>();
            // ✅ Org Admins (No inviter, No supervisor)
            var orgAdmins = fakeProfileGenerator.Clone()
                .RuleFor(p => p.Role, _ => RoleEnum.OrgAdmin)
                .RuleFor(p => p.OrgId, _ => organisation.Id)
                .RuleFor(p => p.UserId, f => f.PickRandom(users).Id)
                .Generate(1);
            orgProfiles.AddRange(orgAdmins);

            // ✅ Org Managers (Invited by Org Admin)
            var countOrgManagers = 0;
            var uniqueUserIdForOrgManagers = UniqueUserIdGenerator(4, users);
            var orgManagers = fakeProfileGenerator.Clone()
                .RuleFor(p => p.Role, _ => RoleEnum.OrgManager)
                .RuleFor(p => p.OrgId, _ => organisation.Id)
                .RuleFor(p => p.Inviter, f => f.PickRandom(orgAdmins).Id)
                .RuleFor(p => p.UserId, f =>uniqueUserIdForOrgManagers[countOrgManagers++])
                .Generate(4);
            orgProfiles.AddRange(orgManagers);

            // ✅ Org Teachers (Invited by Org Manager)
            var countOrgTeachers = 0;
            var uniqueUserIdForTeachers = UniqueUserIdGenerator(5, users);
            var orgTeachers = fakeProfileGenerator.Clone()
                .RuleFor(p => p.Role, _ => RoleEnum.OrgTeacher)
                .RuleFor(p => p.OrgId, _ => organisation.Id)
                .RuleFor(p => p.Inviter, f => f.PickRandom(orgManagers).Id)
                .RuleFor(p => p.UserId, f => uniqueUserIdForTeachers[countOrgTeachers++])
                .Generate(5);
            orgProfiles.AddRange(orgTeachers);

            // ✅ Parents (Invited by Org Teacher, No Supervisor)
            var countParents = 0;
            var uniqueUserIdForParents = UniqueUserIdGenerator(20, users);
            var parents = fakeProfileGenerator.Clone()
                .RuleFor(p => p.Role, _ => RoleEnum.Parent)
                .RuleFor(p => p.OrgId, _ => organisation.Id)
                .RuleFor(p => p.Inviter, f => f.PickRandom(orgTeachers).Id)
                .RuleFor(p => p.UserId, f =>uniqueUserIdForParents[countParents++])
                .Generate(20);
            orgProfiles.AddRange(parents);

            // ✅ Students (Must have both an Inviter (Org Teacher) and a Supervisor (Parent))
            var countStudents = 0;
            var uniqueUserIdForStudents = UniqueUserIdGenerator(40, users);
            var students = new List<Profile>();
            foreach (var parent in parents)
            {
                var studentProfiles = fakeProfileGenerator.Clone()
                    .RuleFor(p => p.Role, _ => RoleEnum.Student)
                    .RuleFor(p => p.OrgId, _ => organisation.Id)
                    .RuleFor(p => p.Inviter, f => f.PickRandom(orgTeachers).Id)
                    .RuleFor(p => p.Supervisor, _ => parent.Id)
                    .RuleFor(p => p.UserId, f =>uniqueUserIdForStudents[countStudents++])
                    .Generate(2);

                students.AddRange(studentProfiles);
            }
            orgProfiles.AddRange(students);

            profiles.AddRange(orgProfiles);
        }
        return profiles;
    }

    public static List<Guid> UniqueUserIdGenerator(int number, List<User> users)
    {
        if (users.Count < number) 
            throw new ArgumentException("Not enough users to generate unique IDs.");

        var uniqueUserIds = new HashSet<Guid>();
        var random = new Random();

        while (uniqueUserIds.Count < number)
        {
            var selectedUser = users[random.Next(users.Count)];
            uniqueUserIds.Add(selectedUser.Id);
        }

        return uniqueUserIds.ToList();
    }
   
    public async Task InitializeAsync()
    {
        var organisations = GetOrganisations();
        await _dbContext.Organisations.AddRangeAsync(organisations);
        await _dbContext.SaveChangesAsync();

        var users = GetUsers();
        await _dbContext.Users.AddRangeAsync(users);
        await _dbContext.SaveChangesAsync();

        var profiles = GetProfiles(users, organisations);
        await _dbContext.Profiles.AddRangeAsync(profiles);
        await _dbContext.SaveChangesAsync();

        foreach (var user in users)
        {
            var userProfiles = profiles.Where(p => p.UserId == user.Id).ToList();
            if (userProfiles.Any())
            {
                user.UpdateProfile(userProfiles.First().Id);
            }
        }

        await _dbContext.SaveChangesAsync();
    }
    public async Task<bool> CanSeedAsync()
    {
        return !(await _dbContext.Users.AnyAsync() || 
                 await _dbContext.Profiles.AnyAsync() || 
                 await _dbContext.Organisations.AnyAsync());
    }
}