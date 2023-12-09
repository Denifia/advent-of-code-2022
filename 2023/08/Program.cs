// Haunted Wasteland

var lines = File.ReadAllLines("input.txt");
var (instructions, map) = ParseInput(lines);
var repeatingInstructions = RepeatingInstructions(instructions);

var starts = map.Where(x => x.Key.EndsWith("A")).Select(x => x.Key).ToArray();
var counts = starts.Select(x => WalkMap(x, repeatingInstructions, map)).ToArray();
var result = GetLowestCommonMultiple(counts);

// question 
Console.WriteLine($"Answer: {result}");

static long GetLowestCommonMultiple(int[] numbers)
{
    long result = numbers[0];
    for (int i = 1; i < numbers.Length; i++)
    {
        result = LowestCommonMultiple(result, numbers[i]);
    }
    return result;
}

static long GreatestCommonFactor(long a, long b)
{
    while (b != 0)
    {
        long temp = b;
        b = a % b;
        a = temp;
    }
    return a;
}

static long LowestCommonMultiple(long a, long b)
{
    var gcf = GreatestCommonFactor(a, b);
    Console.WriteLine($"Highest common factor of {a} and {b} is {gcf}");

    var lcm = (a / gcf) * b;
    Console.WriteLine($"Lowest common multiple of {a} and {b} is {lcm}");

    return lcm;
}

int WalkMap(string start, IEnumerable<int> repeatingInstructions, Dictionary<string, string[]> map)
{
    var count = 0;
    var row = start;
    foreach (var instruction in repeatingInstructions)
    {
        count++;
        row = map[row][instruction];
        if (row.EndsWith("Z"))
        {
            break;
        }
    }
    return count;
}

(int[], Dictionary<string, string[]>) ParseInput(string[] lines)
{
    var instructions = lines[0].Select(x => x == 'R' ? 1 : 0).ToArray();

    var letterMap = lines.Skip(2).ToDictionary(
        keySelector: line => line[..3],
        elementSelector: line => new string[] { line.Substring(7, 3), line.Substring(12, 3) });

    //(int, int)[] map = new (int, int)[lines.Length - 2];

    return (instructions, letterMap);
}

IEnumerable<int> RepeatingInstructions(int[] instructions)
{
    var index = 0;
    while (true)
    {
        yield return instructions[index];
        index = (index + 1) % instructions.Length;
    }
}
