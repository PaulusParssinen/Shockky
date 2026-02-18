using System.Drawing;

using Shockky.IO;

namespace Shockky.Resources;

public sealed class PaletteResource : IShockwaveItem, IResource
{
    public OsType Kind => OsType.CLUT;

    public Color[] Colors { get; set; }

    public PaletteResource()
    { }
    public PaletteResource(ref ShockwaveReader input, ReaderContext context)
    {
        Colors = new Color[input.Length / 6];
        for (int i = 0; i < Colors.Length; i++)
        {
            Colors[i] = input.ReadColor();
        }
    }

    public int GetBodySize(WriterOptions options) => Colors.Length * 6;

    public void WriteTo(ShockwaveWriter output, WriterOptions options)
    {
        foreach (Color color in Colors)
        {
            output.WriteColor(color);
        }
    }
}