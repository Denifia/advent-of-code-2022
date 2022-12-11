// Day 11: Monkey in the Middle

var lines = File.ReadAllLines("test-input.txt");

var monkeys = new List<Monkey>();
var monkeyBuilder = new MonkeyBuilder();
foreach (var line in lines)
{
    if (string.IsNullOrWhiteSpace(line))
    {
        monkeys.Add(monkeyBuilder.Build());
        monkeyBuilder = new MonkeyBuilder();
        continue;
    }

    monkeyBuilder.ProcessLine(line);
}

// last monkey has not trailing newline
monkeys.Add(monkeyBuilder.Build());

for (int round = 0; round < 20; round++)
{
    foreach (var monkey in monkeys)
    {
        if (!monkey.HoldingItems) continue;

        while (monkey.HoldingItems)
        {
            monkey.InspectNextItem();
            (var catchingMonkey, var thrownItem) = monkey.ThrowNextItem();
            monkeys[catchingMonkey].CatchItem(thrownItem);
        }
    }
}

var monkeyBusiness = monkeys
    .OrderByDescending(x => x.InspectedItems)
    .Take(2)
    .Aggregate(1, (x, monkey) => x * monkey.InspectedItems);

// What is the level of monkey business after 20 rounds of stuff-slinging simian shenanigans?
Console.WriteLine($"Part 1 Answer: {monkeyBusiness}");

// question 2
Console.WriteLine($"Part 2 Answer: {true}");

class Item
{
    public int WorryLevel { get; set; }
    public Item(int worryLevel) => WorryLevel = worryLevel;
    public void WorryRelief() => WorryLevel /= 3;
}

class Monkey
{
    public int InspectedItems { get ; private set; }
    public bool HoldingItems => _items.Any();

    private Queue<Item> _items = new Queue<Item>();
    private readonly Action<Item> _inspectionOperation;
    private readonly Func<Item, bool> _test;
    private readonly int _nextMonkeyIfTestTrue;
    private readonly int _nextMonkeyIfTestFalse;

    public Monkey(int[] startingItems, Action<Item> operation, Func<Item, bool> test, int nextMonkeyIfTestTrue, int nextMonkeyIfTestFalse)
    {
        foreach (var item in startingItems)
        {
            _items.Enqueue(new Item(item));
        }

        _inspectionOperation = operation;
        _test = test;
        _nextMonkeyIfTestTrue = nextMonkeyIfTestTrue;
        _nextMonkeyIfTestFalse = nextMonkeyIfTestFalse;
    }

    public void InspectNextItem()
    {
        var item = _items.Peek();
        _inspectionOperation(item);
        item.WorryRelief();
        InspectedItems++;
    }

    public (int, Item) ThrowNextItem()
    {
        var item = _items.Dequeue();
        return _test.Invoke(item) switch
        {
            true => (_nextMonkeyIfTestTrue, item),
            false => (_nextMonkeyIfTestFalse, item)
        };
    }

    public void CatchItem(Item item)
    {
        _items.Enqueue(item);
    }
}

class MonkeyBuilder
{
    private int[] _items = Array.Empty<int>();
    private Action<Item> _operation = item => { };
    private int _testDivideNumber;
    private int _nextMonkeyIfTestTrue;
    private int _nextMonkeyIfTestFalse;

    public void ProcessLine(string line)
    {
        Action<MonkeyBuilder> action = line.Split(' ', StringSplitOptions.RemoveEmptyEntries) switch
        {
            ["Monkey", ..] => builder => { },
            [_, "items:", ..var items] => builder => { builder._items = items.Select(x => int.Parse(x.TrimEnd(','))).ToArray(); },
            ["Operation:", _, _, _, ..var parts] =>
                parts switch
                {
                    ["+", var amount] => builder => { builder._operation = item => item.WorryLevel += int.Parse(amount); },
                    ["*", "old"] => builder => { builder._operation = item => item.WorryLevel *= item.WorryLevel; },
                    ["*", var amount] => builder => { builder._operation = item => item.WorryLevel *= int.Parse(amount); },
                    _ => throw new NotSupportedException()
                },
            ["Test:", _, _, var amount] => builder => { builder._testDivideNumber = int.Parse(amount); },
            ["If", "true:", .., "monkey", var monkeyNumber] => builder => { builder._nextMonkeyIfTestTrue = int.Parse(monkeyNumber); },
            ["If", "false:", .., "monkey", var monkeyNumber] => builder => { builder._nextMonkeyIfTestFalse = int.Parse(monkeyNumber); },
            _ => throw new NotSupportedException()
        };
        action.Invoke(this);
    }

    public Monkey Build() => new Monkey(_items, _operation, item => item.WorryLevel % _testDivideNumber == 0, _nextMonkeyIfTestTrue, _nextMonkeyIfTestFalse);
}