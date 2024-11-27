// Day 2: Bathroom Security


var lines = File.ReadAllLines("input.txt");

var keypad = new Keypad();
var code = new List<int>(lines.Length);
foreach (var line in lines)
{
    foreach (var instruction in line.AsSpan())
    {
        keypad.Move(instruction);
    }
    code.Add(keypad.CurrentKey);
}

// question 1
Console.WriteLine($"Part 1 Answer: {string.Join(null, code)}");

// question 2
Console.WriteLine($"Part 2 Answer: {true}");

public class Keypad
{
    public int CurrentKey => _keys[_keyLocation.Y][_keyLocation.X];
    private readonly KeyLocation _keyLocation = new(1, 1);
    private static readonly int[][] _keys = [[1, 2, 3], [4, 5, 6], [7, 8, 9]];

    public void Move(char input)
    {
        var direction = input switch
        {
            'U' => KeyDirection.Up,
            'D' => KeyDirection.Down,
            'L' => KeyDirection.Left,
            'R' => KeyDirection.Right,
            _ => throw new NotSupportedException()
        };
        Move(direction);
    }

    private void Move(KeyDirection direction)
    {
        Action move = direction switch
        {
            KeyDirection.Up => MoveUp,
            KeyDirection.Down => MoveDown,
            KeyDirection.Left => MoveLeft,
            KeyDirection.Right => MoveRight,
            _ => throw new NotSupportedException()
        };
        move();
    }

    private void MoveUp() => _keyLocation.Y = ConstrainMovement(_keyLocation.Y - 1);

    private void MoveDown() => _keyLocation.Y = ConstrainMovement(_keyLocation.Y + 1);

    private void MoveLeft() => _keyLocation.X = ConstrainMovement(_keyLocation.X - 1);

    private void MoveRight() => _keyLocation.X = ConstrainMovement(_keyLocation.X + 1);

    private int ConstrainMovement(int input) => Math.Clamp(input, 0, 2);

    public enum KeyDirection
    {
        Up,
        Down,
        Left,
        Right,
    }

    private class KeyLocation
    {
        public KeyLocation(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; set; }
        public int Y { get; set; }
    }
}