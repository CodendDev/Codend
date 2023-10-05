using Bogus;
using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Epics.Commands.CreateEpic;
using Codend.Domain.Core.Errors;
using Codend.Domain.Core.Primitives;
using Codend.Domain.Entities;
using Codend.Domain.Repositories;
using FluentAssertions;
using Moq;

namespace Codend.UnitTests.Application.CommandHandlers.Epics;

public class CreateEpicCommandHandlerTests
{
    private readonly Mock<IEpicRepository> _epicRepository = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<IProjectRepository> _projectRepository = new();
    private readonly Mock<IProjectTaskStatusRepository> _statusRepository = new();
    private readonly CreateEpicCommandHandler _instance;

    public CreateEpicCommandHandlerTests()
    {
        _instance = new CreateEpicCommandHandler(
            _epicRepository.Object,
            _unitOfWork.Object,
            _projectRepository.Object,
            _statusRepository.Object);
    }

    [Fact]
    public async Task Handle_WhenValidCommandPassed_ShouldPersistInEpicRepository()
    {
        // Arrange
        var faker = new Faker();
        var command = new CreateEpicCommand(
            faker.Random.Word(),
            faker.Lorem.Paragraph(),
            faker.Random.Guid().GuidConversion<ProjectId>(),
            faker.Random.Guid().GuidConversion<ProjectTaskStatusId>()
        );
        var project = Project.Create(faker.Random.Guid().GuidConversion<UserId>(), faker.Random.Word()).Value;
        _projectRepository
            .Setup(r => r.GetByIdAsync(command.ProjectId)).ReturnsAsync(project);
        _statusRepository
            .Setup(r => r.StatusExistsWithStatusIdAsync(command.StatusId!, command.ProjectId, CancellationToken.None))
            .ReturnsAsync(true);

        // Act
        var result = await _instance.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _epicRepository.Verify(r => r.Add(It.IsAny<Epic>()), Times.Once);
        _unitOfWork.Verify(u => u.SaveChangesAsync(CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenValidCommandPassedWithoutStatus_ShouldPersistInEpicRepositoryWithDefaultStatus()
    {
        // Arrange
        var faker = new Faker();
        var command = new CreateEpicCommand(
            faker.Random.Word(),
            faker.Lorem.Paragraph(),
            faker.Random.Guid().GuidConversion<ProjectId>(),
            null
        );
        var project = Project.Create(faker.Random.Guid().GuidConversion<UserId>(), faker.Random.Word()).Value;
        project.EditDefaultStatus(faker.Random.Guid().GuidConversion<ProjectTaskStatusId>());
        _projectRepository
            .Setup(r => r.GetByIdAsync(command.ProjectId)).ReturnsAsync(project);

        // Act
        var result = await _instance.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _epicRepository.Verify(r => r.Add(It.Is<Epic>(e => e.StatusId == project.DefaultStatusId)), Times.Once);
        _unitOfWork.Verify(u => u.SaveChangesAsync(CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenCommandWithInvalidStatusPassed_ShouldReturnFailedResultWithInvalidStatusIdError()
    {
        // Arrange
        var faker = new Faker();
        var command = new CreateEpicCommand(
            faker.Random.Word(),
            faker.Lorem.Paragraph(),
            faker.Random.Guid().GuidConversion<ProjectId>(),
            faker.Random.Guid().GuidConversion<ProjectTaskStatusId>()
        );
        var project = Project.Create(faker.Random.Guid().GuidConversion<UserId>(), faker.Random.Word()).Value;
        project.EditDefaultStatus(faker.Random.Guid().GuidConversion<ProjectTaskStatusId>());
        _projectRepository
            .Setup(r => r.GetByIdAsync(command.ProjectId)).ReturnsAsync(project);
        _statusRepository
            .Setup(r => r.StatusExistsWithStatusIdAsync(command.StatusId!, command.ProjectId, CancellationToken.None))
            .ReturnsAsync(false);

        // Act
        var result = await _instance.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.HasError<DomainErrors.ProjectTaskStatus.InvalidStatusId>().Should().BeTrue();
        _epicRepository.Verify(r => r.Add(It.Is<Epic>(e => e.StatusId == project.DefaultStatusId)), Times.Never);
        _unitOfWork.Verify(u => u.SaveChangesAsync(CancellationToken.None), Times.Never);
    }
}