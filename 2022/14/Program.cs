// Day 14: Regolith Reservoir

var lines = File.ReadAllLines("input.txt");
var spots = new Dictionary<(int x, int y), char>();
foreach (var line in lines)
{
    var coords = line.Split(" -> ").Select(x => x.Split(',').Select(int.Parse).ToArray()).ToArray();
	spots.TryAdd((coords[0][0], coords[0][1]), '#');
	for (var i = 1; i < coords.Length; i++)
	{
		var xaxis = coords[i - 1][0].CompareTo(coords[i][0]);
        var yaxis = coords[i - 1][1].CompareTo(coords[i][1]);
		var newSpots = (xaxis, yaxis) switch
		{
			(0, -1) => Enumerable.Range(coords[i - 1][1], coords[i][1] - coords[i - 1][1] + 1).Select(c => (coords[i][0], c)).ToArray(),
            (0, 1) => Enumerable.Range(coords[i][1], coords[i - 1][1] - coords[i][1] + 1).Select(c => (coords[i][0], c)).ToArray(),
            (-1, 0) => Enumerable.Range(coords[i - 1][0], coords[i][0] - coords[i - 1][0] + 1).Select(c => (c, coords[i][1])).ToArray(),
            (1, 0) => Enumerable.Range(coords[i][0], coords[i - 1][0] - coords[i][0] + 1).Select(c => (c, coords[i][1])).ToArray(),
            _ => Array.Empty<(int, int)>()
		};
		foreach (var newSpot in newSpots)
		{
            spots.TryAdd(newSpot, '#');
        }
	}
}

var sandCountToVoid = 0;
var sandCountToRoof = 0;
var voidDepth = spots.Keys.Select(key => key.y).Max() + 1;
var fallingForever = false;
var sandAtRoof = false;
var floorY = voidDepth + 1;
while (!sandAtRoof)
{
    sandCountToRoof++;
    (int x, int y) lastSpot = (500, 0);
    var atRest = false;
    do
    {
        if (!spots.ContainsKey((lastSpot.x, lastSpot.y + 1)))
        {
            lastSpot = (lastSpot.x, lastSpot.y + 1);
        }
        else if (!spots.ContainsKey((lastSpot.x - 1, lastSpot.y + 1)))
        {
            lastSpot = (lastSpot.x - 1, lastSpot.y + 1);
        }
        else if (!spots.ContainsKey((lastSpot.x + 1, lastSpot.y + 1)))
        {
            lastSpot = (lastSpot.x + 1, lastSpot.y + 1);
        }
        else
        {
            atRest = true;
        }
        if (lastSpot.y >= voidDepth)
        {
            if (!fallingForever)
            {
                sandCountToVoid = sandCountToRoof - 1;
            }
            fallingForever = true;
        }
        if (lastSpot.y == floorY - 1)
        {
            atRest = true;
        }
        if (lastSpot == (500, 0))
        {
            sandAtRoof = true;
        }
    } while (!atRest);
    spots.TryAdd(lastSpot, 'o');
}

// How many units of sand come to rest before sand starts flowing into the abyss below?
Console.WriteLine($"Part 1 Answer: {sandCountToVoid}");

// Using your scan, simulate the falling sand until the source of the sand becomes blocked. How many units of sand come to rest?
Console.WriteLine($"Part 2 Answer: {sandCountToRoof}");