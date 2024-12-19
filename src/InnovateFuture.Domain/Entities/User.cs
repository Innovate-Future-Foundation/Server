using System.Text.RegularExpressions;
using InnovateFuture.Domain.Exceptions;

namespace InnovateFuture.Domain.Entities
{
    public class User
    {
        public Guid User_id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        // public string default_tenant { get; private set; }
        // public string default_profile { get; private set; }

        public User(string name, string email)
        {
            string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new IFDomainValidationException("User name is required.");
            }

            if (!Regex.IsMatch(email, emailPattern))
            {
                throw new IFDomainValidationException("Email is not valid.");
            }

            User_id = Guid.NewGuid();
            Name = name;
            Email = email;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}