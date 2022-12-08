// Day 5: Supply Stacks

var lines = await File.ReadAllLinesAsync("input.txt");
var startingArrangement = lines[..8].Reverse().ToArray();
var instructions = lines[10..];

var stacksV1 = Enumerable.Range(0, 9).Select(_ => new Stack<char>()).ToArray();
var stacksV2 = Enumerable.Range(0, 9).Select(_ => new Stack<char>()).ToArray();
for (int i = 0; i < startingArrangement.Length; i++)
{
    for (int s = 0; s < startingArrangement[i].Length; s += 4)
    {
        var item = startingArrangement[i][s + 1];
        if (item == ' ') continue;
        stacksV1[s / 4].Push(item);
        stacksV2[s / 4].Push(item);
    }
}

// Move one at a time
foreach (var instruction in instructions)
{
    var parts = GetInstructionParts(instruction);

    for (int i = 0; i < parts[0]; i++)
    {
        stacksV1[parts[2] - 1].Push(stacksV1[parts[1] - 1].Pop());
    }
}
var topOfStacks = string.Join(string.Empty, stacksV1.Select(x => x.Peek()));

// After the rearrangement procedure completes, what crate ends up on top of each stack?
Console.WriteLine($"Part 1 Answer: {topOfStacks}");

// Move many at a time
foreach (var instruction in instructions)
{
    var parts = GetInstructionParts(instruction);

    var stackToMove = new List<char>();
    for (int i = 0; i < parts[0]; i++)
    {
        stackToMove.Add(stacksV2[parts[1] - 1].Pop());
    }
    stackToMove.Reverse();
    foreach (var item in stackToMove)
    {
        stacksV2[parts[2] - 1].Push(item);
    }
}
topOfStacks = string.Join(string.Empty, stacksV2.Select(x => x.Peek()));

// After the rearrangement procedure completes with the CrateMover 9001, what crate ends up on top of each stack?
Console.WriteLine($"Part 2 Answer: {topOfStacks}");

static int[] GetInstructionParts(string instruction)
{
    return instruction
        .Split(' ')
        .Select(x => int.TryParse(x, out var number) ? number : -1)
        .Where(x => x >= 0)
        .ToArray();
}