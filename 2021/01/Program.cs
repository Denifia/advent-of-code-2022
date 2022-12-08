// Day 1: Sonar Sweep

var measurements = File.ReadAllLines("input.txt").Select(int.Parse).ToArray();
var increases = 0;
for (int i = 1; i < measurements.Length; i++)
{
    increases += measurements[i] > measurements[i - 1] ? 1 : 0;
}

// How many measurements are larger than the previous measurement?
Console.WriteLine($"Part 1 Answer: {increases}");

increases = 0;
for (int i = 1; i < measurements.Length - 2; i++)
{
    increases += measurements[i..(i+3)].Sum() > measurements[(i-1)..(i+2)].Sum() ? 1 : 0;
}

// Consider sums of a three-measurement sliding window. How many sums are larger than the previous sum?
Console.WriteLine($"Part 2 Answer: {increases}");