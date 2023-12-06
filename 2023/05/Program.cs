// If You Give A Seed A Fertilizer

var lines = File.ReadAllLines("input.txt");
var (seeds, maps) = ParseInput(lines, useSeedRanges: true);
var lowestLocation = GetLowestLocations(seeds, maps);

// question 1
Console.WriteLine($"Part 1 Answer: {lowestLocation}");

// question 2
Console.WriteLine($"Part 2 Answer: {true}");

static (IEnumerable<long> seeds, List<Map> maps) ParseInput(string[] lines, bool useSeedRanges)
{
    var seeds = useSeedRanges
        ? GetSeedListFromFirstLineAsRanges(lines)
        : GetSeedListFromFirstLine(lines);

    List<Map> maps = [];
    var mapIndex = -1;

    foreach (var line in lines.Skip(1))
    {
        if (string.IsNullOrWhiteSpace(line))
        {
            continue;
        }

        if (line.EndsWith("map:"))
        {
            maps.Add(new Map());
            mapIndex++;
            continue;
        }

        AddToMap(maps[mapIndex], line);
    }

    return (seeds, maps);
}

static IEnumerable<long> GetSeedListFromFirstLine(string[] lines) 
    => lines.First()["seeds: ".Length..].Split(" ").Select(long.Parse).ToArray();

static IEnumerable<long> GetSeedListFromFirstLineAsRanges(string[] lines)
{
    var numbers = lines.First()["seeds: ".Length..].Split(" ").Select(long.Parse).ToArray();

    for (var i = 0; i < numbers.Length; i += 2)
    {
        var seedStart = numbers[i];
        var seedRange = numbers[i + 1];

        Console.WriteLine($"start = {seedStart}, range = {seedRange}");

        for (var j = 0; j < seedRange; j++) 
        { 
            yield return seedStart + j;
        }
    }
}

static void AddToMap(Map map, string line)
{
    var parts = line.Split(" ");
    var destinationStart = long.Parse(parts[0]);
    var sourceStart = long.Parse(parts[1]);
    var range = long.Parse(parts[2]);
    map.Ranges.Add(new Range(sourceStart, sourceStart + range, destinationStart - sourceStart));
}

static long GetLowestLocations(IEnumerable<long> seeds, List<Map> maps)
{
    long minLocation = 0;

    foreach (var seed in seeds)
    {
        var source = seed;
        foreach (var map in maps)
        {
            source = GetFromMapOrSame(source, map);
        }

        if (minLocation == 0 || source <  minLocation)
        {
            minLocation = source;
            Console.WriteLine($"new min location of {minLocation}");
        }
    }

    return minLocation;

    static long GetFromMapOrSame(long source, Map map)
    {
        var range = map.Ranges.FirstOrDefault(x => source >= x.Start && source <= x.End);

        if (range is null)
        {
            return source;
        }

        return source + range.Offset;
    }
}

class Map()
{
    public List<Range> Ranges { get; set; } = [];
}

record Range(long Start, long End, long Offset)
{
    public long Total = End - Start;
}

record Chunk(long Start, long End);