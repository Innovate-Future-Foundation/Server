using InnovateFuture.Api.Controllers.ProfilesController;
using InnovateFuture.Api.Controllers.RolesController;
using InnovateFuture.Api.Controllers.UsersController;
using InnovateFuture.Application.Profiles.Commands.UpdateProfile;
using InnovateFuture.Application.Roles.Queries.GetRoles;
using InnovateFuture.Application.Users.Commands.CreateUser;
using InnovateFuture.Application.Users.Commands.UpdateUser;
using InnovateFuture.Application.Users.Queries.GetUsers;
using InnovateFuture.Domain.Entities;
using Profile = AutoMapper.Profile;

namespace InnovateFuture.Api.Profiles;

public class AutoMapperProfile: Profile
{
    public AutoMapperProfile()
    {
        CreateMap<CreateUserRequest, CreateUserCommand>();
        
        CreateMap<UpdateUserRequest, UpdateUserCommand>();

        CreateMap<QueryUsersRequest, GetUsersQuery>();

        CreateMap<User, GetUserResponse>();
        
        CreateMap<UpdateProfileRequest, UpdateProfileCommand>();

        CreateMap<InnovateFuture.Domain.Entities.Profile, GetProfileResponse>();
        
        CreateMap<QueryRolesRequest, GetRolesQuery>();
        
        CreateMap<Role,GetRoleResponse>();
    }
}