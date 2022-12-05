const int Rock = 1;
const int Paper = 2;
const int Sizzors = 3;
const int Win = 6;
const int Draw = 3;
const int Loss = 0;

var lines = await File.ReadAllLinesAsync("input.txt");

// A = Rock, B = Paper, C = Sizzors
// X = Rock, Y = Paper, Z = Sizzors
var totalScoreV1 = lines.Sum(line => line switch
{
    "A X" => Rock + Draw,
    "B X" => Rock + Loss,
    "C X" => Rock + Win,
    "A Y" => Paper + Win,
    "B Y" => Paper + Draw,
    "C Y" => Paper + Loss,
    "A Z" => Sizzors + Loss,
    "B Z" => Sizzors + Win,
    "C Z" => Sizzors + Draw,
    _ => throw new IndexOutOfRangeException()
});
Console.WriteLine($"Part 1 Answer: {totalScoreV1}");

// A = Rock, B = Paper, C = Sizzors
// X = Loss, Y = Draw, Z = win
var totalScoreV2 = lines.Sum(line => line switch
{
    "A X" => Sizzors + Loss,
    "B X" => Rock + Loss,
    "C X" => Paper + Loss,
    "A Y" => Rock + Draw,
    "B Y" => Paper + Draw,
    "C Y" => Sizzors + Draw,
    "A Z" => Paper + Win,
    "B Z" => Sizzors + Win,
    "C Z" => Rock + Win,
    _ => throw new IndexOutOfRangeException()
});
Console.WriteLine($"Part 2 Answer: {totalScoreV2}");