// Title

global using Triangle = (int X, int Y, int Z);
using aoc;

var lines = File.ReadAllLines("input.txt");

int possibleTriangles = 0;
foreach (var line in lines)
{
    var triangle = InputConverter.Convert(line.AsSpan());
    if (TriangleVerifier.IsPossible(triangle))
    {
        possibleTriangles++;
    }
}

// question 1
Console.WriteLine($"Part 1 Answer: {possibleTriangles}");

// question 2
Console.WriteLine($"Part 2 Answer: {true}");

