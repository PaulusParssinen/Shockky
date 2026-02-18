using Shockky.IO;

namespace Shockky.Resources;

/// <summary>
/// Represents the guide grid in the Director.
/// </summary>
public sealed class GridResource : IShockwaveItem, IResource
{
    public OsType Kind => OsType.GRID;

    public int Unknown { get; set; }
    public short Width { get; set; }
    public short Height { get; set; }
    public GridDisplay Display { get; set; }
    public short GridColor { get; set; }

    public short GuideColor { get; set; }
    public Guide[] Guides { get; set; }

    public GridResource(ref ShockwaveReader input, ReaderContext context)
    {
        input.ReverseEndianness = true;

        Unknown = input.ReadInt32LittleEndian();

        Width = input.ReadInt16LittleEndian();
        Height = input.ReadInt16LittleEndian();
        Display = (GridDisplay)input.ReadInt16LittleEndian();
        GridColor = input.ReadInt16LittleEndian();

        Guides = new Guide[input.ReadInt16LittleEndian()];
        GuideColor = input.ReadInt16LittleEndian();
        for (int i = 0; i < Guides.Length; i++)
        {
            Guides[i] = Guide.Read(ref input, context);
        }
    }

    public int GetBodySize(WriterOptions options)
    {
        const int GUIDE_ENTRY_SIZE = sizeof(short) + sizeof(short);

        int size = 0;
        size += sizeof(int);

        size += sizeof(short);
        size += sizeof(short);
        size += sizeof(short);
        size += sizeof(short);

        size += sizeof(short);
        size += sizeof(short);
        size += Guides.Length * GUIDE_ENTRY_SIZE;
        return size;
    }

    public void WriteTo(ShockwaveWriter output, WriterOptions options)
    {
        output.WriteInt32LittleEndian(Unknown);

        output.WriteInt16LittleEndian(Height);
        output.WriteInt16LittleEndian(Width);
        output.WriteInt16LittleEndian((short)Display);
        output.WriteInt16LittleEndian(GridColor);

        output.WriteInt16LittleEndian((short)Guides.Length);
        output.WriteInt16LittleEndian(GuideColor);
        foreach (Guide guide in Guides)
        {
            guide.WriteTo(output, options);
        }
    }
}