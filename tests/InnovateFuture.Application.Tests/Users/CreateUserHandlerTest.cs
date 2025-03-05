using InnovateFuture.Application.Users.Commands.CreateUser;
using InnovateFuture.Domain.Entities;
using InnovateFuture.Domain.Enums;
using InnovateFuture.Infrastructure.Organisations.Persistence.Interfaces;
using InnovateFuture.Infrastructure.Profiles.Persistence.Interfaces;
using InnovateFuture.Infrastructure.Users.Persistence.Interfaces;
using Moq;

namespace InnovateFuture.Application.Tests.Users;
public class CreateUserHandlerTest
{
    [Fact]
    public async Task Handle_CreatesUserAndProfileSuccessfully()
    {
        // Arrange
        // var mockedUserRepository = new Mock<IUserRepository>();
        // var mockedOrgRepository = new Mock<IOrgRepository>();
        // var mockedProfileRepository = new Mock<IProfileRepository>();
        // var handler = new CreateUserHandler(mockedUserRepository.Object,mockedOrgRepository.Object,mockedProfileRepository.Object);
        // var command = new CreateUserCommand
        // {
        //     Email = "test@example.com",
        //     RoleEnum = RoleEnum.Parent,
        //     OrgId = Guid.NewGuid(),
        // };
        // mockedOrgRepository.Setup(o=>o.GetByIdAsync(command.OrgId))
        //     .ReturnsAsync(new Organisation("org_name_test01"));
        // mockedUserRepository.Setup(u => u.AddAsync(It.IsAny<User>()))
        //     .Returns(Task.CompletedTask);
        //
        // // Act
        // var userId = await handler.Handle(command, CancellationToken.None);
        //
        // // Assert
        // Assert.NotEqual(Guid.Empty, userId);
        // mockedOrgRepository.Verify(o => o.GetByIdAsync(command.OrgId), Times.Once);
        //
        // mockedUserRepository.Verify(u => u.AddAsync(It.Is<User>(user =>
        //     user.Email == command.Email &&
        //     user.Profiles.Count == 1 &&
        //     user.Profiles.First().UserId == user.Id &&
        //     user.Profiles.First().Id == user.DefaultProfileId 
        // )), Times.Once);
    }
}