using Shockky.IO;

namespace Shockky.Resources;

public sealed class MemoryMapResource : IShockwaveItem, IResource
{
    public OsType Kind => OsType.mmap;

    public const short ENTRIES_OFFSET = 24;
    public const short ENTRY_SIZE = 20;

    public ResourceEntry[] Entries { get; set; }

    public int LastJunkIndex { get; set; }
    public int SomeLinkedIndex { get; set; }
    public int LastFreeIndex { get; set; }

    public ResourceEntry this[int index] => Entries[index];

    public MemoryMapResource(ref ShockwaveReader input)
    {
        input.ReadInt16BigEndian();
        input.ReadInt16BigEndian();

        input.ReadInt32BigEndian();
        Entries = new ResourceEntry[input.ReadInt32BigEndian()];

        LastJunkIndex = input.ReadInt32BigEndian();
        SomeLinkedIndex = input.ReadInt32BigEndian();
        LastFreeIndex = input.ReadInt32BigEndian();

        for (int i = 0; i < Entries.Length; i++)
        {
            Entries[i] = new ResourceEntry(ref input);
        }
    }

    public int GetBodySize(WriterOptions options)
    {
        int size = 0;
        size += sizeof(short);
        size += sizeof(short);
        size += sizeof(int);
        size += sizeof(int);
        size += sizeof(int);
        size += sizeof(int);
        size += sizeof(int);
        size += Entries.Length * ENTRY_SIZE;
        return size;
    }

    public void WriteTo(ShockwaveWriter output, WriterOptions options)
    {
        output.WriteInt16BigEndian(ENTRIES_OFFSET);
        output.WriteInt16BigEndian(ENTRY_SIZE);

        output.WriteInt32BigEndian(Entries.Length);
        output.WriteInt32BigEndian(Entries.Length);

        output.WriteInt32BigEndian(LastJunkIndex);
        output.WriteInt32BigEndian(SomeLinkedIndex);
        output.WriteInt32BigEndian(LastFreeIndex);
        foreach (var entry in Entries)
        {
            entry.WriteTo(output, options);
        }
    }
}