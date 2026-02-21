using Shockky.IO;

namespace Shockky.Resources;

public sealed class LingoNamesResource : IShockwaveItem, IResource
{
    public OsType Kind => OsType.Lnam;

    public List<string> Names { get; set; } = [];

    public LingoNamesResource()
    { }
    public LingoNamesResource(ref ShockwaveReader input, ReaderContext context)
    {
        input.ReverseEndianness = false;

        input.ReadInt32BigEndian();
        input.ReadInt32BigEndian();
        input.ReadInt32BigEndian();
        input.ReadInt32BigEndian();

        short nameOffset = input.ReadInt16BigEndian();
        Names = new List<string>(input.ReadInt16BigEndian());

        input.Position = nameOffset;
        for (int i = 0; i < Names.Capacity; i++)
        {
            Names.Add(input.ReadPString());
        }
    }

    public int GetBodySize(WriterOptions options)
    {
        int size = 0;
        size += sizeof(int);
        size += sizeof(int);
        size += sizeof(int);
        size += sizeof(int);
        size += sizeof(short);
        size += sizeof(short);
        size += Names.Sum(n => n.Length + 1);
        return size;
    }

    public void WriteTo(ShockwaveWriter output, WriterOptions options)
    {
        const short NAME_OFFSET = 20;
        int namesLength = Names?.Sum(static n => sizeof(byte) + n.Length) ?? 0;

        output.WriteInt32BigEndian(0);
        output.WriteInt32BigEndian(0);
        output.WriteInt32BigEndian(namesLength);
        output.WriteInt32BigEndian(namesLength);
        output.WriteInt16BigEndian(NAME_OFFSET);
        output.WriteInt16BigEndian((short)Names.Count);

        foreach (string name in Names)
        {
            output.WriteString(name);
        }
    }
}