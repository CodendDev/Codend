using Bogus;
using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Core.Abstractions.Services;
using Codend.Application.Projects.Commands.AddMember;
using Codend.Contracts.Responses;
using Codend.Domain.Core.Errors;
using Codend.Domain.Core.Primitives;
using Codend.Domain.Entities;
using Codend.Domain.Repositories;
using FluentAssertions;
using Moq;

namespace Codend.UnitTests.Application.Projects;

public class AddMemberCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<IProjectRepository> _projectRepository = new();
    private readonly Mock<IProjectMemberRepository> _projectMemberRepository = new();
    private readonly Mock<IUserService> _userService = new();

    private readonly AddMemberCommandHandler _instance;

    public AddMemberCommandHandlerTests()
    {
        _instance = new AddMemberCommandHandler(
            _unitOfWork.Object,
            _projectRepository.Object,
            _projectMemberRepository.Object,
            _userService.Object);
    }

    [Fact]
    public async Task Handle_WhenValidCommandPassed_ShouldAddMemberToProject()
    {
        // Arrange
        var faker = new Faker();
        var userDetails = new UserDetails(faker.Random.Guid(), faker.Person.FirstName, faker.Person.LastName,
            faker.Person.Email, faker.Internet.Url());
        var command = new AddMemberCommand(
            faker.Random.Guid().GuidConversion<ProjectId>(),
            faker.Person.Email
        );
        var project = Project.Create(userDetails.Id.GuidConversion<UserId>(), faker.Random.Word()).Value;
        _projectRepository.Setup(r => r.GetByIdAsync(command.ProjectId)).ReturnsAsync(project);
        _projectMemberRepository.Setup(r => r.GetProjectMembersCount(command.ProjectId))
            .ReturnsAsync(Project.MaxMembersCount - 1);
        _userService.Setup(r => r.GetUserByEmailAsync(command.Email)).ReturnsAsync(userDetails);
        _projectMemberRepository.Setup(r => r.IsProjectMember(project.OwnerId, project.Id, CancellationToken.None))
            .ReturnsAsync(true);

        // Act
        var result = await _instance.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _projectRepository.Verify(r => r.Update(It.IsAny<Project>()), Times.Once);
        _unitOfWork.Verify(u => u.SaveChangesAsync(CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenValidCommandPassedButProjectMembersMaxCountReached_ShouldReturnAnError()
    {
        // Arrange
        var faker = new Faker();
        var userDetails = new UserDetails(faker.Random.Guid(), faker.Person.FirstName, faker.Person.LastName,
            faker.Person.Email, faker.Internet.Url());
        var command = new AddMemberCommand(
            faker.Random.Guid().GuidConversion<ProjectId>(),
            faker.Person.Email
        );
        var project = Project.Create(userDetails.Id.GuidConversion<UserId>(), faker.Random.Word()).Value;
        _projectRepository.Setup(r => r.GetByIdAsync(command.ProjectId)).ReturnsAsync(project);
        _projectMemberRepository.Setup(r => r.GetProjectMembersCount(command.ProjectId))
            .ReturnsAsync(Project.MaxMembersCount);

        // Act
        var result = await _instance.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.First().Should().BeOfType<DomainErrors.Project.ProjectHasMaximumNumberOfMembers>();
        _projectRepository.Verify(r => r.Update(It.IsAny<Project>()), Times.Never);
        _unitOfWork.Verify(u => u.SaveChangesAsync(CancellationToken.None), Times.Never);
    }
}