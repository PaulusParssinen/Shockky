using Shockky.IO;

namespace Shockky.Resources;

public sealed class ScoreReferenceResource : IShockwaveItem, IResource
{
    public OsType Kind => OsType.SCRF;

    public int Unknown { get; set; }
    public Dictionary<short, int> Entries { get; }

    public ScoreReferenceResource()
    { }
    public ScoreReferenceResource(ref ShockwaveReader input, ReaderContext context)
    {
        input.ReadInt32LittleEndian();
        input.ReadInt32LittleEndian();

        int entryCount = input.ReadInt32LittleEndian();
        input.ReadInt32LittleEndian();

        input.ReadInt16LittleEndian();
        input.ReadInt16LittleEndian();

        Unknown = input.ReadInt32LittleEndian();

        Entries = new Dictionary<short, int>(entryCount);
        for (int i = 0; i < entryCount; i++)
        {
            Entries.Add(input.ReadInt16LittleEndian(), input.ReadInt32LittleEndian());
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
        size += sizeof(int);
        size += Entries.Count * (sizeof(short) + sizeof(int));
        return size;
    }

    public void WriteTo(ShockwaveWriter output, WriterOptions options)
    {
        const short ENTRY_OFFSET = 24;
        const short UNKNOWN = 8;

        output.WriteInt32LittleEndian(0);
        output.WriteInt32LittleEndian(0);

        output.WriteInt32LittleEndian(Entries.Count);
        output.WriteInt32LittleEndian(Entries.Count);

        output.WriteInt16LittleEndian(ENTRY_OFFSET);
        output.WriteInt16LittleEndian(UNKNOWN);

        output.WriteInt32LittleEndian(Unknown);
        foreach ((short number, int castMapPtrId) in Entries)
        {
            output.WriteInt16LittleEndian(number);
            output.WriteInt32LittleEndian(castMapPtrId);
        }
    }
}