using System;

public class Role
{
    public Guid RoleId { get; private set; }
    public string Name { get; private set; }
    public short CodeName { get; private set; }
    public string Description { get; private set; }

    private Role() { }

    public Role(string name, short codeName, string description)
    {
        RoleId = Guid.NewGuid();
        Name = name ?? throw new ArgumentNullException(nameof(name));
        CodeName = codeName;
        Description = description ?? throw new ArgumentNullException(nameof(description));
    }

    public void Update(string name, string description)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description ?? throw new ArgumentNullException(nameof(description));
    }
}
