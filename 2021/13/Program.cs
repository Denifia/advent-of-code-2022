// Day 13: Transparent Origami

using System.Text;

var lines = File.ReadAllLines("test-input.txt");

var dots = new HashSet<(int X, int Y)>();
var folds = new List<(string axis, int value)>();
var lineNumber = 0;
while (!string.IsNullOrWhiteSpace(lines[lineNumber]))
{
    // Parse dots
    var parts = lines[lineNumber].Split(',').Select(int.Parse).ToArray();
    dots.Add((parts[0], parts[1]));
    lineNumber++;
}
lineNumber++;
while (lineNumber < lines.Length)
{
    // Parse folds
    var parts = lines[lineNumber].Split(' ')[2].Split('=').ToArray();
    folds.Add((parts[0], int.Parse(parts[1])));
    lineNumber++;
}

var sb = new StringBuilder();
for (int x = 0; x < dots.Max(dot => dot.X); x++)
{
    for (int y = 0; y < dots.Max(dot => dot.Y); y++)
    {
        sb.Append(dots.Contains((x, y)) ? "#" : ".");
    }
    sb.AppendLine();
}
Console.WriteLine(sb);

Console.WriteLine();
Console.WriteLine();

foreach (var fold in folds.Take(1))
{
    var toFold = dots.Where(d =>
        fold switch
        {
            ("x", var value) => d.Y > value,
            ("y", var value) => d.X > value,
            _ => false
        }).ToArray();

    for (int i = 0; i < toFold.Length; i++)
    {
        (int X, int Y) newDot = fold switch
        {
            ("x", var value) => (toFold[i].X, value - (toFold[i].Y % value)),
            ("y", var value) => (value - (toFold[i].X % value), toFold[i].Y),
            _ => throw new NotSupportedException()
        };
        dots.Remove(toFold[i]);
        dots.Add(newDot);
    }
}

sb = new StringBuilder();
for (int x = 0; x < dots.Max(dot => dot.X); x++)
{
    for (int y = 0; y < dots.Max(dot => dot.Y); y++)
    {
        sb.Append(dots.Contains((x, y)) ? "#" : ".");
    }
    sb.AppendLine();
}
Console.WriteLine(sb);

// question 1
Console.WriteLine($"Part 1 Answer: {dots.Count}");

// question 2
Console.WriteLine($"Part 2 Answer: {true}");

//class Dot
//{
//    public int X { get; set; }
//    public int Y { get; set; }

//    public Dot(int x, int y)
//    {
//        X = x;
//        Y = y;
//    }
//}