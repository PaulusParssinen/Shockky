using System.Text;

using Shockky.IO;

namespace Shockky.Resources.Cast;

public sealed class CastListEntry : IShockwaveItem
{
    public string Name { get; set; }
    public string FilePath { get; set; }

    public short PreloadSettings { get; set; }
    public short MemberMin { get; set; }
    public short MemberCount { get; set; }

    public int Id { get; set; }

    public CastListEntry()
    { }
    public CastListEntry(ref ShockwaveReader input, ReaderContext context)
    {
        Name = input.ReadString();
        FilePath = input.ReadString();
        PreloadSettings = input.ReadInt16LittleEndian();
        MemberMin = input.ReadInt16LittleEndian();
        MemberCount = input.ReadInt16LittleEndian();
        Id = input.ReadInt32LittleEndian();
    }

    public int GetBodySize(WriterOptions options)
    {
        int size = 0;
        size += Encoding.UTF8.GetByteCount(Name) + 1;
        size += Encoding.UTF8.GetByteCount(FilePath) + 1;
        size += sizeof(short);
        size += sizeof(short);
        size += sizeof(short);
        size += sizeof(int);
        return size;
    }

    public void WriteTo(ShockwaveWriter output, WriterOptions options)
    {
        output.WriteString(Name);
        output.WriteString(FilePath);
        output.WriteInt16LittleEndian(PreloadSettings);
        output.WriteInt16LittleEndian(MemberMin);
        output.WriteInt16LittleEndian(MemberCount);
        output.WriteInt32LittleEndian(Id);
    }
}
