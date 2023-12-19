// Title

var lines = File.ReadAllLines("input.txt");

var maxEnergized = 0;
var maxRow = lines.Length - 1;
var maxCol = lines[0].Length - 1;

var tiles = new List<Tile>();
for (int row = 0; row < lines.Length; row++)
{
    for (int col = 0; col < lines[row].Length; col++)
    {
        var c = lines[row][col];
        tiles.Add(TileFactory.New(row, col, c));
    }
}

List<Beam> beams = [new Beam()];

for (int row = 0; row < lines.Length; row++)
{
    for (int col = 0; col < lines[row].Length; col++)
    {
        if (row == 0)
        {
            maxEnergized = Math.Max(maxEnergized, CountEnergizedTiles(tiles, beams, row, col, Direction.Down, maxRow, maxCol));
            Cleanup();
        }

        if (row == maxRow)
        {
            maxEnergized = Math.Max(maxEnergized, CountEnergizedTiles(tiles, beams, row, col, Direction.Left, maxRow, maxCol));
            Cleanup();
        }

        if (col == 0)
        {
            maxEnergized = Math.Max(maxEnergized, CountEnergizedTiles(tiles, beams, row, col, Direction.Right, maxRow, maxCol));
            Cleanup();
        }

        if (col == maxRow)
        {
            maxEnergized = Math.Max(maxEnergized, CountEnergizedTiles(tiles, beams, row, col, Direction.Left, maxRow, maxCol));
            Cleanup();
        }
    }
}

void Cleanup()
{
    foreach (var tile in tiles)
    {
        tile.Cleanup();
    }

    beams.Clear();
    beams.Add(new Beam());
}

int CountEnergizedTiles(List<Tile> tiles, List<Beam> beams, int startRow, int startCol, Direction startDirection, int maxRow, int maxCol)
{
    var topLeftTile = tiles.First(x => x.Row == startRow && x.Col == startCol);
    var entryResult = topLeftTile.HandleBeam(startDirection);
    beams[0].MoveTo(topLeftTile, entryResult.Direction);

    while (beams.Any(x => !x.Done))
    {
        var temp = beams.Where(x => !x.Done).ToArray();
        for (int i = 0; i < temp.Length; i++)
        {
            var beam = temp[i];
            (int row, int col) = beam.GetNextTilePosition();
            if (OutOfBounds(row, col))
            {
                beam.Done = true;
                continue;
            }

            var tile = tiles.First(x => x.Row == row && x.Col == col);
            var result = tile.HandleBeam(beam._direction);

            if (result.Visited)
            {
                beam.Done = true;
                continue;
            }

            if (result.Split)
            {
                // split
                beam.MoveTo(tile, result.Direction);

                var newBeam = new Beam();
                newBeam.MoveTo(tile, result.SplitDirection);
                beams.Add(newBeam);
            }
            else
            {
                // no split
                beam.MoveTo(tile, result.Direction);
            }
        }
    }

    return tiles.Count(x => x.Energized);
}

bool OutOfBounds(int row, int col)
{
    return row < 0 || row > maxRow || col < 0 || col > maxCol;
}


Console.WriteLine($"Answer: {maxEnergized}");

abstract class Tile
{
    public bool Energized { get; set; }
    public int Row { get; }
    public int Col { get; }

    public Tile(int row, int col)
    {
        Row = row;
        Col = col;
    }

    public BeamResult HandleBeam(Direction incoming)
    {
        if (_tracked.Contains(incoming))
            return new BeamResult() { Visited = true };

        _tracked.Add(incoming);

        return HandleBeamInner(incoming);
    }

    public abstract BeamResult HandleBeamInner(Direction incoming);


    private HashSet<Direction> _tracked = new HashSet<Direction>();

    public void Cleanup()
    {
        _tracked.Clear();
        Energized = false;
    }
}

class Beam
{
    public bool Done { get; set; }
    private Stack<Tile> _tiles = new Stack<Tile>();
    public Direction _direction;

