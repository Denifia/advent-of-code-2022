// Wait For It

var lines = File.ReadAllLines("input.txt");
var race = GetRacesFromInput(lines[0], lines[1]).ToArray().First();
var errorMargin = race.CalculateNumberOfWaysToWin();

Console.WriteLine($"Answer: {errorMargin}");

static IEnumerable<Race> GetRacesFromInput(string times, string distances)
{
    var separators = new char[] { ':' };
    var allowedTimes = times.Split(separators, StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(x => long.Parse(x.Replace(" ", ""))).ToList();
    var bestDistances = distances.Split(separators, StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(x => long.Parse(x.Replace(" ", ""))).ToList();

    for (int i = 0; i < allowedTimes.Count; i++)
    {
        yield return new Race(allowedTimes[i], bestDistances[i]);
    }
}

record Race(long AllowedTimeMs, long BestDistanceMs)
{
    public long CalculateNumberOfWaysToWin()
    {
        long firstWin = 0;
        for (long i = 0; i <= AllowedTimeMs; i++)
        {
            if (CanWin(i))
            {
                firstWin = i;
                break;
            }
        }

        long lastWin = 0;
        for (long i = AllowedTimeMs; i >= 0; i--)
        {
            if (CanWin(i))
            {
                lastWin = i;
                break;
            }
        }

        return lastWin - firstWin + 1;
    }

    private bool CanWin(long holdDuration) => ((AllowedTimeMs - holdDuration) * holdDuration) > BestDistanceMs;
}