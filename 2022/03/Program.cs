// Day 3: Rucksack Reorganization 

const string items = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
var lines = await File.ReadAllLinesAsync("input.txt");

var totalPriority = 0;
foreach (var rucksack in lines)
{
    var halfwayPoint = (rucksack.Length / 2);
    var compartment1 = rucksack[..halfwayPoint].ToList();
    var compartment2 = rucksack[halfwayPoint..].ToList();
    totalPriority += items.IndexOf(compartment1.Intersect(compartment2).First()) + 1;
}

// Find the item type that appears in both compartments of each rucksack. What is the sum of the priorities of those item types?
Console.WriteLine($"Part 1 Answer: {totalPriority}");

totalPriority = 0;
for (int i = 0; i < lines.Length; i += 3)
{
    var badge = lines
        .Skip(i)
        .Take(3)
        .SelectMany((listOfItems, rucksack) => listOfItems.Distinct().ToArray().Select(item => (rucksack, item)))
        .GroupBy(x => x.item)
        .Single(x => x.Count() == 3)
        .Key;
    totalPriority += items.IndexOf(badge) + 1;
}

// Find the item type that corresponds to the badges of each three-Elf group. What is the sum of the priorities of those item types?
Console.WriteLine($"Part 2 Answer: {totalPriority}");