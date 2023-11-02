using Codend.Shared.Infrastructure.Lexorank;
using FluentAssertions;

namespace Codend.UnitTests.Shared.Infrastructure;

public class LexorankTests
{
    [Theory]
    [InlineData("0", "z", "i")]
    [InlineData("dwefas", "dwef0", "dwef5")]
    [InlineData("0", "01", "00i")]
    [InlineData("def", "dea", "ded")]
    [InlineData("zzz", "zzy", "zzyi")]
    public void GetMiddle_WhenTwoValidLexoranks_ShouldReturnMiddleLexorank(
        string value1,
        string value2,
        string expected)
    {
        // arrange
        var lex1 = new Lexorank(value1);
        var lex2 = new Lexorank(value2);

        // act
        var result = Lexorank.GetMiddle(lex1, lex2);

        // assert
        result.Value.Should().NotBeEmpty();
        result.Value.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void GetMiddle_WhenTwoNullsPassed_ShouldReturnMiddleAlphabetChar()
    {
        // arrange
        var alphabetMiddleValue = Lexorank.LexorankSystem.MidChar.ToString();
        // act
        var result = Lexorank.GetMiddle();
        // assert
        result.Value.Should().BeEquivalentTo(alphabetMiddleValue);
    }

    [Fact]
    public void GetMiddle_WhenOnlyPreviousValuePassed_ShouldReturnMiddleValueBetweenPassedValueAndLastAlphabetChar()
    {
        // arrange
        const string expectedValue = "r";
        var prev = new Lexorank("i");
        // act
        var result = Lexorank.GetMiddle(prev);
        // assert
        result.Value.Should().BeEquivalentTo(expectedValue);
    }

    [Fact]
    public void GetMiddle_WhenOnlyNextValuePassed_ShouldReturnMiddleValueBetweenFirstAlphabetCharAndNextValue()
    {
        // arrange
        const string expectedValue = "9";
        var next = new Lexorank("i");
        // act
        var result = Lexorank.GetMiddle(null, next);
        // assert
        result.Value.Should().BeEquivalentTo(expectedValue);
    }

    /// <summary>
    /// Checks one of the worst scenarios, which is adding many elements to the beginning.
    /// </summary>
    [Fact]
    public void GetMiddle_WhenCalledManyTimesFromNullToFirst_ShouldInsertElementsInAscendingOrder()
    {
        // arrange
        var lexorankList = new List<Lexorank> { Lexorank.GetMiddle() };

        // act
        for (var i = 0; i < 100; i++)
        {
            var mid = Lexorank.GetMiddle(null, lexorankList[0]);
            lexorankList.Insert(0, mid);
        }

        // assert
        lexorankList.Should().BeInAscendingOrder(x => x.Value);
    }

    /// <summary>
    /// Checks one of the worst scenarios, which is adding many elements to the end.
    /// </summary>
    [Fact]
    public void GetMiddle_WhenCalledManyTimesFromLastToNull_ShouldAddElementsInAscendingOrder()
    {
        // arrange
        var lexorankList = new List<Lexorank> { Lexorank.GetMiddle() };

        // act
        for (var i = 0; i < 100; i++)
        {
            var mid = Lexorank.GetMiddle(lexorankList.Last());
            lexorankList.Add(mid);
        }

        // assert
        lexorankList.Should().BeInAscendingOrder(x => x.Value);
    }

    [Theory]
    [InlineData("abcde", "abce", 33, "abcdp1", "abcdpx")]
    [InlineData("dwaef", "nwadasewa", 1300, "i00zi", "iz3w")]
    [InlineData("0", "z", 3, "i9", "ir")]
    [InlineData("0", "z", 50000, "i000x", "izd5c")]
    public void GetSpacedOutValuesBetween_WhenValidValuePassed_ShouldReturnSpacedLexoranksList(
        string from,
        string to,
        int amount,
        string expectedStart,
        string expectedEnd)
    {
        // arrange
        var lexFrom = new Lexorank(from);
        var lexTo = new Lexorank(to);

        // act
        var result = Lexorank.GetSpacedOutValuesBetween(amount, lexFrom, lexTo);

        // assert
        result.Count.Should().Be(amount);
        result[0].Value.Should().Be(expectedStart);
        result[^1].Value.Should().Be(expectedEnd);
    }

    [Fact]
    public void
        GetSpacedOutValuesBetween_WhenNullAndValidValuePassed_ShouldReturnSpacedLexoranksListBetweenMinAndGiven()
    {
        // arrange
        const int amount = 132;
        const string expectedStart = "0509";
        const string expectedEnd = "05x0i";
        var lexTo = new Lexorank("0abc");

        // act
        var result = Lexorank.GetSpacedOutValuesBetween(amount, null, lexTo);

        // assert
        result.Count.Should().Be(amount);
        result[0].Value.Should().Be(expectedStart);
        result[^1].Value.Should().Be(expectedEnd);
    }
}