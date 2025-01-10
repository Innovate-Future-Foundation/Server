using MediatR;
using InnovateFuture.Domain.Entities;
using InnovateFuture.Infrastructure.Users.Persistence.Interfaces;

namespace InnovateFuture.Application.Users.Commands.UpdateUser;
public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, Guid>
{
    private readonly IUserRepository _UserRepository;
    public UpdateUserHandler(IUserRepository UserRepository)
    {
        _UserRepository = UserRepository;
    }

    public async Task<Guid> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
    { 
        // get existing user by id
        User user = await _UserRepository.GetByIdAsync(command.UserId);
        
        // update
        user.UpdateUserDetails(
            command.DefaultProfile, 
            command.Email,
            command.GivenName, 
            command.FamilyName,
            command.Phone,
            command.Birthday
            );
        
        await _UserRepository.UpdateAsync();
        
        return user.UserId;
    }
}

