using Xunit;

namespace aoc;

internal class InputConverter
{
    public static Triangle Convert(ReadOnlySpan<char> input)
    {
        var parts = new Span<Range>(new Range[3]);
        _ = input.Split(parts, ' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        Triangle triangle = new()
        {
            X = int.Parse(input[parts[0]]),
            Y = int.Parse(input[parts[1]]),
            Z = int.Parse(input[parts[2]])
        };
        return triangle;
    }
}

public class InputConverterTests
{
    public static IEnumerable<object[]> GetData()
    {
        yield return ["1 2 3", (1, 2, 3)];
        yield return ["  1    2       3", (1, 2, 3)];
    }

    [Theory]
    [MemberData(nameof(GetData))]
    public void IdentifiesPossibleTriangles(string input, Triangle expected)
    {
        Assert.Equal(expected, InputConverter.Convert(input.AsSpan()));
    }
}