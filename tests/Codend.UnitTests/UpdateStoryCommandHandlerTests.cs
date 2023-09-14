using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Stories.Commands.UpdateStory;
using Codend.Contracts.Abstractions;
using Codend.Domain.Entities;
using Codend.Domain.Repositories;
using FluentAssertions;
using FluentResults;
using Moq;

namespace Codend.UnitTests;

public class UpdateStoryCommandHandlerTests
{
    private readonly Mock<IStoryRepository> _storyRepository = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();

    [Fact]
    public async Task Handle_InvalidNameAndDescription_ReturnResultFailed()
    {
        var story = new Mock<Story>();
        var error = new Mock<IError>();
        story.Setup(s => s.EditName(It.IsAny<string>())).Returns(Result.Fail(error.Object));
        story.Setup(s => s.EditDescription(It.IsAny<string>())).Returns(Result.Fail(error.Object));

        var storyId = new StoryId(Guid.NewGuid());
        _storyRepository.Setup(r => r.GetByIdAsync(storyId)).Returns(async () => story.Object);

        var request = new UpdateStoryCommand(storyId.Value, "", "");
        var handler = new UpdateStoryCommandHandler(_storyRepository.Object, _unitOfWork.Object);

        // act
        var result = await handler.Handle(request, default);

        result.IsFailed.Should().BeTrue();
        result.Errors.Should().HaveCount(2);
        result.Errors.Should().AllSatisfy(o => o.Should().Be(error.Object));
        _storyRepository.Verify(r => r.Update(story.Object), Times.Never);
        _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}