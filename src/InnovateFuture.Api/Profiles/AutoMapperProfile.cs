using InnovateFuture.Api.Controllers.OrderController;
using InnovateFuture.Api.Controllers.OrdersController;
using InnovateFuture.Api.Controllers.ProfilesController;
using InnovateFuture.Api.Controllers.RolesController;
using InnovateFuture.Api.Controllers.UsersController;
using InnovateFuture.Application.Orders.Commands.CreateOrder;
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
        CreateMap<CreateOrderRequest, CreateOrderCommand>();

        CreateMap<CreateOrderRequest.CreateOrderItem, CreateOrderCommand.CreateOrderItem>();

        CreateMap<Order,GetOrderResponse>();
        
        CreateMap<CreateUserRequest, CreateUserCommand>();
        
        CreateMap<UpdateUserRequest, UpdateUserCommand>();

        CreateMap<QueryUsersRequest, GetUsersQuery>();

        CreateMap<User, GetUserResponse>();
        
        CreateMap<UpdateProfileRequest, UpdateProfileCommand>();

        CreateMap<InnovateFuture.Domain.Entities.Profile, GetProfileResponse>()
            .ForMember(dest => dest.Role,
                opt => opt.MapFrom(
                    src => (src.Role.Name)))
            .ForMember(dest => dest.OrgName,
                opt => opt.MapFrom(
                    src => (src.Organisation.OrgName)))
            .ForMember(dest => dest.InvitedBy, opt => opt.MapFrom(
                src => (src.InvitedByProfile != null ? src.InvitedByProfile.Name : null)
            ))
            .ForMember(dest => dest.SupervisedBy, opt => opt.MapFrom(
                src => (src.SupervisedByProfile != null ? src.SupervisedByProfile.Name : null)
            ));
        CreateMap<QueryRolesRequest, GetRolesQuery>();
        CreateMap<Role,GetRoleResponse>();
    }
}