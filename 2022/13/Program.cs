// Day 13: Distress Signal

var debug = true;
var lines = File.ReadAllLines("test-input.txt").Where(x => !x.StartsWith("#")).ToArray();

var pairs = new List<Pair>();

for (int i = 0; i < lines.Length; i += 3)
{
    //if (debug) Console.WriteLine($"pair {(i / 3) + 1}");
    pairs.Add(new Pair()
    {
        Index = (i / 3) + 1,
        Left = Item.From(lines[i]),
        Right = Item.From(lines[i + 1])
    });
}

foreach (var pair in pairs)
{
    if (debug) Console.WriteLine($"checking pair {pair.Index}");
    pair.CheckForCorrectOrder();
    if (debug)
    {
        Console.WriteLine($"{pair.Left.Source}");
        Console.WriteLine($"{pair.Right.Source}");
        Console.WriteLine($"{(pair.InCorrectOrder ? "correct" : "incorrect")} order");
        Console.WriteLine();
    }
}

// What is the sum of the indices of those pairs?
if (debug) Console.WriteLine($"Part 1 Answer: {pairs.Where(x => x.InCorrectOrder).Sum(x => x.Index)}");

var items = pairs.SelectMany(x => new[] { x.Left, x.Right }).ToList();
items.Add(Item.From("[[2]]"));
items.Add(Item.From("[[6]]"));

// items.Sort(); // doesn't work

Item temp1 = null;
Item temp2 = null;
var inOrder = false;
while (!inOrder)
{
    inOrder = true;
    for (int i = 0; i < items.Count - 1; i++)
    {
        if (items[i].CompareTo(items[i+1]) == 1)
        {
            temp1 = items[i];
            temp2 = items[i+1];
            items[i+1] = temp1;
            items[i] = temp2;
            inOrder = false;
        }
    }
}

Console.WriteLine();
Console.WriteLine();

for (int i = 0; i < items.Count; i++)
{
    if (items[i].Source == "[[2]]" || items[i].Source == "[[6]]")
    {
        Console.Write("***");
    }
    Console.WriteLine($"{i+1:d4}: {items[i].Source}");
}

var decoderKey = (items.FindIndex(x => x.Source == "[[2]]") + 1) 
               * (items.FindIndex(x => x.Source == "[[6]]") + 1);

// What is the decoder key for the distress signal?
if (debug) Console.WriteLine($"Part 2 Answer: {decoderKey}");

public class Item : IComparable
{
    public string Source { get; set; }
    public int? Number { get; set; }
    public List<Item> Childern { get; set; } = new List<Item>();
    public static Item From(string line)
    {
        
        var item = new Item();
        item.Source = line;
        if (string.IsNullOrWhiteSpace(line))
        {
            //Console.WriteLine("empty");
            // do nothing
            return item;
        }

        if (int.TryParse(line, out var number))
        {
            //Console.WriteLine($"number {number}");
            item.Number = number;
            return item;
        }

        if (line.StartsWith('['))
        {
            var unmatched = 1;
            var index = 0;
            while (unmatched > 0)
            {
                index++;
                if (line[index] == '[')
                {
                    unmatched++;
                    continue;
                }
                else if (line[index] == ']')
                {
                    unmatched--;
                    continue;
                }
            }
            var subLine = line[1..index];
            //Console.WriteLine($"group \"{subLine}\"");

            var parts = new List<string>()
            {
                string.Empty
            };
            unmatched = 0;
            index = 0;
            foreach (var letter in subLine)
            {
                if (letter == '[')
                {
                    unmatched++;
                } 
                else if (letter == ']')
                {
                    unmatched--;
                }

                if (letter == ',' && unmatched == 0)
                {
                    parts.Add(string.Empty);
                    index++;
                    continue;
                }
                parts[index] += letter;
            }
            foreach (var part in parts)
            {
                item.Childern.Add(Item.From(part));
            }
        }
        else
        {
            Console.WriteLine("Nothing!");
        }
        return item;
    }
    public static Item From(int number)
    {
        return new Item
        {
            Childern = new List<Item>
            {
                new Item
                {
                    Number = number
                }
            }
        };
    }

    public int CompareTo(object? obj)
    {
        // [[1],[2,3,4]] <- problem child in test data

        var other = obj as Item;
        if (other == null) return 1;

        if (Number.HasValue && other.Number.HasValue)
            return Number.Value.CompareTo(other.Number.Value);

        if (Number.HasValue && !other.Number.HasValue)
        {
            //Console.WriteLine("second had no number");
            //return Item.From(Number.Value).CompareTo(other);
            if (!other.Childern.Any()) 
                return 1;
            if (!other.Childern[0].Number.HasValue) 
                return 1;
            return Number.Value.CompareTo(other.Childern[0].Number.Value);
        }

        if (!Number.HasValue && other.Number.HasValue)
        {
            //Console.WriteLine("first had no number");
            //return this.CompareTo(Item.From(other.Number.Value));
            if (!Childern.Any()) 
                return -1;
            if (!Childern[0].Number.HasValue) 
                return -1;
            return Childern[0].Number.Value.CompareTo(other.Number.Value);
        }

        var count = 0;
        for (int i = 0; i < Childern.Count; i++)
        {
            if (other.Childern.Count <= i) return 1;
            var compare = Childern[i].CompareTo(other.Childern[i]);
            if (compare != 0) return compare;
            count = i;
        }

        if (other.Childern.Count > count)
        {
            return -1;
        }

        // all children are the same
        return 0;
    }
}

class Pair
{
    public bool InCorrectOrder;
    public int Index = 0;
    public Item Left { get; set; }
    public Item Right { get; set; }
    public void CheckForCorrectOrder()
    {
        InCorrectOrder = Left.CompareTo(Right) == -1;
    }
}