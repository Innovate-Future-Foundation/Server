namespace InnovateFuture.Domain.Entities
{
    /// <summary>
    /// Represents an organisation in the system
    /// </summary>
    public class Organisation
    {
        // Primary key for the organisation
        public Guid OrgId { get; private set; }
    
        // Basic organisation information
        public string OrgName { get; private set; }
        public string LogoUrl { get; private set; } = string.Empty;
        public string WebsiteUrl { get; private set; } = string.Empty;
        public string Address { get; private set; } = string.Empty;
        public string Email { get; private set; }
        public string Subscription { get; private set; } = string.Empty;
        public OrganisationStatus Status { get; private set; }
        public DateTime CreatedDate { get; private set; }

        /// <summary>
        /// Creates a new organisation with required basic information
        /// </summary>
        /// <param name="orgName">The name of the organisation</param>
        /// <param name="email">The email address of the organisation</param>
        /// <exception cref="ArgumentException">Thrown when name or email is empty</exception>
        public Organisation(string orgName, string email)
        {
            // Generate a new unique identifier
            OrgId = Guid.NewGuid();
            
            if (string.IsNullOrWhiteSpace(orgName))
                throw new ArgumentException("Organisation name is required.", nameof(orgName));
                
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email is required.", nameof(email));

            OrgName = orgName;
            Email = email;
            Status = OrganisationStatus.Active;
            CreatedDate = DateTime.UtcNow;
        }

        /// <summary>
        /// Updates the organisation's profile information
        /// </summary>
        public void UpdateProfile(string logoUrl, string websiteUrl, string address)
        {
            LogoUrl = logoUrl ?? string.Empty;
            WebsiteUrl = websiteUrl ?? string.Empty;
            Address = address ?? string.Empty;
        }

        /// <summary>
        /// Updates the organisation's subscription information
        /// </summary>
        public void UpdateSubscription(string subscription)
        {
            Subscription = subscription ?? string.Empty;
        }

        /// <summary>
        /// Updates the organisation's status
        /// </summary>
        public void UpdateStatus(OrganisationStatus status)
        {
            Status = status;
        }

        /// <summary>
        /// Updates the organisation's basic information
        /// </summary>
        public void Update(string orgName, string email, string subscription)
        {
            if (!string.IsNullOrWhiteSpace(orgName))
            {
                OrgName = orgName;
            }
            
            if (!string.IsNullOrWhiteSpace(email))
            {
                Email = email;
            }
            
            if (!string.IsNullOrWhiteSpace(subscription))
            {
                Subscription = subscription;
            }
        }
    }

    /// <summary>
    /// Represents the possible states of an organisation
    /// </summary>
    public enum OrganisationStatus
    {
        /// <summary>
        /// Organisation is active and can use the system
        /// </summary>
        Active,

        /// <summary>
        /// Organisation is inactive and cannot use the system
        /// </summary>
        Inactive,

        /// <summary>
        /// Organisation is suspended due to policy violations or other reasons
        /// </summary>
        Suspended
    }
}