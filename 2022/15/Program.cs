// Day 15: Beacon Exclusion Zone

using System.Drawing;

var lines = File.ReadAllLines("input.txt");
var beacons = new HashSet<Point>();
var sensors = new Dictionary<Point, int>();
foreach (var line in lines)
{
    var parts = line.Split(' ').ToArray();
    var sensor = new Point(int.Parse(parts[2][2..^1]), int.Parse(parts[3][2..^1]));
    var beacon = new Point(int.Parse(parts[8][2..^1]), int.Parse(parts[9][2..]));
    sensors.Add(sensor, ManhattanDistance(sensor, beacon));
    beacons.Add(beacon);
}

var allPoints = sensors.Keys.Concat(beacons);
var minX = sensors.Min(point => point.Key.X - point.Value);
var maxX = sensors.Max(point => point.Key.X + point.Value);
var minY = sensors.Min(point => point.Key.Y - point.Value);
var maxY = sensors.Max(point => point.Key.Y + point.Value);

//var rowToCheck = 10;
var rowToCheck = 2000000;
var positions = 0;
for (var x = minX; x <= maxX; x++)
{
    var testPoint = new Point(x, rowToCheck);

    if (beacons.Contains(testPoint))
        continue;

    if (sensors.ContainsKey(testPoint))
        continue;

    if (sensors.Any(sensor => ManhattanDistance(testPoint, sensor.Key) <= sensor.Value))
        positions++;
}

// In the row where y=2000000, how many positions cannot contain a beacon?
Console.WriteLine($"Part 1 Answer: {positions}");

// Below is O(n) for the 16 trillion possible locations - gotta find something better!

//var max = 20;
var max = 4000000;
Point distressBeacon = Point.Empty;
for (int x = 0; x <= max; x++)
{
    for (int y = 0; y <= max; y++)
    {
        var testPoint = new Point(x, y);
        if (sensors.Any(sensor => ManhattanDistance(testPoint, sensor.Key) <= sensor.Value))
            continue;

        distressBeacon = testPoint;
        break;
    }

    if (distressBeacon != Point.Empty) break;
}

// Find the only possible position for the distress beacon. What is its tuning frequency?
Console.WriteLine($"Part 2 Answer: {(distressBeacon.X * 4000000) + distressBeacon.Y}");

static int ManhattanDistance(Point p1, Point p2) => Math.Abs(p1.X - p2.X) + Math.Abs(p1.Y - p2.Y);