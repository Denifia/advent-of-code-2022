// Day 3: Binary Diagnostic

var rows = File.ReadAllLines("input.txt");
var columns = new List<string>();
foreach (var row in rows)
{
    for (int i = 0; i < row.Length; i++)
    {
        if (columns.Count < i + 1) 
            columns.Add(string.Empty);

        columns[i] += row[i];
    }
}

var gammaRateBits = columns.Select(col => col.GroupBy(x => x).OrderByDescending(x => x.Count()).First().Key).ToArray();
var epsilonRateBits = gammaRateBits.Select(x => x switch
{
    '0' => '1',
    '1' => '0',
    _ => throw new NotSupportedException()
}).ToArray();

var gammaRate = Convert.ToInt32(new string(gammaRateBits), 2);
var epsilonRate = Convert.ToInt32(new string(epsilonRateBits), 2);

// What is the power consumption of the submarine?
Console.WriteLine($"Part 1 Answer: {gammaRate * epsilonRate}");

// question 2
//Console.WriteLine($"Part 2 Answer: {Convert.ToInt32(epsilonRate.ToString(), 2)}");