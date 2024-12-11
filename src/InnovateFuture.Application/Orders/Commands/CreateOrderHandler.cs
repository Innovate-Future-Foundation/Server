using AutoMapper;
using MediatR;
using InnovateFuture.Domain.Entities;
using FluentValidation;
using InnovateFuture.Infrastructure.Persistence.Orders;

namespace InnovateFuture.Application.Orders.Commands
{


    public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, Guid>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateOrderCommand> _validator;
        public CreateOrderHandler(IOrderRepository orderRepository, IMapper mapper, IValidator<CreateOrderCommand> validator)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            // Use AutoMapper to map the request to the Order entity
            var order = _mapper.Map<Order>(request);

            // Add the order to the repository
            await _orderRepository.AddAsync(order);

            // Return the newly created order's ID
            return order.Id;
        }
    }
}

