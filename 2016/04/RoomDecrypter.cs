namespace _04;

internal class RoomDecrypter
{
    public static string Decrypt(Room room)
    {
        var decryptedName = room.EncryptedName.ToArray();
        var sectorId = int.Parse(room.SectorId);
        for (var i = 0; i < decryptedName.Length; i++)
        {
            if (decryptedName[i] == '-')
            {
                decryptedName[i] = ' ';
            }
            else
            {
                var offset = decryptedName[i] - 'a';
                var newChar = (char)('a' + ((offset + sectorId) % 26));
                decryptedName[i] = newChar;
            }
        }
        return new string(decryptedName);
    }
}

public class RoomDecrypterTests
{
    [Theory]
    [InlineData("qzmt-zixmtkozy-ivhz-343[aaaaa]", "very encrypted name")]
    public void IdentifiesRealRooms(string input, string expected)
    {
        Assert.Equal(expected, RoomDecrypter.Decrypt(new Room(input)));
    }
}