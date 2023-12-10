// Pipe Maze

using System.Data;

var lines = File.ReadAllLines("input.txt");
var grid = new Grid(lines);
var startingNode = grid.FindStartingNode();
grid.UpdateNode(startingNode, '|');

var loop = new List<Node>() 
{ 
    startingNode 
};

var currentNode = startingNode;
currentNode = currentNode with { Tile = '|' };
var starting = true;

try
{
    while (true)
    {
        var availableNodes = grid.AvailableMoves(currentNode).ToArray();
        var nextNode = starting ? availableNodes.First() : availableNodes.First(node => loop[^2] != node);
        starting = false;

        //Console.WriteLine($"Next node is {nextNode.Tile} at {nextNode.Coordinates.Row} {nextNode.Coordinates.Column}");

        if (nextNode.Coordinates == loop[0].Coordinates)
        {
            // back at the start of the loop
            break;
        }

        loop.Add(nextNode);
        currentNode = nextNode;
    }
}
catch (Exception ex)
{
    grid.Dump(loop.ToArray());
}

Console.WriteLine($"Answer: {loop.Count / 2}");

// zero based row and column
record Node(char Tile, Coordinates Coordinates)
{
    public (Direction, Coordinates) North = (Direction.North, Coordinates with { Row = Coordinates.Row - 1 });
    public (Direction, Coordinates) East = (Direction.East, Coordinates with { Column = Coordinates.Column + 1 });
    public (Direction, Coordinates) South = (Direction.South, Coordinates with { Row = Coordinates.Row + 1 });
    public (Direction, Coordinates) West = (Direction.West, Coordinates with { Column = Coordinates.Column - 1 });

    public IEnumerable<(Direction, Coordinates)> CardinalCoordinates()
    {
        yield return North;
        yield return East;
        yield return South;
        yield return West;
    }
}

enum Direction
{
    North,
    East,
    South,
    West
}

record Coordinates(int Row, int Column);

static class Tiles
{
    public const char Start = 'S';
    public const char NorthSouth = '|';
    public const char EastWest = '-';
    public const char NorthEast = 'L';
    public const char NorthWest = 'J';
    public const char SouthWest = '7';
    public const char SouthEast = 'F';
    public const char Ground = '.';
}

class Grid
{
    private readonly string[] _lines;
    private readonly char[][] _grid;

    public Grid(string[] lines)
    {
        _lines = lines;
        _grid = lines.Select(x => x.ToArray()).ToArray();
    }

    public IEnumerable<Node> AvailableMoves(Node node)
    {
        List<Node> nodes = [];

        foreach (var (direction, coordinate) in node.CardinalCoordinates())
        {
            if (!WithinGrid(coordinate))
                continue;

            var cardinalNode = GetNode(coordinate);

            bool validSource = (node.Tile, direction) switch
            {
                (Tiles.NorthSouth, Direction.North) => true,
                (Tiles.NorthEast, Direction.North) => true,
                (Tiles.NorthWest, Direction.North) => true,

                (Tiles.NorthSouth, Direction.South) => true,
                (Tiles.SouthEast, Direction.South) => true,
                (Tiles.SouthWest, Direction.South) => true,

                (Tiles.EastWest, Direction.East) => true,
                (Tiles.NorthEast, Direction.East) => true,
                (Tiles.SouthEast, Direction.East) => true,

                (Tiles.EastWest, Direction.West) => true,
                (Tiles.NorthWest, Direction.West) => true,
                (Tiles.SouthWest, Direction.West) => true,

                _ => false
            }; ;

            bool validDestination = (direction, cardinalNode.Tile) switch
            {
                (Direction.North, Tiles.NorthSouth) => true,
                (Direction.North, Tiles.SouthEast) => true,
                (Direction.North, Tiles.SouthWest) => true,

                (Direction.South, Tiles.NorthSouth) => true,
                (Direction.South, Tiles.NorthEast) => true,
                (Direction.South, Tiles.NorthWest) => true,

                (Direction.East, Tiles.EastWest) => true,
                (Direction.East, Tiles.NorthWest) => true,
                (Direction.East, Tiles.SouthWest) => true,
                 
                (Direction.West, Tiles.EastWest) => true,
                (Direction.West, Tiles.NorthEast) => true,
                (Direction.West, Tiles.SouthEast) => true,

                _ => false
            };

            if (validSource && validDestination) yield return cardinalNode;
        }
    }

    private bool WithinGrid(Coordinates coordinates)
    {
        if (coordinates.Row < 0 || coordinates.Column < 0) return false;
        if (coordinates.Row >= _grid.Length || coordinates.Column >= _grid[0].Length) return false;
        return true;
    }

    public Node GetNode(Coordinates coordinates) => new Node(_grid[coordinates.Row][coordinates.Column], coordinates);

    //public Node MoveTo(Coordinates coordinates) => new Node(_grid[coordinates.Row][coordinates.Column], coordinates);

    public void UpdateNode(Node node, char newTile)
    {
        _grid[node.Coordinates.Row][node.Coordinates.Column] = newTile;
    }

    public Node FindStartingNode()
    {
        for (int row = 0; row < _lines.Length; row++)
        {
            if (_lines[row].Contains(Tiles.Start))
            {
                var column = _lines[row].IndexOf(Tiles.Start);
                return new Node(Tiles.Start, new Coordinates(row, column));
            }
        }

        throw new KeyNotFoundException();
    }

    public void Dump(Node[] nodes)
    {
        for (int row = 0; row < _grid.Length; row++)
        {
            for (int column = 0; column < _grid[row].Length; column++)
            {
                _grid[row][column] = '.';
            }
        }

        foreach (var node in nodes)
        {
            _grid[node.Coordinates.Row][node.Coordinates.Column] = node.Tile;
        }

        File.WriteAllLines("output.txt", _grid.Select(x => string.Join(string.Empty, x)).ToArray());
    }

    //public Node DetectStratingNodeRealTile(Node startingNode)
    //{
    //    var nodes = startingNode.CardinalCoordinates().Select(x => (x.Item1, GetNode(x.Item2))).ToArray();

        
    //}
}

