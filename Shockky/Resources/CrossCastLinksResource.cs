using Shockky.IO;

namespace Shockky.Resources;

[ShockwaveItem(BigEndian = true, GenerateSerialization = true)]
public readonly partial record struct CrossCastLink(
    short GlobalCastLibNum,
    [property: ParseStringAs(StringParseKind.PString)] string Path) : IShockwaveItem<CrossCastLink>;

/// <summary>
/// The global cast library numbers and their paths are specified in this resource.
/// </summary>
public sealed class CrossCastLinksResource : IShockwaveItem, IResource
{
    public OsType Kind => OsType.ccl;

    public List<CrossCastLink> Links { get; set; } = [];

    public CrossCastLinksResource() { }

    public CrossCastLinksResource(ref ShockwaveReader input, ReaderContext context)
    {
        input.ReverseEndianness = false;

        int headerSize = input.ReadInt32BigEndian();
        OffsetTable offsets = OffsetTable.Read(ref input);
        Links = new List<CrossCastLink>(offsets.Length);

        for (int i = 0; i < offsets.Length; i++)
        {
            int entrySize = offsets.GetEntrySize(i);
            if (entrySize <= 0) continue;

            int startPos = input.Position;
            var link = CrossCastLink.Read(ref input, context);
            Links.Add(link);

            int consumed = input.Position - startPos;
            if (consumed < entrySize)
            {
                input.Advance(entrySize - consumed);
            }
        }
    }

    public int GetBodySize(WriterOptions options)
    {
        int size = sizeof(int); // headerSize = 4
        size += OffsetTable.GetTableSize(Links.Count);
        foreach (var link in Links)
        {
            size += link.GetBodySize(options);
        }
        return size;
    }

    public void WriteTo(ref ShockwaveWriter output, WriterOptions options)
    {
        output.WriteInt32BigEndian(sizeof(int)); // headerSize = 4

        // TODO: Not entirely happy with this
        int currentOffset = 0;
        var offsetValues = new int[Links.Count + 1];
        for (int i = 0; i < Links.Count; i++)
        {
            offsetValues[i] = currentOffset;
            currentOffset += Links[i].GetBodySize(options);
        }
        offsetValues[Links.Count] = currentOffset;

        var table = new OffsetTable(offsetValues);
        table.WriteTo(ref output);

        foreach (var link in Links)
        {
            link.WriteTo(ref output, options);
        }
    }

    public static CrossCastLinksResource Read(ref ShockwaveReader input, ReaderContext context)
        => new(ref input, context);
}