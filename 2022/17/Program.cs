// Day 17: Pyroclastic Flow

using _17;
using System.Text;

var line = File.ReadAllText("input.txt");

var numberOfJets = line.Length;
var numberOfShapes = 5;
var loop = numberOfJets * numberOfShapes;

long multiple = 1000000000000 / loop;
long remaining = 1000000000000 % loop;

var jetEmitter = new JetEmitter(line);
var shapeEmitter = new ShapeEmitter();
var shapes = new Stack<Shape>();
var recentShapes = new Queue<Shape>();

// process first shape
var currentShape = shapeEmitter.GetNextShape();
currentShape.SetY(4);
while (currentShape.Bits.Any(bit => bit.Y > 1))
{
    while (true)
    {
        currentShape.ApplyJet(jetEmitter.GetNextJet());
        currentShape.Fall();
        if (currentShape.Bits.Any(bit => bit.Y == 0))
        {
            currentShape.UndoLastMovement();
            shapes.Push(currentShape);

            recentShapes.Enqueue(currentShape);

            break;
        }
    }
}

// process the rest
while (shapes.Count() < 2022)
{
	currentShape = shapeEmitter.GetNextShape();
	currentShape.PositionAbove(recentShapes.Select(shape => shape.Bits.Max(bit => bit.Y)).Max());
    var blockableShapes = Array.Empty<Shape>();

    while (true)
    {
        currentShape.ApplyJet(jetEmitter.GetNextJet());
        blockableShapes = GetBlockableShapes(currentShape, recentShapes);
        if (blockableShapes.Any(shape => shape.WillBlock(currentShape)))
        {
            currentShape.UndoLastMovement();
        }

        currentShape.Fall();
        blockableShapes = GetBlockableShapes(currentShape, recentShapes);
        if (blockableShapes.Any(shape => shape.WillBlock(currentShape)) || currentShape.Bits.Any(bit => bit.Y == 0))
        {
            currentShape.UndoLastMovement();
            shapes.Push(currentShape);
            
            if (recentShapes.Count > 10) 
                recentShapes.Dequeue();
            recentShapes.Enqueue(currentShape);

            break;
        }
    }
}

// Display
//var sb = new StringBuilder();
//var allBits = shapes.SelectMany(shape => shape.Bits).OrderByDescending(bit => bit.Y).ToArray();
//for (int y = allBits.Max(bit => bit.Y); y > 0; y--)
//{
//    var bits = allBits.Where(bit => bit.Y == y).ToArray();
//    sb.Append($"{y:d4} |");
//    for (int x = 1; x < 8; x++)
//    {
//        sb.Append(bits.Any(bit => bit.X == x) ? '#' : '.');
//    }
//    sb.Append('|');
//    sb.AppendLine();
//}
//sb.AppendLine("     +-------+");
//Console.WriteLine(sb);

// How many units tall will the tower of rocks be after 2022 rocks have stopped falling?
Console.WriteLine($"Part 1 Answer: {shapes.Select(shape => shape.Bits.Max(bit => bit.Y)).Max()}");

// question 2
Console.WriteLine($"Part 2 Answer: {true}");

static Shape[] GetBlockableShapes(Shape currentShape, IEnumerable<Shape> shapes) 
    => shapes
        .Where(shape => shape.Bits.Max(bit => bit.Y) >= currentShape.Bits.Min(bit => bit.Y))
        .Where(shape => shape.Bits.Select(bit => bit.Y).Intersect(currentShape.Bits.Select(bit => bit.Y)).Any())
        .ToArray();