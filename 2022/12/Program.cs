// Day 12: Hill Climbing Algorithm

var lines = File.ReadAllLines("input.txt");

Dictionary<(int x, int y), Node> map = new();
Node start = new();
Node end = new();

Console.WriteLine("Mapping landscape...");
for (int y = 0; y < lines.Length; y++)
{
	for (int x = 0; x < lines[y].Length; x++)
	{
        if (lines[y][x] == 'S')
        {
            map[(x, y)] = new Node
            {
                x = x,
                y = y,
                height = 0,
                letter = lines[y][x],
                cost = 0
            };
            start = map[(x, y)];
            continue;
        }

        if (lines[y][x] == 'E')
        {
            map[(x, y)] = new Node
            {
                x = x,
                y = y,
                height = 'z' - 'a',
                letter = lines[y][x]
            };
            end = map[(x, y)];
            continue;
        }

        map[(x, y)] = new Node
        {
            x = x,
            y = y,
            height = lines[y][x] - 'a',
            letter = lines[y][x]
        };
    }
}

Console.WriteLine("Estimating paths...");
for (int y = 0; y < lines.Length; y++)
{
    for (int x = 0; x < lines[y].Length; x++)
    {
        var node = map[(x, y)];
        // Manhattan distance
        node.estimateCostToEnd = Math.Abs((node.x - end.x) + (node.y - end.y));
    }
}

var pathFinding = new PathFinding(start, end, map.Values.ToList());
pathFinding.Walk();

// What is the fewest steps required to move from your current position to the location that should get the best signal?
Console.WriteLine($"Part 1 Answer: {end.movementCostSoFar}");

// question 2
Console.WriteLine($"Part 2 Answer: {true}");

class Node
{
    public Node? Parent = null;
    public int x;
    public int y;
    public char letter;
    public int height;
    public int cost = 1;
    public int movementCostSoFar => cost + (Parent?.movementCostSoFar ?? 0);
    public int scoreWithNewParent(Node? parent) => cost + (parent?.cost ?? 0);
    public int estimateCostToEnd; // already computed
    public int score => movementCostSoFar + estimateCostToEnd;
}

class PathFinding
{
    public List<Node> closed = new();
    public List<Node> open = new();
    private readonly Node end;
    private readonly List<Node> allNodes;
    bool debug = false;

    public PathFinding(Node start, Node end, List<Node> nodes)
    {
        this.end = end;
        allNodes = nodes;
        open.Add(start);
        Console.WriteLine($"adding starting node {start.letter} at {start.x},{start.y}");
    }

    public void Walk()
    {
        Console.WriteLine("Calculating shortest path...");
        Node lastNode = null;
        while (open.Any())
        {
            // Get lowest scoring open node
            var currentNode = open.OrderBy(x => x.score).First();
            
            // Remove it from open list, add it to closed list
            open.Remove(currentNode);
            if (debug) Console.WriteLine($"closing {currentNode.letter} at {currentNode.x},{currentNode.y}");
            closed.Add(currentNode);

            // Find all it's connections
            var nodes = GetConnectedOpenNodes(currentNode);

            foreach (var item in nodes)
            {
                if (debug) Console.Write($"> nearby {item.letter} at {item.x},{item.y}");

                if (closed.Contains(item))
                {
                    if (debug) Console.WriteLine(" already closed");
                    continue;
                }

                if (!open.Contains(item))
                {
                    item.Parent = currentNode;
                    open.Add(item);
                    if (debug) Console.WriteLine($" added to open");
                    continue;
                }

                // compute score and update parent if lower now
                if (item.score > item.scoreWithNewParent(currentNode))
                {
                    if (debug) Console.WriteLine($" changing parent to {currentNode.letter} at {currentNode.x},{currentNode.y}");
                    item.Parent = currentNode;
                }
            }

            lastNode = currentNode;
        }

        if (debug)
        {
            Console.WriteLine();
            Console.WriteLine();

            var n = end;
            while (n.Parent != null)
            {
                Console.WriteLine($"{n.letter} at {n.x},{n.y}");
                n = n.Parent;
            }
        }
    }

    public Node[] GetConnectedOpenNodes(Node node)
    {
        return allNodes.Where(other =>
        {
            if (other.x == node.x - 1 && other.y == node.y) // down
            {
                return true;
            }
            if (other.x == node.x + 1 && other.y == node.y) // up
            {
                return true;
            }
            if (other.x == node.x && other.y == node.y + 1) // right
            {
                return true;
            }
            if (other.x == node.x && other.y == node.y - 1) // down
            {
                return true;
            }
            return false;
        }).Where(other =>
        {
            if ((other.height - node.height) <= 1)
            {
                return true;
            }
            return false;
        }).ToArray();
    }
}