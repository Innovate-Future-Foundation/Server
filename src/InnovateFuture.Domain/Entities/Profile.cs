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
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public Profile() { } 
    public Profile(
        User user, 
        Role role, 
        Organisation organisation, 
        Profile? invitedByProfile, 
        Profile? supervisedByProfile
        )
    {
        ProfileId = Guid.NewGuid();
        User = user;
        UserId = user.UserId;
        Role = role;
        RoleId = role.RoleId;
        Organisation = organisation;
        OrgId = Organisation.OrgId;
        InvitedByProfile = invitedByProfile;
        SupervisedByProfile = supervisedByProfile;
        InvitedBy = invitedByProfile?.ProfileId;
        SupervisedBy = supervisedByProfile?.ProfileId;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
    public void UpdateProfile(string? email, string? name, string?phone, string? avatar,Boolean? isActive)
    {
        Email = string.IsNullOrWhiteSpace(email)?Email:email;
        Name = string.IsNullOrWhiteSpace(name)?Name:name;
        Phone = string.IsNullOrWhiteSpace(phone)?Phone:phone;
        Avatar = string.IsNullOrWhiteSpace(avatar)?Avatar:avatar;
        IsActive = isActive??IsActive;
        UpdatedAt = DateTime.UtcNow;
    }
}