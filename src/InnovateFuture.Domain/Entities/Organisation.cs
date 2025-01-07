using InnovateFuture.Domain.Enums;

namespace InnovateFuture.Domain.Entities;

public class Organisation
{
    public Guid OrgId { get; private set; }
    public string OrgName { get; private set; }
    public string? LogoUrl { get; private set; }
    public string? WebsiteUrl { get; private set; }
    public string? Address { get; private set; }
    public string? Email { get; private set; }
    public string? Subscription { get; private set; }
    public StatusEnum Status { get; private set; }
    
    public ICollection<Profile>? Profiles { get; private set; } = new List<Profile>();
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public Organisation(){}
    public Organisation(string orgName, Guid? orgId= null, string? logoUrl=null, string? websiteUrl=null, string? address=null, string? email=null, string? subscription=null)
    {
        OrgId = orgId??Guid.NewGuid();
        OrgName = orgName;
        LogoUrl = logoUrl;
        WebsiteUrl = websiteUrl;
        Address = address;
        Email = email;
        Subscription = subscription;
        Status = StatusEnum.Pending;// initial status
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateOrganisation(string? orgName, string? logoUrl, string? websiteUrl, string? address, string? email, string? subscription)
    {
        OrgName = string.IsNullOrWhiteSpace(orgName)?OrgName:orgName;
        LogoUrl = string.IsNullOrWhiteSpace(logoUrl)?logoUrl:LogoUrl;
        WebsiteUrl = string.IsNullOrWhiteSpace(websiteUrl)?websiteUrl:WebsiteUrl;
        Address = string.IsNullOrWhiteSpace(address)?address:Address;
        Email = string.IsNullOrWhiteSpace(email)?email:Email;
        Subscription = string.IsNullOrWhiteSpace(subscription)?subscription:Subscription;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ChangeStatus(StatusEnum status)
    {
        Status = status;
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