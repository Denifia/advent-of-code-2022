// Day 4: Security Through Obscurity

using _04;

var lines = File.ReadAllLines("input.txt");

var sum = 0;
var part2 = string.Empty;
foreach (var line in lines)
{
    var room = new Room(line);
    if (RoomVerifier.IsRealRoom(room))
    {
        sum += int.Parse(room.SectorId);

        var roomName = RoomDecrypter.Decrypt(room);
        if(roomName.Contains("northpole object storage", StringComparison.OrdinalIgnoreCase))
        {
            part2 = room.SectorId.ToString();
        }
    }
}

// question 1
Console.WriteLine($"Part 1 Answer: {sum}");

// question 2
Console.WriteLine($"Part 2 Answer: {part2}");

