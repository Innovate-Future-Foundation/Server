namespace InnovateFuture.Domain.Entities;

public class Profile
{
    public Guid ProfileId { get; private set; }
    public Guid UserId { get; private set; }
    public User User { get; private set; }
    public Guid? OrgId { get; private set; }
    public Organisation? Organisation { get; private set; }
    public Guid RoleId { get; private set; }
    public Role Role { get; private set; }
    public Guid? Inviter { get; private set; }
    public Profile? InviterProfile { get; private set; }
    public Guid? Supervisor { get; private set; }
    public Profile? SupervisorProfile { get; private set; }
    public string? Email { get; private set; }
    public string? Name { get; private set; }
    public string? Phone { get; private set; }
    public string? AvatarUrl { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsConfirmed { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public Profile() { }
    public Profile(
        Guid userId,
        Guid roleId,
        Guid? orgId = null,
        Guid? inviter = null,
        Guid? supervisor = null,
        Guid? profileId = null
    )
    {
        ProfileId = profileId?? Guid.NewGuid();
        UserId = userId;
        RoleId = roleId;
        OrgId = orgId;
        Inviter = inviter;
        Supervisor = supervisor;
        IsActive = true;
        IsConfirmed = false;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
    public void UpdateProfile(string? email, string? name, string? phone, string? avatarUrl, bool? isActive, bool? isConfirmed)
    {
        Email = string.IsNullOrWhiteSpace(email)? Email : email;
        Name = string.IsNullOrWhiteSpace(name)? Name : name;
        Phone = string.IsNullOrWhiteSpace(phone)? Phone : phone;
        AvatarUrl = string.IsNullOrWhiteSpace(AvatarUrl)? AvatarUrl : avatarUrl;
        IsActive = isActive?? IsActive;
        IsConfirmed = isConfirmed?? IsConfirmed;
        UpdatedAt = DateTime.UtcNow;
        
    }
    // Methods to set navigation properties
    public void AddUser(User user)
    {
        User = user?? throw new ArgumentNullException(nameof(user));
        UserId = user.UserId;
    }
    public void AddRole(Role role)
    {
        Role = role?? throw new ArgumentNullException(nameof(role));
        RoleId = role.RoleId;
    }
    public void AddOrganisation(Organisation? organisation)
    {
        Organisation = organisation;
        OrgId = organisation?.OrgId;
    }
    public void AddInviterProfile(Profile? inviterProfile)
    {
        InviterProfile = inviterProfile;
        Inviter = inviterProfile?.ProfileId;
    }
    
    public void AddSupervisorProfile(Profile? supervisorProfile)
    {
        SupervisorProfile = supervisorProfile;
        Supervisor = supervisorProfile?.ProfileId;
    }  
}