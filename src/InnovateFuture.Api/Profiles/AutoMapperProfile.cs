using InnovateFuture.Api.Controllers.Auth;
using AutoMapper;
using InnovateFuture.Api.Controllers.ProfilesController;
using InnovateFuture.Api.Controllers.UsersController;
using InnovateFuture.Api.Controllers.OrganisationsController;
using InnovateFuture.Application.Common.Models;
using InnovateFuture.Application.Profiles.Commands.UpdateProfile;
using InnovateFuture.Application.Users.Commands.CreateUser;
using InnovateFuture.Application.Users.Commands.UpdateUser;
using InnovateFuture.Application.Users.Queries.GetUsers;
using InnovateFuture.Application.Organisations.Commands.CreateOrganisation;
using InnovateFuture.Application.Organisations.Commands.UpdateOrganisation;
using InnovateFuture.Application.Organisations.Queries.GetOrganisations;
using InnovateFuture.Application.Profiles.Queries.GetProfiles;
using InnovateFuture.Application.Services.Auth.ConfirmEmail;
using InnovateFuture.Domain.Entities;
using InnovateFuture.Domain.Enums;
using APIQueryProfileFilters = InnovateFuture.Api.Controllers.ProfilesController.QueryProfileFilters;
using APPQueryProfileFilters = InnovateFuture.Application.Profiles.Queries.GetProfiles.QueryProfileFilters;
using APIQueryOrganisationsFilters = InnovateFuture.Api.Controllers.OrganisationsController.QueryOrganisationsFilters;
using APPQueryOrganisationsFilters = InnovateFuture.Application.Organisations.Queries.GetOrganisations.QueryOrganisationsFilters;
using AMProfile = AutoMapper.Profile;
using Profile = InnovateFuture.Domain.Entities.Profile;


namespace InnovateFuture.Api.Profiles;

public class AutoMapperProfile: AMProfile
{
    public AutoMapperProfile()
    {
        CreateMap<ConfirmEmailRequest, ConfirmEmailCommand>();
        
        CreateMap<CreateUserRequest, CreateUserCommand>();
        /*
         * User
         */
        CreateMap<CreateUserRequest, CreateUserCommand>()
            .ForMember(dest=>dest.RoleEnum,opt=>opt.MapFrom<RoleCodeToRoleEnumResolver>());
        
        CreateMap<UpdateUserRequest, UpdateUserCommand>();

        CreateMap<QueryUsersRequest, GetUsersQuery>();

        CreateMap<User, GetUserResponse>();
        /*
         * Profile
         */
        CreateMap<QueryProfilesRequest, GetProfilesQuery>();
        CreateMap<APIQueryProfileFilters, 
                APPQueryProfileFilters>()
            .ForMember(dest => dest.RoleEnums, opt => opt.MapFrom<RoleCodesToRoleEnumsResolver>());
        
        CreateMap<Profile, GetProfileResponse>()
            .ForMember(dest => dest.RoleCode, opt => opt.MapFrom(src => src.Role.ToString()));

        CreateMap<Profile, GetProfileWithDetailsResponse>()
            .ForMember(dest => dest.RoleCode, opt => opt.MapFrom(src => src.Role.ToString()))
            .ForMember(dest => dest.InviterProfile, opt => opt.MapFrom(src => src.InviterProfile))
            .ForMember(dest => dest.SupervisorProfile, opt => opt.MapFrom(src => src.SupervisorProfile))
            .ForMember(dest => dest.Organisation, opt => opt.MapFrom(src => src.Organisation));
        
        CreateMap<(List<Profile> data, int totalItems), GetProfilePaginatedResponse>()
            .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.data))
            .ForMember(dest => dest.Meta, opt => opt.MapFrom(src => new Meta { TotalItems = src.totalItems }));
        
        CreateMap<(List<Profile> data, int totalItems), GetProfileWithDetailsPaginatedResponse>()
            .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.data))
            .ForMember(dest => dest.Meta, opt => opt.MapFrom(src => new Meta { TotalItems = src.totalItems }));
        
        CreateMap<UpdateProfileRequest, UpdateProfileCommand>();
        /*
         * Organisation
         */

        CreateMap<InnovateFuture.Domain.Entities.Profile, GetProfileResponse>();
        
        CreateMap<CreateOrganisationRequest, CreateOrganisationCommand>();
        
        CreateMap<QueryOrganisationsRequest, GetOrganisationsQuery>();
        
        CreateMap<APIQueryOrganisationsFilters,
                APPQueryOrganisationsFilters>();

        CreateMap<Organisation, GetOrganisationsResponse>();

        CreateMap<(List<Organisation> data, int totalItems), GetOrganisationPaginatedResponse>()
            .ForMember(desc=>desc.Data, opt=>opt.MapFrom(src=>src.data))
            .ForMember(desc=>desc.Meta,opt=>opt.MapFrom(src=>new Meta(){TotalItems = src.totalItems}));
        
        CreateMap<UpdateOrganisationRequest, UpdateOrganisationCommand>();
        
    }
}
public class RoleCodesToRoleEnumsResolver : IValueResolver<APIQueryProfileFilters, APPQueryProfileFilters, RoleEnum[]>
{
    public RoleEnum[] Resolve(APIQueryProfileFilters source, APPQueryProfileFilters destination, RoleEnum[] destMember,
        ResolutionContext context)
    {
        return source.RoleCodes?
            .Split(",")
            .Select(roleCode => Enum.TryParse<RoleEnum>(roleCode.Trim(), out var roleEnum) ? roleEnum : default)
            .Where(roleEnum => roleEnum != default)
            .ToArray()??[];
    }
}

public class RoleCodeToRoleEnumResolver : IValueResolver<CreateUserRequest, CreateUserCommand, RoleEnum>
{
    public RoleEnum Resolve(CreateUserRequest source, CreateUserCommand destination, RoleEnum destMember,
        ResolutionContext context)
    {
        return  Enum.TryParse<RoleEnum>(source.RoleCode.Trim(), out var roleEnum) ? roleEnum : default;
    }
}