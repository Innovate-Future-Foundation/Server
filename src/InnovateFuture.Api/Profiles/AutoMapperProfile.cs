using InnovateFuture.Api.Controllers.ProfilesController;
using InnovateFuture.Api.Controllers.RolesController;
using InnovateFuture.Api.Controllers.UsersController;
using InnovateFuture.Api.Controllers.OrganisationsController;
using InnovateFuture.Application.Common.Models;
using InnovateFuture.Application.Profiles.Commands.UpdateProfile;
using InnovateFuture.Application.Roles.Queries.GetRoles;
using InnovateFuture.Application.Users.Commands.CreateUser;
using InnovateFuture.Application.Users.Commands.UpdateUser;
using InnovateFuture.Application.Users.Queries.GetUsers;
using InnovateFuture.Application.Organisations.Commands.CreateOrganisation;
using InnovateFuture.Application.Organisations.Commands.UpdateOrganisation;
using InnovateFuture.Application.Organisations.Queries.GetOrganisations;
using InnovateFuture.Domain.Entities;
using Profile = AutoMapper.Profile;
using QueryOrganisationsFilters = InnovateFuture.Application.Organisations.Queries.GetOrganisations.QueryOrganisationsFilters;

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

        CreateMap<CreateOrganisationRequest, CreateOrganisationCommand>();

        CreateMap<Organisation, GetOrganisationResponse>()
            .ForMember(des => des.Subscription,
                opt => opt.MapFrom(src => src.Subscription.HasValue ? src.Subscription.Value.ToString() : null))
            .ForMember(des => des.Status,
                opt => opt.MapFrom(src => src.Status.ToString()));
        
        CreateMap<QueryOrganisationsRequest, GetOrganisationsQuery>();

        CreateMap<PaginatedResult<Organisation>,PaginatedResult<GetOrganisationResponse>>();
        
        CreateMap<InnovateFuture.Api.Controllers.OrganisationsController.QueryOrganisationsFilters,
            QueryOrganisationsFilters>();

        CreateMap<UpdateOrganisationRequest, UpdateOrganisationCommand>();
    }
}