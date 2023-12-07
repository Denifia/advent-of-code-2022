// Scratchcards

var lines = File.ReadAllLines("input.txt");
var cards = GetCardsFromInputLines(lines);
var totalScore = cards.Sum(card => card.CalculateSimpleScore());

// question 1
Console.WriteLine($"Part 1 Answer: {totalScore}");

// question 2
Console.WriteLine($"Part 2 Answer: {CalculateComplexScore(cards)}");

static IEnumerable<Card> GetCardsFromInputLines(string[] lines)
{
    foreach (var line in lines)
    {
        var parts = line.Split(':', '|');
        var number = int.Parse(parts[0][4..]);
        var winningNumbers = parts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
        var yourNumbers = parts[2].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
        yield return new Card(number, winningNumbers, yourNumbers);
    }
}

int CalculateComplexScore(IEnumerable<Card> cards)
{
    Dictionary<int, int> CardCopies = [];

    foreach (var card in cards)
    {
        if (!CardCopies.ContainsKey(card.Number))
        {
            CardCopies.Add(card.Number, 1);
        }
        else
        {
            CardCopies[card.Number]++;
        }

        foreach (var wonCopy in card.WonCardCopies)
        {
            if (!CardCopies.ContainsKey(wonCopy))
            {
                CardCopies.Add(wonCopy, 0);
            }
            CardCopies[wonCopy] += CardCopies[card.Number];
        }
    }

    return CardCopies.Sum(x => x.Value);
}

record Card(int Number, int[] WinningNumbers, int[] YourNumbers)
{
    public int CalculateSimpleScore() => YourNumbers.Where(WinningNumbers.Contains).Aggregate(0, (total, _) => total == 0 ? 1 : total * 2);

    public int[] WonCardCopies = YourNumbers.Where(WinningNumbers.Contains).Select((_, i) => i + Number + 1).ToArray();
}