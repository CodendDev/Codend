using Codend.Shared.Infrastructure.Lexorank;
using FluentAssertions;

namespace Codend.UnitTests.Infrastructure;

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
        var alphabetMiddleValue = Lexorank.LexorankSystem.GetMidChar().ToString();
        // act
        var result = Lexorank.GetMiddle();
        // assert
        result.Value.Should().BeEquivalentTo(alphabetMiddleValue);
    }
    
    [Fact]
    public void GetMiddle_WhenOnlyPreviousValuePassed_ShouldReturnMiddleValueBetweenPassedValueAndLastAlphabetChar()
    {
        // arrange
        var expectedValue = "r";
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
        var expectedValue = "9";
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
        lexorankList.Should().BeInAscendingOrder(x=>x.Value);
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
        lexorankList.Should().BeInAscendingOrder(x=>x.Value);
    }
}