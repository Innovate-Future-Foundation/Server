namespace InnovateFuture.Domain.Entities;

public class Profile
{
    public Guid ProfileId { get; private set; }
    public Guid UserId { get; private set; }
    public Guid OrgId { get; private set; }
    public Guid RoleId { get; private set; }
    public Boolean IsActive { get; private set; }
    public string? Email { get; private set; }
    public Guid? InvitedBy { get; private set; }
    public Guid? SupervisedBy { get; private set; }
    public string? Name { get; private set; }
    public string? Phone { get; private set; }
    public string? Avatar { get; private set; }
    
    // navigation properties
    public User User { get; private set; }
    public Role Role { get; private set; }
    public Organisation Organisation { get; private set; }
    public Profile? InvitedByProfile { get; private set; }
    public Profile? SupervisedByProfile { get; private set; }

    public ICollection<Profile>? InvitedProfiles { get; private set; } = new List<Profile>();
    public ICollection<Profile>? SupervisedProfiles { get; private set; } = new List<Profile>();
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public Profile(
        Guid userId, 
        Guid orgId, 
        Guid roleId, 
        Guid? invitedBy, 
        Guid? supervisedBy
        )
    {
        ProfileId = Guid.NewGuid();
        UserId = userId;
        OrgId = orgId;
        RoleId = roleId;
        InvitedBy = invitedBy;
        SupervisedBy = supervisedBy;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void UpdateProfile(string? email, string? name, string?phone, string? avatar)
    {
        Email = string.IsNullOrWhiteSpace(email)?Email:email;
        Name = string.IsNullOrWhiteSpace(name)?Name:name;
        Phone = string.IsNullOrWhiteSpace(phone)?Phone:phone;
        Avatar = string.IsNullOrWhiteSpace(avatar)?Avatar:avatar;
        UpdatedAt = DateTime.UtcNow;
    }

    public void DisableProfile()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }
}