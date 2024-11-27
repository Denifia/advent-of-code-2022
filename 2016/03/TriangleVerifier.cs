using Xunit;

namespace aoc;

internal class TriangleVerifier
{
    public static bool IsPossible(Triangle triangle)
    {
        return triangle.X + triangle.Y > triangle.Z 
            && triangle.Y + triangle.Z > triangle.X 
            && triangle.X + triangle.Z > triangle.Y;
    }
}

public class TriangleVerifierTests
{
    [Theory]
    [InlineData(2, 2, 2,true)]
    [InlineData(5, 10, 25, false)]
    [InlineData(5, 1, 1, false)]
    [InlineData(1, 5, 1, false)]
    [InlineData(1, 1, 5, false)]
    public void IdentifiesPossibleTriangles(int x, int y, int z, bool expected)
    {
        Assert.Equal(expected, TriangleVerifier.IsPossible((x, y, z)));
    }
}
