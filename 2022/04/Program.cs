// Day 4: Camp Cleanup

var lines = await File.ReadAllLinesAsync("input.txt");

var containedPairs = 0;
foreach (var line in lines)
{
    // Fully Overlap
    // line example "1-4,2-3"
    var assignments = line.Split(',').Select(x =>
    {
        var assignment = x.Split('-');
        return (int.Parse(assignment[0]), int.Parse(assignment[1]));
    }).ToArray();

    if (assignments[0].Item1 <= assignments[1].Item1 && assignments[0].Item2 >= assignments[1].Item2)
    {
        containedPairs++;
    }
    else if (assignments[1].Item1 <= assignments[0].Item1 && assignments[1].Item2 >= assignments[0].Item2)
    {
        containedPairs++;
    }
}

// In how many assignment pairs does one range fully contain the other?
Console.WriteLine($"Part 1 Answer: {containedPairs}");

containedPairs = 0;
foreach (var line in lines)
{
    // Partial Overlap
    // line example "1-2,2-3"
    var assignments = line.Split(',').Select(x =>
    {
        var assignment = x.Split('-');
        return (int.Parse(assignment[0]), int.Parse(assignment[1]));
    }).ToArray();

    if (assignments[0].Item1 <= assignments[1].Item1 && assignments[0].Item2 >= assignments[1].Item1)
    {
        containedPairs++;
    }
    else if (assignments[1].Item1 <= assignments[0].Item1 && assignments[1].Item2 >= assignments[0].Item1)
    {
        containedPairs++;
    }
}

// In how many assignment pairs do the ranges overlap?
Console.WriteLine($"Part 2 Answer: {containedPairs}");