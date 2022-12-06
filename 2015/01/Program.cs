// Day 1: Not Quite Lisp

var instructions = await File.ReadAllTextAsync("input.txt");

var finalLevel = instructions.Select(ProcessInstruction).Sum();

// To what floor do the instructions take Santa?
Console.WriteLine($"Part 1 Answer: {finalLevel}");

var currentLevel = 0;
var position = 0;
for (int i = 0; i < instructions.Length; i++)
{
    currentLevel += ProcessInstruction(instructions[i]);
    if (currentLevel == -1)
    {
        position = i + 1;
        break;
    }
}

// What is the position of the character that causes Santa to first enter the basement?
Console.WriteLine($"Part 2 Answer: {position}");

static int ProcessInstruction(char x)
{
    return x switch
    {
        '(' => 1,
        ')' => -1,
        _ => 0
    };
}