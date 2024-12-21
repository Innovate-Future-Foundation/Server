using InnovateFuture.Domain.Enums;
using InnovateFuture.Domain.Exceptions;

namespace InnovateFuture.Domain.Entities;

public class Role
{
    public Guid RoleId { get; private set; }
    public string Name { get; private set; }
    public RoleEnum CodeName { get;private set; }
    public string? Description { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public ICollection<Profile>? Profiles { get; private set; } = new List<Profile>();

    public Role(string name, RoleEnum codeName, string? description)
    {
        RoleId = Guid.NewGuid();
        Name = name;
        CodeName = codeName;
        Description = description;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
}