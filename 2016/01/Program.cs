// Day 1: No Time for a Taxicab

var input = File.ReadAllText("input.txt");
var position = new Position();
position.FollowInstructions(input.AsSpan());

// question 1
//Console.WriteLine($"Part 1 Answer: {position._distance}");

// question 2
// nested in Position class


enum Direction
{
    North,
    East,
    South,
    West
}

class Position
{
    HashSet<(int x, int y)> _visitedLocations = new();
    private int _x;
    private int _y;
    private int _distance;
    private Direction _direction = Direction.North;

    public int GetStandardiseDistance() => Math.Abs(_distance);

    public void FollowInstructions(ReadOnlySpan<char> instructions)
    {
        foreach (var range in instructions.Split(", "))
        {
            var instruction = instructions[range];
            FollowInstruction(instruction);
        }
    }

    private void FollowInstruction(ReadOnlySpan<char> instruction)
    {
        Turn(instruction);

        var moveDistance = int.Parse(instruction.Slice(1));
        for (int i = 1; i <= moveDistance; i++)
        {
            Move();
            HaveWeVisitedThisLocation();  
        }
    }

    private void HaveWeVisitedThisLocation()
    {
        if (_visitedLocations.Contains((_x, _y)))
        {
            Console.WriteLine($"Part 2 Answer: {GetStandardiseDistance()}");
            Environment.Exit(0);
        }
        _visitedLocations.Add((_x, _y));
    }

    private void Move(int moveDistance = 1)
    {
        
        switch (_direction)
        {
            case Direction.North:
                _distance += moveDistance;
                _y += moveDistance;
                break;
            case Direction.East:
                _distance += moveDistance;
                _x += moveDistance;
                break;
            case Direction.South:
                _distance -= moveDistance;
                _y -= moveDistance;
                break;
            case Direction.West:
                _distance -= moveDistance;
                _x -= moveDistance;
                break;
            default:
                throw new NotImplementedException();
        }
    }

    private void Turn(ReadOnlySpan<char> turnDirection)
    {
        _direction = turnDirection[0] switch
        {
            'R' => GetTurnRightDirection(),
            'L' => GetTurnLeftDirection(),
            _ => throw new NotImplementedException(),
        };
    }

    private Direction GetTurnLeftDirection()
    {
        return _direction switch
        {
            Direction.North => Direction.West,
            Direction.West => Direction.South,
            Direction.South => Direction.East,
            Direction.East => Direction.North,
            _ => throw new NotImplementedException(),
        };
    }

    private Direction GetTurnRightDirection()
    {
        return _direction switch
        {
            Direction.North => Direction.East,
            Direction.East => Direction.South,
            Direction.South => Direction.West,
            Direction.West => Direction.North,
            _ => throw new NotImplementedException(),
        };
    }
}
