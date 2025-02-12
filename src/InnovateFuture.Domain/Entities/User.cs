using Microsoft.AspNetCore.Identity;

namespace InnovateFuture.Domain.Entities;

public class User: IdentityUser<Guid>
{
    public Guid? DefaultProfileId { get; private set; }
    public Guid? IdpSubject { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    
    // Navigation
    public virtual ICollection<Profile>? Profiles { get; private set; } = new List<Profile>();

    public User()
    {
    }

    public User(
        string userName,
        string email,
        Guid? defaultProfileId=null,
        Guid? idpSubject = null
        )
    {
        UserName = userName ?? throw new ArgumentNullException(nameof(userName));
        Email = email ?? throw new ArgumentNullException(nameof(email));
        DefaultProfileId = defaultProfileId;
        IdpSubject = idpSubject;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateProfile(Guid profileId)
    {
        DefaultProfileId = profileId;
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