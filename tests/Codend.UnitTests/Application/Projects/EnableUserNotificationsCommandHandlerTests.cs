using Bogus;
using Codend.Application.Core.Abstractions.Authentication;
using Codend.Application.Core.Abstractions.Data;
using Codend.Application.Projects.Commands.EnableNotifications;
using Codend.Domain.Core.Primitives;
using Codend.Domain.Entities;
using Codend.Domain.Repositories;
using FluentAssertions;
using Moq;

namespace Codend.UnitTests.Application.Projects;

public class EnableUserNotificationsCommandHandlerTests
{
    private readonly Mock<IHttpContextProvider> _context = new();
    private readonly Mock<IProjectMemberRepository> _memberRepository = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();

    private readonly EnableUserNotificationsCommandHandler _instance;

    public EnableUserNotificationsCommandHandlerTests()
    {
        _instance = new EnableUserNotificationsCommandHandler(
            _context.Object,
            _memberRepository.Object,
            _unitOfWork.Object);
    }

    [Fact]
    public async Task Handle_WhenValidCommandPassed_ShouldEnableUserNotifications()
    {
        // Arrange
        var faker = new Faker();
        var command = new EnableUserNotificationsCommand(
            faker.Random.Guid().GuidConversion<ProjectId>()
        );
        var projectMember = ProjectMember.Create(command.ProjectId,
            faker.Random.Guid().GuidConversion<UserId>()).Value;
        _memberRepository
            .Setup(r => r.GetByProjectAndMemberId(projectMember.ProjectId, projectMember.MemberId,
                CancellationToken.None)).ReturnsAsync(projectMember);
        _context.Setup(c => c.UserId).Returns(projectMember.MemberId);

        // Act
        var result = await _instance.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _memberRepository.Verify(r => r.Update(It.IsAny<ProjectMember>()), Times.Once);
        _unitOfWork.Verify(u => u.SaveChangesAsync(CancellationToken.None), Times.Once);
    }
}