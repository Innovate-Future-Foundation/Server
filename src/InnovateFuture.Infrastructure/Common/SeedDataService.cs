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
            .RuleFor(o => o.OrgStatus, f => f.PickRandom<OrgStatusEnum>())
            .RuleFor(o => o.Subscription, f => f.PickRandom<SubscriptionEnum>())
            .RuleFor(o => o.CreatedAt, f => DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified))
            .RuleFor(o => o.UpdatedAt, f => DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified));
        
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
            .RuleFor(u => u.CreatedAt, f => DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified))
            .RuleFor(u => u.UpdatedAt, f => DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified));
        
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
            .RuleFor(p => p.CreatedAt, f => DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified))
            .RuleFor(p => p.UpdatedAt, f => DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified))
            .RuleFor(p=>p.AvatarUrl, f => f.Internet.Avatar())
            .RuleFor(p=>p.IsActive, f => f.Random.Bool())
            .RuleFor(p=>p.IsConfirmed, f => f.Random.Bool());
        
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

    // Seed activities
    private static List<Activity> GetActivities(List<Organisation> organisations)
    {
        var fakeActivityGenerator = new Faker<Activity>("en")
            .RuleFor(a => a.Id, f => Guid.NewGuid())
            .RuleFor(a => a.Title, f => f.Lorem.Paragraph())
            .RuleFor(a => a.Comment, f => f.Lorem.Paragraph())
            .RuleFor(a => a.Summary, f => f.Lorem.Paragraph())
            .RuleFor(a => a.Text, f => f.Lorem.Sentence())
            .RuleFor(a => a.Location, f => f.Address.FullAddress())
            .RuleFor(a => a.CoverImgUrl, f => f.Image.PicsumUrl())
            .RuleFor(a => a.Status, f => f.PickRandom<TourStatusEnum>())
            .RuleFor(a => a.StartTime, f => DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified))
            .RuleFor(a => a.EndTime, f => DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified))
            .RuleFor(o => o.CreatedAt, f => DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified))
            .RuleFor(o => o.UpdatedAt, f => DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified));

        var activities = new List<Activity>();
        foreach (var organisation in organisations)
        {
            var orgActivities = fakeActivityGenerator.Clone()
                .RuleFor(p => p.OrgId, _ => organisation.Id)
                .Generate(200);
            activities.AddRange(orgActivities);
        }
        return activities;
    }
    
    // Seed days
    private static List<Day> GetDays(List<Tour> tours)
    {
        var fakeDayGenerator = new Faker<Day>("en")
            .RuleFor(a => a.Id, f => Guid.NewGuid())
            .RuleFor(a => a.Title, f => f.Lorem.Paragraph())
            .RuleFor(a => a.Comment, f => f.Lorem.Paragraph())
            .RuleFor(a => a.Summary, f => f.Lorem.Paragraph())
            .RuleFor(a => a.Text, f => f.Lorem.Sentence())
            .RuleFor(a => a.CoverImgUrl, f => f.Image.PicsumUrl())
            .RuleFor(a => a.Status, f => f.PickRandom<TourStatusEnum>())
            .RuleFor(o => o.CreatedAt, f => DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified))
            .RuleFor(o => o.UpdatedAt, f => DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified));

        var days = new List<Day>();
        
        foreach (var tour in tours)
        {
            var tourDays = fakeDayGenerator.Clone()
                .RuleFor(d => d.OrgId, _ => tour.OrgId)
                .RuleFor(d => d.TourId, _ => tour.Id)
                .Generate(5);
            days.AddRange(tourDays);
        }
        return days;
    }
    // Seed tours
    private static List<Tour> GetTours(List<Organisation> organisations,List<Guid> teacherProfileIds)
    {
        var fakeTourGenerator = new Faker<Tour>("en")
            .RuleFor(a => a.Id, f => Guid.NewGuid())
            .RuleFor(a => a.Title, f => f.Lorem.Paragraph())
            .RuleFor(a => a.Comment, f => f.Lorem.Paragraph())
            .RuleFor(a => a.Summary, f => f.Lorem.Paragraph())
            .RuleFor(a => a.Text, f => f.Lorem.Sentence())
            .RuleFor(a => a.CoverImgUrl, f => f.Image.PicsumUrl())
            .RuleFor(a => a.Status, f => f.PickRandom<TourStatusEnum>())
            .RuleFor(a => a.Leader, f => f.PickRandom(teacherProfileIds))
            .RuleFor(a => a.StartDate, f => DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified))
            .RuleFor(a => a.EndDate, f => DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified))
            .RuleFor(a => a.CreatedAt, f => DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified))
            .RuleFor(a => a.UpdatedAt, f => DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified));

        var tours = new List<Tour>();
        foreach (var organisation in organisations)
        {
            var orgTours = fakeTourGenerator.Clone()
                .RuleFor(p => p.OrgId, _ => organisation.Id)
                .Generate(100);
            
            tours.AddRange(orgTours);
        }
        return tours;
    }

    private static List<StudentTourEnrollment> GetStudentTourEnrollments(List<Guid> tourIds, List<Profile> studentsProfiles)
    {
        var fakeStudentTourEnrollmentGenerator = new Faker<StudentTourEnrollment>("en")
            .RuleFor(s => s.Status, f => f.PickRandom<EnrollmentStatusEnum>())
            .RuleFor(s => s.EnrollmentDate, f => DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified))
            .RuleFor(s => s.TourId, f => f.PickRandom(tourIds))
            .RuleFor(s => s.CreatedAt, f => DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified))
            .RuleFor(s => s.UpdatedAt, f => DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified));

        var studentTourEnrollments = new List<StudentTourEnrollment>();
        foreach (var studentsProfile in studentsProfiles)
        {
            var studentTourEnrollment = fakeStudentTourEnrollmentGenerator.Clone()
                .RuleFor(s => s.ProfileId, _ => studentsProfile.Id)
                .Generate(1);
            
            studentTourEnrollments.AddRange(studentTourEnrollment);
        }
        return studentTourEnrollments;
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
                user.UpdateDefaultProfile(userProfiles.First().Id);
            }
        }
        
        await _dbContext.SaveChangesAsync();
    }
    public async Task<bool> CanSeedAsync()
    {
        return !(await _dbContext.Users.AnyAsync() || 
                 await _dbContext.Profiles.AnyAsync() || 
                 await _dbContext.Organisations.AnyAsync()
                 );
    }
}