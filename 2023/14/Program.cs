// Parabolic Reflector Dish

using System.Data.Common;
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

long iterations = 3;
iterations = 1_000_000_000;

long sampleSize = 500_000;

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
        Rocks = rocks;
        MaxRow = maxRow;
        MaxCol = maxCol;

        ColumnView = new SortedDictionary<int, Rock>[maxCol];
        for (int col = 0; col < maxCol; col++)
        {
            ColumnView[col] = new SortedDictionary<int, Rock>();
        }

        RowView = new SortedDictionary<int, Rock>[maxRow];
        for (int row = 0; row < maxRow; row++)
        {
            RowView[row] = new SortedDictionary<int, Rock>();
        }

        foreach (var rock in Rocks)
        {
            ColumnView[rock.Col].Add(rock.Row, rock);
            RowView[rock.Row].Add(rock.Col, rock);
        }
    }

    private SortedDictionary<int, Rock>[] ColumnView { get; set; }
    private SortedDictionary<int, Rock>[] RowView { get; set; }

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

    public IEnumerable<Rock> TiltingNorth(int column) => ColumnView[column].OrderBy(x => x.Key).Select(x => x.Value);

    public IEnumerable<Rock> TiltingSouth(int column) => ColumnView[column].OrderByDescending(x => x.Key).Select(x => x.Value);

    public IEnumerable<Rock> TiltingWest(int row) => RowView[row].OrderBy(x => x.Key).Select(x => x.Value);

    public IEnumerable<Rock> TiltingEast(int row) => RowView[row].OrderByDescending(x => x.Key).Select(x => x.Value);

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
            var updates = new Dictionary<int, Rock>();

            SlideRocks(TiltingNorth(column), 0, true, rock => rock.Row, (rock, amount) =>
            {
                updates.Add(rock.Row, rock);
                RowView[rock.Row].Remove(rock.Col);
                rock.Row -= amount;
                RowView[rock.Row].Add(rock.Col, rock);
            });

            foreach (var key in updates.Keys)
            {
                ColumnView[column].Remove(key);
                ColumnView[column].Add(updates[key].Row, updates[key]);
            }
        }
    }

    void TiltSouth()
    {
        foreach (var column in Columns())
        {
            var updates = new Dictionary<int, Rock>();

            SlideRocks(TiltingSouth(column), MaxCol - 1, false, rock => rock.Row, (rock, amount) =>
            {
                updates.Add(rock.Row, rock);
                RowView[rock.Row].Remove(rock.Col);
                rock.Row -= amount;
                RowView[rock.Row].Add(rock.Col, rock);
            });

            foreach (var key in updates.Keys)
            {
                ColumnView[column].Remove(key);
                ColumnView[column].Add(updates[key].Row, updates[key]);
            }
        }
    }

    void TiltWest()
    {
        foreach (var row in Rows())
        {
            var updates = new Dictionary<int, Rock>();
            SlideRocks(TiltingWest(row), 0, true, rock => rock.Col, (rock, amount) =>
            {
                updates.Add(rock.Col, rock);
                ColumnView[rock.Col].Remove(rock.Row);
                rock.Col -= amount;
                ColumnView[rock.Col].Add(rock.Row, rock);
            });

            foreach (var key in updates.Keys)
            {
                RowView[row].Remove(key);
                RowView[row].Add(updates[key].Col, updates[key]);
            }
        }
    }

    void TiltEast()
    {
        foreach (var row in Rows())
        {
            var updates = new Dictionary<int, Rock>();
            SlideRocks(TiltingEast(row), MaxRow - 1, false, rock => rock.Col, (rock, amount) =>
            {
                updates.Add(rock.Col, rock);
                ColumnView[rock.Col].Remove(rock.Row);
                rock.Col -= amount;
                ColumnView[rock.Col].Add(rock.Row, rock);
            });

            foreach (var key in updates.Keys)
            {
                RowView[row].Remove(key);
                RowView[row].Add(updates[key].Col, updates[key]);
            }
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