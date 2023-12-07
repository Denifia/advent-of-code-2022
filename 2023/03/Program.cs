// Gear Ratios

var lines = File.ReadAllLines("input.txt");

const byte Zero = (byte)'0';
const byte Nine = (byte)'9';
const byte Dot = (byte)'.';

var partNumbers = GetPartNumbers(lines).ToArray();

// question 1
Console.WriteLine($"Part 1 Answer: {partNumbers.Sum(x => x.Number)}");

var gearRatioSum = partNumbers
    .Where(x => x.Symbol.Character == '*')
    .GroupBy(x => x.Symbol)
    .Where(x => x.Count() == 2)
    .Select(x => x.Aggregate(1, (total, nextNumber) => total * nextNumber))
    .Sum();

// question 2
Console.WriteLine($"Part 2 Answer: {gearRatioSum}");

IEnumerable<PartNumber> GetPartNumbers(string[] lines)
{
    for (int lineIndex = 0; lineIndex < lines.Length; lineIndex++)
    {
        var line = lines[lineIndex];
        for (int i = 0; i < line.Length; i++)
        {
            if (IsNumber(line[i]))
            {
                // We have a number
                var number = line[i] - Zero;
                var startIndex = Math.Max(i - 1, 0);
                i++;

                // Get the rest of the number
                while (i < line.Length && IsNumber(line[i]))
                {
                    number = (number * 10) + (line[i] - Zero);
                    i++;
                }
                var endIndex = Math.Min(i + 1, line.Length);

                // Check for adjacent symbol on the current line
                AdjacentSymbol? adjacentSymbol = null;
                if (TryGetAdjacentSymbol(lines[lineIndex], lineIndex, startIndex, endIndex, out var symbol1))
                {
                    adjacentSymbol = symbol1;
                }

                // Check previous line if there is one
                if (adjacentSymbol is null && lineIndex > 0 && TryGetAdjacentSymbol(lines[lineIndex - 1], lineIndex - 1, startIndex, endIndex, out var symbol2))
                {
                    adjacentSymbol = symbol2;
                }

                // Check next line if there is one
                if (adjacentSymbol is null && lineIndex < lines.Length - 1 && TryGetAdjacentSymbol(lines[lineIndex + 1], lineIndex + 1, startIndex, endIndex, out var symbol3))
                {
                    adjacentSymbol = symbol3;
                }

                if (adjacentSymbol is not null)
                {
                    yield return new PartNumber(number, adjacentSymbol);
                }
            }
        }
    }
}

bool TryGetAdjacentSymbol(string line, int lineIndex, int startIndex, int endIndex, out AdjacentSymbol? adjacentSymbol)
{
    adjacentSymbol = null;
    var symbol = line.Substring(startIndex, endIndex - startIndex).FirstOrDefault(c => !IsNumber(c) && c != Dot);
    if (symbol != default)
    {
        var x = line.AsSpan().Slice(startIndex, endIndex - startIndex).IndexOf(symbol);
        adjacentSymbol = new AdjacentSymbol(symbol, startIndex + x, lineIndex);
        return true;
    }
    return false;
}

bool IsNumber(char c) => c >= Zero && c <= Nine;

record PartNumber(int Number, AdjacentSymbol Symbol)
{
    public static implicit operator int(PartNumber partNumber) => partNumber.Number;
}

record AdjacentSymbol(char Character, int X, int Y);
