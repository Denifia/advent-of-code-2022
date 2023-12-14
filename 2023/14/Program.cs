// Parabolic Reflector Dish

using System.Diagnostics;

var lines = File.ReadAllLines("test-input.txt");

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

Stopwatch stopwatch = Stopwatch.StartNew();
for (int i = 0; i < 1_000_000_000; i++)
{
    platform.Cycle();
    if (i > 0 && i % 1_000_000 == 0)
        break;
}
stopwatch.Stop();

var totalSeconds = (stopwatch.ElapsedMilliseconds / 1000) * (1_000_000_000 / 1_000_000);
var timespan = TimeSpan.FromSeconds(totalSeconds);

var sum = platform.GetNorthernLoad();

// question 1
Console.WriteLine($"Answer: {timespan.TotalHours}");

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
}

class Platform
{
    public Platform(List<Rock> rocks, int maxRow, int maxCol)
    {
        Rocks = rocks;
        MaxRow = maxRow;
        MaxCol = maxCol;
    }

    public List<Rock> Rocks { get; }
    public int MaxRow { get; }
    public int MaxCol { get; }

    public IEnumerable<int> Rows()
    {
        for (int row = 0; row < MaxRow; row++)
        {
            yield return row;
        }
    }

    public IEnumerable<int> Columns()
    {
        for (int col = 0; col < MaxCol; col++)
        {
            yield return col;
        }
    }

    public IEnumerable<Rock> TiltingNorth(int column) => Rocks.Where(x => x.Col == column).OrderBy(x => x.Row);

    public IEnumerable<Rock> TiltingSouth(int column) => Rocks.Where(x => x.Col == column).OrderByDescending(x => x.Row);

    public IEnumerable<Rock> TiltingWest(int row) => Rocks.Where(x => x.Row == row).OrderBy(x => x.Col);

    public IEnumerable<Rock> TiltingEast(int row) => Rocks.Where(x => x.Row == row).OrderByDescending(x => x.Col);

    static void SlideRocks(IEnumerable<Rock> rocks, int start, bool incrementing, Func<Rock, int> position, Action<Rock, int> slide)
    {
        var previousRock = start;
        foreach (var rock in rocks)
        {
            if (!rock.CanMove)
            {
                previousRock = incrementing
                    ? position(rock) + 1
                    : position(rock) - 1;

                continue;
            }

            previousRock = position(rock) - previousRock;

            if (previousRock != 0)
            {
                slide(rock, previousRock);
            }

            previousRock = incrementing
                ? position(rock) + 1
                : position(rock) - 1;
        }
    }

    public void Cycle()
    {
        TiltNorth();
        TiltWest();
        TiltSouth();
        TiltEast();
        //Dump();
    }

    void TiltNorth()
    {
        foreach (var column in Columns())
        {
            SlideRocks(TiltingNorth(column), 0, true, rock => rock.Row, (rock, amount) => rock.Row -= amount);
        }
    }

    void TiltSouth()
    {
        foreach (var column in Columns())
        {
            SlideRocks(TiltingSouth(column), MaxCol - 1, false, rock => rock.Row, (rock, amount) => rock.Row -= amount);
        }
    }

    void TiltWest()
    {
        foreach (var row in Rows())
        {
            SlideRocks(TiltingWest(row), 0, true, rock => rock.Col, (rock, amount) => rock.Col -= amount);
        }
    }

    void TiltEast()
    {
        foreach (var row in Rows())
        {
            SlideRocks(TiltingEast(row), MaxRow - 1, false, rock => rock.Col, (rock, amount) => rock.Col -= amount);
        }
    }

    public int GetNorthernLoad()
    {
        var sum = 0;
        foreach (var col in Columns())
        {
            sum += TiltingNorth(col).Where(x => x.CanMove).Sum(x => MaxRow - x.Row);
        }
        return sum;
    }

    void Dump()
    {
        foreach (var row in Rows())
        {
            var previousRock = -1;
            foreach (var rock in TiltingWest(row))
            {
                for (int i = previousRock; i < rock.Col - 1; i++)
                {
                    Console.Write(".");
                }

                Console.Write(rock.CanMove ? "O" : "#");
                previousRock = rock.Col;
            }

            for (int i = previousRock; i < MaxCol - 1; i++)
            {
                Console.Write(".");
            }

            Console.WriteLine();
        }
        Console.WriteLine();
    }
}