using System.Diagnostics;

namespace _17;

public class JetEmitter
{
	private int index = 0;
	private readonly string jets = string.Empty;

	public JetEmitter(string input)
	{
		jets = input;
    }

	public char GetNextJet()
	{
		if (index == jets.Length) 
			index = 0;

		var jet = jets[index];
		index++;

        //Debug.WriteLine($"JET {jet}");
		return jet;
	}
}