// Day 10: Cathode-Ray Tube

using System.Text;

var lines = File.ReadAllLines("input.txt");

var registerX = new Register();
var instructions = new Queue<Instruction>();
foreach (var item in lines)
{
    var parts = item.Split(' ');
    instructions.Enqueue(parts switch
    {
        ["noop", ..] => new NoOp(registerX),
        ["addx", var amount] => new AddX(registerX, amount),
        _ => throw new NotSupportedException()
    });
}

var signalStrength = 0;
var signalCycles = new int[] { 20, 60, 100, 140, 180, 220 };
var cycle = 1;
var spritePosition = new int[] { 0, 1, 2 };
var displayBuffer = new StringBuilder();
var currentRow = 0;

// CPU Cycles
while (instructions.TryPeek(out var instruction))
{
    // During Cycle
    instruction.CycleExecute();
    CheckSignalStrength(cycle);
    UpdateDisplay(cycle);

    // After Cycle
    instruction.PostCycleExecute();
    UpdateSpritePosition(registerX);

    if (instruction.Completed)
        _ = instructions.Dequeue();

    cycle++;
}

void CheckSignalStrength(int cycle)
{
    if (signalCycles.Contains(cycle))
    {
        signalStrength += (cycle * registerX.Value);
    }
}

void UpdateSpritePosition(Register register)
{
    spritePosition[0] = register.Value - 1;
    spritePosition[1] = register.Value;
    spritePosition[2] = register.Value + 1;
}

void UpdateDisplay(int cycle)
{
    var col = (cycle - 1) % 40;
    var row = (cycle - 1) / 40;

    if (row > currentRow)
    {
        displayBuffer.AppendLine();
        currentRow = row;
    }

    var pixel = spritePosition.Contains(col) ? '#' : '.';
    displayBuffer.Append(pixel);
}

// Find the signal strength during the 20th, 60th, 100th, 140th, 180th, and 220th cycles. What is the sum of these six signal strengths?
Console.WriteLine($"Part 1 Answer: {signalStrength}");

// Render the image given by your program. What eight capital letters appear on your CRT?
Console.WriteLine("Part 2 Answer:");
Console.WriteLine(displayBuffer); // Renders PAPJCBHP

abstract class Instruction
{
    protected readonly Register _register;

    protected Instruction(Register register)
    {
        _register = register;
    }

    protected int _cyclesToComplete;
    protected int _currentCycle;
    public bool Completed => _cyclesToComplete == _currentCycle;

    public void CycleExecute()
    {
        _currentCycle++;
    }

    public abstract void PostCycleExecute();
}

class AddX : Instruction
{
    private readonly int _amount;

    public AddX(Register register, string amount) : base(register)
    {
        _cyclesToComplete = 2;
        _amount = int.Parse(amount);
    }

    public override void PostCycleExecute()
    {
        if (_cyclesToComplete == _currentCycle)
        {
            _register.Value += _amount;
        }
    }
}

class NoOp : Instruction
{
    public NoOp(Register register) : base(register)
    {
        _cyclesToComplete = 1;
    }

    public override void PostCycleExecute()
    {
        // noop
    }
}

class Register
{
    public int Value { get; set; } = 1;
}