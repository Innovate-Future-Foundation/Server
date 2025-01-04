
namespace InnovateFuture.Domain.Entities;

public class User
{
    public Guid UserId { get; private set; }
    public string Email { get; private set; }
    public Guid? CognitoUuid { get; private set; }
    public Guid? DefaultProfile { get; private set; }
    public string? FullName { get; private set; }
    public string? Phone { get; private set; }
    public DateTime? Birthday { get; private set; }
    // navigation properties
    public ICollection<Profile>? Profiles { get; private set; } = new List<Profile>();
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public User(string email,string? fullName, string? phone, DateTime? birthday)
    {
        UserId = Guid.NewGuid();
        Email = email;
        FullName = fullName;
        Phone = phone;
        Birthday = birthday;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
    public void UpdateDefaultProfile(Guid updatedDefaultProfile)
    {
        DefaultProfile = updatedDefaultProfile;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void UpdateUserDetails(Guid? cognitoUuid, Guid? updatedDefaultProfile, string? email, string? fullName, string? phone, DateTime? birthday)
    {
        CognitoUuid = cognitoUuid??CognitoUuid;
        DefaultProfile = updatedDefaultProfile??DefaultProfile;
        Email = string.IsNullOrWhiteSpace(email)?Email:email;
        FullName = string.IsNullOrWhiteSpace(fullName)?fullName:FullName;
        Phone = string.IsNullOrWhiteSpace(phone)?Phone:phone;
        Birthday = birthday??Birthday;
        UpdatedAt = DateTime.UtcNow;
    }
    public void AddProfile(Profile profile)
    {
        if (profile == null)
        {
            // to programmer
            throw new ArgumentNullException(nameof(profile), "Profile cannot be null.");
        }
        Profiles?.Add(profile);
    }
}