// Lens Library

var input = File.ReadAllText("input.txt");
var sum = input.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => Step.Parse(x).GetHashCode()).Sum();

// question 1
Console.WriteLine($"Part 1 Answer: {sum}");

// question 2
Console.WriteLine($"Part 2 Answer: {true}");

class Step
{
    private string Instruction { get; set; } = string.Empty;

    public static Step Parse(string instruction)
    {
        return new Step
        {
            Instruction = instruction
        };
    }

    public override int GetHashCode()
    {
        int currentValue = 0;

        foreach (var character in Instruction)
        {
            currentValue += character;
            currentValue = currentValue * 17;
            currentValue = currentValue % 256;
        }

        return currentValue;
    }
}