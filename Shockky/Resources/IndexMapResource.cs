using Shockky.IO;

namespace Shockky.Resources;

public sealed class IndexMapResource : IShockwaveItem, IResource
{
    public OsType Kind => OsType.imap;

    // TODO: Assert expected constants support to generator
    public int MemoryMapCount { get; set; }
    public int MemoryMapOffset { get; set; }
    public DirectorVersion Version { get; set; } // TODO: Generator, override underlying enum type

    public int Field0C { get; set; }
    public int Field10 { get; set; }
    public int Field14 { get; set; }

    public IndexMapResource(ref ShockwaveReader input)
    {
        int memoryMapCount = input.ReadInt32BigEndian();
        MemoryMapOffset = input.ReadInt32BigEndian();
        Version = (DirectorVersion)input.ReadInt32BigEndian();
        Field0C = input.ReadInt32BigEndian();
        Field10 = input.ReadInt32BigEndian();
        Field14 = input.ReadInt32BigEndian();
    }

    public int GetBodySize(WriterOptions options)
    {
        int size = 0;
        size += sizeof(int);
        size += sizeof(int);
        size += sizeof(int);
        size += sizeof(int);
        size += sizeof(int);
        size += sizeof(int);
        return size;
    }

    public void WriteTo(ref ShockwaveWriter output, WriterOptions options)
    {
        output.WriteInt32BigEndian(1);
        output.WriteInt32BigEndian(MemoryMapOffset);
        output.WriteInt32BigEndian((int)Version);
        output.WriteInt32BigEndian(Field0C);
        output.WriteInt32BigEndian(Field10);
        output.WriteInt32BigEndian(Field14);
    }
}