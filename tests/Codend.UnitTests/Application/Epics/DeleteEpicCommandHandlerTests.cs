using Bogus;
using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Epics.Commands.DeleteEpic;
using Codend.Domain.Core.Primitives;
using Codend.Domain.Entities;
using Codend.Domain.Repositories;
using FluentAssertions;
using Moq;

namespace Codend.UnitTests.Application.Epics;

public class DeleteEpicCommandHandlerTests
{
    private readonly Mock<IEpicRepository> _epicRepository = new();
    private readonly Mock<IStoryRepository> _storyRepository = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly DeleteEpicCommandHandler _instance;

    public DeleteEpicCommandHandlerTests()
    {
        _instance = new DeleteEpicCommandHandler(
            _epicRepository.Object,
            _storyRepository.Object,
            _unitOfWork.Object);
    }

    [Fact]
    public async Task
        Handle_WhenValidCommandPassed_ShouldRemoveEpicInRepositoryAndReplaceDependentEpicIdInStoriesWithNull()
    {
        // Arrange
        var faker = new Faker();
        var epic = Epic.Create(
            faker.Random.Word(),
            faker.Lorem.Paragraph(),
            faker.Random.Guid().GuidConversion<ProjectId>(),
            faker.Random.Guid().GuidConversion<ProjectTaskStatusId>()).Value;
        var command = new DeleteEpicCommand(
            epic.Id
        );
        var storyList = new List<Story>
        {
            Story.Create(faker.Random.Word(), faker.Lorem.Paragraph(), epic.ProjectId, epic.Id, epic.StatusId).Value
        };

        _epicRepository.Setup(r => r.GetByIdAsync(command.EpicId, CancellationToken.None)).ReturnsAsync(epic);
        _storyRepository.Setup(r => r.GetStoriesByEpicIdAsync(epic.Id, CancellationToken.None)).ReturnsAsync(storyList);

        // Act
        var result = await _instance.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        storyList.Should().OnlyContain(s => s.EpicId == null);
        _storyRepository.Verify(r => r.UpdateRange(It.IsAny<IEnumerable<Story>>()), Times.Once);
        _epicRepository.Verify(r => r.Remove(It.IsAny<Epic>()), Times.Once);
        _unitOfWork.Verify(u => u.SaveChangesAsync(CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenCommandWithInvalidIdPassed_ShouldReturnResultWithDomainNotFoundError()
    {
        // Arrange
        var faker = new Faker();
        var command = new DeleteEpicCommand(
            faker.Random.Guid().GuidConversion<EpicId>()
        );
        _epicRepository.Setup(r => r.GetByIdAsync(command.EpicId, CancellationToken.None)).ReturnsAsync((Epic?)null);

        // Act
        var result = await _instance.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
        _storyRepository.Verify(r => r.UpdateRange(It.IsAny<IEnumerable<Story>>()), Times.Never);
        _epicRepository.Verify(r => r.Remove(It.IsAny<Epic>()), Times.Never);
        _unitOfWork.Verify(u => u.SaveChangesAsync(CancellationToken.None), Times.Never);
    }
}