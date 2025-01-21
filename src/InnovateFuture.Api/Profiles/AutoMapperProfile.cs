using AutoMapper;
using InnovateFuture.Api.Controllers.OrderController;
using InnovateFuture.Application.Orders.Commands.CreateOrder;
using InnovateFuture.Domain.Entities;

namespace InnovateFuture.Api.Profiles;

public class AutoMapperProfile: Profile
{
    public AutoMapperProfile()
    {
        CreateMap<CreateOrderRequest, CreateOrderCommand>();

        CreateMap<CreateOrderRequest.CreateOrderItem, CreateOrderCommand.CreateOrderItem>();

        CreateMap<Order,GetOrderResponse>();
    }
}