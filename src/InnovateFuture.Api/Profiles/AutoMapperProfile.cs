using InnovateFuture.Api.Controllers.Auth;
using AutoMapper;
using InnovateFuture.Api.Controllers.Organisations;
using InnovateFuture.Api.Controllers.Profiles;
using InnovateFuture.Api.Controllers.Users;
using InnovateFuture.Application.Auth.Commands.ConfirmEmail;
using InnovateFuture.Application.Auth.Commands.Login;
using InnovateFuture.Application.Auth.Commands.Register;
using InnovateFuture.Application.Auth.Commands.ResendVerificationEmail;
using InnovateFuture.Application.Common.Models;
using InnovateFuture.Application.Profiles.Commands.UpdateProfile;
using InnovateFuture.Application.Users.Commands.CreateUser;
using InnovateFuture.Application.Users.Commands.UpdateUser;
using InnovateFuture.Application.Users.Queries.GetUsers;
using InnovateFuture.Application.Organisations.Commands.CreateOrganisation;
using InnovateFuture.Application.Organisations.Commands.UpdateOrganisation;
using InnovateFuture.Application.Organisations.Queries.GetOrganisations;
using InnovateFuture.Application.Profiles.Queries.GetProfiles;
using InnovateFuture.Domain.Entities;
using InnovateFuture.Domain.Enums;
using APIQueryProfileFilters = InnovateFuture.Api.Controllers.Profiles.QueryProfileFilters;
using APPQueryProfileFilters = InnovateFuture.Application.Profiles.Queries.GetProfiles.QueryProfileFilters;
using APIQueryOrganisationsFilters = InnovateFuture.Api.Controllers.Organisations.QueryOrganisationsFilters;
using APPQueryOrganisationsFilters = InnovateFuture.Application.Organisations.Queries.GetOrganisations.QueryOrganisationsFilters;
using AMProfile = AutoMapper.Profile;
using Profile = InnovateFuture.Domain.Entities.Profile;




