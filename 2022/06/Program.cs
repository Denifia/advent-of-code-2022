var buffer = File.ReadAllText("input.txt").AsSpan();

// start of packet marker
var markerBuffer = new char[4];
Console.WriteLine($"Part 1 Answer: {FindMarker(buffer, 4, ref markerBuffer)}");

// start of message marker
markerBuffer = new char[14];
Console.WriteLine($"Part 2 Answer: {FindMarker(buffer, 14, ref markerBuffer)}");

static int FindMarker(ReadOnlySpan<char> buffer, int markerLength, ref char[] markerBuffer)
{
    for (int i = markerLength; i < buffer.Length; i++)
    {
        buffer.Slice(i - markerLength, markerLength).CopyTo(markerBuffer);
        if (markerBuffer.Distinct().Count() == markerLength)
        {
            return i;
        }
    }
    throw new IndexOutOfRangeException();
}