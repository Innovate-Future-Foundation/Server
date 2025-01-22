using System.Linq.Expressions;
using InnovateFuture.Application.Roles.Queries.GetRoles;
using InnovateFuture.Domain.Entities;
using InnovateFuture.Domain.Enums;
using InnovateFuture.Infrastructure.Roles.Persistence.Interfaces;
using Moq;

namespace InnovateFuture.Application.Tests.Roles;

public class GetRolesHandlerTest
{
    private readonly Mock<IRoleRepository> _mockedRoleRepository;
    private readonly GetRolesHandler _handler;

    public GetRolesHandlerTest()
    {
        _mockedRoleRepository = new Mock<IRoleRepository>();
        _handler = new GetRolesHandler(_mockedRoleRepository.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnAllRoles_WhenQueryIsEmpty()
    {
        // Arrange
        var query = new GetRolesQuery();
        var expectedRoles = new List<Role>
        {
            new Role ("Organisation Admin", RoleEnum.OrgAdmin,null ),
            new Role ("Organisation Teacher", RoleEnum.OrgTeacher,null ),
        };

        _mockedRoleRepository
            .Setup(repo => repo.GetAnyAsync(Role=>true))
            .ReturnsAsync(expectedRoles);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedRoles.Count, result.Count());
        _mockedRoleRepository.Verify(repo => repo.GetAnyAsync(It.Is<Expression<Func<Role, bool>>>(predicate => predicate.Compile()(expectedRoles.First()))), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldFilterRolesByEmail_WhenOnlyEmailIsProvided()
    {
        // Arrange
        var query = new GetRolesQuery { Name = "Organisation Admin" };
        _mockedRoleRepository
            .Setup(repo => repo.GetAnyAsync(It.IsAny<Expression<Func<Role, bool>>>()))
            .ReturnsAsync(new List<Role>());

        // Act
        await _handler.Handle(query, CancellationToken.None);

        // Assert
        _mockedRoleRepository.Verify(repo => repo.GetAnyAsync(It.Is<Expression<Func<Role, bool>>>(predicate =>
            predicate.Compile().Invoke(new Role ("Organisation Admin", RoleEnum.OrgAdmin,null)
               ) &&
            !predicate.Compile().Invoke( new Role ("Organisation Teacher", RoleEnum.OrgTeacher,null))
        )), Times.Once);
    }
    
    [Fact]
    public async Task Handle_ShouldFilterRolesByGivenNameNFamilyName_WhenMultipleMixedConditionsProvided()
    {
        // Arrange
        var query = new GetRolesQuery
        {
            Name = "Student",
            CodeName = RoleEnum.Student,
        };

        var correctRole = new Role("Student", RoleEnum.Student, null);
        var wrongRole = new Role("Organisation Teacher", RoleEnum.OrgTeacher, null);

        _mockedRoleRepository
            .Setup(repo => repo.GetAnyAsync(It.IsAny<Expression<Func<Role, bool>>>()))
            .ReturnsAsync(new List<Role>());

        // Act
        await _handler.Handle(query, CancellationToken.None);

        // Assert
        _mockedRoleRepository.Verify(repo => repo.GetAnyAsync(It.Is<Expression<Func<Role, bool>>>(predicate =>
            predicate.Compile().Invoke(correctRole) &&
            !predicate.Compile().Invoke(wrongRole)
        )), Times.Once);
    }
}