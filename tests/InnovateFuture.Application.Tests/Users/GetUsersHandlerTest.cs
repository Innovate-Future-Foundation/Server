using System.Linq.Expressions;
using InnovateFuture.Application.Users.Queries.GetUsers;
using InnovateFuture.Domain.Entities;
using InnovateFuture.Infrastructure.Users.Persistence.Interfaces;
using Moq;

namespace InnovateFuture.Application.Tests.Users;

public class GetUsersHandlerTest
{
    private readonly Mock<IUserRepository> _mockedUserRepository;
    private readonly GetUsersHandler _handler;

    public GetUsersHandlerTest()
    {
        _mockedUserRepository = new Mock<IUserRepository>();
        _handler = new GetUsersHandler(_mockedUserRepository.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnAllUsers_WhenQueryIsEmpty()
    {
        // Arrange
        var query = new GetUsersQuery();
        var expectedUsers = new List<User>
        {
            new User (Guid.NewGuid(),"test1@example.com" ),
            new User (Guid.NewGuid(),"test2@example.com" ),
        };

        _mockedUserRepository
            .Setup(repo => repo.GetAnyAsync(user=>true))
            .ReturnsAsync(expectedUsers);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedUsers.Count, result.Count());
        _mockedUserRepository.Verify(repo => repo.GetAnyAsync(It.Is<Expression<Func<User, bool>>>(predicate => predicate.Compile()(expectedUsers.First()))), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldFilterUsersByEmail_WhenOnlyEmailIsProvided()
    {
        // Arrange
        var query = new GetUsersQuery { Email = "test@example.com" };
        _mockedUserRepository
            .Setup(repo => repo.GetAnyAsync(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync(new List<User>());

        // Act
        await _handler.Handle(query, CancellationToken.None);

        // Assert
        _mockedUserRepository.Verify(repo => repo.GetAnyAsync(It.Is<Expression<Func<User, bool>>>(predicate =>
            predicate.Compile().Invoke(new User (Guid.NewGuid(),"test@example.com" )
               ) &&
            !predicate.Compile().Invoke( new User (Guid.NewGuid(),"other@example.com" ))
        )), Times.Once);
    }
    
    [Fact]
    public async Task Handle_ShouldFilterUsersByGivenNameNFamilyName_WhenMultipleMixedConditionsProvided()
    {
        // Arrange
        var query = new GetUsersQuery
        {
            Email = "test@example.com",
            GivenName = "John",
            FamilyName = "Doe"
        };
        
        var correctUser = new User
            (Guid.NewGuid(),"test@example.com");
        correctUser.UpdateUserDetails(null,null,"John","Doe",null,null);
        var wrongUser = new User
            (Guid.NewGuid(),"wrong@example.com");
        wrongUser.UpdateUserDetails(null,null,"Mary","Green",null,null);

        _mockedUserRepository
            .Setup(repo => repo.GetAnyAsync(It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync(new List<User>());

        // Act
        await _handler.Handle(query, CancellationToken.None);

        // Assert
        _mockedUserRepository.Verify(repo => repo.GetAnyAsync(It.Is<Expression<Func<User, bool>>>(predicate =>
            predicate.Compile().Invoke(correctUser) &&
            !predicate.Compile().Invoke(wrongUser)
        )), Times.Once);
    }
}