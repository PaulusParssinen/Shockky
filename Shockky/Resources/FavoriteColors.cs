using Shockky.IO;

namespace Shockky.Resources;

public sealed class FavoriteColors : IShockwaveItem, IResource
{
    public OsType Kind => OsType.FCOL;

    public (int R, int G, int B)[] Colors { get; } = new (int R, int G, int B)[16];

    public FavoriteColors()
    {
        Colors = new (int R, int G, int B)[16]
        {
            (0, 0, 0),
            (17, 17, 17),
            (34, 34, 34),
            (51, 51, 51),
            (68, 68, 68),
            (85, 85, 85),
            (102, 102, 102),
            (119, 119, 119),
            (136, 136, 136),
            (153, 153, 153),
            (170, 170, 170),
            (187, 187, 187),
            (204, 204, 204),
            (221, 221, 221),
            (238, 238, 238),
            (255, 255, 255)
        };
    }

    public int GetBodySize(WriterOptions options)
    {
        int size = 0;
        size += sizeof(int);
        size += sizeof(int);

        size += Colors.Length * 3;
        return size;
    }

    public void WriteTo(ShockwaveWriter output, WriterOptions options)
    {
        output.WriteInt32LittleEndian(1);
        output.WriteInt32LittleEndian(1);

        foreach ((int r, int g, int b) in Colors)
        {
            output.WriteByte((byte)r);
            output.WriteByte((byte)g);
            output.WriteByte((byte)b);
        }
    }

    public static FavoriteColors Read(ref ShockwaveReader input, ReaderContext context)
    {
        FavoriteColors favoriteColors = new();

        input.ReadInt32LittleEndian();
        input.ReadInt32LittleEndian();

        for (int i = 0; i < favoriteColors.Colors.Length; i++)
        {
            favoriteColors.Colors[i] = (input.ReadByte(), input.ReadByte(), input.ReadByte());
        }
        return favoriteColors;
    }
}