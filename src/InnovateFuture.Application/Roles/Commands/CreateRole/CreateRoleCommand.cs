using MediatR;

namespace InnovateFuture.Application.Roles.Commands.CreateRole
{
    public class CreateRoleCommand : IRequest<Guid>
    {
        public string Name { get; set; }
        public short CodeName { get; set; }
        public string Description { get; set; }
    }
}
