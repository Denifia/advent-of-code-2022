// Day 2: Bathroom Security

var lines = File.ReadAllLines("input.txt");

// question 1
Console.WriteLine($"Part 1 Answer: {ConvertInstructionsToCode(lines, new Keypad())}");

// question 2
Console.WriteLine($"Part 2 Answer: {ConvertInstructionsToCode(lines, new SpecialKeypad())}");

string ConvertInstructionsToCode(string[] instructions, IKeypad keypad)
{
    var code = new List<char>(lines.Length);
    foreach (var line in lines)
    {
        foreach (var instruction in line.AsSpan())
        {
            keypad.Move(instruction);
        }
        code.Add(keypad.CurrentKey);
    }
    return string.Join(null, code);
}

public interface IKeypad
{
    char CurrentKey { get; }

    void Move(char input);
}

public class Keypad : IKeypad
{
    public char CurrentKey => _keys[_keyLocation.Y][_keyLocation.X];
    private readonly KeyLocation _keyLocation = new(1, 1);
    private static readonly char[][] _keys = 
        [['1', '2', '3'], 
         ['4', '5', '6'], 
         ['7', '8', '9']];

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

public class SpecialKeypad : IKeypad
{
    public char CurrentKey => _keys[_keyLocation.Y][_keyLocation.X];
    private readonly KeyLocation _keyLocation = new(0, 2);
    private static readonly char[][] _keys = 
        [[' ', ' ', '1', ' ', ' '], 
         [' ', '2', '3', '4', ' '], 
         ['5', '6', '7', '8', '9'], 
         [' ', 'A', 'B', 'C', ' '], 
         [' ', ' ', 'D', ' ', ' ']];
    private static readonly (int index, int lowerLimit, int upperLimit)[] movementLimits = [(0, 2, 2), (1, 1, 3), (2, 0, 4), (3, 1, 3), (4, 2, 2)];

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

    private void MoveUp() => _keyLocation.Y = ConstrainMovement(_keyLocation.Y - 1, _keyLocation.X);

    private void MoveDown() => _keyLocation.Y = ConstrainMovement(_keyLocation.Y + 1, _keyLocation.X);

    private void MoveLeft() => _keyLocation.X = ConstrainMovement(_keyLocation.X - 1, _keyLocation.Y);

    private void MoveRight() => _keyLocation.X = ConstrainMovement(_keyLocation.X + 1, _keyLocation.Y);

    private int ConstrainMovement(int input, int index) => Math.Clamp(input, movementLimits[index].lowerLimit, movementLimits[index].upperLimit);

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
