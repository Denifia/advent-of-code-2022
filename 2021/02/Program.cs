// Day 2: Dive!

var instructions = File.ReadAllLines("input.txt")
    .Select<string, Instruction>(x => x.Split(' ') switch
    {
        ["up", var amount] => new MoveUpInstruction(amount),
        ["down", var amount] => new MoveDownInstruction(amount),
        ["forward", var amount] => new MoveForwardInstruction(amount),
        _ => throw new NotImplementedException()
    })
    .ToArray();

var submarine = new Submarine();
var submarineV2 = new SubmarineV2();
foreach (var instruction in instructions)
{
    submarine.Execute(instruction);
    submarineV2.Execute(instruction);
}

// What do you get if you multiply your final horizontal position by your final depth?
Console.WriteLine($"{submarine.Depth * submarine.HorizontalPosition}");

// What do you get if you multiply your final horizontal position by your final depth?
Console.WriteLine($"{submarineV2.Depth * submarineV2.HorizontalPosition}");

class Submarine
{
    public int Depth { get; protected set; }
    public int HorizontalPosition { get; protected set; }

    public virtual void MoveUp(int amount) => Depth -= amount;
    public virtual void MoveDown(int amount) => Depth += amount;
    public virtual void MoveForward(int amount) => HorizontalPosition += amount;
    public virtual void Execute(Instruction instruction) => instruction.Execute(this);
}

class SubmarineV2 : Submarine
{
    public int Aim { get; protected set; }

    public override void MoveUp(int amount) => Aim -= amount;
    public override void MoveDown(int amount) => Aim += amount;
    public override void MoveForward(int amount)
    {
        HorizontalPosition += amount;
        Depth += Aim * amount;
    }
}

abstract class Instruction
{
    protected readonly int amount;
    public Instruction(string amount) => this.amount = int.Parse(amount);
    public abstract void Execute(Submarine submarine);
}

class MoveUpInstruction : Instruction
{
    public MoveUpInstruction(string amount) : base(amount) { }
    public override void Execute(Submarine submarine) => submarine.MoveUp(amount);
}

class MoveDownInstruction : Instruction
{
    public MoveDownInstruction(string amount) : base(amount) { }
    public override void Execute(Submarine submarine) => submarine.MoveDown(amount);
}

class MoveForwardInstruction : Instruction
{
    public MoveForwardInstruction(string amount) : base(amount) { }
    public override void Execute(Submarine submarine) => submarine.MoveForward(amount);
}