    public (int row, int col) GetNextTilePosition()
    {
        var tile = _tiles.Pop();
        return _direction switch
        {
            Direction.Up => (tile.Row - 1, tile.Col),
            Direction.Down => (tile.Row + 1, tile.Col),
            Direction.Left => (tile.Row, tile.Col - 1),
            Direction.Right => (tile.Row, tile.Col + 1),
            _ => throw new Exception("Invalid direction")
        };
    }

    public Beam()
    {

    }

    public void MoveTo(Tile tile, Direction direction)
    {
        _tiles.Push(tile);
        _direction = direction;
    }

    public void Cleanup()
    {
        _tiles.Clear();
    }
}

enum Direction
{
    Right = 0,
    Down = 1,
    Left = 2,
    Up = 3
}

struct BeamResult
{
    public bool Visited { get; set; }
    public bool Split { get; set; }
    public Direction Direction { get; set; }
    public Direction SplitDirection { get; set; }
}

class Mirror : Tile
{
    public char Character { get; set; }

    public Mirror(int row, int col, char c) : base(row, col)
    {
        Character = c;
    }

    public override BeamResult HandleBeamInner(Direction incoming)
    {
        Energized = true;

        return (incoming, Character) switch
        {
            (Direction.Right, '/') => new BeamResult { Direction = Direction.Up },
            (Direction.Left, '/') => new BeamResult { Direction = Direction.Down },
            (Direction.Up, '/') => new BeamResult { Direction = Direction.Right },
            (Direction.Down, '/') => new BeamResult { Direction = Direction.Left },
            (Direction.Right, '\\') => new BeamResult { Direction = Direction.Down },
            (Direction.Left, '\\') => new BeamResult { Direction = Direction.Up },
            (Direction.Up, '\\') => new BeamResult { Direction = Direction.Left },
            (Direction.Down, '\\') => new BeamResult { Direction = Direction.Right },
            _ => throw new Exception("Invalid direction")
        };
    }
}

class Splitter : Tile
{
    public char Character { get; set; }

    public Splitter(int row, int col, char c) : base(row, col)
    {
        Character = c;
    }

    public override BeamResult HandleBeamInner(Direction incoming)
    {
        Energized = true;

        return (incoming, Character) switch
        {
            (Direction.Right, '|') => new BeamResult { Split = true, Direction = Direction.Up, SplitDirection = Direction.Down },
            (Direction.Left, '|') => new BeamResult { Split = true, Direction = Direction.Up, SplitDirection = Direction.Down },
            (Direction.Up, '|') => new BeamResult { Split = false, Direction = Direction.Up },
            (Direction.Down, '|') => new BeamResult { Split = false, Direction = Direction.Down },
            (Direction.Right, '-') => new BeamResult { Split = false, Direction = Direction.Right },
            (Direction.Left, '-') => new BeamResult { Split = false, Direction = Direction.Left },
            (Direction.Up, '-') => new BeamResult { Split = true, Direction = Direction.Left, SplitDirection = Direction.Right },
            (Direction.Down, '-') => new BeamResult { Split = true, Direction = Direction.Left, SplitDirection = Direction.Right },
            _ => throw new Exception("Invalid direction")
        };
    }
}

class EmptySpace : Tile
{
    public EmptySpace(int row, int col) : base(row, col)
    {

    }

    public override BeamResult HandleBeamInner(Direction incoming)
    {
        Energized = true;
        return new BeamResult { Direction = incoming };
    }
}

static class TileFactory
{
    public static Tile New(int row, int col, char c)
    {
        return c switch
        {
            '.' => new EmptySpace(row, col),
            '/' => new Mirror(row, col, c),
            '\\' => new Mirror(row, col, c),
            '|' => new Splitter(row, col, c),
            '-' => new Splitter(row, col, c),
            _ => throw new Exception("Invalid tile")
        };
    }
}