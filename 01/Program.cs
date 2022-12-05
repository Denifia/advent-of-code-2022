var elfs = new List<int>();
var lines = await File.ReadAllLinesAsync("input.txt");
var currentElf = new List<int>();
var newElfTime = true;
for (int i = 0; i < lines.Length; i++)
{
    if (string.IsNullOrEmpty(lines[i]))
    {
        newElfTime = true;
        continue;
    }

    if (newElfTime && currentElf.Any())
    {
        elfs.Add(currentElf.Sum());
        currentElf.Clear();
    }

    currentElf.Add(int.Parse(lines[i]));
    newElfTime = false;
}
Console.WriteLine($"Part 1 Answer: {elfs.OrderByDescending(x => x).First()}");
Console.WriteLine($"Part 2 Answer: {elfs.OrderByDescending(x => x).Take(3).Sum()}");