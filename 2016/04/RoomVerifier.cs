namespace _04;

internal class RoomVerifier
{
    public static bool IsRealRoom(Room room)
    {
        var computedChecksum = room.EncryptedName
            .ToArray()
            .Where(c => c != '-')
            .AggregateBy(
                keySelector: c => c,
                seedSelector: c => 0,
                func: (accumulate, c) => accumulate + 1
            )
            .OrderByDescending(keySelector: c => c.Value)
            .ThenBy(keySelector: c => c.Key)
            .Take(5)
            .Select(selector: c => c.Key)
            .ToArray()
            .AsSpan();

        return computedChecksum.SequenceEqual(room.Checksum);
    }
}

public class RoomVerifierTests
{
    [Theory]
    [InlineData("aaaaa-bbb-z-y-x-123[abxyz]", true)]
    [InlineData("a-b-c-d-e-f-g-h-987[abcde]", true)]
    [InlineData("not-a-real-room-404[oarel]", true)]
    [InlineData("totally-real-room-200[decoy]", false)]
    public void IdentifiesRealRooms(string input, bool expected)
    {
        Assert.Equal(expected, RoomVerifier.IsRealRoom(new Room(input)));
    }
}