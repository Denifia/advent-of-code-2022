// Day 2: Cube Conundrum

var lines = File.ReadAllLines("test-input.txt");

lines.Select(line =>
{
    var gameNumber = int.Parse(line.Substring(5, line.IndexOf(":")));
    var handfulls = line
        .Substring(line.IndexOf(":") + 2)
        .Split("; ")
        .SelectMany(draw => 
            draw.Split(", ")
                .Select(x => x.Split(" "))
                .Select(x => x[1] switch
                    {
                        "red" => new Dice(Color.Red, int.Parse(x[0])),
                        "blue" => new Dice(Color.Blue, int.Parse(x[0])),
                        "green" => new Dice(Color.Green, int.Parse(x[0])),
                    })
        .ToArray();
    return new Game(gameNumber, handfulls);
});

// question 1
Console.WriteLine($"Part 1 Answer: {true}");

// question 2
Console.WriteLine($"Part 2 Answer: {true}");


record Game(int Number, Handfulls[] Handfulls);
record Handfulls(int Red, int Blue, int Green);
record Bag(int Red, int Blue, int Green);
record Dice(Color Color, int Amount);
enum Color
{
    Red = 0,
    Blue = 1,
    Green = 2,
}