using Shockky.IO;

namespace Shockky.Resources;

public class LingoContextItem : IShockwaveItem
{
    public int ChunkIndex { get; set; }
    public LingoContextItemFlags Flags { get; set; }

    /// <summary>
    /// Points to next free context item.
    /// </summary>
    public short Link { get; set; }

    public LingoContextItem()
    { }
    public LingoContextItem(ref ShockwaveReader input)
    {
        input.ReadInt32BigEndian();
        ChunkIndex = input.ReadInt32BigEndian();
        Flags = (LingoContextItemFlags)input.ReadInt16BigEndian();
        Link = input.ReadInt16BigEndian();
    }

    public int GetBodySize(WriterOptions options)
    {
        int size = 0;
        size += sizeof(int);
        size += sizeof(int);
        size += sizeof(short);
        size += sizeof(short);
        return size;
    }

    public void WriteTo(ShockwaveWriter output, WriterOptions options)
    {
        output.WriteInt32BigEndian(0);
        output.WriteInt32BigEndian(ChunkIndex);
        output.WriteInt16BigEndian((short)Flags);
        output.WriteInt16BigEndian(Link);
    }
}