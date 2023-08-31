using Codend.Domain.ValueObjects;
using FluentAssertions;

namespace Codend.UnitTests;

public class ProjectNameTests
{
    [Theory]
    [InlineData("Name")]
    public void Create_ValidName_ResultIsSuccessful(string name)
    {
        // arrange

        // act
        var result = ProjectName.Create(name);

        // assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Name.Should().Be(name);
    }
}