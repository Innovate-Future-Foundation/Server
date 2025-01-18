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
    public Boolean IsActive { get; private set; }
    public string? Email { get; private set; }
    public Guid? InvitedBy { get; private set; }
    public Profile? InvitedByProfile { get; private set; }
    public Guid? SupervisedBy { get; private set; }
    public Profile? SupervisedByProfile { get; private set; }
    public string? Name { get; private set; }
    public string? Phone { get; private set; }
    public string? Avatar { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public Profile() { }
    public Profile(
        Guid userId,
        Guid roleId,
        Guid? orgId = null,
        Guid? invitedBy = null,
        Guid? supervisedBy = null,
        Guid? profileId = null
    )
    {
        ProfileId = profileId?? Guid.NewGuid();
        UserId = userId;
        RoleId = roleId;
        OrgId = orgId;
        InvitedBy = invitedBy;
        SupervisedBy = supervisedBy;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateProfile(string? email, string? name, string? phone, string? avatar, Boolean? isActive)
    {
        Email = string.IsNullOrWhiteSpace(email)? Email : email;
        Name = string.IsNullOrWhiteSpace(name)? Name : name;
        Phone = string.IsNullOrWhiteSpace(phone)? Phone : phone;
        Avatar = string.IsNullOrWhiteSpace(avatar)? Avatar : avatar;
        IsActive = isActive?? IsActive;
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
        Role = role ?? throw new ArgumentNullException(nameof(role));
        RoleId = role.RoleId;
    }

    public void AddOrganisation(Organisation? organisation)
    {
        Organisation = organisation;
        OrgId = organisation?.OrgId;
    }

    public void AddInvitedByProfile(Profile? invitedByProfile)
    {
        InvitedByProfile = invitedByProfile;
        InvitedBy = invitedByProfile?.ProfileId;
    }

    public void AddSupervisedByProfile(Profile? supervisedByProfile)
    {
        SupervisedByProfile = supervisedByProfile;
        SupervisedBy = supervisedByProfile?.ProfileId;
    }  

}