namespace InnovateFuture.Api.Profiles;
public class AutoMapperProfile: AMProfile
{
    public AutoMapperProfile()
    {
        CreateMap<ConfirmEmailRequest, ConfirmEmailCommand>();
        CreateMap<ResendVerficationEmailRequest,ResendVerificationEmailCommand>();
        CreateMap<RegisterOrganisationAdminRequest, RegisterOrganisationAdminCommand>();
        CreateMap<LoginRequest, LoginCommand>();

        CreateMap<Profile,GetMeResponse>()
            .ForMember(dest => dest.RoleCode, opt => opt.MapFrom(src => src.Role.ToString()));
        
        CreateMap<CreateUserRequest, CreateUserCommand>();
        /*
         * User
         */
        CreateMap<CreateUserRequest, CreateUserCommand>()
            .ForMember(dest => dest.RoleEnum,
                opt => opt.MapFrom<RoleCodeToRoleEnumResolver>());
        CreateMap<UpdateUserRequest, UpdateUserCommand>();

        CreateMap<QueryUsersRequest, GetUsersQuery>();

        CreateMap<User, GetUserResponse>();
        /*
         * Profile
         */
        CreateMap<QueryProfilesRequest, GetProfilesQuery>();
        
        CreateMap<UpdateProfileRequest, UpdateProfileCommand>();
        
        CreateMap<APIQueryProfileFilters, 
                APPQueryProfileFilters>()
            .ForMember(dest => dest.RoleEnums, opt => opt.MapFrom<RoleCodesToRoleEnumsResolver>())
            .ForMember(dest => dest.Supervisors, opt => opt.MapFrom(src => 
                RoleCodesToRoleEnumsResolver.ConvertStringToGuidArr(src.Supervisors??"")));

        CreateMap<Profile, GetProfileResponse>()
            .ForMember(dest => dest.RoleCode, opt => opt.MapFrom(src => src.Role.ToString()));
        
        CreateMap<Profile, GetProfileWithDetailsResponse>()
            .ForMember(dest => dest.RoleCode, opt => opt.MapFrom(src => src.Role.ToString()));
        
        CreateMap<(List<Profile> data, int totalItems), GetProfilePaginatedResponse>()
            .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.data))
            .ForMember(dest => dest.Meta, opt => opt.MapFrom(src => new Meta { TotalItems = src.totalItems }));
        
        CreateMap<(List<Profile> data, int totalItems), GetProfileWithDetailsPaginatedResponse>()
            .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.data))
            .ForMember(dest => dest.Meta, opt => opt.MapFrom(src => new Meta { TotalItems = src.totalItems }));
        /*
         * Organisation
         */
        CreateMap<Organisation, GetOrganisationsResponse>()
            .ForMember(dest=>dest.SubscriptionCode,opt=>opt.MapFrom(src=>src.Subscription.ToString()))
            .ForMember(dest=>dest.OrgStatusCode,opt=>opt.MapFrom(src=>src.OrgStatus.ToString()));
        
        CreateMap<CreateOrganisationRequest, CreateOrganisationCommand>();
        CreateMap<UpdateOrganisationRequest, UpdateOrganisationCommand>()
            .ForMember(dest => dest.SubscriptionEnum,
                opt => opt.MapFrom<UpdateOrgSubscriptionCodeToSubscriptionEnumResolver>())
            .ForMember(dest => dest.OrgStatusEnum,
                opt => opt.MapFrom<UpdateOrgOrgStatusCodeToOrgStatusEnumResolver>());

        CreateMap<QueryOrganisationsRequest, GetOrganisationsQuery>();
        
        CreateMap<APIQueryOrganisationsFilters,
                APPQueryOrganisationsFilters>()
            .ForMember(dest => dest.SubscriptionEnum,
                opt => opt.MapFrom<FiltersSubscriptionCodeToSubscriptionEnumResolver>())
            .ForMember(dest => dest.OrgStatusEnum,
                opt => opt.MapFrom<FiltersOrgStatusCodeToOrgStatusEnumResolver>());

        CreateMap<(List<Organisation> data, int totalItems), GetOrganisationPaginatedResponse>()
            .ForMember(desc=>desc.Data, opt=>opt.MapFrom(src=>src.data))
            .ForMember(desc=>desc.Meta,opt=>opt.MapFrom(src=>new Meta(){TotalItems = src.totalItems}));
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
    public static Guid[] ConvertStringToGuidArr(string combinedString=""){
        var res = combinedString.Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(s => s.Trim())
            .Where(s => Guid.TryParse(s, out _))
            .Select(Guid.Parse)
            .ToArray()??[];
        return res;
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

public class UpdateOrgSubscriptionCodeToSubscriptionEnumResolver : IValueResolver<UpdateOrganisationRequest, UpdateOrganisationCommand, SubscriptionEnum?>
{
    public SubscriptionEnum? Resolve(UpdateOrganisationRequest source, UpdateOrganisationCommand destination, SubscriptionEnum? destMember,
        ResolutionContext context)
    {
        return  Enum.TryParse<SubscriptionEnum>(source.SubscriptionCode?.Trim(), out var subscriptionEnum) ? subscriptionEnum : null;
    }
}
public class UpdateOrgOrgStatusCodeToOrgStatusEnumResolver : IValueResolver<UpdateOrganisationRequest, UpdateOrganisationCommand, OrgStatusEnum?>
{
    public OrgStatusEnum? Resolve(UpdateOrganisationRequest source, UpdateOrganisationCommand destination, OrgStatusEnum? destMember,
        ResolutionContext context)
    {
        return  Enum.TryParse<OrgStatusEnum>(source.OrgStatusCode?.Trim(), out var orgStatusEnum) ? orgStatusEnum : null;
    }
}

public class FiltersSubscriptionCodeToSubscriptionEnumResolver : IValueResolver<APIQueryOrganisationsFilters,
    APPQueryOrganisationsFilters, SubscriptionEnum?>
{
    public SubscriptionEnum? Resolve(APIQueryOrganisationsFilters source, APPQueryOrganisationsFilters destination, SubscriptionEnum? destMember,
        ResolutionContext context)
    {
        return  Enum.TryParse<SubscriptionEnum>(source.SubscriptionCode?.Trim(), out var subscriptionEnum) ? subscriptionEnum : null;
    }
}

public class FiltersOrgStatusCodeToOrgStatusEnumResolver : IValueResolver<APIQueryOrganisationsFilters,
    APPQueryOrganisationsFilters, OrgStatusEnum?>
{
    public OrgStatusEnum? Resolve(APIQueryOrganisationsFilters source, APPQueryOrganisationsFilters destination, OrgStatusEnum? destMember,
        ResolutionContext context)
    {
        return  Enum.TryParse<OrgStatusEnum>(source.OrgStatusCode?.Trim(), out var orgStatusEnum) ? orgStatusEnum : null;
    }
}
