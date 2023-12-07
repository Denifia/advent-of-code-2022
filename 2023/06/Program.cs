// Title

var lines = File.ReadAllLines("input.txt");
var races = GetRacesFromInput(lines[0], lines[1]);
var errorMargin = races
            .Select(r => r.CalculateNumberOfWaysToWin())
            .Aggregate(1, (total, waysToWin) => total * waysToWin);

// question 1
Console.WriteLine($"Part 1 Answer: {errorMargin}");

// question 2
Console.WriteLine($"Part 2 Answer: {true}");

static IEnumerable<Race> GetRacesFromInput(string times, string distances)
{
    var separators = new char[] { ':', ' ' };
    var allowedTimes = times.Split(separators, StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(int.Parse).ToList();
    var bestDistances = distances.Split(separators, StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(int.Parse).ToList();

    for (int i = 0; i < allowedTimes.Count; i++)
    {
        yield return new Race(allowedTimes[i], bestDistances[i]);
    }
}

record Race(int AllowedTimeMs, int BestDistanceMs)
{
    public int CalculateNumberOfWaysToWin() => Enumerable.Range(0, AllowedTimeMs).Count(CanWin);
    private bool CanWin(int holdDuration) => ((AllowedTimeMs - holdDuration) * holdDuration) > BestDistanceMs;
}