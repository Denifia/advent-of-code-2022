// Lens Library

var input = File.ReadAllText("input.txt");
var steps = input.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(Step.Parse);
var boxes = new List<Box>();
for (int i = 0; i < 256; i++)
{
    boxes.Add(new Box());
}

foreach (var step in steps)
{
    boxes[step.LabelHashCode].FollowStep(step);
}

var focusingPower = 0;
for (int i = 0; i < 256; i++)
{
    int boxNumber = i + 1;
    for (int j = 0; j < boxes[i].Lenses.Count; j++)
    {
        int lensNumber = j + 1;
        focusingPower += boxNumber * lensNumber * boxes[i].Lenses.ElementAt(j).Value;
    }   
}

// question 1
Console.WriteLine($"Part 1 Answer: {steps.Select(x => x.HashCode).Sum()}");

// question 2
Console.WriteLine($"Part 2 Answer: {focusingPower}");

class Step
{
    private string Instruction { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public char Operation { get; set; } = ' ';
    public int FocalLength { get; set; } = 0;
    public int HashCode { get; set; } = 0;
    public int LabelHashCode { get; set; } = 0;

    internal static readonly char[] separator = ['-', '='];

    public static Step Parse(string instruction)
    {
        var step = new Step
        {
            Instruction = instruction,
            Label = instruction.Split(separator, StringSplitOptions.RemoveEmptyEntries)[0],
            Operation = instruction.IndexOf('-') > -1 ? '-' : '=',
        };

        if (step.Operation == '=')
        {
            step.FocalLength = int.Parse(instruction.Split('=')[1]);
        }

        step.HashCode = step.GetHashCode(instruction);
        step.LabelHashCode = step.GetHashCode(step.Label);

        return step;
    }

    public int GetHashCode(string input)
    {
        int currentValue = 0;

        foreach (var character in input)
        {
            currentValue += character;
            currentValue = currentValue * 17;
            currentValue = currentValue % 256;
        }

        return currentValue;
    }
}

class Box
{
    public Dictionary<string, int> Lenses { get; set; } = new Dictionary<string, int>();

    public void FollowStep(Step step)
    {
        if (step.Operation == '-')
        {
            // Remove lens
            Lenses.Remove(step.Label);
            return;
        }

        if (step.Operation == '=')
        {
            if (Lenses.ContainsKey(step.Label))
            {
                // Update lens
                Lenses[step.Label] = step.FocalLength;
                return;
            }

            // Add lens
            Lenses = Lenses.Append(new KeyValuePair<string, int>(step.Label, step.FocalLength)).ToDictionary();
            return;
        }
    }
}
