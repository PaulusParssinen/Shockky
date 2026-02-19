using System.Diagnostics;

using Shockky.IO;

namespace Shockky.Resources;

[DebuggerDisplay("{Kind} | Length: {Length}, Offset: {Offset}")]
public sealed class ResourceEntry : IShockwaveItem
{
    public OsType Kind { get; set; }
    public int Length { get; set; }
    public int Offset { get; set; }
    public ResourceEntryFlags Flags { get; set; }
    public short Unknown { get; set; }
    public int Link { get; set; }

    public ResourceEntry(ref ShockwaveReader input)
    {
        Kind = (OsType)input.ReadInt32BigEndian();
        Length = input.ReadInt32BigEndian();
        Offset = input.ReadInt32BigEndian();
        Flags = (ResourceEntryFlags)input.ReadInt16BigEndian();
        Unknown = input.ReadInt16BigEndian();
        Link = input.ReadInt32BigEndian();
    }

    public int GetBodySize(WriterOptions options)
    {
        int size = 0;
        size += sizeof(int);
        size += sizeof(int);
        size += sizeof(int);
        size += sizeof(short);
        size += sizeof(short);
        size += sizeof(int);
        return size;
    }

    public void WriteTo(ShockwaveWriter output, WriterOptions options)
    {
        output.WriteInt32BigEndian((int)Kind);
        output.WriteInt32BigEndian(Length);
        output.WriteInt32BigEndian(Offset);
        output.WriteInt16BigEndian((short)Flags);
        output.WriteInt16BigEndian(Unknown);
        output.WriteInt32BigEndian(Link);
    }
}