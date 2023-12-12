// Hot Springs

using System.Text.RegularExpressions;

var lines = File.ReadAllLines("input.txt");
var rows = lines.Select(line => new Row(line)).ToList();

var sum = 0;
foreach (var row in rows)
{
    var n = row.CalculatePossibleConfigurations();
    Console.WriteLine($"{row.Line} = {n}");

    sum += n;
}

// question 1
Console.WriteLine($"Part 1 Answer: {sum}");

// question 2
Console.WriteLine($"Part 2 Answer: {true}");

class Row
{
    public Row(string line)
    {
        ParseInput(line);
    }

    public Row(List<HotSpring> hotSprings)
    {
        HotSprings = hotSprings;
    }

    private void ParseInput(string line)
    {
        Line = line;
        var parts = line.Split(' ');
        Length = parts[0].Length;
        foreach (var condition in parts[0])
        {
            var hotSpring = condition switch
            {
                Conditions.Unknown => new HotSpring(Condition.Unknown),
                Conditions.Operational => new HotSpring(Condition.Operational),
                Conditions.Damaged => new HotSpring(Condition.Damaged),
                _ => throw new Exception("Invalid condition")
            };
            HotSprings.Add(hotSpring);
        }

        Report = parts[1].Split(',').Select(int.Parse).ToList();
    }

    public string Line { get; set; }
    public int Length { get; set; }
    public List<HotSpring> HotSprings { get; } = [];
    public List<int> Report { get; private set; } = [];

    public int CalculatePossibleConfigurations()
    {
        List<Row> possibleRows = [];
        var rowLength = HotSprings.Count;
        return GetAllValidPermutations().Count();
    }

    public IEnumerable<string> GetAllValidPermutations()
    {
        var regex = GenerateRegex();
        var permutations = GetAllPermutations(regex, Length);

        foreach (var permutation in permutations)
        {
            if (IsValidPermutation(permutation))
            {
                yield return permutation;
            }
        }
    }

    private Regex GenerateRegex() 
        => new Regex(@"^\.*" + string.Join(@"\.{1,}", Report.Select(x => $"#{{{x}}}")) + @"\.*$");

    private IEnumerable<string> GetAllPermutations(Regex regex, int length)
    {
        var chars = ".#?";
        return GetPermutations(chars, length).Where(s => NotEvenClose(s) && regex.IsMatch(s));
    }

    private bool NotEvenClose(string s)
    {
        for (int i = 0; i < s.Length; i++)
        {
            if (s[i] == Conditions.Damaged && Line[i] == Conditions.Operational)
                return false;

            if (s[i] == Conditions.Operational && Line[i] == Conditions.Damaged)
                return false;
        }
        return true;
    }

    private IEnumerable<string> GetPermutations(string chars, int length)
    {
        if (length == 1) return chars.ToCharArray().Select(c => c.ToString());

        return GetPermutations(chars, length - 1)
            .SelectMany(c => chars, (c, ch) => c + ch)
            .Where(x => NotEvenClose(x));
    }

    private bool IsValidPermutation(string permutation)
    {
        return permutation.Select((c, i) =>
        {
            var condition = c switch
            {
                Conditions.Unknown => new HotSpring(Condition.Unknown),
                Conditions.Operational => new HotSpring(Condition.Operational),
                Conditions.Damaged => new HotSpring(Condition.Damaged),
                _ => throw new Exception("Invalid condition")
            };

            return condition.Equals(HotSprings[i].Condition);
        })
        .All(x => x == true);
    }   
}

record HotSpring(Condition Condition) : IEquatable<Condition>
{
    public bool Equals(Condition other)
    {
        if (other is Condition.Unknown)
        {
            return true;
        }

        return Condition == other;
    }
}

enum Condition
{
    Unknown,
    Operational,
    Damaged
}

class Conditions
{
    public const char Unknown = '?';
    public const char Operational = '.';
    public const char Damaged = '#';
}