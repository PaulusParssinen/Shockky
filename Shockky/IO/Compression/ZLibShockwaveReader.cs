using System.Buffers.Binary;
using System.IO.Compression;
using System.Text;

namespace Shockky.IO;

public sealed class ZLibShockwaveReader(Stream innerStream, bool isBigEndian, bool leaveOpen)
    : BinaryReader(new ZLibStream(innerStream, CompressionMode.Decompress, leaveOpen))
{
    private readonly bool _isBigEndian = isBigEndian;

    public new int Read7BitEncodedInt()
    {
        int value = 0;
        byte b;
        do
        {
            b = ReadByte();
            value = (value << 7) | (b & 0x7F);
        }
        while (b >> 7 != 0);
        return value;
    }

    public string ReadCString()
    {
        char currentChar;
        StringBuilder builder = new();
        while ((currentChar = ReadChar()) != '\0')
        {
            builder.Append(currentChar);
        }
        return builder.ToString();
    }

    public short ReadInt16LittleEndian()
    {
        return _isBigEndian ? BinaryPrimitives.ReverseEndianness(ReadInt16()) : ReadInt16();
    }
    public short ReadInt16BigEndian()
    {
        return _isBigEndian ? ReadInt16() : BinaryPrimitives.ReverseEndianness(ReadInt16());
    }

    public int ReadInt32LittleEndian()
    {
        return _isBigEndian ? BinaryPrimitives.ReverseEndianness(ReadInt32()) : ReadInt32();
    }
    public int ReadInt32BigEndian()
    {
        return _isBigEndian ? ReadInt32() : BinaryPrimitives.ReverseEndianness(ReadInt32());
    }
}