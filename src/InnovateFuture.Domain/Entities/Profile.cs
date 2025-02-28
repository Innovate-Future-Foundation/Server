using InnovateFuture.Domain.Enums;

namespace InnovateFuture.Domain.Entities;

public class Profile
{
    public Guid Id { get; private set; }
    public Guid UserId { get;  set; }
    public User User { get;  private set; } = null!;
    public Guid? OrgId { get; private set; }
    public Organisation? Organisation { get; private set; }
    public Guid? Inviter { get; private set; }
    public Profile? InviterProfile { get; private set; }
    public Guid? Supervisor { get; private set; }
    public Profile? SupervisorProfile { get; private set; }
    public RoleEnum Role { get; private set; }
    public string? Name { get; private set; }
    public string? Email { get; private set; }
    public string? Phone { get; private set; }
    public string? AvatarUrl { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsConfirmed { get; private set; }
    
    public ICollection<Activity>? AssignedActivities { get; private set; } = new List<Activity>();
    public ICollection<Tour>? LeadingTours { get; private set; } = new List<Tour>();
    public ICollection<StudentTourEnrollment>? StudentTourEnrollments { get; private set; } = new List<StudentTourEnrollment>();
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public Profile() {}
    
    public Profile(
        Guid userId,
        RoleEnum role,
        Guid? orgId = null,
        Guid? inviter = null,
        Guid? supervisor = null,
        string? name = null,
        string? email = null,
        string? phone = null,
        string? avatarUrl = null,
        Guid? id=null
    )
    {
        Id = id?? Guid.NewGuid();
        UserId = userId;
        Role = role;
        OrgId = orgId;
        Inviter = inviter;
        Supervisor = supervisor;
        Name = name;
        Email = email;
        Phone = phone;
        AvatarUrl = avatarUrl;
        Inviter = inviter;
        Supervisor = supervisor;
        IsActive = true;
        IsConfirmed = false;
        CreatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);
        UpdatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);
    }

    public void UpdateProfile(string? email, string? name, string? phone, string? avatarUrl, bool? isActive, bool? isConfirmed)
    {
        Email = string.IsNullOrWhiteSpace(email)? Email : email;
        Name = string.IsNullOrWhiteSpace(name)? Name : name;
        Phone = string.IsNullOrWhiteSpace(phone)? Phone : phone;
        AvatarUrl = string.IsNullOrWhiteSpace(avatarUrl)? AvatarUrl : avatarUrl;
        IsActive = isActive?? IsActive;
        IsConfirmed = isConfirmed?? IsConfirmed;
    }
    // Methods to set navigation properties
    public void AddUser(User user)
    {
        User = user?? throw new ArgumentNullException(nameof(user));
        UserId = user.Id;
    }
    public void AddOrganisation(Organisation? organisation)
    {
        Organisation = organisation;
        OrgId = organisation?.Id;
    }
    public void AddInviterProfile(Profile? inviterProfile)
    {
        InviterProfile = inviterProfile;
        Inviter = inviterProfile?.Id;
    }
    
    public void AddSupervisorProfile(Profile? supervisorProfile)
    {
        SupervisorProfile = supervisorProfile;
        Supervisor = supervisorProfile?.Id;
    }  

    public void ConfirmRole()
    {
        IsConfirmed = true;
        UpdatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);
    }

    public void AddStudentTourEnrollment(StudentTourEnrollment studentTourEnrollment)
    {
        if (studentTourEnrollment == null)
        {
            throw new ArgumentNullException(nameof(studentTourEnrollment),"studentTourEnrollment cannot be null.");
        }
        StudentTourEnrollments?.Add(studentTourEnrollment);
    }
    public void RemoveStudentTourEnrollment(StudentTourEnrollment studentTourEnrollment)
    {
        if (studentTourEnrollment == null)
        {
            throw new ArgumentNullException(nameof(studentTourEnrollment),"studentTourEnrollment cannot be null.");
        }
        StudentTourEnrollments?.Remove(studentTourEnrollment);
    }
}