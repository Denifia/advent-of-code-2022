// Mirage Maintenance

var lines = File.ReadAllLines("input.txt");
var oasis = new OASIS();
var values = lines.Skip(2).Select(oasis.PredictNextValue).ToArray();
var forwardPrediction = values.Select(x => x.ForwardPrediction).Sum();
var backwardsPrediction = values.Select(x => x.BackwardsPrediction).Sum();

Console.WriteLine($"Answer 1: {forwardPrediction}");
Console.WriteLine($"Answer 1: {backwardsPrediction}");

class OASIS
{
    public (long ForwardPrediction, long BackwardsPrediction) PredictNextValue(string input)
    {
        var numbers = ParseInput(input);
        var endNumbers = new List<long>
        {
            numbers[^1]
        };
        var startNumbers = new List<long>
        {
            numbers[0]
        };

        while (true)
        {
            numbers = GetNextStep(numbers);
            endNumbers.Add(numbers[^1]);
            startNumbers.Add(numbers[0]);
            if (numbers.All(x => x == 0))
                break;
        }

        var forwardPrediction = endNumbers.Sum();
        startNumbers.Reverse();

        long startNumber = 0;
        var backwardsPredictions = new List<long>();
        for (int i = 0; i < startNumbers.Count; i++)
        {
            startNumber = startNumbers[i] - startNumber;
            backwardsPredictions.Add(startNumber);
        }

        return (forwardPrediction, backwardsPredictions[^1]);
    }

    private long[] GetNextStep(long[] numbers) => numbers.Take(numbers.Length - 1).Select((number, index) => numbers[index + 1] - number).ToArray();
    private long[] ParseInput(string input) => input.Split(' ').Select(long.Parse).ToArray();
}