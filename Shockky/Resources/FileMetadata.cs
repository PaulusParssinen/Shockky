using Shockky.IO;

namespace Shockky.Resources;

public class FileMetadata : IShockwaveItem
{
    public OsType Kind { get; }
    public CodecKind Codec { get; set; }

    public int FileLength { get; }
    public bool IsLittleEndian => Kind == OsType.XFIR;

    public FileMetadata(ref ShockwaveReader input)
    {
        Kind = (OsType)input.ReadUInt32BigEndian();
        FileLength = IsLittleEndian ? input.ReadInt32LittleEndian() : input.ReadInt32BigEndian();
        Codec = (CodecKind)(IsLittleEndian ? input.ReadUInt32LittleEndian() : input.ReadUInt32BigEndian());
    }

    public int GetBodySize(WriterOptions options)
    {
        int size = 0;
        size += sizeof(int);
        size += sizeof(int);
        size += sizeof(int);
        return size;
    }

    public void WriteTo(ShockwaveWriter output, WriterOptions options)
    {
        // TODO: Maybe allow assembly in big-endian
        output.WriteUInt32BigEndian((uint)OsType.XFIR);
        output.WriteInt32LittleEndian(FileLength);
        output.WriteUInt32LittleEndian((uint)Codec);
    }
}