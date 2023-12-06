// If You Give A Seed A Fertilizer

var lines = File.ReadAllLines("input.txt");
var (seeds, maps) = ParseInput(lines);
var lowestLocation = GetLowestLocations(seeds, maps);
Console.WriteLine($"Answer: {lowestLocation}");

static (Chunk[] seeds, List<Map> maps) ParseInput(string[] lines)
{
    var seeds = GetSeedListFromFirstLineAsRanges(lines);

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

static Chunk[] GetSeedListFromFirstLineAsRanges(string[] lines)
{
    var numbers = lines.First()["seeds: ".Length..].Split(" ").Select(long.Parse).ToArray();

    List<Chunk> chunks = [];

    for (var i = 0; i < numbers.Length; i += 2)
    {
        var seedStart = numbers[i];
        var seedRange = numbers[i + 1];

        chunks.Add(new Chunk(seedStart, seedStart + seedRange));
    }

    return chunks.ToArray();
}

static void AddToMap(Map map, string line)
{
    var parts = line.Split(" ");
    var destinationStart = long.Parse(parts[0]);
    var sourceStart = long.Parse(parts[1]);
    var range = long.Parse(parts[2]);
    map.Ranges.Add(new Range(sourceStart, sourceStart + range - 1, destinationStart - sourceStart));
}

static long GetLowestLocations(Chunk[] seedChunks, List<Map> maps)
{
    foreach (var map in maps)
    {
        seedChunks = map.ProcessChunks(seedChunks);
    }

    return seedChunks.Min(x => x.Start);
}

class Map()
{
    public List<Range> Ranges { get; set; } = [];

    internal Chunk[] ProcessChunks(Chunk[] seedChunks)
    {
        List<Chunk> chunks = [];
        foreach (var chunk in seedChunks)
        {
            for (var i = chunk.Start; i <= chunk.End; i++)
            {
                // Covered by a range?
                var range = Ranges.FirstOrDefault(x => i >= x.Start && i <= x.End);
                if (range is not null)
                {
                    var chunkEnd = Math.Min(chunk.End, range.End);
                    chunks.Add(new Chunk(i, chunkEnd, range.Offset));
                    i = chunkEnd;
                    continue;
                }

                // Not covered by a range?
                var nextRange = Ranges.OrderBy(x => x.Start).FirstOrDefault(x => x.Start > i);
                if (nextRange is not null)
                {
                    var chunkEnd = Math.Min(chunk.End, nextRange.Start);
                    chunks.Add(new Chunk(i, chunkEnd));
                    i = chunkEnd;
                    continue;
                }

                // No future ranges to be caught by
                chunks.Add(new Chunk(i, chunk.End));
                break;
            }
        }

        return [.. chunks];
    }
}

record Range(long Start, long End, long Offset)
{
    public long Total = End - Start;
}

record Chunk(long Start, long End)
{
    public Chunk(long start, long end, long offset)
        : this(start + offset, end + offset)
    {
    }
}