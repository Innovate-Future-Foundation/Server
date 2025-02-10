using InnovateFuture.Domain.Enums;

namespace InnovateFuture.Domain.Entities;

public class Organisation
{
    public Guid Id { get; private set; }
    public string OrgName { get; private set; }
    public string? LogoUrl { get; private set; }
    public string? WebsiteUrl { get; private set; }
    public string? Address { get; private set; }
    public string? Email { get; private set; }
    public SubscriptionEnum Subscription { get; private set; }
    public OrgStatusEnum OrgStatus { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    
    // Navigation
    public ICollection<Profile>? Profiles { get; private set; } = new List<Profile>();
    
    public Organisation(){}
    public Organisation(string orgName, Guid? id= null, string? logoUrl=null, string? websiteUrl=null, string? address=null, string? email=null)
    {
        Id = id??Guid.NewGuid();
        OrgName = orgName;
        LogoUrl = logoUrl;
        WebsiteUrl = websiteUrl;
        Address = address;
        Email = email;
        Subscription = SubscriptionEnum.Free;
        OrgStatus = OrgStatusEnum.Pending;// initial status
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateOrganisationDetails(string? orgName, string? logoUrl, string? websiteUrl, string? address, string? email, SubscriptionEnum? subscription, OrgStatusEnum? status)
    {
        OrgName = string.IsNullOrWhiteSpace(orgName) ? OrgName : orgName;
        LogoUrl = string.IsNullOrWhiteSpace(logoUrl) ? LogoUrl : logoUrl;
        WebsiteUrl = string.IsNullOrWhiteSpace(websiteUrl) ? WebsiteUrl : websiteUrl;
        Address = string.IsNullOrWhiteSpace(address) ? Address : address;
        Email = string.IsNullOrWhiteSpace(email) ? Email : email;
        Subscription = subscription?? Subscription;
        OrgStatus = status??OrgStatus;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ChangeStatus(OrgStatusEnum orgStatus)
    {
        OrgStatus = orgStatus;
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