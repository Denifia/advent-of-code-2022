// Parabolic Reflector Dish

using System.Diagnostics;

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

long sampleSize = 1000;

Stopwatch stopwatch = Stopwatch.StartNew();
for (int i = 0; i < iterations; i++)
{
    platform.Cycle();
    if (i > 0 && i % sampleSize == 0)
        break;
}
stopwatch.Stop();

var totalSeconds = (stopwatch.ElapsedMilliseconds / 1000) * (iterations / sampleSize);
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
        //Rocks = rocks;

        foreach (var rock in rocks)
        {
            Rocks.Add(rock);
        }

        MaxRow = maxRow;
        MaxCol = maxCol;
    }

    public HashSet<Rock> Rocks { get; } = new();
    public int MaxRow { get; }
    public int MaxCol { get; }

    //static void SlideRocks(IEnumerable<Rock> rocks, int start, bool incrementing, Func<Rock, int> position, Func<Rock, int> section, Action<Rock, int> slide)
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

    public void Cycle()
    {
        TiltNorth();
        //Dump();
        TiltWest();
        TiltSouth();
        TiltEast();
        //Dump();
    }

    //void TiltNorth() => SlideRocks(Rocks.OrderBy(x => x.Col).ThenBy(x => x.Row), 0, true, rock => rock.Row, rock => rock.Col, (rock, amount) => rock.Row -= amount);
    //void TiltSouth() => SlideRocks(Rocks.OrderBy(x => x.Col).ThenByDescending(x => x.Row), MaxCol - 1, false, rock => rock.Row, rock => rock.Col, (rock, amount) => rock.Row -= amount);
    //void TiltWest() => SlideRocks(Rocks.OrderBy(x => x.Row).ThenBy(x => x.Col), 0, true, rock => rock.Col, rock => rock.Row, (rock, amount) => rock.Col -= amount);
    //void TiltEast() => SlideRocks(Rocks.OrderBy(x => x.Row).ThenByDescending(x => x.Col), MaxRow - 1, false, rock => rock.Col, rock => rock.Row, (rock, amount) => rock.Col -= amount);

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

    void Dump()
    {
        for (int row = 0; row < MaxRow; row++)
        {
            for (int col = 0; col < MaxCol; col++)
            {
                var rock = Rocks.FirstOrDefault(x => x.Row == row && x.Col == col);
                if (rock is null)
                {
                    Console.Write(".");
                    continue;
                }

                Console.Write(rock.CanMove ? "O" : "#");
            }
            Console.WriteLine();
        }
        Console.WriteLine();   
    }
}