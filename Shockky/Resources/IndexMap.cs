using System.Diagnostics;

using Shockky.IO;

namespace Shockky.Resources;

public sealed class IndexMap : IShockwaveItem, IResource
{
    public OsType Kind => OsType.imap;

    public int MemoryMapOffset { get; set; }
    public DirectorVersion Version { get; set; }

    public int Field0C { get; set; }
    public int Field10 { get; set; }
    public int Field14 { get; set; }

    public IndexMap(ref ShockwaveReader input)
    {
        int memoryMapCount = input.ReadInt32BigEndian();
        Debug.Assert(memoryMapCount == 1);
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

    public void WriteTo(ShockwaveWriter output, WriterOptions options)
    {
        output.WriteInt32BigEndian(1);
        output.WriteInt32BigEndian(MemoryMapOffset);
        output.WriteInt32BigEndian((int)Version);
        output.WriteInt32BigEndian(Field0C);
        output.WriteInt32BigEndian(Field10);
        output.WriteInt32BigEndian(Field14);
    }
}