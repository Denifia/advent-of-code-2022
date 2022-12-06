// Day 2: I Was Told There Would Be No Math

var lines = await File.ReadAllLinesAsync("input.txt");

var presents = lines.Select(x =>
    {
        var sides = x.Split('x').Select(y => int.Parse(y)).ToArray();
        var faces = new int[]
        {
            sides[0] * sides[1],
            sides[1] * sides[2],
            sides[2] * sides[0]
        };
        var area = sides.Aggregate(1, (a, b) => a * b);
        var perimeters = new int[]
        {
            sides[0] + sides[1],
            sides[1] + sides[2],
            sides[2] + sides[0]
        };
        return ((2 * faces[0]) + (2 * faces[1]) + (2 * faces[2]) + faces.Min(), (perimeters.Order().First() * 2) + area);
    });

// How many total square feet of wrapping paper should they order?
Console.WriteLine($"Part 1 Answer: {presents.Sum(x => x.Item1)}");

// How many total feet of ribbon should they order?
Console.WriteLine($"Part 2 Answer: {presents.Sum(x => x.Item2)}");