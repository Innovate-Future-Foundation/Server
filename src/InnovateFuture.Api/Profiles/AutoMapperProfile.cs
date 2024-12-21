using InnovateFuture.Api.Controllers.OrderController;
using InnovateFuture.Api.Controllers.OrdersController;
using InnovateFuture.Api.Controllers.UsersController;
using InnovateFuture.Application.Orders.Commands.CreateOrder;
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
    }
}