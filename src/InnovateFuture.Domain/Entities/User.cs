
namespace InnovateFuture.Domain.Entities;

public class User
{
    public Guid UserId { get; private set; }
    public Guid CognitoUuid { get; private set; }
    public string Email { get; private set; }
    public Guid? DefaultProfile { get; private set; }
    public string? GivenName { get; private set; }
    public string? FamilyName { get; private set; }
    public string? Phone { get; private set; }
    public DateTime? Birthday { get; private set; }
    // navigation properties
    public ICollection<Profile>? Profiles { get; private set; } = new List<Profile>();
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    // todo: common usage about ...
    public User(Guid cognitoUuid, string email)
    {
        UserId = Guid.NewGuid();
        CognitoUuid = cognitoUuid;
        Email = email;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
    public void UpdateDefaultProfile(Guid updatedDefaultProfile)
    {
        DefaultProfile = updatedDefaultProfile;
        UpdatedAt = DateTime.UtcNow;
    }
    public void UpdateUserDetails(Guid? updatedDefaultProfile, string? email, string? givenName, string? familyName,  string? phone, DateTime? birthday)
    {
        DefaultProfile = updatedDefaultProfile??DefaultProfile;
        Email = string.IsNullOrWhiteSpace(email)?Email:email;
        GivenName = string.IsNullOrWhiteSpace(givenName)?GivenName:givenName;
        FamilyName =  string.IsNullOrWhiteSpace(familyName)?FamilyName:familyName;
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