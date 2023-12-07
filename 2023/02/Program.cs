// Day 2: Cube Conundrum

var lines = File.ReadAllLines("test-input.txt");

List<Game> games = [];
foreach (var line in lines)
{
    var parts = line.Split(":");
    var gameNumber = ParseGameNumber(parts[0]);
    var handfuls = parts[1].Split(";").Select(x => new Handful(ParseDice(x).ToArray()));
    games.Add(new Game(gameNumber, handfuls.ToArray()));
}

var bag = BuildBag(green: 13, red: 12, blue: 14);

var notPossibleGames = games.Where(game => game.Handfuls.Any(handful =>
        handful.RedCount > bag.RedCount ||
        handful.BlueCount > bag.BlueCount ||
        handful.GreenCount > bag.GreenCount))
    .Select(x => x);

//var sum = games.Except(notPossibleGames).Sum(game => game.Number);
//sum.Dump("question 1");

var sum = games
    .Select(game =>
        BuildBag(
            game.Handfuls.Max(h => h.GreenCount),
            game.Handfuls.Max(h => h.RedCount),
            game.Handfuls.Max(h => h.BlueCount)))
    .Select(bag => bag.GreenCount * bag.RedCount * bag.BlueCount)
    .Sum();

Console.WriteLine($"Answer: {sum}");


Bag BuildBag(int green, int red, int blue) => new Bag(
    Enumerable.Range(0, green).Select(x => new Dice(Colour.green))
        .Concat(Enumerable.Range(0, red).Select(x => new Dice(Colour.red)))
        .Concat(Enumerable.Range(0, blue).Select(x => new Dice(Colour.blue)))
        .ToArray());

// Parses a string like "Game 1" into the int 1
int ParseGameNumber(string s) => int.Parse(s.AsSpan()["Game".Length..]);

// Parses a string like " 3 blue" into a Dice enumerator
IEnumerable<Dice> ParseDice(string s)
{
    foreach (var diceString in s.Split(","))
    {
        int mid = diceString.LastIndexOf(" ");
        var span = diceString.AsSpan();
        var numberOfDice = int.Parse(span[..mid]);
        var colourOfDice = (Colour)Enum.Parse(typeof(Colour), span[mid..]);
        for (int i = 0; i < numberOfDice; i++)
        {
            yield return new Dice(colourOfDice);
        }
    }
}

record Game(int Number, Handful[] Handfuls);

record Handful(Dice[] Dice)
{
    public int RedCount => Dice.Count(x => x.Colour == Colour.red);
    public int GreenCount => Dice.Count(x => x.Colour == Colour.green);
    public int BlueCount => Dice.Count(x => x.Colour == Colour.blue);
}

record Dice(Colour Colour);

record Bag(Dice[] Dice)
{
    public int RedCount => Dice.Count(x => x.Colour == Colour.red);
    public int GreenCount => Dice.Count(x => x.Colour == Colour.green);
    public int BlueCount => Dice.Count(x => x.Colour == Colour.blue);
}

enum Colour
{
    red = 0,
    blue = 1,
    green = 2,
}