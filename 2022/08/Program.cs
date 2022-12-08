// Day 8: Treetop Tree House

using System.Text;

var lines = await File.ReadAllLinesAsync("input.txt");
var trees = new Dictionary<(int, int), Tree>();

for (int row = 0; row < lines.Length; row++)
{
    string? line = lines[row];
    for (int column = 0; column < line.Length; column++)
    {
        var height = line[column];
        trees.Add((row, column), new Tree(height, row, column));
    }
}

var rowWidth = trees.Values.Max(x => x.Row) + 1;
var columnWidth = trees.Values.Max(x => x.Column) + 1;

var sb = new StringBuilder();
for (int row = 0; row < rowWidth; row++)
{
    var rowTrees = trees.Values.Where(x => x.Row == row).OrderBy(x => x.Column).ToArray();
    for (int column = 0; column < columnWidth; column++)
    {
        var columnTrees = trees.Values.Where(x => x.Column == column).OrderBy(x => x.Row).ToArray();
        var tree = trees[(row, column)];

        var left = rowTrees[..(tree.Column)];
        var right = rowTrees[(tree.Column + 1)..];
        var top = columnTrees[..(tree.Row)];
        var bottom = columnTrees[(tree.Row + 1)..];

        if (row == 0 || row == rowWidth - 1 || column == 0 || column == columnWidth - 1)
        {
            tree.Visible = true;
        }
        else if (left.All(x => x.Height < tree.Height) || right.All(x => x.Height < tree.Height)
            || top.All(x => x.Height < tree.Height) || bottom.All(x => x.Height < tree.Height))
        {
            tree.Visible = true;
        }

        var leftScore = left.Count() - left.OrderByDescending(x => x.Column).SkipWhile(x => x.Height < tree.Height).Skip(1).Count();
        var rightScore = right.Count() - right.OrderBy(x => x.Column).SkipWhile(x => x.Height < tree.Height).Skip(1).Count();
        var topScore = top.Count() - top.OrderByDescending(x => x.Row).SkipWhile(x => x.Height < tree.Height).Skip(1).Count();
        var bottomScore = bottom.Count() - bottom.OrderBy(x => x.Row).SkipWhile(x => x.Height < tree.Height).Skip(1).Count();

        tree.Score = leftScore * rightScore * topScore * bottomScore;

        sb.Append(tree.Visible ? tree.Height : "O");
    }

    // Print out a visual of the forrest
    sb.AppendLine();
}

Console.WriteLine(sb);

// Consider your map; how many trees are visible from outside the grid?
Console.WriteLine($"Part 1 Answer: {trees.Values.Where(x => x.Visible).Count()}");

// Consider each tree on your map. What is the highest scenic score possible for any tree?
Console.WriteLine($"Part 2 Answer: {trees.Values.Max(x => x.Score)}");

class Tree
{
    public int Height { get; set; }
    public int Row { get; set; }
    public int Column { get; set; }
    public bool Visible { get; set; } = false;
    public int Score { get; set; }

    public Tree(char height, int row, int column)
    {
        Height = int.Parse(height.ToString());
        Row = row;
        Column = column;
    }
}