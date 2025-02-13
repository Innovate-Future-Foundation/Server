using InnovateFuture.Application.Services.Auth.UserService;
using InnovateFuture.Application.Services.SendEmail;
using InnovateFuture.Domain.Entities;
using InnovateFuture.Domain.Enums;
using InnovateFuture.Infrastructure.Profiles.Persistence.Interfaces;
using InnovateFuture.Infrastructure.UnitOfWork.Persistence.Interface;
using MediatR;

namespace InnovateFuture.Application.Services.Auth.Register;

public class RegisterOrganisationAdminHandler : IRequestHandler<RegisterOrganisationAdminCommand, (Guid ProfileId, User User)?>
{
    private readonly IProfileRepository _profileRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserService _userService;
    
    public RegisterOrganisationAdminHandler(IProfileRepository profileRepository, IUnitOfWork unitOfWork, IUserService userService)
    {
        _profileRepository = profileRepository;
        _unitOfWork = unitOfWork;
        _userService = userService;
    }

    public async Task<(Guid ProfileId, User User)?> Handle(RegisterOrganisationAdminCommand command, CancellationToken cancellationToken)
    {
        var transactions = await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            // 1⃣️ Create Organisation (EF Core will track relationships automatically)
            var organisation = new Organisation(
                command.OrgName,
                null,
                command.LogoUrl,
                command.WebsiteUrl,
                command.Address,
                command.OrgEmail
            );
            
            // 2⃣️ Create Organisation Admin (User)
            var user = new User(
                userName: command.UserName,
                email: command.UserEmail
            );
            await _userService.CreateUserAsync(user, command.Password, cancellationToken);
            
            // 3⃣️ Create Organisation Admin Profile (EF Core handles relationships)
            var profile = new Profile(
                userId: user.Id,
                role: RoleEnum.OrgAdmin,
                orgId: organisation.Id,
                name: command.UserName,
                email: command.UserEmail
            );
            profile.AddUser(user);
            profile.AddOrganisation(organisation);
            user.UpdateDefaultProfile(profile.Id);
            await _profileRepository.AddAsync(profile, cancellationToken);

            // 5⃣️ Save Everything in One Transaction
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync();
            return (profile.Id, user);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR]: {ex.Message}");
            await _unitOfWork.RollbackTransactionAsync();
            return null;
        }
    }
}
