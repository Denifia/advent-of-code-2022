// Cosmic Expansion
// hint: https://simple.wikipedia.org/wiki/Manhattan_distance

var lines = File.ReadAllLines("input.txt");
var universe = new Universe(lines);
var sum = universe.SumOfGalaxyShortestDistances();

// question 1
Console.WriteLine($"Part 1 Answer: {sum}");

// question 2
Console.WriteLine($"Part 2 Answer: {true}");

class Universe
{
    private readonly List<Galaxy> _galaxies = [];

    public Universe(string[] lines)
    {
        ConvertInputToGalaxies(lines);
        ExpandTheUniverse();
    }

    private void ConvertInputToGalaxies(string[] lines)
    {
        for (var row = 0; row < lines.Length; row++)
        {
            var line = lines[row];
            for (var col = 0; col < line.Length; col++)
            {
                var c = line[col];
                if (c == '#')
                {
                    _galaxies.Add(new Galaxy(row, col));
                }
            }
        }
    }

    private void ExpandTheUniverse()
    {
        var rowCount = _galaxies.Max(x => x.Row);
        for (int row = 0; row < rowCount; row++)
        {
            var rowGalaxies = _galaxies.Where(x => x.Row == row).ToArray();

            if (rowGalaxies.Length == 0)
            {
                foreach (var galaxy in _galaxies.Where(x => x.Row > row))
                {
                    galaxy.RowExpansions += 1;
                }
            }
        }

        var colCount = _galaxies.Max(x => x.Col);
        for (int col = 0; col < colCount; col++)
        {
            var colGalaxies = _galaxies.Where(x => x.Col == col).ToArray();

            if (colGalaxies.Length == 0)
            {
                foreach (var galaxy in _galaxies.Where(x => x.Col > col))
                {
                    galaxy.ColExpansions += 1;
                }
            }
        }

        foreach (var galaxy in _galaxies)
        {
            galaxy.Expand();
        }
    }

    public long SumOfGalaxyShortestDistances()
    {
        var sum = 0L;
        for (int i = 0; i < _galaxies.Count; i++)
        {
            var galaxy = _galaxies[i];
            var otherGalaxies = _galaxies[i..].ToArray();
            foreach (var otherGalaxy in otherGalaxies)
            {
                // Manhattan Distance
                sum += Math.Abs(galaxy.Row - otherGalaxy.Row) + Math.Abs(galaxy.Col - otherGalaxy.Col);
            }
        }
        return sum;
    }

    private void Dump()
    {
        Console.WriteLine();
        for (int row = 0; row <= _galaxies.Max(x => x.Row); row++)
        {
            for (int col = 0; col <= _galaxies.Max(x => x.Col); col++)
            {
                var galaxy = _galaxies.FirstOrDefault(x => x.Row == row && x.Col == col);
                if (galaxy != null)
                {
                    Console.Write("#");
                }
                else
                {
                    Console.Write(".");
                }   
            }
            Console.WriteLine();
        }
    }
}


record Galaxy
{
    const long ExpansionRate = 1000000L;

    public Galaxy(long row, long col)
    {
        Row = row;
        Col = col;
    }

    public long Row { get; set; }
    public long Col { get; set; }

    public long RowExpansions { get; set; }
    public long ColExpansions { get; set; }

    public void Expand()
    {
        Row += (ExpansionRate * RowExpansions) - RowExpansions;
        Col += (ExpansionRate * ColExpansions) - ColExpansions;
    }
}