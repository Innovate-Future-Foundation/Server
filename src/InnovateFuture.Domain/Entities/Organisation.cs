using InnovateFuture.Domain.Enums;

namespace InnovateFuture.Domain.Entities;
public class Address
{
    public string? Street { get; private set; }
    public string? Suburb { get; private set; }
    public string? State { get; private set; }
    public string? PostCode { get;private set; }
    public string? Country { get; private set; }
}
public class Organisation
{
    public Guid Id { get; private set; }
    public string OrgName { get; private set; }
    public string? LogoUrl { get; private set; }
    public string? WebsiteUrl { get; private set; }
    public Address? Address { get; private set; }
    public string? Email { get; private set; }
    public SubscriptionEnum Subscription { get; private set; }
    public OrgStatusEnum OrgStatus { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    
    // Navigation
    public ICollection<Profile>? Profiles { get; private set; } = new List<Profile>();
    public ICollection<Activity>? Activities { get; private set; } = new List<Activity>();
    public ICollection<Day>? Days { get; private set; } = new List<Day>();
    public ICollection<Tour>? Tours { get; private set; } = new List<Tour>();
    public Organisation(){}
    public Organisation(string orgName, Guid? id = null, string? logoUrl = null, string? websiteUrl = null, Address? address = null, string? email = null)
    {
        Id = id ?? Guid.NewGuid();
        OrgName = orgName;
        LogoUrl = string.IsNullOrWhiteSpace(logoUrl) ? null : logoUrl;
        WebsiteUrl = string.IsNullOrWhiteSpace(websiteUrl) ? null : websiteUrl;
        Email = string.IsNullOrWhiteSpace(email) ? null : email;
        Address = address;
        Subscription = SubscriptionEnum.Free;
        OrgStatus = OrgStatusEnum.Pending; // Initial status
        CreatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);
        UpdatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);
    }

    public void UpdateOrganisationDetails(string? orgName, string? logoUrl, string? websiteUrl, Address? address, string? email, SubscriptionEnum? subscription, OrgStatusEnum? status)
    {
        OrgName = string.IsNullOrWhiteSpace(orgName) ? OrgName : orgName;
        LogoUrl = string.IsNullOrWhiteSpace(logoUrl) ? LogoUrl : logoUrl;
        WebsiteUrl = string.IsNullOrWhiteSpace(websiteUrl) ? WebsiteUrl : websiteUrl;
        Address = address ?? Address;
        Email = string.IsNullOrWhiteSpace(email) ? Email : email;
        Subscription = subscription?? Subscription;
        OrgStatus = status??OrgStatus;
        UpdatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);
    }

    public void ChangeStatus(OrgStatusEnum orgStatus)
    {
        OrgStatus = orgStatus;
        UpdatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);
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

    public void AddTour(Tour tour)
    {
        if (tour == null)
        {
            // to programmer
            throw new ArgumentNullException(nameof(tour), "Tour cannot be null.");
        }
        Tours?.Add(tour);
    }
    public void AddDay(Day day)
    {
        if (day == null)
        {
            // to programmer
            throw new ArgumentNullException(nameof(day), "Day cannot be null.");
        }
        Days?.Add(day);
    }
    public void AddActivity(Activity activity)
    {
        if (activity == null)
        {
            // to programmer
            throw new ArgumentNullException(nameof(activity), "Activity cannot be null.");
        }
        Activities?.Add(activity);
    }
}