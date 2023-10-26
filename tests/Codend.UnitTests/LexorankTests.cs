using Codend.Infrastructure.Lexorank;
using FluentAssertions;

namespace Codend.UnitTests;

public class LexorankTests
{
    [Theory]
    [InlineData("bbbbbb", "dzzzzz", "cccccc")]
    [InlineData("dwefas", "dwefas1", "dwefas0i")]
    [InlineData("0", "01", "00i")]
    [InlineData("def", "dea", "ded")]
    [InlineData("zzz", "zzy", "zzyi")]
    public void GetMiddleMethod_WhenTwoValidLexoranks_ShouldReturnMiddleLexorank(string value1, string value2,
        string expected)
    {
        // arrange
        var lex1 = Lexorank.FromString(value1);
        var lex2 = Lexorank.FromString(value2);

        // act
        var result = Lexorank.GetMiddle(lex1, lex2);

        // assert
        result.Value.Should().NotBeEmpty();
        result.Value.Should().BeEquivalentTo(expected);
    }
}