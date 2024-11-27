// Title

using System.Collections;

class InputOrderer : IEnumerable<Triangle>
{
    private readonly List<int> col1 = [];
    private readonly List<int> col2 = [];
    private readonly List<int> col3 = [];

    public void Add((int One, int Two, int Three) inputRow)
    {
        col1.Add(inputRow.One);
        col2.Add(inputRow.Two);
        col3.Add(inputRow.Three);
    }

    public IEnumerator<Triangle> GetEnumerator()
    {
        var enumerator = col1.Concat(col2).Concat(col3).GetEnumerator();
        
        while (enumerator.MoveNext())
        {
            Triangle triangle = new();
            triangle.X = enumerator.Current;
            enumerator.MoveNext();
            triangle.Y = enumerator.Current;
            enumerator.MoveNext();
            triangle.Z = enumerator.Current;
            yield return triangle;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}