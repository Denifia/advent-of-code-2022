// Parabolic Reflector Dish

using System.Text;

var lines = File.ReadAllLines("input.txt");

List<Rock> rocks = [];
for (int col = 0; col < lines[0].Length; col++)
{
    for (int row = 0; row < lines.Length; row++)
    {
        if (lines[row][col] == 'O')
        {
            rocks.Add(new Rock(row, col, true));
        } 
        else if (lines[row][col] == '#')
        {
            rocks.Add(new Rock(row, col, false));
        }
    }
}

var platform = new Platform(rocks, lines.Count(), lines[0].Length);

long iterations = 1;
iterations = 1_000_000_000;
var lookingForLoop = true;
for (long i = 0; i < iterations; i++)
{
    var (foundLoop, loopLength) = platform.Cycle(lookingForLoop);

    if (foundLoop)
    {
        var skip = ((iterations - i) / loopLength) * loopLength;
        i += skip;
        lookingForLoop = false;
    }
}
var sum = platform.GetNorthernLoad();

// question 2
Console.WriteLine($"Answer: {sum}");

class Rock
{
    public Rock(int row, int col, bool canMove)
    {
        Row = row;
        Col = col;
        CanMove = canMove;
    }

    public int Row { get; set; }
    public int Col { get; set; }
    public bool CanMove { get; }

    public override string ToString()
    {
        return $"{Row}{Col}{CanMove}";
    }
}

class Platform
{
    public Platform(List<Rock> rocks, int maxRow, int maxCol)
    {
        foreach (var rock in rocks)
        {
            Rocks.Add(rock);
        }

        MaxRow = maxRow;
        MaxCol = maxCol;

        _observedConfigurations.Add(GetConfig(), 0);
    }

    public HashSet<Rock> Rocks { get; } = new();
    public int MaxRow { get; }
    public int MaxCol { get; }

    static void SlideRocks(IEnumerable<Rock> rocks, int start, bool incrementing, bool horizontal)
    {
        var previousSection = -1;
        var previousRock = start;
        foreach (var rock in rocks)
        {
            var section = horizontal
                    ? rock.Row
                    : rock.Col;

            if (previousSection == -1)
            {
                previousSection = section;
            } 
            else if (previousSection != section)
            {
                previousRock = start;
                previousSection = section;
            }

            var position = horizontal
                ? rock.Col
                : rock.Row;

            if (!rock.CanMove)
            {
                previousRock = incrementing
                    ? position + 1
                    : position - 1;

                continue;
            }

            previousRock = position - previousRock;

            if (previousRock != 0)
            {
                if (horizontal)
                    rock.Col -= previousRock;
                else
                    rock.Row -= previousRock;

                position = horizontal
                    ? rock.Col
                    : rock.Row;
            }

            previousRock = incrementing
                ? position + 1
                : position - 1;
        }
    }

    Dictionary<string, int> _observedConfigurations = new Dictionary<string, int>();

    public (bool, int) Cycle(bool lookingForLoop)
    {
        TiltNorth();
        TiltWest();
        TiltSouth();
        TiltEast();
        //Dump();

        if (lookingForLoop && SeenConfigurationBefore())
        {
            var loopStart = _observedConfigurations[GetConfig()];
            var loopLength = _observedConfigurations.Count - loopStart;
            return (true, loopLength);
        }

        return (false, 0);
    }

    bool SeenConfigurationBefore()
    {
        var config = GetConfig();
        if (_observedConfigurations.ContainsKey(config))
            return true;

        _observedConfigurations.Add(config, _observedConfigurations.Count);
        return false;
    }

    void TiltNorth() => SlideRocks(Rocks.OrderBy(x => x.Col).ThenBy(x => x.Row), 0, true, false);
    void TiltSouth() => SlideRocks(Rocks.OrderBy(x => x.Col).ThenByDescending(x => x.Row), MaxCol - 1, false, false);
    void TiltWest() => SlideRocks(Rocks.OrderBy(x => x.Row).ThenBy(x => x.Col), 0, true, true);
    void TiltEast() => SlideRocks(Rocks.OrderBy(x => x.Row).ThenByDescending(x => x.Col), MaxRow - 1, false, true);

    public int GetNorthernLoad()
    {
        var sum = 0;
        sum += Rocks.OrderBy(x => x.Col).ThenBy(x => x.Row).Where(x => x.CanMove).Sum(x => MaxRow - x.Row);
        return sum;
    }

    string GetConfig() => string.Join(string.Empty, Rocks.OrderBy(x => x.Col).ThenBy(x => x.Row).Select(x => x.ToString()));

    string Dump(bool toConsole = true)
    {
        var sb = new StringBuilder();
        for (int row = 0; row < MaxRow; row++)
        {
            for (int col = 0; col < MaxCol; col++)
            {
                var rock = Rocks.FirstOrDefault(x => x.Row == row && x.Col == col);
                if (rock is null)
                {
                    sb.Append(".");
                    continue;
                }

                sb.Append(rock.CanMove ? "O" : "#");
            }
            sb.AppendLine();
        }
        sb.AppendLine();

        var s = sb.ToString();
        if (toConsole)
        {
            Console.WriteLine(s);
        }
        return s;
    }
}