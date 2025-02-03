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
using InnovateFuture.Application.Profiles.Queries.GetProfiles;
using QueryProfileFilters = InnovateFuture.Application.Profiles.Queries.GetProfiles.QueryProfileFilters;

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
        
        CreateMap<QueryOrganisationsRequest, GetOrganisationsQuery>();
        
        CreateMap<InnovateFuture.Api.Controllers.OrganisationsController.QueryOrganisationsFilters,
                QueryOrganisationsFilters>();

        CreateMap<Organisation, GetOrganisationsResponse>();

        CreateMap<(List<Organisation> data, int totalItems), GetOrganisationPaginatedResponse>()
            .ForMember(desc=>desc.Data, opt=>opt.MapFrom(src=>src.data))
            .ForMember(desc=>desc.Meta,opt=>opt.MapFrom(src=>new Meta(){TotalItems = src.totalItems}));
        
        CreateMap<UpdateOrganisationRequest, UpdateOrganisationCommand>();

        CreateMap<QueryProfilesRequest, GetProfilesQuery>();
        CreateMap<QueryProfileFilters, 
            InnovateFuture.Application.Profiles.Queries.GetProfiles.QueryProfileFilters>();
        CreateMap<(List<Profile> data, int totalItems), GetProfilePaginatedResponse>()
            .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.data))
            .ForMember(dest => dest.Meta, opt => opt.MapFrom(src => new Meta { TotalItems = src.totalItems }));
        
        CreateMap<(List<Profile> data, int totalItems), GetProfileWithDetailsPaginatedResponse>()
            .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.data))
            .ForMember(dest => dest.Meta, opt => opt.MapFrom(src => new Meta { TotalItems = src.totalItems }));

        CreateMap<Profile, GetProfileResponse>();

        CreateMap<Profile, GetProfileWithDetailsResponse>()
            .ForMember(dest => dest.Inviter, opt => opt.MapFrom(src => src.InviterProfile))
            .ForMember(dest => dest.Supervisor, opt => opt.MapFrom(src => src.SupervisorProfile))
            .ForMember(dest => dest.Org, opt => opt.MapFrom(src => src.Organisation))
            .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.RoleName));
    }

}