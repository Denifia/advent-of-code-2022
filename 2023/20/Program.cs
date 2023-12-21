// Pulse Propagation

using System.Diagnostics;
using System.Threading.Channels;

var lines = File.ReadAllLines("input.txt");
var modules = new Dictionary<string, IModule>();
var channel = Channel.CreateUnbounded<Work>();
var writer = channel.Writer;
var reader = channel.Reader;
long lowPulseCount = 0;
long highPulseCount = 0;

// Manually add the button module
var button = new Button();
button.AddDestinationModule("broadcaster");
modules.Add("button", button);

// Parse input to add all other modules
foreach (var line in lines)
{
    var parts = line.Split(" -> ");
    var id = parts[0];

    IModule module = id[0] switch
    {
        'b' => new Broadcaster(),
        '%' => new FlipFlop(id[1..]),
        '&' => new Conjunction(id[1..]),
        _ => throw new NotImplementedException()
    };

    foreach (var destination in parts[1].Split(", ", StringSplitOptions.RemoveEmptyEntries))
    {
        module.AddDestinationModule(destination);
    }

    modules.Add(module.Name, module);
}

foreach (var module in modules.Where(x => x.Value.GetType() == typeof(Conjunction)).Select(x => x.Value))
{
    foreach (var input in modules.Where(x => x.Value.DestinationModules.Contains(module.Name)).Select(x => x.Value))
    {
        (module as Conjunction).AddInputModule(input.Name);
    }
}

long buttonPressCount = 0;
//List<Pulse> rxPulses = [];
bool hasRxLowPulse = false;

// Iterate 1000 button presses
for (long i = 0; i < 1_000_000_000_000_000; i++)
{
    await writer.WriteAsync(new Work(Pulse.Low, "button", string.Empty));
    while (reader.TryRead(out var workItem))
    {
        //if (workItem.SourceModule != "button")
        //{
        //    if (workItem.Pulse == Pulse.High)
        //    {
        //        highPulseCount++;
        //    }
        //    else
        //    {
        //        lowPulseCount++;
        //    }
        //}

        //if (workItem.SourceModule != string.Empty)
        //    Console.WriteLine($"{workItem.SourceModule} -{workItem.Pulse}-> {workItem.DestinationModule}");

        if (modules.TryGetValue(workItem.DestinationModule, out var module))
        {
            await module.HandleWork(workItem, writer);
        };

        if (workItem.DestinationModule == "rx" && workItem.Pulse == Pulse.Low)
        {
            hasRxLowPulse = true;
            //if (workItem.Pulse == Pulse.Low)
            //    Debug.Fail("Arst");

            //rxPulses.Add(workItem.Pulse);
        }       
    }

    buttonPressCount++;

    if (hasRxLowPulse)
    {
        break;
    }

    //if (rxPulses.Count == 1 && rxPulses.All(x => x == Pulse.Low))
    //{
    //    // done
    //    break;
    //}
    //else
    //{
    //    rxPulses.Clear();
    //}

    if (buttonPressCount % 500_000 == 0)
    {
        await Console.Out.WriteLineAsync(buttonPressCount.ToString());
    }

    //Console.WriteLine();
}

// Answer 1
//Console.WriteLine($"Answer: {lowPulseCount} * {highPulseCount} = {lowPulseCount * highPulseCount}");

// Answer 2
Console.WriteLine($"Answer: {buttonPressCount}");

record Work(Pulse Pulse, string DestinationModule, string SourceModule);

enum Pulse
{
    High,
    Low
}

interface IModule
{
    string Name { get; }
    List<string> DestinationModules { get; }
    Task HandleWork(Work work, ChannelWriter<Work> workQueue);
    void AddDestinationModule(string ModuleKey);
}

abstract class Module
{
    public Module(string name)
    {
        Name = name;
    }

    public List<string> DestinationModules { get; } = [];

    public string Name { get; }

    public void AddDestinationModule(string ModuleKey)
    {
        DestinationModules.Add(ModuleKey);
    }

    protected async Task Broadcast(Pulse pulse, ChannelWriter<Work> workQueue)
    {
        foreach (var module in DestinationModules)
        {
            await workQueue.WriteAsync(new Work(pulse, module, Name));
        }
    }
}

class FlipFlop : Module, IModule
{
    private bool On = false;

    public FlipFlop(string name) : base(name)
    {
    }

    public async Task HandleWork(Work work, ChannelWriter<Work> workQueue)
    {
        if (work.Pulse == Pulse.High)
            return;

        On = !On;

        await Broadcast(On ? Pulse.High : Pulse.Low, workQueue);
    }
}

class Conjunction : Module, IModule
{
    protected Dictionary<string, Pulse> InputModules = [];

    public Conjunction(string name) : base(name)
    {
    }

    public void AddInputModule(string ModuleKey)
    {
        InputModules.Add(ModuleKey, Pulse.Low);
    }

    public async Task HandleWork(Work work, ChannelWriter<Work> workQueue)
    {
        InputModules[work.SourceModule] = work.Pulse;

        var output = InputModules.Values.All(x => x == Pulse.High) 
            ? Pulse.Low
            : Pulse.High;

        await Broadcast(output, workQueue);
    }
}

class Broadcaster : Module, IModule
{
    public Broadcaster() : base("broadcaster")
    {
    }

    public async Task HandleWork(Work work, ChannelWriter<Work> workQueue)
    {
        await Broadcast(work.Pulse, workQueue);
    }
}

class Button : Module, IModule
{
    public Button() : base("button")
    {
    }

    public async Task HandleWork(Work work, ChannelWriter<Work> workQueue)
    {
        await Broadcast(Pulse.Low, workQueue);
    }
}