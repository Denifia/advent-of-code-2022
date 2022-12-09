// Day 9: Rope Bridge

var lines = await File.ReadAllLinesAsync("input.txt");
var instructions = lines.Select(x => (x[0], int.Parse(x[2..]))).ToArray();

int Simulate(int count)
{
    var knots = Enumerable.Range(0, count).Select(x => new Knot()).ToArray();
    var visited = new HashSet<(int, int)> { (0, 0) };

    foreach (var instruction in instructions)
    {
        Action<Knot> action = instruction.Item1 switch
        {
            'U' => (knot) => knot.Col++,
            'D' => (knot) => knot.Col--,
            'L' => (knot) => knot.Row--,
            'R' => (knot) => knot.Row++,
            _ => (x) => { }
        };
        for (int i = 0; i < instruction.Item2; i++)
        {
            action.Invoke(knots[0]); // move head
            MoveOtherKnots();
        }
    }

    return visited.Count;

    void MoveOtherKnots()
    {
        for (int i = 1; i < knots.Length; i++)
        {
            MoveNextKnot(knots[i - 1], knots[i]);
            if (i == knots.Length - 1)
            {
                visited.Add((knots[i].Col, knots[i].Row));
            }
        }
    }
}

void MoveNextKnot(Knot head, Knot next)
{
    if (head.Col > (next.Col + 1))
    {
        next.Col++;
        if (head.Row > next.Row) next.Row++;
        if (head.Row < next.Row) next.Row--;
    }

    if (head.Col < (next.Col - 1))
    {
        next.Col--;
        if (head.Row > next.Row) next.Row++;
        if (head.Row < next.Row) next.Row--;
    }

    if (head.Row > (next.Row + 1))
    {
        next.Row++;
        if (head.Col > next.Col) next.Col++;
        if (head.Col < next.Col) next.Col--;
    }

    if (head.Row < (next.Row - 1))
    {
        next.Row--;
        if (head.Col > next.Col) next.Col++;
        if (head.Col < next.Col) next.Col--;
    }
};

// Simulate your complete hypothetical series of motions. How many positions does the tail of the rope visit at least once?
Console.WriteLine($"Part 1 Answer: {Simulate(2)}");

// Simulate your complete series of motions on a larger rope with ten knots. How many positions does the tail of the rope visit at least once?
Console.WriteLine($"Part 2 Answer: {Simulate(10)}");

class Knot
{
    public int Col { get; set; }
    public int Row { get; set; }
}