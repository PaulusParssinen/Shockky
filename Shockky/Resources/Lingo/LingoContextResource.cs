using Shockky.IO;

namespace Shockky.Resources;

public sealed class LingoContextResource : IShockwaveItem, IResource
{
    public OsType Kind => OsType.LctX;

    private const short SECTION_SIZE = 12;
    private const short EVENT_COUNT = 25;

    public List<LingoContextItem> Items { get; set; }
    public short[] EventHandlerNames { get; } = new short[25];

    public int Type { get; set; }
    public LingoContextFlags Flags { get; set; }

    public int ValuesResourceIndex { get; set; }
    public int NameResourceIndex { get; set; }

    public short ValidCount { get; set; }
    public short FreeChunkIndex { get; set; }

    public LingoContextResource()
    {
        Items = new List<LingoContextItem>();
    }
    public LingoContextResource(ref ShockwaveReader input, ReaderContext context)
    {
        input.ReverseEndianness = false;

        input.ReadInt32BigEndian();
        input.ReadInt32BigEndian();

        Items = new List<LingoContextItem>(input.ReadInt32BigEndian());
        input.ReadInt32BigEndian();

        input.ReadInt16BigEndian();
        input.ReadInt16BigEndian();

        int unk4 = input.ReadInt32BigEndian();
        Type = input.ReadInt32BigEndian(); //TODO: ??

        ValuesResourceIndex = input.ReadInt32BigEndian();
        NameResourceIndex = input.ReadInt32BigEndian();

        ValidCount = input.ReadInt16BigEndian();
        Flags = (LingoContextFlags)input.ReadInt16BigEndian();
        FreeChunkIndex = input.ReadInt16BigEndian();

        input.ReadInt16BigEndian();
        input.ReadInt16BigEndian(); //EnvIndex some_parent_maybe_index

        for (int i = 0; i < EVENT_COUNT; i++)
        {
            EventHandlerNames[i] = input.ReadInt16BigEndian();
        }

        for (int i = 0; i < Items.Capacity; i++)
        {
            Items.Add(new LingoContextItem(ref input));
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
        size += sizeof(int);
        size += sizeof(int);
        size += sizeof(int);
        size += sizeof(short);
        size += sizeof(short);
        size += sizeof(short);
        size += sizeof(short);
        size += sizeof(short);
        size += sizeof(short) * EVENT_COUNT;
        size += Items.Count * SECTION_SIZE;
        return size;
    }

    public void WriteTo(ShockwaveWriter output, WriterOptions options)
    {
        const short ENTRY_OFFSET = 96;

        output.WriteInt32BigEndian(0);
        output.WriteInt32BigEndian(0);
        output.WriteInt32BigEndian(Items.Count);
        output.WriteInt32BigEndian(Items.Count);

        output.WriteInt16BigEndian(ENTRY_OFFSET);
        output.WriteInt16BigEndian(SECTION_SIZE);

        output.WriteInt32BigEndian(0);
        output.WriteInt32BigEndian(Type);

        output.WriteInt32BigEndian(ValuesResourceIndex);
        output.WriteInt32BigEndian(NameResourceIndex);

        output.WriteInt16BigEndian(ValidCount);
        output.WriteInt16BigEndian((short)Flags);
        output.WriteInt16BigEndian(FreeChunkIndex);

        output.WriteInt16BigEndian(-1);
        output.WriteInt16BigEndian(-1);

        for (int i = 0; i < EVENT_COUNT; i++)
        {
            output.WriteInt16BigEndian(EventHandlerNames[i]);
        }

        foreach (LingoContextItem section in Items)
        {
            section.WriteTo(output, options);
        }
    }
}