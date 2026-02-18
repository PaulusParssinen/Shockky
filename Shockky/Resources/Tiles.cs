using System.Drawing;

using Shockky.IO;
using Shockky.Resources.Cast;

namespace Shockky.Resources;

//TODO: v5 = memberNum or MemberId
public record Tile(CastMemberId Id, Rectangle Rect) : IShockwaveItem
{
    public Tile(ref ShockwaveReader input, ReaderContext context)
        : this(new CastMemberId(input.ReadInt16LittleEndian(), input.ReadInt16LittleEndian()), input.ReadRectLittleEndian())
    { }

    public int GetBodySize(WriterOptions options)
    {
        int size = 0;
        return size;
    }

    public void WriteTo(ShockwaveWriter output, WriterOptions options)
    {
        throw new NotImplementedException();
    }

    public static Tile Read(ref ShockwaveReader input, ReaderContext context)
    {
        throw new NotImplementedException();
    }
}

public sealed class Tiles : IShockwaveItem, IResource
{
    public OsType Kind => OsType.VWTL;

    public Tile[] Items { get; } = new Tile[8];

    public Tiles(ref ShockwaveReader input, ReaderContext context)
    {
        for (int i = 0; i < Items.Length; i++)
        {
            input.ReadInt32LittleEndian();
            input.ReadInt32LittleEndian();

            Items[i] = new Tile(ref input, context);
        }
    }

    public int GetBodySize(WriterOptions options)
    {
        throw new NotImplementedException();
    }

    public void WriteTo(ShockwaveWriter output, WriterOptions options)
    {
        throw new NotImplementedException();
    }
}