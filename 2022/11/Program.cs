// Day 11: Monkey in the Middle

using System.Numerics;

public static int reliefValue = 0;

var monkeyBusiness = Simulate(20, withRelief: true);
// What is the level of monkey business after 20 rounds of stuff-slinging simian shenanigans?
Console.WriteLine($"Part 1 Answer: {monkeyBusiness}");

monkeyBusiness = Simulate(10000, withRelief: false);
// Starting again from the initial state in your puzzle input, what is the level of monkey business after 10000 rounds?
Console.WriteLine($"Part 2 Answer: {monkeyBusiness}");


ulong Simulate(int rounds, bool withRelief)
{
    var lines = File.ReadAllLines("input.txt");

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

    reliefValue = monkeys.Select(x => x.TestNumber).Aggregate(1, (x, y) => x * y);

    ulong[] inspectedItems = Enumerable.Range(0, monkeys.Count).Select(x => (ulong)0).ToArray();
    for (int round = 1; round <= rounds; round++)
    {
        foreach (var monkey in monkeys)
        {
            //var monkeyNumber = monkeys.IndexOf(monkey);
            while (monkey.HoldingItems)
            {
                monkey.InspectNextItem(withRelief);
                (var catchingMonkey, var thrownItem) = monkey.ThrowNextItem();
                monkeys[catchingMonkey].CatchItem(thrownItem);
            }
        }
    }

    return monkeys
        .OrderByDescending(x => x.InspectedItems)
        .Take(2)
        .Aggregate<Monkey, ulong>(1, (x, monkey) => x * monkey.InspectedItems);
}
class Item
{
    private BigInteger _worryLevel;

    public Item(int worryLevel)
    {
        _worryLevel = new BigInteger(worryLevel);
    }

    public void IncreaseWorryLevelBy(int amount)
    {
        _worryLevel += amount;
    }

    public void MultiplyWorryLevelBy(int amount)
    {
        _worryLevel *= amount;
    }

    public void MultiplyWorryLevelByWorryLevel()
    {
        _worryLevel *= _worryLevel;
    }

    public void WorryRelief()
    {
        _worryLevel /= 3;
    }

    public void WorryRelief2()
    {
        _worryLevel %= reliefValue;
    }

    public bool Test(int amount) => _worryLevel % amount == 0;
}

class Monkey
{
    public ulong InspectedItems { get; private set; }
    public bool HoldingItems => _items.Any();

    private Queue<Item> _items = new Queue<Item>();
    private readonly Action<Item> _inspectionOperation;
    public int TestNumber { get; private set; }
    private readonly int _nextMonkeyIfTestTrue;
    private readonly int _nextMonkeyIfTestFalse;

    public Monkey(int[] startingItems, Action<Item> operation, int testNumber, int nextMonkeyIfTestTrue, int nextMonkeyIfTestFalse)
    {
        foreach (var item in startingItems)
        {
            _items.Enqueue(new Item(item));
        }

        _inspectionOperation = operation;
        TestNumber = testNumber;
        _nextMonkeyIfTestTrue = nextMonkeyIfTestTrue;
        _nextMonkeyIfTestFalse = nextMonkeyIfTestFalse;
    }

    public void InspectNextItem(bool withRelief = false)
    {
        var item = _items.Peek();
        _inspectionOperation(item);
        if (withRelief)
        {
            item.WorryRelief();
        }
        else
        {
            item.WorryRelief2();
        }

        InspectedItems++;
    }

    public (int, Item) ThrowNextItem()
    {
        var item = _items.Dequeue();
        return item.Test(TestNumber) switch
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
                    ["+", var amount] => builder => { builder._operation = item => item.IncreaseWorryLevelBy(int.Parse(amount)); },
                    ["*", "old"] => builder => { builder._operation = item => item.MultiplyWorryLevelByWorryLevel(); },
                    ["*", var amount] => builder => { builder._operation = item => item.MultiplyWorryLevelBy(int.Parse(amount)); },
                    _ => throw new NotSupportedException()
                },
            ["Test:", _, _, var amount] => builder => { builder._testDivideNumber = int.Parse(amount); },
            ["If", "true:", .., "monkey", var monkeyNumber] => builder => { builder._nextMonkeyIfTestTrue = int.Parse(monkeyNumber); },
            ["If", "false:", .., "monkey", var monkeyNumber] => builder => { builder._nextMonkeyIfTestFalse = int.Parse(monkeyNumber); },
            _ => throw new NotSupportedException()
        };
        action.Invoke(this);
    }

    public Monkey Build() => new Monkey(_items, _operation, _testDivideNumber, _nextMonkeyIfTestTrue, _nextMonkeyIfTestFalse);
}