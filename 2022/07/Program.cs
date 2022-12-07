// Day 7: No Space Left On Device

using App;

var lines = await System.IO.File.ReadAllLinesAsync("input.txt");

var root = new App.Directory("/", null);
var location = new Stack<App.Directory>();
var folders = new List<App.Directory>();

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

    var fileParts = line.Split(' ');
    location.Peek().AddFile(fileParts[1], int.Parse(fileParts[0]));
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

    location.Push(location.Peek().GetDirectory(path));
}

// Find all of the directories with a total size of at most 100000. What is the sum of the total sizes of those directories?
Console.WriteLine($"Part 1 Answer: {folders.Where(x => x.Size <= 100000).Sum(x => x.Size)}");

var totalSpace = 70000000;
var usedSpace = root.Size;
var requiredSpace = 30000000;
var requiredMinimumDeleteSize = requiredSpace - (totalSpace - usedSpace);

// Find the smallest directory that, if deleted, would free up enough space on the filesystem to run the update. What is the total size of that directory?
Console.WriteLine($"Part 1 Answer: {folders.Where(x => x.Size <= requiredMinimumDeleteSize).OrderByDescending(x => x.Size).First().Size}");

namespace App
{
//    public static class InputParser
//    {
//        public static Command Parse(string line)
//        {
//            if (line[0] == '$')
//            {
//                return ParseCommand(line[2..]);
//            }

//            return ParseCommandOutput(line);
//        }
//        private static Command ParseCommand(string command)
//        {
//            if (command.StartsWith("cd")) return ParseNavigateCommand(command[2..]);
//            if (command.StartsWith("ls")) return new ParseListCommand();
//            throw new ApplicationException($"unsupported command: {command}");
//        }

//        private static Command ParseNavigateCommand(string path)
//        {
//            if (path == "/") return new ReturnToRootCommand();
//            if (path == "..") return new NavigateUpCommand();
//            return new NavigateDownCommand(path);
//        }

//        private static Command ParseCommandOutput(string line)
//        {
//            throw new NotImplementedException();
//        }
//    }

    public interface ISizable
    {
        int Size { get; }
    }

    public class Directory : ISizable
    {
        private readonly string _name;
        private readonly Dictionary<string, Directory> _directories = new();
        private readonly Dictionary<string, File> _files = new();
        private readonly Directory? _parent;
        public int Size { get; private set; }

        public Directory(string name, Directory? parent)
        {
            _name = name;
            _parent = parent;
        }

        public Directory AddDirectory(string name)
        {
            var subDirectory = new Directory(name, this);
            _directories.Add(name, subDirectory);
            return subDirectory;
        }

        public Directory GetDirectory(string name)
        {
            return _directories[name];
        }

        public IEnumerable<Directory> SubDirectories => _directories.Values;

        public void AddFile(string name, int size)
        {
            _files.Add(name, new File(name, size));
            UpdateSize(size);
        }

        public void UpdateSize(int size)
        {
            Size += size;
            _parent?.UpdateSize(size);
        }
    }

    public class File : ISizable
    {
        private readonly string _name;
        public int Size { get; init; }

        public File(string name, int size)
        {
            _name = name;
            Size = size;
        }
    }

    //public abstract class Command
    //{
    //    public abstract void Execute();
    //}

    //public class ReturnToRootCommand : Command
    //{
    //    public override void Execute()
    //    {

    //    }
    //}

    //public class NavigateDownCommand : Command
    //{
    //    private readonly string _directoryName;

    //    public NavigateDownCommand(string directoryName)
    //    {
    //        _directoryName = directoryName;
    //    }

    //    public override void Execute()
    //    {

    //    }
    //}

    //public class NavigateUpCommand : Command
    //{
    //    public override void Execute()
    //    {

    //    }
    //}

    //public class ParseListCommand : Command
    //{
    //    public override void Execute()
    //    {

    //    }
    //}
}
