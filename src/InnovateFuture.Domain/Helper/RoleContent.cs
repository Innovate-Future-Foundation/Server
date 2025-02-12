using InnovateFuture.Domain.Enums;

namespace InnovateFuture.Domain.Helper;

public static class RoleContent
{
    // Role Dictionary
    private static readonly Dictionary<RoleEnum, string> RoleMap = new()
    {
        { RoleEnum.PlatformAdmin, "Platform Admin" },
        { RoleEnum.OrgAdmin, "Admin" },
        { RoleEnum.OrgManager, "Manager" },
        { RoleEnum.OrgTeacher, "Teacher" },
        { RoleEnum.Parent, "Parent"},
        { RoleEnum.Student, "Student" },
    };

    // "Admin" -> OrgAdmin
    // convert string value to enum value
    public static RoleEnum GetRoleEnumFromStringName(string roleName)
    {
        var roleEntry = RoleMap.FirstOrDefault(x => x.Value.Equals(roleName, StringComparison.OrdinalIgnoreCase));
        if (string.IsNullOrEmpty(roleEntry.Value))
        {
            throw new ArgumentException($"Invalid role name: {roleName}");
        }
        return roleEntry.Key;
    }

    public static string GetRoleNameFromEnum(RoleEnum roleEnum)
    {
        var success = RoleMap.TryGetValue(roleEnum, out var roleName);
        if (!success)
        {
            throw new ArgumentException($"Invalid role enum: {roleEnum}");
        }
        return roleName;
    } 
}