// Day 4: Security Through Obscurity

using _04;

var lines = File.ReadAllLines("input.txt");

var sum = 0;
foreach (var line in lines)
{
    var room = new Room(line);
    if (RoomVerifier.IsRealRoom(room))
    {
        sum += int.Parse(room.SectorId);
    }
}

// question 1
Console.WriteLine($"Part 1 Answer: {sum}");

// question 2
Console.WriteLine($"Part 2 Answer: {true}");

