using Bogus;
using Codend.Application.Core.Abstractions.Authentication;
using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Core.Abstractions.Services;
using Codend.Application.Projects.Commands.AddMember;
using Codend.Application.ProjectTasks.Commands.AssignUser;
using Codend.Contracts.Responses;
using Codend.Domain.Core.Enums;
using Codend.Domain.Core.Errors;
using Codend.Domain.Core.Primitives;
using Codend.Domain.Entities;
using Codend.Domain.Repositories;
using FluentAssertions;
using Moq;

namespace Codend.UnitTests.Application.ProjectTasks;

public class AssignUserCommandHandlerTests
{
    private readonly Mock<IProjectTaskRepository> _taskRepository = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<IProjectMemberRepository> _projectMemberRepository = new();
    private readonly Mock<IHttpContextProvider> _contextProvider = new();

    private readonly AssignUserCommandHandler _instance;

    public AssignUserCommandHandlerTests()
    {
        _instance = new AssignUserCommandHandler(
            _taskRepository.Object,
            _unitOfWork.Object,
            _projectMemberRepository.Object,
            _contextProvider.Object);
    }

    [Fact]
    public async Task Handle_WhenValidCommandPassed_ShouldAssignUserToTask()
    {
        // Arrange
        var faker = new Faker();
        var userId = faker.Random.Guid().GuidConversion<UserId>();
        var command = new AssignUserCommand(
            faker.Random.Guid().GuidConversion<ProjectId>(),
            faker.Random.Guid().GuidConversion<ProjectTaskId>(),
            faker.Random.Guid().GuidConversion<UserId>()
        );
        var task = BaseProjectTask
            .Create(
                new BaseProjectTaskCreateProperties(command.ProjectId, faker.Random.Word(),
                    ProjectTaskPriority.Normal.Name, null, null, null, null, null, null),
                faker.Random.Guid().GuidConversion<UserId>()).Value;
        var assignerProjectMember = ProjectMember.Create(command.ProjectId,
            userId).Value;
        var assigneeProjectMember = ProjectMember.Create(command.ProjectId,
            command.AssigneeId).Value;
        _contextProvider.Setup(c => c.UserId).Returns(userId);
        _projectMemberRepository
            .Setup(r => r.GetByProjectAndMemberId(command.ProjectId, userId, CancellationToken.None))
            .ReturnsAsync(assignerProjectMember);
        _projectMemberRepository
            .Setup(r => r.GetByProjectAndMemberId(command.ProjectId, command.AssigneeId, CancellationToken.None))
            .ReturnsAsync(assigneeProjectMember);
        _taskRepository.Setup(r => r.GetByIdAsync(command.ProjectTaskId, CancellationToken.None)).ReturnsAsync(task);

        // Act
        var result = await _instance.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        task.AssigneeId.Should().BeEquivalentTo(command.AssigneeId);
        _unitOfWork.Verify(u => u.SaveChangesAsync(CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenInvalidCommandPassedWithAssigneeNotBeingProjectMember_ShouldReturnAnError()
    {
        // Arrange
        var faker = new Faker();
        var userId = faker.Random.Guid().GuidConversion<UserId>();
        var command = new AssignUserCommand(
            faker.Random.Guid().GuidConversion<ProjectId>(),
            faker.Random.Guid().GuidConversion<ProjectTaskId>(),
            faker.Random.Guid().GuidConversion<UserId>()
        );
        var assignerProjectMember = ProjectMember.Create(command.ProjectId,
            userId).Value;
        _contextProvider.Setup(c => c.UserId).Returns(userId);
        _projectMemberRepository
            .Setup(r => r.GetByProjectAndMemberId(command.ProjectId, userId, CancellationToken.None))
            .ReturnsAsync(assignerProjectMember);
        _projectMemberRepository
            .Setup(r => r.GetByProjectAndMemberId(command.ProjectId, command.AssigneeId, CancellationToken.None))
            .ReturnsAsync((ProjectMember?)null);

        // Act
        var result = await _instance.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.First().Should().BeOfType<DomainErrors.ProjectTaskErrors.InvalidAssigneeId>();
        _unitOfWork.Verify(u => u.SaveChangesAsync(CancellationToken.None), Times.Never);
    }
}