// Camel Cards

var lines = File.ReadAllLines("input.txt");
var totalWinnings = GetHandsFromInput(lines)
            .Order()
            .Select((hand, index) => hand.Bid * (index + 1))
            .Aggregate(0, (total, hand) => total + hand);

// question 1
Console.WriteLine($"Part 1 Answer: {totalWinnings}");

// question 2
Console.WriteLine($"Part 2 Answer: {true}");

IEnumerable<Hand> GetHandsFromInput(string[] lines)
{
    foreach(var line in lines)
    {
        var bid = int.Parse(line[6..]);
        yield return new Hand(
            [
                new Card(line[0]),
                new Card(line[1]),
                new Card(line[2]),
                new Card(line[3]),
                new Card(line[4]),
            ], 
            bid);
    }
}

class Hand : IComparable<Hand>
{
    public Hand(Card[] cards, int bid)
    {
        Cards = cards;
        Bid = bid;
        HandType = DetermineHandType();
    }

    public Card[] Cards { get; }
    public int Bid { get; }
    public HandType HandType { get; }
    private HandType DetermineHandType()
    {
        var groups = Cards
                        .GroupBy(x => x.Label)
                        .Select(x => new { x.Key, Count = x.Count() })
                        .OrderByDescending(x => x.Count)
                        .ToArray();

        if (groups.Length == 1) 
            return HandType.FiveOfAKind;

        if (groups.Length == 2 && groups[0].Count == 4)
            return HandType.FourOfAKind;

        if (groups.Length == 2 && groups[0].Count == 3)
            return HandType.FullHouse;

        if ((groups.Length == 2 || groups.Length == 3) && groups[0].Count == 3)
            return HandType.ThreeOfAKind;

        if (groups.Length == 3 && groups[0].Count == 2 && groups[1].Count == 2)
            return HandType.TwoPair;

        if (groups.Length == 4)
            return HandType.OnePair;
            
        return HandType.HighCard;
    }
    public int CompareTo(Hand? other)
    {
        if (other is null) return 0; // satisfy nullability

        if (HandType > other.HandType) return 1;
        if (HandType < other.HandType) return -1;

        for (int i = 0; i < Cards.Length; i++)
        {
            if (Cards[i].Strength > other.Cards[i].Strength) return 1;
            if (Cards[i].Strength < other.Cards[i].Strength) return -1;
        }

        // Hands are the same
        return 0;
    }
}

enum HandType
{
    HighCard = 0,
    OnePair = 1,
    TwoPair = 2,
    ThreeOfAKind = 3,
    FullHouse = 4,
    FourOfAKind = 5,
    FiveOfAKind = 6,
}

class Card
{
    private static readonly char[] LabelStrengthMap = ['2', '3', '4', '5', '6', '7', '8', '9', 'T', 'J', 'Q', 'K', 'A'];

    public Card(char label)
    {
        Label = label;
        Strength = Array.IndexOf(LabelStrengthMap, label);
    }

    public char Label { get; }
    public int Strength { get; }
}