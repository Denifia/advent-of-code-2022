namespace _04;

internal class Room
{
    private readonly string _data;

    private readonly Range _encryptedName;
    public ReadOnlySpan<char> EncryptedName { get { return _data[_encryptedName]; } }

    private const int _outerChecksumLength = 7;
    private readonly Range _checksum;
    public ReadOnlySpan<char> Checksum { get { return _data[_checksum]; } }

    private const int _sectorIdLength = 3;
    private readonly Range _sectorId;
    public ReadOnlySpan<char> SectorId { get { return _data[_sectorId]; } }

    public Room(string data)
    {
        _data = data;
        var dataLength = data.Length;
        _encryptedName = new Range(0, dataLength - _sectorIdLength - _outerChecksumLength - 1);
        _checksum = new Range(dataLength - _outerChecksumLength + 1, dataLength - 1);
        _sectorId = new Range(dataLength - _sectorIdLength - _outerChecksumLength, dataLength - _outerChecksumLength);
    }
}
