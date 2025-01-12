using InnovateFuture.Domain.Enums;
using InnovateFuture.Infrastructure.Roles.Persistence.Interfaces;


namespace InnovateFuture.Application.Services;

public interface IAccessControlService
{
    Task<List<string>> GetPermissions(Guid roleId);
}

public class AccessControlService : IAccessControlService
{
    private readonly IRoleRepository _roleRepository;
    public AccessControlService(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    private readonly Dictionary<RoleEnum, List<RoleEnum>> _rolePermissions = new()
    {
        { RoleEnum.PlatformAdmin, new List<RoleEnum> { RoleEnum.Student, RoleEnum.OrgTeacher, RoleEnum.PlatformAdmin, RoleEnum.OrgAdmin, RoleEnum.Parent } },
        { RoleEnum.OrgAdmin, new List<RoleEnum> { RoleEnum.Student, RoleEnum.OrgTeacher, RoleEnum.OrgAdmin, RoleEnum.Parent } },
        { RoleEnum.OrgTeacher, new List<RoleEnum> { RoleEnum.Student, RoleEnum.OrgTeacher, RoleEnum.Parent } },
        { RoleEnum.Parent, new List<RoleEnum> { RoleEnum.Student, RoleEnum.Parent } },
        { RoleEnum.Student, new List<RoleEnum> { RoleEnum.Student } }
    };

    public async Task<List<string>> GetPermissions(Guid roleId)
    {
        // Fetch the role asynchronously
        var role = await _roleRepository.GetByIdAsync(roleId); // Make sure to await the async call

        if (role == null) throw new Exception("Role not found");

        // Check if role CodeName exists in the dictionary
        if (_rolePermissions.ContainsKey(role.CodeName))
        {
            //return _rolePermissions[role.CodeName]; // Return permissions from dictionary
            return _rolePermissions[role.CodeName].Select(r => r.ToString()).ToList();
        }

        // Optionally, if not found in dictionary, return role's permissions (assuming role has permissions defined)
        return new List<string>(); // Return an empty list if permissions are not defined
    }
}