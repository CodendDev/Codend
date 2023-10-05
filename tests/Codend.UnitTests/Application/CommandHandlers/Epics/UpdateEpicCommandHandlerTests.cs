using Bogus;
using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Epics.Commands.UpdateEpic;
using Codend.Domain.Core.Errors;
using Codend.Domain.Core.Primitives;
using Codend.Domain.Entities;
using Codend.Domain.Repositories;
using FluentAssertions;
using Moq;

namespace Codend.UnitTests.Application.CommandHandlers.Epics;

public class UpdateEpicCommandHandlerTests
{
    private readonly Mock<IEpicRepository> _epicRepository = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<IProjectTaskStatusRepository> _statusRepository = new();
    private readonly UpdateEpicCommandHandler _instance;

    public UpdateEpicCommandHandlerTests()
    {
        _instance = new UpdateEpicCommandHandler(
            _epicRepository.Object,
            _unitOfWork.Object,
            _statusRepository.Object);
    }

    [Fact]
    public async Task Handle_WhenValidCommandPassed_ShouldUpdateInEpicRepository()
    {
        // Arrange
        var faker = new Faker();
        var command = new UpdateEpicCommand(
            faker.Random.Guid().GuidConversion<EpicId>(),
            faker.Random.Word(),
            null,
            faker.Random.Guid().GuidConversion<ProjectTaskStatusId>()
        );
        var description = faker.Lorem.Paragraph();
        var project = Project.Create(faker.Random.Guid().GuidConversion<UserId>(), faker.Random.Word()).Value;
        var epic = Epic.Create(faker.Random.Word(), description, project.Id,
            faker.Random.Guid().GuidConversion<ProjectTaskStatusId>()).Value;

        _epicRepository.Setup(r => r.GetByIdAsync(command.EpicId, CancellationToken.None)).ReturnsAsync(epic);
        _statusRepository
            .Setup(r => r.StatusExistsWithStatusIdAsync(command.StatusId!, epic.ProjectId, CancellationToken.None))
            .ReturnsAsync(true);

        // Act
        var result = await _instance.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _epicRepository.Verify(r =>
            r.Update(It.Is<Epic>(e =>
                e.Name.Value == command.Name &&
                e.Description.Value == description &&
                e.StatusId == command.StatusId
            )), Times.Once);
        _unitOfWork.Verify(u => u.SaveChangesAsync(CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenCommandWithInvalidEpicIdPassed_ShouldReturnFailedResultWithDomainNotFoundError()
    {
        // Arrange
        var faker = new Faker();
        var command = new UpdateEpicCommand(
            faker.Random.Guid().GuidConversion<EpicId>(),
            null,
            null,
            null
        );

        _epicRepository.Setup(r => r.GetByIdAsync(command.EpicId, CancellationToken.None)).ReturnsAsync((Epic?)null);

        // Act
        var result = await _instance.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.HasError<DomainErrors.General.DomainNotFound>().Should().BeTrue();
        _epicRepository.Verify(r => r.Update(It.IsAny<Epic>()), Times.Never);
        _unitOfWork.Verify(u => u.SaveChangesAsync(CancellationToken.None), Times.Never);
    }
}