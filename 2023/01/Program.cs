// Day 1: Trebuchet?!

var lines = File.ReadAllLines("input.txt");
int sum = 0;

//var digits = "0123456789";
//foreach (var line in lines)
//{
//    var first = line.First(digits.Contains);
//    var last = line.Last(digits.Contains);
//    sum += int.Parse($"{first}{last}");
//}

//// question 1
//Console.WriteLine($"Part 1 Answer: {sum}");

var numbers = new Dictionary<string, string>()
{
    { "0", "0" },
    { "zero", "0" },
    { "1", "1" },
    { "one", "1" },
    { "2", "2" },
    { "two", "2" },
    { "3", "3" },
    { "three", "3" },
    { "4", "4" },
    { "four", "4" },
    { "5", "5" },
    { "five", "5" },
    { "6", "6" },
    { "six", "6" },
    { "7", "7" },
    { "seven", "7" },
    { "8", "8" },
    { "eight", "8" },
    { "9", "9" },
    { "nine", "9" },
};

foreach (var line in lines)
{
    var first = numbers
        .Select(x => new { index = line.IndexOf(x.Key), x.Value })
        .Where(x => x.index >= 0)
        .OrderBy(x => x.index)
        .First().Value;

    var last = numbers
        .Select(x => new { index = line.LastIndexOf(x.Key), x.Value })
        .Where(x => x.index >= 0)
        .OrderByDescending(x => x.index + x.Value.Length)
        .First().Value;

    sum += int.Parse($"{first}{last}");
}

// question 2
Console.WriteLine($"Part 2 Answer: {sum}");