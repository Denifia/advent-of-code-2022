// Day 7: No Space Left On Device

var lines = await File.ReadAllLinesAsync("input.txt");

var root = new Folder(parent: null);
var location = new Stack<Folder>();
var folders = new List<Folder>();

foreach (var line in lines)
{
    if (line.StartsWith("$ cd"))
    {
        Navigate(line[5..]);
        continue;
    }
    if (line.StartsWith("$ ls"))
    {
        // do nothing
        continue;
    }
    if (line.StartsWith("dir"))
    {
        var subDirectory = location.Peek().AddDirectory(line[4..]);
        folders.Add(subDirectory);
        continue;
    }

    location.Peek().UpdateSize(int.Parse(line.Split(' ')[0]));
}

void Navigate(string path)
{
    if (path == "/")
    {
        location.Clear();
        location.Push(root);
        return;
    }
    if (path == "..")
    {
        location.Pop();
        return;
    }

    location.Push(location.Peek().GetFolder(path));
}

// Find all of the directories with a total size of at most 100000. What is the sum of the total sizes of those directories?
Console.WriteLine($"Part 1 Answer: {folders.Where(x => x.Size <= 100000).Sum(x => x.Size)}");

var totalSpace = 70000000;
var usedSpace = root.Size;
var requiredSpace = 30000000;
var requiredMinimumDeleteSize = requiredSpace - (totalSpace - usedSpace);

// Find the smallest directory that, if deleted, would free up enough space on the filesystem to run the update. What is the total size of that directory?
Console.WriteLine($"Part 2 Answer: {folders.Where(x => x.Size > requiredMinimumDeleteSize).OrderBy(x => x.Size).First().Size}");

class Folder
{
    private readonly Dictionary<string, Folder> _folders = new();
    private readonly Folder? _parent;
    public int Size { get; private set; }

    public Folder(Folder? parent)
    {
        _parent = parent;
    }

    public Folder AddDirectory(string name)
    {
        var subFolder = new Folder(parent: this);
        _folders.Add(name, subFolder);
        return subFolder;
    }

    public Folder GetFolder(string name)
    {
        return _folders[name];
    }

    public void UpdateSize(int size)
    {
        Size += size;
        _parent?.UpdateSize(size);
    }
}